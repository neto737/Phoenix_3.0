using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Catalog
{
    internal sealed class GetGiftWrappingConfigurationEvent : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Event)
        {
            ServerMessage Message = new ServerMessage(620u);
            for (int i = 187; i < 191; i++)
            {
                Message.AppendInt32(i);
            }
            Message.AppendInt32(187);
            Message.AppendInt32(188);
            Message.AppendInt32(189);
            Message.AppendInt32(190);
            Message.AppendInt32(191);
            Session.SendMessage(Message);
        }
    }
}