using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication
{
	internal interface MessageEvent
	{
		void parse(GameClient Session, ClientMessage Request);
	}
}
