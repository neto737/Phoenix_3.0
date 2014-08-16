using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class LatestEventsSearchMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int Category = int.Parse(Event.PopFixedString());
			Session.SendMessage(PhoenixEnvironment.GetGame().GetNavigator().SerializeEventListing(Session, Category));
		}
	}
}
