using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Catalogs;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Catalog
{
	internal sealed class GetIsOfferGiftableEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			CatalogItem @class = PhoenixEnvironment.GetGame().GetCatalog().method_2(uint_);
			if (@class != null)
			{
				ServerMessage Message = new ServerMessage(622u);
				Message.AppendUInt(@class.Id);
				Message.AppendBoolean(@class.GetBaseItem().AllowGift);
				Session.SendMessage(Message);
			}
		}
	}
}
