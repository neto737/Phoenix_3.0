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
				RoomUser roomUserByHabbo = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
				Room room = Item.GetRoom();
				if (Item.GetRoom().method_99(Item.GetX, Item.GetY, roomUserByHabbo.X, roomUserByHabbo.Y))
				{
					Item.GetRoom().method_10(roomUserByHabbo, Item);
					int num = Item.GetX;
					int num2 = Item.GetY;
					Item.ExtraData = "11";
					if (roomUserByHabbo.RotBody == 4)
					{
						num2--;
					}
					else
					{
						if (roomUserByHabbo.RotBody == 0)
						{
							num2++;
						}
						else
						{
							if (roomUserByHabbo.RotBody == 6)
							{
								num++;
							}
							else
							{
								if (roomUserByHabbo.RotBody == 2)
								{
									num--;
								}
								else
								{
									if (roomUserByHabbo.RotBody == 3)
									{
										num--;
										num2--;
									}
									else
									{
										if (roomUserByHabbo.RotBody == 1)
										{
											num--;
											num2++;
										}
										else
										{
											if (roomUserByHabbo.RotBody == 7)
											{
												num++;
												num2++;
											}
											else
											{
												if (roomUserByHabbo.RotBody == 5)
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
					roomUserByHabbo.MoveTo(Item.GetX, Item.GetY);
					room.method_79(null, Item, num, num2, 0, false, true, true);
				}
			}
		}
	}
}
