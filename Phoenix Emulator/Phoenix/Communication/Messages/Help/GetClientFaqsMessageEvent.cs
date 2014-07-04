using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class GetClientFaqsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(PhoenixEnvironment.GetGame().GetHelpTool().SerializeFrontpage());
		}
	}
}
