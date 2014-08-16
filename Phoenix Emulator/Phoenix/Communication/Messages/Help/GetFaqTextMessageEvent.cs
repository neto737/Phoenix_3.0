using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Support;
namespace Phoenix.Communication.Messages.Help
{
	internal class GetFaqTextMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			HelpTopic @class = PhoenixEnvironment.GetGame().GetHelpTool().GetTopic(uint_);
			if (@class != null)
			{
				Session.SendMessage(PhoenixEnvironment.GetGame().GetHelpTool().SerializeTopic(@class));
			}
		}
	}
}
