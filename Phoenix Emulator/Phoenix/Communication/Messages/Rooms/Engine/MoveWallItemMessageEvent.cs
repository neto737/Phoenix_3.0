using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class MoveWallItemMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session))
			{
				RoomItem class2 = @class.GetItem(Event.PopWiredUInt());
				if (class2 != null)
				{
					string string_ = Event.PopFixedString();
					@class.method_82(Session, class2, false, string_);
				}
			}
		}
	}
}
