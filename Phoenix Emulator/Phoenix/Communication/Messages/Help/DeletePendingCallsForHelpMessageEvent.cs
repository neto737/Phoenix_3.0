using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class DeletePendingCallsForHelpMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (PhoenixEnvironment.GetGame().GetModerationTool().UsersHasPendingTicket(Session.GetHabbo().Id))
			{
				PhoenixEnvironment.GetGame().GetModerationTool().DeletePendingTicketForUser(Session.GetHabbo().Id);
				ServerMessage Message = new ServerMessage(320);
				Session.SendMessage(Message);
			}
		}
	}
}
