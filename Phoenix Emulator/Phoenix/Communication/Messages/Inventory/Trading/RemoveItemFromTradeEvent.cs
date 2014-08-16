using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Inventory.Trading
{
	internal class RemoveItemFromTradeEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (Room != null && Room.CanTradeInRoom)
			{
				Trade Trade = Room.GetUserTrade(Session.GetHabbo().Id);
				UserItem User = Session.GetHabbo().GetInventoryComponent().GetItem(Event.PopWiredUInt());
				if (Trade != null && User != null)
				{
					Trade.TakeBackItem(Session.GetHabbo().Id, User);
				}
			}
		}
	}
}
