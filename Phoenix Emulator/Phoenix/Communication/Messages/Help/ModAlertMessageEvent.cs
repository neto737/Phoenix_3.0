using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class ModAlertMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint UserId = Event.PopWiredUInt();
				string Message = Event.PopFixedString();

				PhoenixEnvironment.GetGame().GetModerationTool().AlertUser(Session, UserId, Message, true);
			}
		}
	}
}
