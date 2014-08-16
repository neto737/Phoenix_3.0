using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class GetGuestRoomMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint RoomId = Event.PopWiredUInt();
			bool unk = Event.PopWiredBoolean();
			bool unk2 = Event.PopWiredBoolean();

            RoomData Data = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
			if (Data != null)
			{
				ServerMessage Message = new ServerMessage(454);
				Message.AppendBoolean(unk);
				Data.Serialize(Message, false, unk2);
				Message.AppendBoolean(unk2);
				Message.AppendBoolean(unk);
				Session.SendMessage(Message);
			}
		}
	}
}
