using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredTriggerTimer : FurniInteractor
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
				Message.AppendInt32(5);
				Message.AppendInt32(0);
				Message.AppendInt32(Item.GetBaseItem().Sprite);
				Message.AppendUInt(Item.Id);
				Message.AppendStringWithBreak("");
				Message.AppendString("I");
				if (Item.Extra2.Length > 0)
				{
					Message.AppendString(Item.Extra2);
				}
				else
				{
					Message.AppendString("RB");
				}
				Message.AppendStringWithBreak("HRAH");
				Session.SendMessage(Message);
				Item.ReqUpdate(1);
			}
		}
	}
}
