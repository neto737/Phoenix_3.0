using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class PickIssuesMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				PhoenixEnvironment.GetGame().GetModerationTool().method_6(Session, uint_);
			}
		}
	}
}
