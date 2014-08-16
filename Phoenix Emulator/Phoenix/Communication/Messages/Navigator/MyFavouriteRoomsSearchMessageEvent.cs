using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class MyFavouriteRoomsSearchMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(PhoenixEnvironment.GetGame().GetNavigator().SerializeFavoriteRooms(Session));
		}
	}
}
