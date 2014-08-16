using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Messenger
{
	internal class RequestBuddyMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
				if (Session.GetHabbo().CurrentQuestId == 4)
				{
					PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(4, Session);
				}
				Session.GetHabbo().GetMessenger().RequestBuddy(Event.PopFixedString());
			}
		}
	}
}
