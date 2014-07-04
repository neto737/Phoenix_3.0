using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal abstract class FurniInteractor
	{
		public abstract void OnPlace(GameClient Session, RoomItem Item);
		public abstract void OnRemove(GameClient Session, RoomItem Item);
		public abstract void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight);
	}
}
