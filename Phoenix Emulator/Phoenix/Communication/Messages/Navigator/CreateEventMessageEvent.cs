using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class CreateEventMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (Room != null && Room.CheckRights(Session, true) && Room.Event == null && Room.State == 0)
			{
				int Category = Event.PopWiredInt32();
				string Name = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				string Description = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				int Id = Event.PopWiredInt32();

				if (Name.Length >= 1)
				{
					Room.Event = new RoomEvent(Room.RoomId, Name, Description, Category, null);
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
}
