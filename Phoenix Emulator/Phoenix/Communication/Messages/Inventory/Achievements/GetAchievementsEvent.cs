using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Achievements;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Inventory.Achievements
{
	internal sealed class GetAchievementsEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Request)
		{
			Session.SendMessage(AchievementManager.SerializeAchievementList(Session));
		}
	}
}
