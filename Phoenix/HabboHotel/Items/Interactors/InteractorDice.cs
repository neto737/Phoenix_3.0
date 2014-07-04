using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorDice : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem Item)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			RoomUser @class = null;
			if (Session != null)
			{
				@class = Item.GetRoom().GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (@class == null)
				{
					return;
				}
			}
			if (Session == null || Item.GetRoom().method_99(Item.GetX, Item.GetY, @class.X, @class.Y))
			{
				if (Item.ExtraData != "-1")
				{
					if (Request == -1)
					{
						Item.ExtraData = "0";
						Item.method_4();
					}
					else
					{
						Item.InteractingUser = @class.HabboId;
						Item.ExtraData = "-1";
						Item.UpdateState(false, true);
						Item.ReqUpdate(4);
					}
				}
			}
			else
			{
				if (Session != null && @class != null && @class.CanWalk)
				{
					try
					{
						@class.MoveTo(Item.SquareInFront);
					}
					catch
					{
					}
				}
			}
		}
	}
}
