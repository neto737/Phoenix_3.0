using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Avatar
{
	internal sealed class DanceMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
			roomUserByHabbo.Unidle();
			int i = Event.PopWiredInt32();
            
            if (i < 0 || i > 4 || (!Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") && i > 1))
			{
				i = 0;
			}
			if (i > 0 && roomUserByHabbo.CarryItemID > 0)
			{
				roomUserByHabbo.CarryItem(0);
			}
			roomUserByHabbo.DanceId = i;
			ServerMessage Message = new ServerMessage(480);
			Message.AppendInt32(roomUserByHabbo.VirtualId);
			Message.AppendInt32(i);
			room.SendMessage(Message, null);
			
            if (Session.GetHabbo().CurrentQuestId == 6)
			{
				PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(6, Session);
			}
		}
    }
}
