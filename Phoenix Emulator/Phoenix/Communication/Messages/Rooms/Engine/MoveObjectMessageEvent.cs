using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class MoveObjectMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session))
			{
				RoomItem class2 = @class.GetItem(Event.PopWiredUInt());
				if (class2 != null)
				{
					int num = Event.PopWiredInt32();
					int num2 = Event.PopWiredInt32();
					int num3 = Event.PopWiredInt32();
					Event.PopWiredInt32();
					if (Session.GetHabbo().CurrentQuestId == 1u && (num != class2.GetX || num2 != class2.GetY))
					{
						PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(1u, Session);
					}
					else
					{
						if (Session.GetHabbo().CurrentQuestId == 7u && num3 != class2.Rot)
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(7u, Session);
						}
						else
						{
							if (Session.GetHabbo().CurrentQuestId == 9u && class2.GetZ >= 0.1)
							{
								PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(9u, Session);
							}
						}
					}
					bool flag = false;
					string text = class2.GetBaseItem().InteractionType.ToLower();
					if (text != null && text == "teleport")
					{
						flag = true;
					}
					@class.method_79(Session, class2, num, num2, num3, false, false, false);
					if (flag)
					{
						@class.method_64();
					}
				}
			}
		}
	}
}
