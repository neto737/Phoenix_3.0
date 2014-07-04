using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Rooms.Settings
{
	internal sealed class DeleteRoomMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Room class14_ = Session.GetHabbo().CurrentRoom;
			if (class14_ != null && (!(class14_.Owner != Session.GetHabbo().Username) || Session.GetHabbo().Rank == 7u))
			{
				PhoenixEnvironment.GetGame().GetRoomManager().method_2(num);
                RoomData @class = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(num);
				if (@class != null && (!(@class.Owner.ToLower() != Session.GetHabbo().Username.ToLower()) || Session.GetHabbo().Rank == 7u))
				{
					Room class2 = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(@class.Id);
					if (class2 != null)
					{
						for (int i = 0; i < class2.UserList.Length; i++)
						{
							RoomUser class3 = class2.UserList[i];
							if (class3 != null && !class3.IsBot)
							{
								class3.GetClient().SendMessage(new ServerMessage(18u));
								class3.GetClient().GetHabbo().method_11();
							}
						}
						PhoenixEnvironment.GetGame().GetRoomManager().UnloadRoom(class2);
					}
					using (DatabaseClient class4 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class4.ExecuteQuery("DELETE FROM rooms WHERE Id = '" + num + "' LIMIT 1");
						class4.ExecuteQuery("DELETE FROM user_favorites WHERE room_id = '" + num + "'");
						class4.ExecuteQuery("UPDATE items SET room_id = '0' WHERE room_id = '" + num + "'");
						class4.ExecuteQuery("DELETE FROM room_rights WHERE room_id = '" + num + "'");
						class4.ExecuteQuery("UPDATE users SET home_room = '0' WHERE home_room = '" + num + "'");
						class4.ExecuteQuery("UPDATE user_pets SET room_id = '0' WHERE room_id = '" + num + "'");
						Session.GetHabbo().UpdateRooms(class4);
					}
					Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
					Session.GetHabbo().GetInventoryComponent().method_3(true);
					Session.SendMessage(PhoenixEnvironment.GetGame().GetNavigator().GetDynamicNavigatorPacket(Session, -3));
				}
			}
		}
	}
}
