using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class ModBanMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				string string_ = Event.PopFixedString();
				int int_ = Event.PopWiredInt32() * 3600;
				PhoenixEnvironment.GetGame().GetModerationTool().method_17(Session, uint_, int_, string_);
			}
		}
	}
}
