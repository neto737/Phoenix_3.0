using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Marketplace
{
	internal sealed class MakeOfferMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetInventoryComponent() != null)
			{
				if (Session.GetHabbo().InRoom)
				{
					Room class14_ = Session.GetHabbo().CurrentRoom;
					RoomUser @class = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class.IsTrading)
					{
						return;
					}
				}
				int int_ = Event.PopWiredInt32();
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				UserItem class2 = Session.GetHabbo().GetInventoryComponent().GetItem(uint_);
				if (class2 != null && class2.GetBaseItem().AllowTrade)
				{
					PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().method_1(Session, class2.Id, int_);
				}
			}
		}
	}
}
