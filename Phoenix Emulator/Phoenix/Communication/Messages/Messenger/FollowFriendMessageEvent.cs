using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Messenger
{
	internal class FollowFriendMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint Id = Event.PopWiredUInt();
			GameClient mSession = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Id);
			if (mSession != null && mSession.GetHabbo() != null && mSession.GetHabbo().InRoom)
			{
				Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(mSession.GetHabbo().CurrentRoomId);
				if (Room != null && Room != Session.GetHabbo().CurrentRoom)
				{
					ServerMessage Message = new ServerMessage(286);
					Message.AppendBoolean(Room.IsPublic);
					Message.AppendUInt(Room.RoomId);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
