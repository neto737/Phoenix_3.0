using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Sound
{
	internal sealed class GetSoundSettingsEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(308u);
			Message.AppendInt32(Session.GetHabbo().Volume);
			Message.AppendBoolean(false);
			Session.SendMessage(Message);
		}
	}
}
