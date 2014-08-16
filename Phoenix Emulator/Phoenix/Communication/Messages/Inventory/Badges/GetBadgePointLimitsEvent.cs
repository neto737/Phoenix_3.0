using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Inventory.Badges
{
	internal class GetBadgePointLimitsEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(443);
			Message.AppendInt32(Session.GetHabbo().AchievementScore);
			Message.AppendStringWithBreak("");
			Session.SendMessage(Message);
		}
	}
}
