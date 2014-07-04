using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Inventory.Trading
{
	internal sealed class RemoveItemFromTradeEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.Boolean_2)
			{
				Trade class2 = @class.method_76(Session.GetHabbo().Id);
				UserItem class3 = Session.GetHabbo().GetInventoryComponent().GetItem(Event.PopWiredUInt());
				if (class2 != null && class3 != null)
				{
					class2.method_3(Session.GetHabbo().Id, class3);
				}
			}
		}
	}
}
