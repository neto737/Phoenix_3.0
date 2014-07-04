using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Rooms.Session
{
	internal sealed class GoToFlatMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Session.GetHabbo().LoadingRoom = Event.PopWiredUInt();
			Session.GetMessageHandler().LoadRoomForUser();
		}
	}
}
