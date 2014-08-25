using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class MoveAvatarMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room Room = Session.GetHabbo().CurrentRoom;
			if (Room != null)
			{
				RoomUser @class = Room.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (@class != null && @class.CanWalk)
				{
					int num = Event.PopWiredInt32();
					int num2 = Event.PopWiredInt32();
					if (num != @class.X || num2 != @class.Y)
					{
						if (@class.Target != null)
						{
							try
							{
								if (@class.Target.IsBot)
								{
									@class.Unidle();
								}
								@class.Target.MoveTo(num, num2);
								return;
							}
							catch
							{
								@class.Target = null;
								@class.class34_1 = null;
								@class.MoveTo(num, num2);
								Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, true);
								return;
							}
						}
						if (@class.TeleportMode)
						{
							@class.X = num;
							@class.Y = num2;
							@class.UpdateNeeded = true;
						}
						else
						{
							@class.MoveTo(num, num2);
						}
					}
				}
			}
		}
	}
}
