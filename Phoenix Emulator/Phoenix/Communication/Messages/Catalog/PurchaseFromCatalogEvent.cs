using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Catalog
{
    internal class PurchaseFromCatalogEvent : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Event)
        {
            int pageId = Event.PopWiredInt32();
            uint itemId = Event.PopWiredUInt();
            string extraData = Event.PopFixedString();

            if (Session.GetHabbo().BuyCount <= 1)
            {
                PhoenixEnvironment.GetGame().GetCatalog().HandlePurchase(Session, pageId, itemId, extraData, false, "", "", true);
            }
            else
            {
                for (int i = 0; i < Session.GetHabbo().BuyCount; i++)
                {
                    if (!PhoenixEnvironment.GetGame().GetCatalog().HandlePurchase(Session, pageId, itemId, extraData, false, "", "", i == 0))
                    {
                        break;
                    }
                }
                Session.GetHabbo().BuyCount = 1;
            }
        }
    }
}
