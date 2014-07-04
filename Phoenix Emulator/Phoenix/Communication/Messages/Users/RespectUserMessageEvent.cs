using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class RespectUserMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && Session.GetHabbo().DailyRespectPoints > 0)
			{
				RoomUser class2 = @class.GetRoomUserByHabbo(Event.PopWiredUInt());
				if (class2 != null && class2.GetClient().GetHabbo().Id != Session.GetHabbo().Id && !class2.IsBot)
				{
					Session.GetHabbo().DailyRespectPoints--;
					Session.GetHabbo().RespectGiven++;
					class2.GetClient().GetHabbo().Respect++;
					using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class3.ExecuteQuery("UPDATE user_stats SET Respect = respect + 1 WHERE Id = '" + class2.GetClient().GetHabbo().Id + "' LIMIT 1");
						class3.ExecuteQuery("UPDATE user_stats SET RespectGiven = RespectGiven + 1 WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
						class3.ExecuteQuery("UPDATE user_stats SET dailyrespectpoints = dailyrespectpoints - 1 WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
					}
					ServerMessage Message = new ServerMessage(440u);
					Message.AppendUInt(class2.GetClient().GetHabbo().Id);
					Message.AppendInt32(class2.GetClient().GetHabbo().Respect);
					@class.SendMessage(Message, null);
					if (Session.GetHabbo().RespectGiven == 100)
					{
						PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 8u, 1);
					}
					int int_ = class2.GetClient().GetHabbo().Respect;
					if (int_ <= 166)
					{
						if (int_ <= 6)
						{
							if (int_ != 1)
							{
								if (int_ == 6)
								{
									PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 2);
								}
							}
							else
							{
								PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 1);
							}
						}
						else
						{
							if (int_ != 16)
							{
								if (int_ != 66)
								{
									if (int_ == 166)
									{
										PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 5);
									}
								}
								else
								{
									PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 4);
								}
							}
							else
							{
								PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 3);
							}
						}
					}
					else
					{
						if (int_ <= 566)
						{
							if (int_ != 366)
							{
								if (int_ == 566)
								{
									PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 7);
								}
							}
							else
							{
								PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 6);
							}
						}
						else
						{
							if (int_ != 766)
							{
								if (int_ != 966)
								{
									if (int_ == 1116)
									{
										PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 10);
									}
								}
								else
								{
									PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 9);
								}
							}
							else
							{
								PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class2.GetClient(), 14u, 8);
							}
						}
					}
					if (Session.GetHabbo().CurrentQuestId == 5u)
					{
						PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(5u, Session);
					}
				}
			}
		}
	}
}
