using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorAlert : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
			Item.ExtraData = "0";
		}
		public override void OnRemove(GameClient Session, RoomItem Item)
		{
			Item.ExtraData = "0";
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			if (UserHasRight && Item.ExtraData == "0")
			{
				Item.ExtraData = "1";
				Item.UpdateState(false, true);
				Item.ReqUpdate(4);
			}
		}
	}
}
