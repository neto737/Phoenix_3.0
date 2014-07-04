using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Rooms.Session
{
	internal sealed class QuitMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			try
			{
				if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().InRoom)
				{
					PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).RemoveUserFromRoom(Session, true, false);
				}
			}
			catch
			{
			}
		}
	}
}
