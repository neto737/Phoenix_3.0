using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Action
{
	internal sealed class LetUserInMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session))
			{
				string string_ = Event.PopFixedString();
				byte[] array = Event.ReadBytes(1);
				GameClient class2 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(string_);
				if (class2 != null && class2.GetHabbo().Waitingfordoorbell && class2.GetHabbo().LoadingRoom == Session.GetHabbo().CurrentRoomId)
				{
					if (array[0] == Convert.ToByte(65))
					{
						class2.GetHabbo().LoadingChecksPassed = true;
						class2.SendMessage(new ServerMessage(41u));
					}
					else
					{
						class2.SendMessage(new ServerMessage(131u));
					}
				}
			}
		}
	}
}
