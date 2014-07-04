using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Marketplace
{
	internal sealed class GetMarketplaceCanMakeOfferEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(611u);
			Message.AppendBoolean(true);
			Message.AppendInt32(2);
			Session.SendMessage(Message);
		}
	}
}
