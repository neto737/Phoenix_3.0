using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class ReleaseIssuesMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				int num = Event.PopWiredInt32();
				for (int i = 0; i < num; i++)
				{
					uint TicketId = Event.PopWiredUInt();
					PhoenixEnvironment.GetGame().GetModerationTool().ReleaseTicket(Session, TicketId);
				}
			}
		}
	}
}
