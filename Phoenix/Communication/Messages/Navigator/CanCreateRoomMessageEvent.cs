using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class CanCreateRoomMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(512u);
			if (Session.GetHabbo().list_6.Count > GlobalClass.MaxRoomsPerUser)
			{
				Message.AppendBoolean(true);
				Message.AppendInt32(GlobalClass.MaxRoomsPerUser);
			}
			else
			{
				Message.AppendBoolean(false);
			}
			Session.SendMessage(Message);
		}
	}
}
