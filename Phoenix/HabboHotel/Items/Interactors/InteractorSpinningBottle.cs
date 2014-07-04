using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorSpinningBottle : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
            RoomItem_0.ExtraData = "0";
            RoomItem_0.UpdateState(true, false);
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
            RoomItem_0.ExtraData = "0";
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
            if (RoomItem_0.ExtraData != "-1")
			{
                RoomItem_0.ExtraData = "-1";
                RoomItem_0.UpdateState(false, true);
				RoomItem_0.ReqUpdate(3);
			}
		}
	}
}
