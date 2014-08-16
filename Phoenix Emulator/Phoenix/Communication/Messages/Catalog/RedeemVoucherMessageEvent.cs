using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Catalog
{
	internal class RedeemVoucherMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			PhoenixEnvironment.GetGame().GetCatalog().GetVoucherHandler().TryRedeemVoucher(Session, Event.PopFixedString());
		}
	}
}
