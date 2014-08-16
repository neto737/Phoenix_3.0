using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Users.Badges;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Achievements
{
	internal sealed class AchievementManager
	{
        public static Dictionary<uint, Achievement> Achievements = new Dictionary<uint, Achievement>();
        public static Dictionary<string, uint> Badges = new Dictionary<string, uint>();

		public static void LoadAchievements(DatabaseClient dbClient)
		{
            Logging.Write("Loading Achievements..");
			AchievementManager.Achievements.Clear();
			DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM achievements");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					AchievementManager.Achievements.Add((uint)dataRow["Id"], new Achievement((uint)dataRow["Id"], (string)dataRow["type"], (int)dataRow["levels"], (string)dataRow["badge"], (int)dataRow["pixels_base"], (double)dataRow["pixels_multiplier"], PhoenixEnvironment.EnumToBool(dataRow["dynamic_badgelevel"].ToString()), (int)dataRow["score_base"]));
				}
				AchievementManager.Badges.Clear();
				dataTable = dbClient.ReadDataTable("SELECT * FROM badges");
				if (dataTable != null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						AchievementManager.Badges.Add((string)dataRow["badge"], (uint)dataRow["Id"]);
					}
					Logging.WriteLine("completed!");
				}
			}
		}
		public uint BadgeID(string Badge)
		{
			if (Badges.ContainsKey(Badge))
			{
				return Badges[Badge];
			}
            return 0;
		}
		public bool UserHasAchievement(GameClient Session, uint Id, int MinLevel)
		{
			return Session.GetHabbo().Achievements.ContainsKey(Id) && Session.GetHabbo().Achievements[Id] >= MinLevel;
		}
		public static ServerMessage SerializeAchievementList(GameClient Session)
		{
			int num = AchievementManager.Achievements.Count;
			foreach (KeyValuePair<uint, Achievement> current in AchievementManager.Achievements)
			{
				if (current.Value.Type == "hidden")
				{
					num--;
				}
			}
			ServerMessage Message = new ServerMessage(436);
			Message.AppendInt32(num);
			foreach (KeyValuePair<uint, Achievement> current in AchievementManager.Achievements)
			{
				if (!(current.Value.Type == "hidden"))
				{
					int num2 = 0;
					int num3 = 1;
					if (Session.GetHabbo().Achievements.ContainsKey(current.Value.Id))
					{
						num2 = Session.GetHabbo().Achievements[current.Value.Id];
					}
					if (current.Value.Levels > 1 && num2 > 0)
					{
						num3 = num2 + 1;
					}
					if (num3 > current.Value.Levels)
					{
						num3 = current.Value.Levels;
					}
					Message.AppendUInt(current.Value.Id);
					Message.AppendInt32(num3);
					Message.AppendStringWithBreak(AchievementManager.FormatBadgeCode(current.Value.BadgeCode, num3, current.Value.DynamicBadgeLevel));
					Message.AppendInt32(1);
					Message.AppendInt32(0);
					Message.AppendInt32(0);
					Message.AppendInt32(0);
					Message.AppendBoolean(num2 == current.Value.Levels);
					Message.AppendStringWithBreak(current.Value.Type);
					Message.AppendInt32(current.Value.Levels);
				}
			}
			return Message;
		}
		public void UnlockNextAchievement(GameClient Session, uint uint_0)
		{
			if (!AchievementManager.Achievements.ContainsKey(uint_0))
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("AchievementID: " + uint_0 + " does not exist in your database!");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else
			{
				Achievement @class = AchievementManager.Achievements[uint_0];
				if (@class != null)
				{
					if (Session.GetHabbo().Achievements.ContainsKey(uint_0))
					{
						this.UnlockAchievement(Session, uint_0, Session.GetHabbo().Achievements[uint_0 + 1u]);
					}
					else
					{
						this.UnlockAchievement(Session, uint_0, 1);
					}
				}
			}
		}
        public void UnlockAchievement(GameClient Session, uint uint_0, int int_0)
        {
            if (!AchievementManager.Achievements.ContainsKey(uint_0))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("AchievementID: " + uint_0 + " does not exist in our database!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Achievement @class = AchievementManager.Achievements[uint_0];
                if (@class != null && !this.UserHasAchievement(Session, @class.Id, int_0) && int_0 >= 1 && int_0 <= @class.Levels)
                {
                    int num = AchievementManager.CalculateAchievementValue(@class.Dynamic_badgelevel, @class.PixelMultiplier, int_0);
                    int num2 = AchievementManager.CalculateAchievementValue(@class.ScoreBase, @class.PixelMultiplier, int_0);
                    using (TimedLock.Lock(Session.GetHabbo().GetBadgeComponent().BadgeList))
                    {
                        List<string> list = new List<string>();
                        foreach (Badge current in Session.GetHabbo().GetBadgeComponent().BadgeList)
                        {
                            if (current.Code.StartsWith(@class.BadgeCode))
                            {
                                list.Add(current.Code);
                            }
                        }
                        foreach (string current2 in list)
                        {
                            Session.GetHabbo().GetBadgeComponent().RemoveBadge(current2);
                        }
                    }
                    Session.GetHabbo().GetBadgeComponent().GiveBadge(Session, AchievementManager.FormatBadgeCode(@class.BadgeCode, int_0, @class.DynamicBadgeLevel), true);
                    if (Session.GetHabbo().Achievements.ContainsKey(@class.Id))
                    {
                        Session.GetHabbo().Achievements[@class.Id] = int_0;
                        using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
                        {
                            class2.ExecuteQuery(string.Concat(new object[]
							{
								"UPDATE user_achievements SET achievement_level = '",
								int_0,
								"' WHERE user_id = '",
								Session.GetHabbo().Id,
								"' AND achievement_id = '",
								@class.Id,
								"' LIMIT 1; UPDATE user_stats SET AchievementScore = AchievementScore + ",
								num2,
								" WHERE Id = '",
								Session.GetHabbo().Id,
								"' LIMIT 1; "
							}));
                            goto IL_346;
                        }
                    }
                    Session.GetHabbo().Achievements.Add(@class.Id, int_0);
                    using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        class2.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO user_achievements (user_id,achievement_id,achievement_level) VALUES ('",
							Session.GetHabbo().Id,
							"','",
							@class.Id,
							"','",
							int_0,
							"'); UPDATE user_stats SET AchievementScore = AchievementScore + ",
							num2,
							" WHERE Id = '",
							Session.GetHabbo().Id,
							"' LIMIT 1; "
						}));
                    }
                IL_346:
                    ServerMessage Message = new ServerMessage(437u);
                    Message.AppendUInt(@class.Id);
                    Message.AppendInt32(int_0);
                    Message.AppendInt32(1337);
                    Message.AppendStringWithBreak(AchievementManager.FormatBadgeCode(@class.BadgeCode, int_0, @class.DynamicBadgeLevel));
                    Message.AppendInt32(num2);
                    Message.AppendInt32(num);
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                    if (int_0 > 1)
                    {
                        Message.AppendStringWithBreak(AchievementManager.FormatBadgeCode(@class.BadgeCode, int_0 - 1, @class.DynamicBadgeLevel));
                    }
                    else
                    {
                        Message.AppendStringWithBreak("");
                    }
                    Message.AppendStringWithBreak(@class.Type);
                    Session.SendMessage(Message);
                    Session.GetHabbo().AchievementScore += num2;
                    Session.GetHabbo().ActivityPoints += num;
                    Session.GetHabbo().UpdateActivityPointsBalance(num);
                    if (Session.GetHabbo().FriendStreamEnabled)
                    {
                        using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                        {
                            string BadgeCode = "";
                            if (@class.DynamicBadgeLevel)
                            {
                                BadgeCode = @class.BadgeCode + int_0.ToString();
                            }
                            else
                            {
                                BadgeCode = @class.BadgeCode;
                            }

                            if (!string.IsNullOrEmpty(BadgeCode))
                            {
                                string look = PhoenixEnvironment.FilterInjectionChars(Session.GetHabbo().Look);
                                adapter.AddParamWithValue("look", look);
                                adapter.ExecuteQuery("INSERT INTO `friend_stream` (`id`, `type`, `userid`, `gender`, `look`, `time`, `data`) VALUES (NULL, '2', '" + Session.GetHabbo().Id + "', '" + Session.GetHabbo().Gender + "', @look, UNIX_TIMESTAMP(), '" + BadgeCode + "');");
                            }
                        }
                    }
                }
            }
        }
		public static int CalculateAchievementValue(int BaseValue, double Multiplier, int Level)
		{
			return (int)((double)BaseValue * ((double)Level * Multiplier));
		}

		public static string FormatBadgeCode(string BadgeTemplate, int Level, bool Dyn)
		{
			if (!Dyn)
			{
				return BadgeTemplate;
			}
			else
			{
				return BadgeTemplate + Level;
			}
		}
	}
}
