using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class WiredInteractor : FurniInteractor
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
				ServerMessage Message = new ServerMessage(651);
				Message.AppendInt32(0);
				Message.AppendInt32(5);
				Message.AppendInt32(1);
				Message.AppendUInt(Item.Id);
				Message.AppendInt32(Item.GetBaseItem().Sprite);
				Message.AppendUInt(Item.Id);
				Session.SendMessage(Message);
			}
		}
	}
}
