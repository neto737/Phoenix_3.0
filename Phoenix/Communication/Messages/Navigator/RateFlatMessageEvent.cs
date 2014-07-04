using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class RateFlatMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && !Session.GetHabbo().RatedRooms.Contains(@class.RoomId) && !@class.CheckRights(Session, true))
			{
				switch (Event.PopWiredInt32())
				{
				case -1:
					@class.Score--;
					break;
				case 0:
					return;
				case 1:
					@class.Score++;
                    if (Session.GetHabbo().FriendStreamEnabled)
                    {
                        using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
                        {
                            string look = PhoenixEnvironment.FilterInjectionChars(Session.GetHabbo().Look);
                            class2.AddParamWithValue("look", look);
                            class2.ExecuteQuery("INSERT INTO `friend_stream` (`id`, `type`, `userid`, `gender`, `look`, `time`, `data`) VALUES (NULL, '1', '" + Session.GetHabbo().Id + "', '" + Session.GetHabbo().Gender + "', @look, UNIX_TIMESTAMP(), '" + Session.GetHabbo().CurrentRoomId + "');");
                        }
                    }
					break;
				default:
					return;
				}
				using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE rooms SET score = '",
						@class.Score,
						"' WHERE Id = '",
						@class.RoomId,
						"' LIMIT 1"
					}));
				}
				Session.GetHabbo().RatedRooms.Add(@class.RoomId);
				ServerMessage Message = new ServerMessage(345u);
				Message.AppendInt32(@class.Score);
				Session.SendMessage(Message);
			}
		}
	}
}
