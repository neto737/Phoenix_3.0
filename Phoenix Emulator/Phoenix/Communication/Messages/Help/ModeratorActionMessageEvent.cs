using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class ModeratorActionMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				Event.PopWiredInt32();
				int num = Event.PopWiredInt32();
				string Msg = Event.PopFixedString();
				string text2 = "";
				if (num.Equals(3))
				{
					text2 += "Room Cautioned.";
				}
				text2 = text2 + " Message: " + Msg;
				PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, "ModTool - Room Alert", text2);
				PhoenixEnvironment.GetGame().GetModerationTool().method_13(Session.GetHabbo().CurrentRoomId, !num.Equals(3), Msg);
			}
		}
	}
}
