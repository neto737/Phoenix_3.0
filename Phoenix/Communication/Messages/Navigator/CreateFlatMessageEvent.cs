using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class CreateFlatMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().list_6.Count <= GlobalClass.MaxRoomsPerUser)
			{
				string string_ = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				string string_2 = Event.PopFixedString();
				Event.PopFixedString();
                RoomData @class = PhoenixEnvironment.GetGame().GetRoomManager().method_20(Session, string_, string_2);
				if (@class != null)
				{
					ServerMessage Message = new ServerMessage(59u);
					Message.AppendUInt(@class.Id);
					Message.AppendStringWithBreak(@class.Name);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
