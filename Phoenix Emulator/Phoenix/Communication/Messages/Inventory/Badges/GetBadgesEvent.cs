using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Inventory.Badges
{
	internal sealed class GetBadgesEvent : MessageEvent
	{
        public void parse(GameClient Session, ClientMessage Event)
        {
            Session.SendMessage(Session.GetHabbo().GetBadgeComponent().Serialize());
        }
	}
}
