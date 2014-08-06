using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorDefault : FurniInteractor
	{
		private int Modes;
		public InteractorDefault(int Modes)
		{
			this.Modes = Modes - 1;
			if (this.Modes < 0)
			{
				this.Modes = 0;
			}
		}
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem Item)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			if (this.Modes != 0 && (UserHasRight || Item.GetBaseItem().InteractionType.ToLower() == "switch"))
			{
				if (Item.GetBaseItem().InteractionType.ToLower() == "switch" && Session != null)
				{
					RoomUser @class = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class.Coordinate.X - Item.SquareInFront.X > 1 || @class.Coordinate.Y - Item.SquareInFront.Y > 1)
					{
						if (@class.CanWalk)
						{
							@class.MoveTo(Item.Coordinate);
							return;
						}
						return;
					}
				}
				int num = 0;
				if (Item.ExtraData.Length > 0)
				{
					num = int.Parse(Item.ExtraData);
				}
				int num2;
				if (num <= 0)
				{
					num2 = 1;
				}
				else
				{
					if (num >= this.Modes)
					{
						num2 = 0;
					}
					else
					{
						num2 = num + 1;
					}
				}
				if (Item.GetBaseItem().Name.Contains("jukebox"))
				{
					ServerMessage Message = new ServerMessage(327u);
					if (num2 == 1)
					{
						Message.AppendInt32(7);
						Message.AppendInt32(6);
						Message.AppendInt32(7);
						Message.AppendInt32(0);
						Message.AppendInt32(0);
						Item.int_0 = 1;
						Item.TimerRunning = true;
						Item.bool_1 = true;
					}
					else
					{
						Message.AppendInt32(-1);
						Message.AppendInt32(-1);
						Message.AppendInt32(-1);
						Message.AppendInt32(-1);
						Message.AppendInt32(0);
						Item.int_0 = 0;
						Item.TimerRunning = false;
						Item.GetRoom().int_13 = 0;
					}
					Item.GetRoom().SendMessage(Message, null);
				}
				double double_ = Item.Double_1;
				Item.ExtraData = num2.ToString();
				Item.UpdateState();
				if (double_ != Item.Double_1)
				{
					Dictionary<int, AffectedTile> dictionary = Item.Dictionary_0;
					if (dictionary == null)
					{
						dictionary = new Dictionary<int, AffectedTile>();
					}
					Item.GetRoom().UpdateUserStatus(Item.GetRoom().GetUserForSquare(Item.GetX, Item.GetY), true, false);
					foreach (AffectedTile current in dictionary.Values)
					{
						Item.GetRoom().UpdateUserStatus(Item.GetRoom().GetUserForSquare(current.X, current.Y), true, false);
					}
				}
				if (Session != null)
				{
					RoomUser RoomUser_ = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
					Item.GetRoom().method_10(RoomUser_, Item);
				}
			}
		}
	}
}
