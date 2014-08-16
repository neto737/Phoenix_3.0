using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class RoomTagSearchMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Event.PopWiredInt32();
			Session.SendMessage(PhoenixEnvironment.GetGame().GetNavigator().SerializeSearchResults(Event.PopFixedString()));
		}
	}
}
