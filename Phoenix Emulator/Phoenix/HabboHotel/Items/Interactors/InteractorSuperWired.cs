using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorSuperWired : FurniInteractor
	{
        public override void OnPlace(GameClient Session, RoomItem Item) { }
        public override void OnRemove(GameClient Session, RoomItem Item) { }

		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			if (UserHasRight)
			{
				ServerMessage Message = new ServerMessage(651);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(Item.GetBaseItem().SpriteId);
				Message.AppendUInt(Item.Id);
				Message.AppendStringWithBreak(Item.Extra1);
				Message.AppendStringWithBreak("HHSAHH");
				Session.SendMessage(Message);
			}
		}
	}
}
