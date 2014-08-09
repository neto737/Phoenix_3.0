using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
using Phoenix.HabboHotel.SoundMachine;
using Phoenix.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phoenix.Communication.Messages.Sound
{
    class RemoveCDToJukebox : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Event)
        {
            if (((Session != null) && (Session.GetHabbo() != null)) && (Session.GetHabbo().CurrentRoom != null))
            {
                Room currentRoom = Session.GetHabbo().CurrentRoom;
                if (currentRoom.CheckRights(Session, true) && currentRoom.GotMusicController())
                {
                    RoomMusicController roomMusicController = currentRoom.GetRoomMusicController();
                    SongItem item = roomMusicController.RemoveDisk(Event.PopWiredInt32());
                    if (item != null)
                    {
                        item.RemoveFromDatabase();
                        Session.GetHabbo().GetInventoryComponent().AddItem((uint)item.itemID, item.baseItem.ItemId, item.songID.ToString(), false);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.SendMessage(JukeboxDiscksComposer.SerializeSongInventory(Session.GetHabbo().GetInventoryComponent().songDisks));
                        Session.SendMessage(JukeboxDiscksComposer.Compose(roomMusicController.PlaylistCapacity, roomMusicController.Playlist.Values.ToList<SongInstance>()));
                    }
                }
            }
        }
    }
}
