using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Catalog
{
	internal class PurchaseFromCatalogAsGiftEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int PageId = Event.PopWiredInt32();
			uint ItemId = Event.PopWiredUInt();
			string ExtraData = Event.PopFixedString();
			string GiftUser = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
			string GiftMessage = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
			PhoenixEnvironment.GetGame().GetCatalog().HandlePurchase(Session, PageId, ItemId, ExtraData, true, GiftUser, GiftMessage, true);
		}
	}
}
