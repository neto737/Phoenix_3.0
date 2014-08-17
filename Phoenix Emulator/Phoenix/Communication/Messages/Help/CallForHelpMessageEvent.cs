using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class CallForHelpMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			bool errorOccured = false;

			if (PhoenixEnvironment.GetGame().GetModerationTool().UsersHasPendingTicket(Session.GetHabbo().Id))
			{
				errorOccured = true;
			}
			if (!errorOccured)
			{
				string message = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				Event.PopWiredInt32();
				int Type = Event.PopWiredInt32();
				uint ReportedUser = Event.PopWiredUInt();

				PhoenixEnvironment.GetGame().GetModerationTool().SendNewTicket(Session, Type, ReportedUser, message);
			}
			ServerMessage Message = new ServerMessage(321);
			Message.AppendBoolean(errorOccured);
			Session.SendMessage(Message);
		}
	}
}
