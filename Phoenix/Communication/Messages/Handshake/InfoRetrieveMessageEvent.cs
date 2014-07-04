using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Handshake
{
	internal sealed class InfoRetrieveMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(5);
			Message.AppendStringWithBreak(Session.GetHabbo().Id.ToString());
			Message.AppendStringWithBreak(Session.GetHabbo().Username);
			Message.AppendStringWithBreak(Session.GetHabbo().Look);
			Message.AppendStringWithBreak(Session.GetHabbo().Gender.ToUpper());
			Message.AppendStringWithBreak(Session.GetHabbo().Motto);
			Message.AppendStringWithBreak(Session.GetHabbo().RealName);
			Message.AppendBoolean(false);
			Message.AppendInt32(Session.GetHabbo().Respect);
			Message.AppendInt32(Session.GetHabbo().DailyRespectPoints);
			Message.AppendInt32(Session.GetHabbo().DailyPetRespectPoints);
			Message.AppendBoolean(false);
			Session.SendMessage(Message);
		}
	}
}
