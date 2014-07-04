using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Quest
{
	internal sealed class RejectQuestMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			PhoenixEnvironment.GetGame().GetQuestManager().HandleQuest(0, Session);
		}
	}
}
