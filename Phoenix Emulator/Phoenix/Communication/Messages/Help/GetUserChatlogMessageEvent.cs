using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class GetUserChatlogMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_chatlogs"))
			{
				Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeUserChatlog(Event.PopWiredUInt()));
			}
		}
	}
}
