using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.HabboHotel.SoundMachine;
namespace Phoenix.Communication.Messages.Sound
{
    internal sealed class GetUserSongDisksMessageEvent : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Event)
        {
            List<UserItem> songs = new List<UserItem>();
            foreach (UserItem current in Session.GetHabbo().GetInventoryComponent().InventoryItems)
            {
                if (current != null && !(current.GetBaseItem().Name != "song_disk") && !Session.GetHabbo().GetInventoryComponent().list_1.Contains(current.Id))
                {
                    songs.Add(current);
                }
            }
            /*ServerMessage Message = new ServerMessage(333u);
            Message.AppendInt32(list.Count);
            foreach (UserItem current2 in list) //PHOENIX SEN OMA
            {
                int int_ = 0;
                if (current2.string_0.Length > 0)
                {
                    int_ = int.Parse(current2.string_0);
                }
                Soundtrack @class = GoldTree.GetGame().GetItemManager().method_4(int_);
                if (@class == null)
                {
                    return;
                }
                Message.AppendUInt(current2.uint_0);
                Message.AppendInt32(@class.Id);
            }*/

            ServerMessage Message = new ServerMessage(333);
            Message.AppendInt32(songs.Count);
            foreach (UserItem userItem in songs) //MUN OMA
            {
                int int_ = 0;
                if (userItem.ExtraData.Length > 0)
                {
                    int_ = int.Parse(userItem.ExtraData);
                }
                SongData SongData = SongManager.GetSong(int_);
                if (SongData == null)
                {
                    return;
                }
                Message.AppendUInt(userItem.Id);
                Message.AppendInt32(SongData.Id);
            }
            Session.SendMessage(Message);
        }
    }
}
