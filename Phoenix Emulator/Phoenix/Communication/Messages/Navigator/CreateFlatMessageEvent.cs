using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class CreateFlatMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().list_6.Count <= GlobalClass.MaxRoomsPerUser)
			{
				string Name = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				string Model = Event.PopFixedString();

				Event.PopFixedString();
                RoomData Data = PhoenixEnvironment.GetGame().GetRoomManager().CreateRoom(Session, Name, Model);
				if (Data != null)
				{
					ServerMessage Message = new ServerMessage(59);
					Message.AppendUInt(Data.Id);
					Message.AppendStringWithBreak(Data.Name);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
