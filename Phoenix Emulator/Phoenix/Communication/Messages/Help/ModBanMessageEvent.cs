using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class ModBanMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint UserId = Event.PopWiredUInt();
				string Message = Event.PopFixedString();
				int Length = Event.PopWiredInt32() * 3600;

				PhoenixEnvironment.GetGame().GetModerationTool().BanUser(Session, UserId, Length, Message);
			}
		}
	}
}
