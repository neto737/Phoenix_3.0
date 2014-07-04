using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class GetModeratorUserInfoMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				if (PhoenixEnvironment.GetGame().GetClientManager().GetNameById(uint_) != "Unknown User")
				{
					Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().method_18(uint_));
				}
				else
				{
					Session.SendNotif("Could not load user info, invalid user.");
				}
			}
		}
	}
}
