using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class DeletePendingCallsForHelpMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (PhoenixEnvironment.GetGame().GetModerationTool().method_9(Session.GetHabbo().Id))
			{
				PhoenixEnvironment.GetGame().GetModerationTool().method_10(Session.GetHabbo().Id);
				ServerMessage Message5_ = new ServerMessage(320u);
				Session.SendMessage(Message5_);
			}
		}
	}
}
