using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class CallForHelpMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			bool flag = false;
			if (PhoenixEnvironment.GetGame().GetModerationTool().UsersHasPendingTicket(Session.GetHabbo().Id))
			{
				flag = true;
			}
			if (!flag)
			{
				string string_ = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				Event.PopWiredInt32();
				int int_ = Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				PhoenixEnvironment.GetGame().GetModerationTool().SendNewTicket(Session, int_, uint_, string_);
			}
			ServerMessage Message = new ServerMessage(321u);
			Message.AppendBoolean(flag);
			Session.SendMessage(Message);
		}
	}
}
