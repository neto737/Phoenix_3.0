using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.SoundMachine;
namespace Phoenix.Communication.Messages.Sound
{
internal sealed class GetSongInfoMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			ServerMessage Message = new ServerMessage(300u);
			Message.AppendInt32(num);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					int num2 = Event.PopWiredInt32();
                    if (num2 > 0)
                    {
                        /*Soundtrack @class = GoldTree.GetGame().GetItemManager().method_4(num2);
                        Message.AppendInt32(@class.Id);
                        Message.AppendStringWithBreak(@class.Name);
                        Message.AppendStringWithBreak(@class.Track);
                        Message.AppendInt32(@class.Length);
                        Message.AppendStringWithBreak(@class.Author);*/

                        Message.AppendInt32(SongManager.GetSong(num2).Id);
                        Message.AppendStringWithBreak(SongManager.GetSong(num2).Name);
                        Message.AppendStringWithBreak(SongManager.GetSong(num2).Track);
                        Message.AppendInt32(SongManager.GetSong(num2).Length);
                        Message.AppendStringWithBreak(SongManager.GetSong(num2).Author);
                    }
				}
			}
			Session.SendMessage(Message);
		}
	}
}
