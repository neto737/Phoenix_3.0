using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class PopularRoomsSearchMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Session.GetConnection().SendData(PhoenixEnvironment.GetGame().GetNavigator().SerializeNavigator(Session, Event.PopFixedInt32()));
		}
	}
}
