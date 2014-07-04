using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Catalog
{
	internal sealed class PurchaseFromCatalogAsGiftEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int int_ = Event.PopWiredInt32();
			uint uint_ = Event.PopWiredUInt();
			string string_ = Event.PopFixedString();
			string string_2 = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
			string string_3 = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
			PhoenixEnvironment.GetGame().GetCatalog().HandlePurchase(Session, int_, uint_, string_, true, string_2, string_3, true);
		}
	}
}
