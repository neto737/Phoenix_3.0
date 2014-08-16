using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class CancelEventMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (Room != null && Room.CheckRights(Session, true) && Room.Event != null)
			{
				Room.Event = null;
				ServerMessage Message = new ServerMessage(370);
				Message.AppendStringWithBreak("-1");
				Room.SendMessage(Message, null);
			}
		}
	}
}
