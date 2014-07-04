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
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				bool bool_ = true;
				int int_ = 0;
				if (@class.State != 0)
				{
					bool_ = false;
					int_ = 3;
				}
				ServerMessage Message = new ServerMessage(367u);
				Message.AppendBoolean(bool_);
				Message.AppendInt32(int_);
				Session.SendMessage(Message);
			}
		}
	}
}
