using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class EditEventMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true) && @class.Event != null)
			{
				int int_ = Event.PopWiredInt32();
				string string_ = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				string string_2 = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				int num = Event.PopWiredInt32();
				@class.Event.Category = int_;
				@class.Event.Name = string_;
				@class.Event.Description = string_2;
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
