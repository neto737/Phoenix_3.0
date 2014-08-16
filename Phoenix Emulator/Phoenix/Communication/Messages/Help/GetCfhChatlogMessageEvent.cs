using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Support;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class GetCfhChatlogMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				SupportTicket @class = PhoenixEnvironment.GetGame().GetModerationTool().GetTicket(Event.PopWiredUInt());
				if (@class != null)
				{
                    RoomData class2 = PhoenixEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(@class.RoomId);
					if (class2 != null)
					{
                        Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeTicketChatlog(@class, class2, @class.Timestamp));
					}
				}
			}
		}
	}
}
