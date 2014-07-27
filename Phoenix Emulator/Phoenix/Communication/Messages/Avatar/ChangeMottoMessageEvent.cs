using System;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Avatar
{
	internal class ChangeMottoMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			string text = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
			if (text.Length <= 50 && !(text != ChatCommandHandler.ApplyWordFilter(text)) && !(text == Session.GetHabbo().Motto))
			{
				Session.GetHabbo().Motto = text;
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.AddParamWithValue("motto", text);
					adapter.ExecuteQuery("UPDATE users SET motto = @motto WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
				}
				if (Session.GetHabbo().CurrentQuestId == 17)
				{
					PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(17, Session);
				}
				ServerMessage message = new ServerMessage(484);
				message.AppendInt32(-1);
				message.AppendStringWithBreak(Session.GetHabbo().Motto);
				Session.SendMessage(message);
				if (Session.GetHabbo().InRoom)
				{
					Room currentRoom = Session.GetHabbo().CurrentRoom;
					if (currentRoom == null)
					{
						return;
					}
					RoomUser roomUserByHabbo = currentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (roomUserByHabbo == null)
					{
						return;
					}
					ServerMessage message2 = new ServerMessage(266);
					message2.AppendInt32(roomUserByHabbo.VirtualId);
					message2.AppendStringWithBreak(Session.GetHabbo().Look);
					message2.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower()); 
					message2.AppendStringWithBreak(Session.GetHabbo().Motto);
					message2.AppendInt32(Session.GetHabbo().AchievementScore);
					message2.AppendStringWithBreak("");
					currentRoom.SendMessage(message2, null);
				}
				PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 5, 1);
                if (Session.GetHabbo().FriendStreamEnabled)
                {
                    using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        class2.AddParamWithValue("motto", text);
                        string look = PhoenixEnvironment.FilterInjectionChars(Session.GetHabbo().Look);
                        class2.AddParamWithValue("look", look);
                        class2.ExecuteQuery("INSERT INTO `friend_stream` (`id`, `type`, `userid`, `gender`, `look`, `time`, `data`) VALUES (NULL, '3', '" + Session.GetHabbo().Id + "', '" + Session.GetHabbo().Gender + "', @look, UNIX_TIMESTAMP(), @motto);");
                    }
                }
			}
		}
	}
}
