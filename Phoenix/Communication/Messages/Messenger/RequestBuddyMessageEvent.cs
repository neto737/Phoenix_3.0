using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Messenger
{
	internal sealed class RequestBuddyMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
				if (Session.GetHabbo().CurrentQuestId == 4u)
				{
					PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(4u, Session);
				}
				Session.GetHabbo().GetMessenger().method_16(Event.PopFixedString());
			}
		}
	}
}
