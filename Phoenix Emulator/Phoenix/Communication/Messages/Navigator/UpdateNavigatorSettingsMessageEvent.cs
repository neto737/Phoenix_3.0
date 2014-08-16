using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class UpdateNavigatorSettingsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint HomeRoom = Event.PopWiredUInt();
            RoomData Data = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(HomeRoom);
			if (HomeRoom == 0 || (Data != null && !(Data.Owner.ToLower() != Session.GetHabbo().Username.ToLower())))
			{
				Session.GetHabbo().HomeRoom = HomeRoom;
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery("UPDATE users SET home_room = '" + HomeRoom + "' WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1;");
				}
				ServerMessage Message = new ServerMessage(455);
				Message.AppendUInt(HomeRoom);
				Session.SendMessage(Message);
			}
		}
	}
}
