using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorNotUsed : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (RoomItem_0.GetBaseItem().Height_Adjustable.Count > 1)
			{
				Dictionary<int, AffectedTile> dictionary = RoomItem_0.GetRoom().GetAffectedTiles(RoomItem_0.GetBaseItem().Length, RoomItem_0.GetBaseItem().Width, RoomItem_0.GetX, RoomItem_0.GetY, RoomItem_0.Rot);
				RoomItem_0.GetRoom().GenerateMaps();
				RoomItem_0.GetRoom().UpdateUserStatus(RoomItem_0.GetRoom().GetUserForSquare(RoomItem_0.GetX, RoomItem_0.GetY), true, true);
				foreach (AffectedTile current in dictionary.Values)
				{
					RoomItem_0.GetRoom().UpdateUserStatus(RoomItem_0.GetRoom().GetUserForSquare(current.X, current.Y), true, true);
				}
			}
			if (Session != null)
			{
				RoomUser RoomUser_ = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
				RoomItem_0.GetRoom().method_10(RoomUser_, RoomItem_0);
			}
		}
	}
}