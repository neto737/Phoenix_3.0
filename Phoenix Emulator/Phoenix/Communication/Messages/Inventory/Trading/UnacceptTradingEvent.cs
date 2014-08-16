using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Inventory.Trading
{
	internal class UnacceptTradingEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (Room != null && Room.CanTradeInRoom)
			{
				Trade Trade = Room.GetUserTrade(Session.GetHabbo().Id);
				if (Trade != null)
				{
					Trade.Unaccept(Session.GetHabbo().Id);
				}
			}
		}
	}
}
