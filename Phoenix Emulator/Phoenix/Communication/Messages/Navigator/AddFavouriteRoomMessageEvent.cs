using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class AddFavouriteRoomMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint Id = Event.PopWiredUInt();
            RoomData Data = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(Id);
			if (Data == null || Session.GetHabbo().FavoriteRooms.Count >= 30 || Session.GetHabbo().FavoriteRooms.Contains(Id) || Data.Type == "public")
			{
				ServerMessage Message = new ServerMessage(33);
				Message.AppendInt32(-9001);
				Session.SendMessage(Message);
			}
			else
			{
				ServerMessage Message2 = new ServerMessage(459);
				Message2.AppendUInt(Id);
				Message2.AppendBoolean(true);
				Session.SendMessage(Message2);
				Session.GetHabbo().FavoriteRooms.Add(Id);
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery("INSERT INTO user_favorites (user_id,room_id) VALUES ('" + Session.GetHabbo().Id + "','" + Id + "')");
				}
			}
		}
	}
}
