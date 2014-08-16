using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Users.Messenger;
namespace Phoenix.Communication.Messages.Messenger
{
	internal class AcceptBuddyMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
				int num = Event.PopWiredInt32();
				for (int i = 0; i < num; i++)
				{
					uint uint_ = Event.PopWiredUInt();
					MessengerRequest @class = Session.GetHabbo().GetMessenger().GetRequest(uint_);
					if (@class != null)
					{
						if (@class.To != Session.GetHabbo().Id)
						{
							break;
						}
						if (!Session.GetHabbo().GetMessenger().method_9(@class.To, @class.From))
						{
							Session.GetHabbo().GetMessenger().method_12(@class.From);
						}
						Session.GetHabbo().GetMessenger().HandleRequest(uint_);
					}
				}
			}
		}
	}
}
