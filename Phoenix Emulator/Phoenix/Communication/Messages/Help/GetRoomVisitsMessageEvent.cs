using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class GetRoomVisitsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint UserId = Event.PopWiredUInt();
				Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeRoomVisits(UserId));
			}
		}
	}
}
