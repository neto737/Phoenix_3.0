using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorScoreboard : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (bool_0)
			{
				int num = 0;
				if (RoomItem_0.ExtraData.Length > 0)
				{
					num = int.Parse(RoomItem_0.ExtraData);
				}
				if (int_0 == 0)
				{
					if (num <= -1)
					{
						num = 0;
					}
					else
					{
						if (num >= 0)
						{
							num = -1;
						}
					}
				}
				else
				{
					if (int_0 >= 1)
					{
						if (int_0 == 1)
						{
							num--;
							if (num < 0)
							{
								num = 0;
							}
						}
						else
						{
							if (int_0 == 2)
							{
								num++;
								if (num >= 100)
								{
									num = 0;
								}
							}
						}
					}
				}
				RoomItem_0.ExtraData = num.ToString();
				RoomItem_0.UpdateState();
			}
		}
	}
}
