using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Handshake
{
	internal class VersionCheckMessageEvent : MessageEvent
	{
        public void parse(GameClient Session, ClientMessage Event) { }
	}
}
