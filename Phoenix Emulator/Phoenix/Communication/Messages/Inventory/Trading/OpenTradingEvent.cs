using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
namespace Phoenix.Communication.Messages.Inventory.Trading
{
	internal class OpenTradingEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (Room != null)
			{
				if (!Room.CanTradeInRoom)
				{
					Session.GetHabbo().Sendselfwhisper(TextManager.GetText("trade_error_roomdisabled"));
				}
				else
				{
					RoomUser UserOne = Room.GetRoomUserByHabbo(Session.GetHabbo().Id);
					RoomUser UserTwo = Room.GetRoomUserByVirtualId(Event.PopWiredInt32());
					if (UserOne != null && UserTwo != null && UserTwo.GetClient().GetHabbo().AcceptTrading)
					{
						Room.TryStartTrade(UserOne, UserTwo);
					}
					else
					{
						Session.GetHabbo().Sendselfwhisper(TextManager.GetText("trade_error_targetdisabled"));
					}
				}
			}
		}
	}
}
