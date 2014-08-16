using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class PickIssuesMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				Event.PopWiredInt32();
				uint TicketId = Event.PopWiredUInt();
				PhoenixEnvironment.GetGame().GetModerationTool().PickTicket(Session, TicketId);
			}
		}
	}
}
