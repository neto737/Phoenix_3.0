using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class RateFlatMessageEvent : MessageEvent
	{
        public void parse(GameClient Session, ClientMessage Event)
        {
            Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (Room != null && !Session.GetHabbo().RatedRooms.Contains(Room.RoomId) && !Room.CheckRights(Session, true))
            {
                switch (Event.PopWiredInt32())
                {
                    case -1:
                        Room.Score--;
                        break;
                    case 0:
                        return;
                    case 1:
                        Room.Score++;
                        if (Session.GetHabbo().FriendStreamEnabled)
                        {
                            using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                            {
                                string look = PhoenixEnvironment.FilterInjectionChars(Session.GetHabbo().Look);
                                adapter.AddParamWithValue("look", look);
                                adapter.ExecuteQuery("INSERT INTO `friend_stream` (`id`, `type`, `userid`, `gender`, `look`, `time`, `data`) VALUES (NULL, '1', '" + Session.GetHabbo().Id + "', '" + Session.GetHabbo().Gender + "', @look, UNIX_TIMESTAMP(), '" + Session.GetHabbo().CurrentRoomId + "');");
                            }
                        }
                        break;
                    default:
                        return;
                }
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.ExecuteQuery("UPDATE rooms SET score = '" + Room.Score + "' WHERE Id = '" + Room.RoomId + "' LIMIT 1");
                }
                Session.GetHabbo().RatedRooms.Add(Room.RoomId);
                ServerMessage Message = new ServerMessage(345);
                Message.AppendInt32(Room.Score);
                Session.SendMessage(Message);
            }
        }
	}
}
