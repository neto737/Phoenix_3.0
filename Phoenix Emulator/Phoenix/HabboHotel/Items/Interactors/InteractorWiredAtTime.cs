using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredAtTime : FurniInteractor
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
				ServerMessage Message = new ServerMessage(650u);
				Message.AppendInt32(0);
				Message.AppendInt32(5);
				Message.AppendInt32(0);
				Message.AppendInt32(RoomItem_0.GetBaseItem().SpriteId);
				Message.AppendUInt(RoomItem_0.Id);
				Message.AppendStringWithBreak("");
				Message.AppendString("I");
				if (RoomItem_0.Extra2.Length > 0)
				{
					Message.AppendString(RoomItem_0.Extra2);
				}
				else
				{
					Message.AppendString("I");
				}
				Message.AppendStringWithBreak("IKH");
				Session.SendMessage(Message);
			}
		}
	}
}
