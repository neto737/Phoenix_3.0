using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Avatar
{
	internal sealed class WaveMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (room != null)
			{
				RoomUser class2 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (class2 != null)
				{
					class2.Unidle();
					class2.DanceId = 0;
					ServerMessage Message = new ServerMessage(481);
					Message.AppendInt32(class2.VirtualId);
					room.SendMessage(Message, null);
					if (Session.GetHabbo().CurrentQuestId == 8)
					{
						PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(8, Session);
					}
				}
			}
		}
	}
}
