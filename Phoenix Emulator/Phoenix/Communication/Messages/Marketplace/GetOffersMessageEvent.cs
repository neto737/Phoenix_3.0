using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Marketplace
{
	internal sealed class GetOffersMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int int_ = Event.PopWiredInt32();
			int int_2 = Event.PopWiredInt32();
			string string_ = Event.PopFixedString();
			int int_3 = Event.PopWiredInt32();
			Session.SendMessage(PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().method_5(int_, int_2, string_, int_3));
		}
	}
}
