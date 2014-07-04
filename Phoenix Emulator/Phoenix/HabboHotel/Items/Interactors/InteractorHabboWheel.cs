using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorHabboWheel : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
			Item.ExtraData = "-1";
			Item.ReqUpdate(10);
		}
		public override void OnRemove(GameClient Session, RoomItem Item)
		{
			Item.ExtraData = "-1";
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			if (UserHasRight && Item.ExtraData != "-1")
			{
				Item.ExtraData = "-1";
				Item.method_4();
				Item.ReqUpdate(10);
			}
		}
	}
}
