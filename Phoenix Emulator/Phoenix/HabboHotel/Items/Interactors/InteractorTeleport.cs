using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Pathfinding;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorTeleport : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
			if (RoomItem_0.InteractingUser != 0u)
			{
				RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(RoomItem_0.InteractingUser);
				if (@class != null)
				{
					@class.ClearMovement(true);
					@class.AllowOverride = false;
					@class.CanWalk = true;
				}
				RoomItem_0.InteractingUser = 0u;
			}
			if (RoomItem_0.InteractingUser2 != 0u)
			{
				RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(RoomItem_0.InteractingUser2);
				if (@class != null)
				{
					@class.ClearMovement(true);
					@class.AllowOverride = false;
					@class.CanWalk = true;
				}
				RoomItem_0.InteractingUser2 = 0u;
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
					@class.UnlockWalking();
				}
				RoomItem_0.InteractingUser = 0u;
			}
			if (RoomItem_0.InteractingUser2 != 0u)
			{
				RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(RoomItem_0.InteractingUser2);
				if (@class != null)
				{
					@class.UnlockWalking();
				}
				RoomItem_0.InteractingUser2 = 0u;
			}
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int int_0, bool bool_0)
		{
			RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (roomUserByHabbo != null && roomUserByHabbo.class34_1 == null)
			{
				if ((roomUserByHabbo.Coordinate == Item.Coordinate) || (roomUserByHabbo.Coordinate == Item.SquareInFront))
				{
					if (Item.InteractingUser == 0u)
					{
						roomUserByHabbo.TeleDelay = -1;
						Item.InteractingUser = roomUserByHabbo.GetClient().GetHabbo().Id;
						roomUserByHabbo.Item = Item;
					}
				}
				else
				{
					if (roomUserByHabbo.CanWalk)
					{
						try
						{
							roomUserByHabbo.MoveTo(Item.SquareInFront);
						}
						catch
						{
						}
					}
				}
			}
		}
	}
}
