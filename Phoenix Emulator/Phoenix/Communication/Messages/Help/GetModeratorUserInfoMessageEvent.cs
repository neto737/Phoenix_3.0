using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class GetModeratorUserInfoMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint Id = Event.PopWiredUInt();
				if (PhoenixEnvironment.GetGame().GetClientManager().GetNameById(Id) != "Unknown User")
				{
					Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeUserInfo(Id));
				}
				else
				{
					Session.SendNotif("Could not load user info, invalid user.");
				}
			}
		}
	}
}
