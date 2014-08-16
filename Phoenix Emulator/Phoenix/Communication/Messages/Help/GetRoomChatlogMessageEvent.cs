using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class GetRoomChatlogMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_chatlogs"))
			{
				Event.PopWiredInt32();
				uint roomID = Event.PopWiredUInt();
				if (PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(roomID) != null)
				{
					Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeRoomChatlog(roomID));
				}
			}
		}
	}
}
