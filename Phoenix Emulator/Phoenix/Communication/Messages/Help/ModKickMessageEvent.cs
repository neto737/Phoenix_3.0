using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class ModKickMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint User = Event.PopWiredUInt();
				string Msg = Event.PopFixedString();
				string Kick = string.Concat(new object[]
				{
					"User: ",
					User,
					", Message: ",
					Msg
				});
				PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, "ModTool - Kick User", Kick);
				PhoenixEnvironment.GetGame().GetModerationTool().KickUser(Session, User, Msg, false);
			}
		}
	}
}
