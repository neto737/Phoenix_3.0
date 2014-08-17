using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class CloseIssuesMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				int Result = Event.PopWiredInt32();
				Event.PopWiredInt32();
				uint TicketId = Event.PopWiredUInt();

				PhoenixEnvironment.GetGame().GetModerationTool().CloseTicket(Session, TicketId, Result);
			}
		}
	}
}
