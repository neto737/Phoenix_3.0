using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Chat
{
	internal sealed class StartTypingMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (room != null)
			{
				RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (roomUserByHabbo != null)
				{
					ServerMessage Message = new ServerMessage(361);
					Message.AppendInt32(roomUserByHabbo.VirtualId);
					Message.AppendBoolean(true);
					room.SendMessage(Message, null);
				}
			}
		}
	}
}
