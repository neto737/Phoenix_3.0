using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class RoomsWithHighestScoreSearchMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Session.GetConnection().SendData(PhoenixEnvironment.GetGame().GetNavigator().SerializeNavigator(Session, -2));
		}
	}
}
