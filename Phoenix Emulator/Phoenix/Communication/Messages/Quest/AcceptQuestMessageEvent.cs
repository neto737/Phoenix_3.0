using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Quest
{
	internal class AcceptQuestMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint id = Event.PopWiredUInt();
			PhoenixEnvironment.GetGame().GetQuestManager().HandleQuest(id, Session);
		}
	}
}
