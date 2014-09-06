using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredGivePoints : FurniInteractor
	{
        public override void OnPlace(GameClient Session, RoomItem Item) { }
        public override void OnRemove(GameClient Session, RoomItem Item) { }

		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			if (UserHasRight)
			{
				ServerMessage Message = new ServerMessage(651);
				Message.AppendInt32(0);
				Message.AppendInt32(5);
				Message.AppendInt32(0);
				Message.AppendInt32(Item.GetBaseItem().SpriteId);
				Message.AppendUInt(Item.Id);
				Message.AppendStringWithBreak("");
				Message.AppendString("J");
				if (Item.Extra1.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(Item.Extra1));
				}
				else
				{
					Message.AppendString("QA");
				}
				if (Item.Extra2.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(Item.Extra2));
				}
				else
				{
					Message.AppendInt32(1);
				}
				Message.AppendStringWithBreak("HRAHH");
				Session.SendMessage(Message);
			}
		}
	}
}
