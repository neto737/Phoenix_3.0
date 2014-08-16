using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class ModMessageMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint User = Event.PopWiredUInt();
				string Msg = Event.PopFixedString();
				string Alert = string.Concat(new object[]
				{
					"User: ",
					User,
					", Message: ",
					Msg
				});
				PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, "ModTool - Alert User", Alert);
				PhoenixEnvironment.GetGame().GetModerationTool().AlertUser(Session, User, Msg, false);
			}
		}
	}
}
