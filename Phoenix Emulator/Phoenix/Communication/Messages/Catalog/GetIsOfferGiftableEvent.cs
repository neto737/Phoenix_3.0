using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Catalogs;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Catalog
{
	internal class GetIsOfferGiftableEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint Id = Event.PopWiredUInt();

			CatalogItem Item = PhoenixEnvironment.GetGame().GetCatalog().FindItem(Id);
			if (Item != null)
			{
				ServerMessage Message = new ServerMessage(622);
				Message.AppendUInt(Item.Id);
				Message.AppendBoolean(Item.GetBaseItem().AllowGift);
				Session.SendMessage(Message);
			}
		}
	}
}
