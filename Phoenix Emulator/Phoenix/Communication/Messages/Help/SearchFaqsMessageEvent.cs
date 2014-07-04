using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class SearchFaqsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			string text = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
			if (text.Length >= 1)
			{
				Session.SendMessage(PhoenixEnvironment.GetGame().GetHelpTool().SerializeSearchResults(text));
			}
		}
	}
}
