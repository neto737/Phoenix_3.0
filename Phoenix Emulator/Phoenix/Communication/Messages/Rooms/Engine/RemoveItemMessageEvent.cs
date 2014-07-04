using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class RemoveItemMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				RoomItem class2 = @class.GetItem(Event.PopWiredUInt());
				if (class2 != null && !(class2.GetBaseItem().InteractionType.ToLower() != "postit"))
				{
					@class.RemoveFurniture(Session, class2.Id, true, true);
				}
			}
		}
	}
}
