using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
namespace Phoenix.Communication.Messages.Inventory.Trading
{
	internal sealed class OpenTradingEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				if (!@class.Boolean_2)
				{
					Session.GetHabbo().Sendselfwhisper(TextManager.GetText("trade_error_roomdisabled"));
				}
				else
				{
					RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
					RoomUser class3 = @class.method_52(Event.PopWiredInt32());
					if (class2 != null && class3 != null && class3.GetClient().GetHabbo().bool_2)
					{
						@class.method_77(class2, class3);
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
