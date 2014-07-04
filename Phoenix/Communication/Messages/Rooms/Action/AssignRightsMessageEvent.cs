using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Rooms.Action
{
	internal sealed class AssignRightsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(num);
			if (room != null && room.CheckRights(Session, true) && roomUserByHabbo != null && !roomUserByHabbo.IsBot && !room.UsersWithRights.Contains(num))
			{
				room.UsersWithRights.Add(num);
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO room_rights (room_id,user_id) VALUES ('",
						room.RoomId,
						"','",
						num,
						"')"
					}));
				}
				ServerMessage Message = new ServerMessage(510);
				Message.AppendUInt(room.RoomId);
				Message.AppendUInt(num);
				Message.AppendStringWithBreak(roomUserByHabbo.GetClient().GetHabbo().Username);
				Session.SendMessage(Message);
				roomUserByHabbo.AddStatus("flatctrl", "");
				roomUserByHabbo.UpdateNeeded = true;
				roomUserByHabbo.GetClient().SendMessage(new ServerMessage(42));
			}
		}
	}
}
