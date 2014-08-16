using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Messenger
{
	internal class RemoveBuddyMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
				int num = Event.PopWiredInt32();
				for (int i = 0; i < num; i++)
				{
					Session.GetHabbo().GetMessenger().method_13(Event.PopWiredUInt());
				}
			}
		}
	}
}
