using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Handshake
{
	internal class InitCryptoMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Request)
		{
			MessageEvent Event;
			if (PhoenixEnvironment.GetPacketManager().Get(1817, out Event))
			{
				Event.parse(Session, null);
			}
		}
	}
}
