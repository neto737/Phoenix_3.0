using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Furniture
{
	internal sealed class RoomDimmerChangeStateMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null && @class.CheckRights(Session, true) && @class.MoodlightData != null)
				{
					RoomItem class2 = null;
					foreach (RoomItem class3 in @class.Hashtable_1.Values)
					{
						if (class3.GetBaseItem().InteractionType.ToLower() == "dimmer")
						{
							class2 = class3;
							break;
						}
					}
					if (class2 != null)
					{
						if (@class.MoodlightData.Enabled)
						{
							@class.MoodlightData.method_1();
						}
						else
						{
							@class.MoodlightData.method_0();
						}
						class2.ExtraData = @class.MoodlightData.method_7();
						class2.UpdateState();
					}
				}
			}
			catch
			{
			}
		}
	}
}
