using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class SetItemDataMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				RoomItem class2 = @class.GetItem(Event.PopWiredUInt());
				if (class2 != null && !(class2.GetBaseItem().InteractionType.ToLower() != "postit"))
				{
					string text = Event.PopFixedString();
					string text2 = text.Split(new char[]
					{
						' '
					})[0];
					string str = PhoenixEnvironment.FilterInjectionChars(text.Substring(text2.Length + 1), true, true);
					if (@class.CheckRights(Session) || text.StartsWith(class2.ExtraData))
					{
						string text3 = text2;
						if (text3 != null && (text3 == "FFFF33" || text3 == "FF9CFF" || text3 == "9CCEFF" || text3 == "9CFF9C"))
						{
							class2.ExtraData = text2 + " " + str;
							class2.UpdateState(true, true);
						}
					}
				}
			}
		}
	}
}
