using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredGivePoints : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (bool_0)
			{
				ServerMessage Message = new ServerMessage(651u);
				Message.AppendInt32(0);
				Message.AppendInt32(5);
				Message.AppendInt32(0);
				Message.AppendInt32(RoomItem_0.GetBaseItem().SpriteId);
				Message.AppendUInt(RoomItem_0.Id);
				Message.AppendStringWithBreak("");
				Message.AppendString("J");
				if (RoomItem_0.Extra1.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(RoomItem_0.Extra1));
				}
				else
				{
					Message.AppendString("QA");
				}
				if (RoomItem_0.Extra2.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(RoomItem_0.Extra2));
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
