using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Messenger
{
	internal sealed class FriendsListUpdateEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
            if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().GetMessenger() != null)
			{
				Session.SendMessage(Session.GetHabbo().GetMessenger().SerializeUpdates());
			}
		}
	}
}
