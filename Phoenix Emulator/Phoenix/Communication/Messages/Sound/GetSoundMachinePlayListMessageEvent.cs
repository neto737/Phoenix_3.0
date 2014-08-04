using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Sound
{
	internal sealed class GetSoundMachinePlayListMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(323);
			Message.AppendUInt(Session.GetHabbo().CurrentRoomId);
			Message.AppendInt32(1);
			Message.AppendInt32(1);
			Message.AppendInt32(1);
			Message.AppendStringWithBreak("Watercolour");
			Message.AppendStringWithBreak("Pendulum");
			Message.AppendInt32(1);
			Session.SendMessage(Message);
		}
	}
}
