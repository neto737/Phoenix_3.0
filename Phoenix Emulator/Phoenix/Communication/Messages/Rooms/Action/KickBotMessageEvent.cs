using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Action
{
	internal sealed class KickBotMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				RoomUser class2 = @class.GetRoomUserByVirtualId(Event.PopWiredInt32());
				if (class2 != null && class2.IsBot)
				{
					@class.RemoveBot(class2.VirtualId, true);
				}
			}
		}
	}
}
