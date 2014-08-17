using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Support;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class GetCfhChatlogMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				SupportTicket Ticket = PhoenixEnvironment.GetGame().GetModerationTool().GetTicket(Event.PopWiredUInt());
				if (Ticket != null)
				{
                    RoomData Data = PhoenixEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(Ticket.RoomId);
					if (Data != null)
					{
                        Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeTicketChatlog(Ticket, Data, Ticket.Timestamp));
					}
				}
			}
		}
	}
}
