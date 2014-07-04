using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Handshake
{
	internal sealed class InitCryptoMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			MessageEvent @interface;
			if (PhoenixEnvironment.GetPacketManager().Get(1817u, out @interface))
			{
				@interface.parse(Session, null);
			}
		}
	}
}
