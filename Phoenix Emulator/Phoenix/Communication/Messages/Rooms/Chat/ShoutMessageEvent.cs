using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Chat
{
	internal sealed class ShoutMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (room != null)
			{
				RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (roomUserByHabbo != null)
				{
					roomUserByHabbo.Chat(Session, PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString()), true);
				}
			}
		}
	}
}
