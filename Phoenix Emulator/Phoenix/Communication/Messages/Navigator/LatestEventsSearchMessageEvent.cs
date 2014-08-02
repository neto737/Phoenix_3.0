using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class LatestEventsSearchMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int int_ = int.Parse(Event.PopFixedString());
			Session.SendMessage(PhoenixEnvironment.GetGame().GetNavigator().SerializeEventListing(Session, int_));
		}
	}
}
