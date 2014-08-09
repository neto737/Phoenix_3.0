using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class PickupObjectMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Event.PopWiredInt32();
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				RoomItem class2 = @class.GetItem(Event.PopWiredUInt());
				if (class2 != null)
				{
					string text = class2.GetBaseItem().InteractionType.ToLower();
					if (text == null || !(text == "postit"))
					{
						@class.RemoveFurniture(Session, class2.Id, false, true);
						Session.GetHabbo().GetInventoryComponent().AddItem(class2.Id, class2.BaseItem, class2.ExtraData, false);
						Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
						if (Session.GetHabbo().CurrentQuestId == 10u)
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(10u, Session);
						}
					}
				}
			}
		}
	}
}
