using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Pathfinding;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorPuzzleBox : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
		{
			Room room = Item.GetRoom();
			RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (roomUserByHabbo != null && room != null)
			{
				Coord coord = new Coord(Item.GetX + 1, Item.GetY);
				Coord coord2 = new Coord(Item.GetX - 1, Item.GetY);
				Coord coord3 = new Coord(Item.GetX, Item.GetY + 1);
				Coord coord4 = new Coord(Item.GetX, Item.GetY - 1);
				if ((roomUserByHabbo.Coordinate != coord) && (roomUserByHabbo.Coordinate != coord2) && (roomUserByHabbo.Coordinate != coord3) && (roomUserByHabbo.Coordinate != coord4))
				{
					if (roomUserByHabbo.CanWalk)
					{
						roomUserByHabbo.MoveTo(Item.Coordinate);
					}
				}
				else
				{
					int num = Item.GetX;
					int num2 = Item.GetY;
					if (roomUserByHabbo.Coordinate == coord)
					{
						num = Item.GetX - 1;
						num2 = Item.GetY;
					}
					else
					{
						if (roomUserByHabbo.Coordinate == coord2)
						{
							num = Item.GetX + 1;
							num2 = Item.GetY;
						}
						else
						{
							if (roomUserByHabbo.Coordinate == coord3)
							{
								num = Item.GetX;
								num2 = Item.GetY - 1;
							}
							else
							{
								if (roomUserByHabbo.Coordinate == coord4)
								{
									num = Item.GetX;
									num2 = Item.GetY + 1;
								}
							}
						}
					}
					if (room.method_37(num, num2, true, true, true, true, false))
					{
						List<RoomItem> list_ = new List<RoomItem>();
						list_ = room.method_93(num, num2);
						double double_ = room.method_84(num, num2, list_);
						ServerMessage Message = new ServerMessage(230u);
						Message.AppendInt32(Item.GetX);
						Message.AppendInt32(Item.GetY);
						Message.AppendInt32(num);
						Message.AppendInt32(num2);
						Message.AppendInt32(1);
						Message.AppendUInt(Item.Id);
						Message.AppendByte(2);
						Message.AppendStringWithBreak(double_.ToString());
						Message.AppendString("M");
						room.SendMessage(Message, null);
						room.method_81(Item, num, num2, double_);
					}
				}
			}
		}
	}
}
