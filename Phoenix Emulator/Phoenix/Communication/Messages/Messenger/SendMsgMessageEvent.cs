using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Messenger
{
	internal class SendMsgMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			string text = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
			if (Session.GetHabbo().GetMessenger() != null)
			{
				if (num == 0 && Session.GetHabbo().HasRole("cmd_sa"))
				{
					ServerMessage Message = new ServerMessage(134);
					Message.AppendUInt(0);
					Message.AppendString(Session.GetHabbo().Username + ": " + text);
					PhoenixEnvironment.GetGame().GetClientManager().SendStaffChat(Session, Message);
				}
				else
				{
					if (num == 0)
					{
						ServerMessage Message2 = new ServerMessage(261);
						Message2.AppendInt32(4);
						Message2.AppendUInt(0);
						Session.SendMessage(Message2);
					}
					else
					{
						Session.GetHabbo().GetMessenger().method_18(num, text);
					}
				}
			}
		}
	}
}
