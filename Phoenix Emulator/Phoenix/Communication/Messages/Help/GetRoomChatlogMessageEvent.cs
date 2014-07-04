using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class GetRoomChatlogMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_chatlogs"))
			{
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				if (PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(uint_) != null)
				{
					Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().method_22(uint_));
				}
			}
		}
	}
}
