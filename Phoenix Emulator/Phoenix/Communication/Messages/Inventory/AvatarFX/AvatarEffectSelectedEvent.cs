using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Inventory.AvatarFX
{
	internal sealed class AvatarEffectSelectedEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(Event.PopWiredInt32(), false);
		}
	}
}
