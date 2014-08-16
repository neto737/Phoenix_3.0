using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Messenger
{
	internal class DeclineBuddyMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
				int num = Event.PopWiredInt32();
				int num2 = Event.PopWiredInt32();
				if (num == 0 && num2 == 1)
				{
					uint uint_ = Event.PopWiredUInt();
					Session.GetHabbo().GetMessenger().HandleRequest(uint_);
				}
				else
				{
					if (num == 1)
					{
						Session.GetHabbo().GetMessenger().HandleAllRequests();
					}
				}
			}
		}
	}
}
