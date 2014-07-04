using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class ModKickMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint num = Event.PopWiredUInt();
				string text = Event.PopFixedString();
				string string_ = string.Concat(new object[]
				{
					"User: ",
					num,
					", Message: ",
					text
				});
				PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, "ModTool - Kick User", string_);
				PhoenixEnvironment.GetGame().GetModerationTool().method_15(Session, num, text, false);
			}
		}
	}
}
