using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Quests;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Quests
{
	internal sealed class QuestManager
	{
		public List<Quest> Quests;

		public QuestManager()
		{
			this.Quests = new List<Quest>();
		}

		public void InitQuests()
		{
            Logging.Write("Loading Quests..");
			this.Quests.Clear();
			DataTable Data = null;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				Data = adapter.ReadDataTable("SELECT Id,type,action,needofcount,level_num,pixel_reward FROM quests WHERE enabled = '1' ORDER by level_num");
			}
			if (Data != null)
			{
				foreach (DataRow Row in Data.Rows)
				{
					Quest Quest = new Quest((uint)Row["Id"], (string)Row["type"], (string)Row["action"], (int)Row["needofcount"], (int)Row["level_num"], (int)Row["pixel_reward"]);
					if (Quest != null)
					{
						this.Quests.Add(Quest);
					}
				}
				Logging.WriteLine("completed!");
			}
		}

		public void ProgressUserQuest(uint Id, GameClient Session)
		{
			Session.GetHabbo().CurrentQuestProgress++;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.ExecuteQuery("UPDATE user_stats SET quest_progress = quest_progress + 1 WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
			}
			PhoenixEnvironment.GetGame().GetQuestManager().HandleQuest(Id, Session);
		}

		public int GetHighestLevelForType(string Type)
		{
			int i = 0;
			foreach (Quest Q in this.Quests)
			{
				if (Q.Type == Type)
				{
					i++;
				}
			}
			return i;
		}

		public uint GetQuestIdBy1More(int LevelType, string Type)
		{
			foreach (Quest Q in Quests)
			{
				if (Q.Type == Type && Q.Level == LevelType + 1)
				{
					return Q.QuestId();
				}
			}
			return 0;
		}

        public void ActivateNextQuest(GameClient Session)
        {
            Quest Quest = this.GetQuest(Session.GetHabbo().LastQuestId);

            string Type = Quest.Type.ToLower();
            int Level = 0;

            switch (Type)
            {
                case "social":
                    Level = Session.GetHabbo().LevelSocial;
                    break;
                case "room_builder":
                    Level = Session.GetHabbo().LevelBuilder;
                    break;
                case "identity":
                    Level = Session.GetHabbo().LevelIdentity;
                    break;
                case "explore":
                    Level = Session.GetHabbo().LevelExplorer;
                    break;
            }

            if (GetQuestIdBy1More(Level, Type) != 0)
            {
                HandleQuest(GetQuestIdBy1More(Level, Type), Session);
            }
        }

		public ServerMessage SerializeQuestList(GameClient Session)
		{
			ServerMessage Message = new ServerMessage(800);
			Message.AppendInt32(4);

			ParseNeed(Session, Message);

			Message.AppendInt32(1);
			return Message;
		}

		public Quest GetQuest(uint Id)
		{
			foreach (Quest Quest in Quests)
			{
				if (Quest.QuestId() == Id)
				{
					return Quest;
				}
			}
			return null;
		}

		public void HandleQuest(uint Id, GameClient Session)
		{
			if (Session != null)
			{
				if (Session.GetHabbo().CurrentQuestId != Id)
				{
					Session.GetHabbo().CurrentQuestId = Id;
					Session.GetHabbo().CurrentQuestProgress = 0;
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.AddParamWithValue("uid", Session.GetHabbo().Id);
						adapter.AddParamWithValue("qid", Id);
						adapter.ExecuteQuery("UPDATE user_stats SET quest_id = @qid, quest_progress = '0' WHERE Id = @uid LIMIT 1");
					}
				}
				if (Id == 0)
				{
					Session.SendMessage(SerializeQuestList(Session));
					ServerMessage Message = new ServerMessage(803);
					Session.SendMessage(Message);
				}
				else
				{
					Quest Quest = GetQuest(Id);
					if (Quest.NeedForLevel <= Session.GetHabbo().CurrentQuestProgress)
					{
						this.CompleteQuest(Id, Session);
					}
					else
					{
						ServerMessage Message = new ServerMessage(802);
						Quest.Serialize(Message, Session, true);
						Session.SendMessage(Message);
					}
				}
			}
		}

        public void CompleteQuest(uint Id, GameClient Session)
        {
            Session.GetHabbo().CurrentQuestId = 0;
            Session.GetHabbo().LastQuestId = Id;

            using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
            {
                adapter.AddParamWithValue("userid", Session.GetHabbo().Id);
                adapter.AddParamWithValue("questid", Id);
                adapter.ExecuteQuery("UPDATE user_stats SET quest_id = '0',quest_progress = '0', lev_" + GetQuest(Id).Type.Replace("room_", "") + " = lev_" + GetQuest(Id).Type.Replace("room_", "") + " + 1 WHERE Id = @userid LIMIT 1");
                adapter.ExecuteQuery("INSERT INTO user_quests (user_id,quest_id) VALUES (@userid,@questid)");
            }

            switch (GetQuest(Id).Type.ToLower())
            {
                case "identity":
                    Session.GetHabbo().LevelIdentity++;
                    break;
                case "room_builder":
                    Session.GetHabbo().LevelBuilder++;
                    break;
                case "social":
                    Session.GetHabbo().LevelSocial++;
                    break;
                case "explore":
                    Session.GetHabbo().LevelExplorer++;
                    break;
            }
            Session.GetHabbo().LoadQuests();

            ServerMessage Message = new ServerMessage(801);
            Quest Quest = GetQuest(Id);
            Quest.Serialize(Message, Session, true);
            ParseNeed(Session, Message);
            Message.AppendInt32(1);

            Session.SendMessage(Message);
            Session.GetHabbo().ActivityPoints += Quest.PixelReward;
            Session.GetHabbo().UpdateActivityPointsBalance(true);
            Session.GetHabbo().CurrentQuestProgress = 0;
        }

		public void ParseNeed(GameClient Session, ServerMessage Message)
		{
			bool DidSocial = false;
			bool DidBuilder = false;
			bool DidId = false;
			bool DidExplorer = false;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (Quest Quest in Quests)
			{
                if (Quest.QuestId() == Session.GetHabbo().CurrentQuestId)
                {
                    Quest.Serialize(Message, Session, false);

                    switch (Quest.Type.ToLower())
                    {
                        case "social":
                            DidSocial = true;
                            break;
                        case "room_builder":
                            DidBuilder = true;
                            break;
                        case "identity":
                            DidId = true;
                            break;
                        case "explore":
                            DidExplorer = true;
                            break;
                    }
                }
				if (Quest.Type.ToLower() == "room_builder" && num2 < Session.GetHabbo().LevelBuilder)
				{
					num2 = Quest.Level;
				}
				if (Quest.Type.ToLower() == "social" && num < Session.GetHabbo().LevelSocial)
				{
					num = Quest.Level;
				}
				if (Quest.Type.ToLower() == "identity" && num3 < Session.GetHabbo().LevelIdentity)
				{
					num3 = Quest.Level;
				}
				if (Quest.Type.ToLower() == "explore" && num4 < Session.GetHabbo().LevelExplorer)
				{
					num4 = Quest.Level;
				}
				if (Quest.Type.ToLower() == "room_builder" && !DidBuilder && Quest.Level == GetHighestLevelForType("room_builder") && Session.GetHabbo().LevelBuilder == this.GetHighestLevelForType("room_builder"))
				{
					Quest.Serialize(Message, Session, false);
					DidBuilder = true;
				}
				if (Quest.Type.ToLower() == "social" && !DidSocial && Quest.Level == GetHighestLevelForType("social") && Session.GetHabbo().LevelSocial == this.GetHighestLevelForType("room_social"))
				{
					Quest.Serialize(Message, Session, false);
					DidSocial = true;
				}
				if (Quest.Type.ToLower() == "identity" && !DidId && Quest.Level == GetHighestLevelForType("identity") && Session.GetHabbo().LevelIdentity == this.GetHighestLevelForType("identity"))
				{
					Quest.Serialize(Message, Session, false);
					DidId = true;
				}
				if (Quest.Type.ToLower() == "explore" && !DidExplorer && Quest.Level == GetHighestLevelForType("explore") && Session.GetHabbo().LevelExplorer == this.GetHighestLevelForType("explore"))
				{
					Quest.Serialize(Message, Session, false);
					DidExplorer = true;
				}
				if (Quest.Type.ToLower() == "room_builder" && !DidBuilder && Quest.Level == Session.GetHabbo().LevelBuilder + 1)
				{
					Quest.Serialize(Message, Session, false);
					DidBuilder = true;
				}
				if (Quest.Type.ToLower() == "social" && !DidSocial && Quest.Level == Session.GetHabbo().LevelSocial + 1)
				{
					Quest.Serialize(Message, Session, false);
					DidSocial = true;
				}
				if (Quest.Type.ToLower() == "identity" && !DidId && Quest.Level == Session.GetHabbo().LevelIdentity + 1)
				{
					Quest.Serialize(Message, Session, false);
					DidId = true;
				}
				if (Quest.Type.ToLower() == "explore" && !DidExplorer && Quest.Level == Session.GetHabbo().LevelExplorer + 1)
				{
					Quest.Serialize(Message, Session, false);
					DidExplorer = true;
				}
			}
			if (!DidBuilder || !DidSocial || !DidId || !DidExplorer)
			{
				foreach (Quest Quest in Quests)
				{
					if (Quest.Type.ToLower() == "room_builder" && !DidBuilder && Quest.Level == num2)
					{
						Quest.Serialize(Message, Session, false);
						DidBuilder = true;
					}
					if (Quest.Type.ToLower() == "social" && !DidSocial && Quest.Level == num)
					{
						Quest.Serialize(Message, Session, false);
						DidSocial = true;
					}
					if (Quest.Type.ToLower() == "identity" && !DidId && Quest.Level == num3)
					{
						Quest.Serialize(Message, Session, false);
						DidId = true;
					}
					if (Quest.Type.ToLower() == "explore" && !DidExplorer && Quest.Level == num4)
					{
						Quest.Serialize(Message, Session, false);
						DidExplorer = true;
					}
				}
			}
		}
	}
}
