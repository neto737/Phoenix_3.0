using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Marketplace
{
	internal class MakeOfferMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetInventoryComponent() != null)
			{
				if (Session.GetHabbo().InRoom)
				{
					Room Room = Session.GetHabbo().CurrentRoom;
					RoomUser User = Room.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (User.IsTrading)
					{
						return;
					}
				}
				int SellingPrice = Event.PopWiredInt32();
				Event.PopWiredInt32();
				uint ItemId = Event.PopWiredUInt();
				UserItem Item = Session.GetHabbo().GetInventoryComponent().GetItem(ItemId);
				if (Item != null && Item.GetBaseItem().AllowTrade)
				{
					PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().SellItem(Session, Item.Id, SellingPrice);
				}
			}
		}
	}
}
