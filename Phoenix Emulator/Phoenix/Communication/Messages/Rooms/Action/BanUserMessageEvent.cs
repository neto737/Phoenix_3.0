using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Action
{
	internal sealed class BanUserMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				uint uint_ = Event.PopWiredUInt();
				RoomUser class2 = @class.GetRoomUserByHabbo(uint_);
				if (class2 != null && !class2.IsBot && !class2.GetClient().GetHabbo().HasRole("acc_unbannable"))
				{
					@class.method_70(uint_);
					@class.RemoveUserFromRoom(class2.GetClient(), true, true);
				}
			}
		}
	}
}
