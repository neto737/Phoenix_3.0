using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class SearchFaqsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			string Query = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
			if (Query.Length >= 1)
			{
				Session.SendMessage(PhoenixEnvironment.GetGame().GetHelpTool().SerializeSearchResults(Query));
			}
		}
	}
}
