using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Users.Badges;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class GetSelectedBadgesMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null)
			{
				Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null)
				{
					RoomUser class2 = @class.GetRoomUserByHabbo(Event.PopWiredUInt());
					if (class2 != null && !class2.IsBot && class2.GetClient() != null)
					{
						ServerMessage Message = new ServerMessage(228u);
						Message.AppendUInt(class2.GetClient().GetHabbo().Id);
						Message.AppendInt32(class2.GetClient().GetHabbo().GetBadgeComponent().Int32_1);
						using (TimedLock.Lock(class2.GetClient().GetHabbo().GetBadgeComponent().List_0))
						{
							foreach (Badge current in class2.GetClient().GetHabbo().GetBadgeComponent().List_0)
							{
								if (current.Slot > 0)
								{
									Message.AppendInt32(current.Slot);
									Message.AppendStringWithBreak(current.Code);
								}
							}
						}
						Session.SendMessage(Message);
					}
				}
			}
		}
	}
}
