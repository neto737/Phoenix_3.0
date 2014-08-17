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
			uint CategoryId = Event.PopWiredUInt();

			HelpCategory Category = PhoenixEnvironment.GetGame().GetHelpTool().GetCategory(CategoryId);
			if (Category != null)
			{
				Session.SendMessage(PhoenixEnvironment.GetGame().GetHelpTool().SerializeCategory(Category));
			}
		}
	}
}
