using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class ModAlertMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				string string_ = Event.PopFixedString();
				PhoenixEnvironment.GetGame().GetModerationTool().method_16(Session, uint_, string_, true);
			}
		}
	}
}
