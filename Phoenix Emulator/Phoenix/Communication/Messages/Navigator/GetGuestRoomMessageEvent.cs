using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class GetGuestRoomMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			bool bool_ = Event.PopWiredBoolean();
			bool flag = Event.PopWiredBoolean();
            RoomData @class = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(uint_);
			if (@class != null)
			{
				ServerMessage Message = new ServerMessage(454u);
				Message.AppendBoolean(bool_);
				@class.method_3(Message, false, flag);
				Message.AppendBoolean(flag);
				Message.AppendBoolean(bool_);
				Session.SendMessage(Message);
			}
		}
	}
}
