using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Items.Interactors
{
	internal sealed class InteractorBanzaiScoreCounter : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
            //if (Item.team == Team.none)
            //    return;

            //Item.ExtraData = Item.GetRoom().GetGameManager()
		}

        public override void OnRemove(GameClient Session, RoomItem Item) { }

		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRight)
		{
			if (UserHasRight)
			{
				int num = 0;
				if (Item.ExtraData.Length > 0)
				{
					num = int.Parse(Item.ExtraData);
				}
				if (Request == 0)
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
					if (Request >= 1)
					{
						if (Request == 1)
						{
							if (!Item.TimerRunning)
							{
								Item.TimerRunning = true;
								Item.ReqUpdate(1);
								if (Session != null)
								{
									RoomUser RoomUser_ = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
									Item.GetRoom().method_14(RoomUser_);
								}
							}
							else
							{
								Item.TimerRunning = false;
							}
						}
						else
						{
							if (Request == 2)
							{
								num += 60;
								if (num >= 600)
								{
									num = 0;
								}
							}
						}
					}
				}
				Item.ExtraData = num.ToString();
				Item.UpdateState(true, true);
			}
		}
	}
}
