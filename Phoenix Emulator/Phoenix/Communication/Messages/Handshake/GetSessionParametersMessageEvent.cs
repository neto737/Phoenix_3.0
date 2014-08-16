using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Handshake
{
	internal class GetSessionParametersMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(257);
			Message.AppendInt32(0);
			Session.SendMessage(Message);
		}
	}
}
