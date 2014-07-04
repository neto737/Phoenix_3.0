using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class GetFurnitureAliasesMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().LoadingRoom > 0u)
			{
				ServerMessage Message = new ServerMessage(297u);
				Message.AppendInt32(0);
				Session.SendMessage(Message);
			}
		}
	}
}
