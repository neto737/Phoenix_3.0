using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Marketplace
{
	internal class GetOffersMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int MinCost = Event.PopWiredInt32();
			int MaxCost = Event.PopWiredInt32();
			string SearchQuery = Event.PopFixedString();
			int FilterMode = Event.PopWiredInt32();

			Session.SendMessage(PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().SerializeOffersNew(MinCost, MaxCost, SearchQuery, FilterMode));
		}
	}
}
