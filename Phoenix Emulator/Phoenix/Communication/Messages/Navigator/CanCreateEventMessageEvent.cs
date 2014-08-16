using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class CanCreateEventMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (Room != null && Room.CheckRights(Session, true))
			{
				bool bool_ = true;
				int int_ = 0;
				if (Room.State != 0)
				{
					bool_ = false;
					int_ = 3;
				}
				ServerMessage Message = new ServerMessage(367);
				Message.AppendBoolean(bool_);
				Message.AppendInt32(int_);
				Session.SendMessage(Message);
			}
		}
	}
}
