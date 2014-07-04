using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorFootball : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem Item)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			if (Session != null)
			{
				RoomUser @class = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
				Room class2 = Item.GetRoom();
				if (Item.GetRoom().method_99(Item.GetX, Item.GetY, @class.X, @class.Y))
				{
					Item.GetRoom().method_10(@class, Item);
					int num = Item.GetX;
					int num2 = Item.GetY;
					Item.ExtraData = "11";
					if (@class.RotBody == 4)
					{
						num2--;
					}
					else
					{
						if (@class.RotBody == 0)
						{
							num2++;
						}
						else
						{
							if (@class.RotBody == 6)
							{
								num++;
							}
							else
							{
								if (@class.RotBody == 2)
								{
									num--;
								}
								else
								{
									if (@class.RotBody == 3)
									{
										num--;
										num2--;
									}
									else
									{
										if (@class.RotBody == 1)
										{
											num--;
											num2++;
										}
										else
										{
											if (@class.RotBody == 7)
											{
												num++;
												num2++;
											}
											else
											{
												if (@class.RotBody == 5)
												{
													num++;
													num2--;
												}
											}
										}
									}
								}
							}
						}
					}
					@class.MoveTo(Item.GetX, Item.GetY);
					class2.method_79(null, Item, num, num2, 0, false, true, true);
				}
			}
		}
	}
}
