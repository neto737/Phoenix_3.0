using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredMatchFurni : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem Item)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
		{
			if (UserHasRights && Session != null)
			{
				Item.method_9();
				ServerMessage Message = new ServerMessage(651u);
				Message.AppendInt32(0);
				Message.AppendInt32(5);
				if (Item.Extra4.Length > 0)
				{
					Message.AppendString(Item.Extra4);
				}
				else
				{
					Message.AppendInt32(0);
				}
				Message.AppendInt32(Item.GetBaseItem().Sprite);
				Message.AppendUInt(Item.Id);
				Message.AppendStringWithBreak("");
				Message.AppendString("K");
				if (Item.Extra2.Length > 0)
				{
					Message.AppendString(Item.Extra2);
				}
				else
				{
					Message.AppendString("HHH");
				}
				Message.AppendString("IK");
				if (Item.Extra5.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(Item.Extra5));
				}
				else
				{
					Message.AppendInt32(0);
				}
				Message.AppendStringWithBreak("H");
				Session.SendMessage(Message);
			}
		}
	}
}
