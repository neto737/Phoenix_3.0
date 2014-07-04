using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Inventory.Purse
{
	internal sealed class GetCreditsInfoEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Session.GetHabbo().UpdateCreditsBalance(false);
			Session.GetHabbo().UpdateActivityPointsBalance(false);
		}
	}
}
