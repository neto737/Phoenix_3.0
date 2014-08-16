using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class EditEventMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (Room != null && Room.CheckRights(Session, true) && Room.Event != null)
			{
				int Category = Event.PopWiredInt32();
				string Name = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				string Description = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				int Id = Event.PopWiredInt32();

				Room.Event.Category = Category;
				Room.Event.Name = Name;
				Room.Event.Description = Description;
				Room.Event.Tags = new List<string>();
				for (int i = 0; i < Id; i++)
				{
					Room.Event.Tags.Add(PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString()));
				}
				Room.SendMessage(Room.Event.Serialize(Session), null);
			}
		}
	}
}
