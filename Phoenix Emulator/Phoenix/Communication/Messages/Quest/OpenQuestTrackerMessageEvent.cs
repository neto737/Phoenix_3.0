using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Quest
{
	internal class OpenQuestTrackerMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			PhoenixEnvironment.GetGame().GetQuestManager().ActivateNextQuest(Session);
		}
	}
}
