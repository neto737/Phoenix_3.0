using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Quest
{
	internal sealed class GetQuestsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(PhoenixEnvironment.GetGame().GetQuestManager().SerializeQuestList(Session));
		}
	}
}
