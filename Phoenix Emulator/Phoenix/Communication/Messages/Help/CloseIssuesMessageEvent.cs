using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class CloseIssuesMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				int int_ = Event.PopWiredInt32();
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				PhoenixEnvironment.GetGame().GetModerationTool().method_8(Session, uint_, int_);
			}
		}
	}
}
