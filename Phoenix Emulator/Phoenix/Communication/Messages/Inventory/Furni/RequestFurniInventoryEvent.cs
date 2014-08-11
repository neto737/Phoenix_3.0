using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Inventory.Furni
{
	internal sealed class RequestFurniInventoryEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null)
			{
				Session.SendMessage(Session.GetHabbo().GetInventoryComponent().SerializeFloorItemInventory());
			}
		}
	}
}
