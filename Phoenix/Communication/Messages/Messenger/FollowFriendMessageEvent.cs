using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Messenger
{
	internal sealed class FollowFriendMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			GameClient @class = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_);
			if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().InRoom)
			{
				Room class2 = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(@class.GetHabbo().CurrentRoomId);
				if (class2 != null && class2 != Session.GetHabbo().CurrentRoom)
				{
					ServerMessage Message = new ServerMessage(286u);
					Message.AppendBoolean(class2.IsPublic);
					Message.AppendUInt(class2.RoomId);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
