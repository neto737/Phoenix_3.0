using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class ScrGetUserInfoMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			string text = Event.PopFixedString();
			ServerMessage Message = new ServerMessage(7u);
			Message.AppendStringWithBreak(text.ToLower());
			if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip"))
			{
				double num = (double)Session.GetHabbo().GetSubscriptionManager().GetSubscription("habbo_vip").ExpireTime;
				double num2 = num - PhoenixEnvironment.GetUnixTimestamp();
				int num3 = (int)Math.Ceiling(num2 / 86400.0);
				int num4 = num3 / 31;
				if (num4 >= 1)
				{
					num4--;
				}
				Message.AppendInt32(num3 - num4 * 31);
				Message.AppendBoolean(true);
				Message.AppendInt32(num4);
				Message.AppendBoolean(true);
				Message.AppendBoolean(true);
				Message.AppendBoolean(Session.GetHabbo().Vip);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
			}
			else
			{
				if (Session.GetHabbo().GetSubscriptionManager().HasSubscription(text))
				{
					double num = (double)Session.GetHabbo().GetSubscriptionManager().GetSubscription(text).ExpireTime;
					double num2 = num - PhoenixEnvironment.GetUnixTimestamp();
					int num3 = (int)Math.Ceiling(num2 / 86400.0);
					int num4 = num3 / 31;
					if (num4 >= 1)
					{
						num4--;
					}
					Message.AppendInt32(num3 - num4 * 31);
					Message.AppendBoolean(true);
					Message.AppendInt32(num4);
					if (Session.GetHabbo().Rank >= 2u)
					{
						Message.AppendInt32(1);
						Message.AppendInt32(1);
						Message.AppendInt32(2);
					}
					else
					{
						Message.AppendInt32(1);
					}
				}
				else
				{
					for (int i = 0; i < 3; i++)
					{
						Message.AppendInt32(0);
					}
				}
			}
			Session.SendMessage(Message);
		}
	}
}
