using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredTriggerState : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem Item)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			if (UserHasRight && Session != null)
			{
				Item.method_10();
				ServerMessage Message = new ServerMessage(651);
				Message.AppendInt32(0);
				Message.AppendInt32(5);
				if (Item.Extra1.Length > 0)
				{
					Message.AppendString(Item.Extra1);
				}
				else
				{
					Message.AppendInt32(0);
				}
				Message.AppendInt32(Item.GetBaseItem().SpriteId);
				Message.AppendUInt(Item.Id);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendStringWithBreak("");
				Session.SendMessage(Message);
			}
		}
	}
}
