using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorGate : FurniInteractor
	{
		private int Modes;
		public InteractorGate(int Modes)
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
			if (UserHasRight)
			{
				if (this.Modes == 0)
				{
					Item.UpdateState(false, true);
				}
				int num = 0;
				int num2 = 0;
				if (Item.ExtraData.Length > 0)
				{
					num = int.Parse(Item.ExtraData);
				}
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
				if (num2 == 0)
				{
					if (Item.GetRoom().method_97(Item.GetX, Item.GetY))
					{
						return;
					}
					Dictionary<int, AffectedTile> dictionary = Item.GetRoom().GetAffectedTiles(Item.GetBaseItem().Length, Item.GetBaseItem().Width, Item.GetX, Item.GetY, Item.Rot);
					if (dictionary == null)
					{
						dictionary = new Dictionary<int, AffectedTile>();
					}
					foreach (AffectedTile current in dictionary.Values)
					{
						if (Item.GetRoom().method_97(current.X, current.Y))
						{
							return;
						}
					}
				}
				Item.ExtraData = num2.ToString();
				Item.method_4();
				Item.GetRoom().GenerateMaps();
			}
		}
	}
}
