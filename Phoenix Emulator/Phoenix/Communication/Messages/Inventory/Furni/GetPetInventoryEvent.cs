using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Inventory.Furni
{
	internal sealed class GetPetInventoryEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetInventoryComponent() != null)
			{
				Session.SendMessage(Session.GetHabbo().GetInventoryComponent().SerializePetInventory());
			}
		}
	}
}
