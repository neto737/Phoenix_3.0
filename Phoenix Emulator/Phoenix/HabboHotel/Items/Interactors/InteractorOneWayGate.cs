using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Pathfinding;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorOneWayGate : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
			Item.ExtraData = "0";
			if (Item.InteractingUser != 0u)
			{
				RoomUser @class = Item.GetRoom().GetRoomUserByHabbo(Item.InteractingUser);
				if (@class != null)
				{
					@class.ClearMovement(true);
					@class.UnlockWalking();
				}
				Item.InteractingUser = 0u;
			}
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
			if (RoomItem_0.InteractingUser != 0u)
			{
				RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(RoomItem_0.InteractingUser);
				if (@class != null)
				{
					@class.ClearMovement(true);
					@class.UnlockWalking();
				}
				RoomItem_0.InteractingUser = 0u;
			}
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int int_0, bool bool_0)
		{
			RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (roomUserByHabbo != null && (Item.SquareBehind.X < Item.GetRoom().Model.MapSizeX && Item.SquareBehind.Y < Item.GetRoom().Model.MapSizeY))
			{
				if ((roomUserByHabbo.Coordinate != Item.SquareInFront) && roomUserByHabbo.CanWalk)
				{
					roomUserByHabbo.MoveTo(Item.SquareInFront);
				}
				else
				{
					if (Item.GetRoom().CanWalk(Item.SquareBehind.X, Item.SquareBehind.Y, Item.Double_0, true, false) && Item.InteractingUser == 0u)
					{
						Item.InteractingUser = roomUserByHabbo.HabboId;
						roomUserByHabbo.CanWalk = false;
						if (roomUserByHabbo.IsWalking && (roomUserByHabbo.GoalX != Item.SquareInFront.X || roomUserByHabbo.GoalY != Item.SquareInFront.Y))
						{
							roomUserByHabbo.ClearMovement(true);
						}
						roomUserByHabbo.AllowOverride = true;
						roomUserByHabbo.MoveTo(Item.Coordinate);
						Item.ReqUpdate(3);
					}
				}
			}
		}
	}
}
