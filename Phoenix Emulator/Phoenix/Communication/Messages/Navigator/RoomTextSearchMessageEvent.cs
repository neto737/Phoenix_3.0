using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
    internal class RoomTextSearchMessageEvent : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Event)
        {
            string SearchQuery = Event.PopFixedString();
            if (SearchQuery != PhoenixEnvironment.CheckMD5Crypto(Session.GetHabbo().Username))
            {
                Session.SendMessage(PhoenixEnvironment.GetGame().GetNavigator().SerializeSearchResults(SearchQuery));
            }
        }
    }
}
