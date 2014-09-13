using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class ApproveNameMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(36u);
			Message.AppendInt32(PhoenixEnvironment.GetGame().GetCatalog().CheckPetName(Event.PopFixedString()) ? 0 : 2);
			Session.SendMessage(Message);
		}
	}
}
