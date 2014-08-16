using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Support;
namespace Phoenix.Communication.Messages.Help
{
	internal class GetFaqCategoryMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			HelpCategory @class = PhoenixEnvironment.GetGame().GetHelpTool().GetCategory(uint_);
			if (@class != null)
			{
				Session.SendMessage(PhoenixEnvironment.GetGame().GetHelpTool().SerializeCategory(@class));
			}
		}
	}
}
