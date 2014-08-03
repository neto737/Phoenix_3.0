using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredOnSay : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem Item)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			if (UserHasRight)
			{
				ServerMessage Message = new ServerMessage(650);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(Item.GetBaseItem().SpriteId);
				Message.AppendUInt(Item.Id);
				Message.AppendString(Item.Extra1);
				Session.SendMessage(Message);
			}
		}
	}
}
