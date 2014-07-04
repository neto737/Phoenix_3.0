using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class CreateEventMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true) && @class.Event == null && @class.State == 0)
			{
				int int_ = Event.PopWiredInt32();
				string text = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				string string_ = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				int num = Event.PopWiredInt32();
				if (text.Length >= 1)
				{
					@class.Event = new RoomEvent(@class.RoomId, text, string_, int_, null);
					@class.Event.Tags = new List<string>();
					for (int i = 0; i < num; i++)
					{
						@class.Event.Tags.Add(PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString()));
					}
					@class.SendMessage(@class.Event.Serialize(Session), null);
				}
			}
		}
	}
}
