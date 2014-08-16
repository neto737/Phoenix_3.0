using System;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Handshake
{
	internal class SSOTicketMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo() == null)
			{
				Session.Login(Event.PopFixedString());
				if (Session.GetHabbo() != null && Session.GetHabbo().MutedUsers != null && Session.GetHabbo().MutedUsers.Count > 0)
				{
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						try
						{
							ServerMessage Message = new ServerMessage(420);
							Message.AppendInt32(Session.GetHabbo().MutedUsers.Count);
							foreach (uint Id in Session.GetHabbo().MutedUsers)
							{
								string string_ = adapter.ReadString("SELECT username FROM users WHERE Id = " + Id + " LIMIT 1;");
								Message.AppendStringWithBreak(string_);
							}
							Session.SendMessage(Message);
						}
						catch
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Logging.WriteLine("Login error: User is ignoring a user that no longer exists");
							Console.ForegroundColor = ConsoleColor.Gray;
						}

					}
				}
			}
		}
	}
}
