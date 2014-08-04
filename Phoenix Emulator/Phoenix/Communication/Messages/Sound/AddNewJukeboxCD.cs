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
    class AddNewJukeboxCD : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Event)
        {
            if (((Session != null) && (Session.GetHabbo() != null)) && (Session.GetHabbo().CurrentRoom != null))
            {
                Room currentRoom = Session.GetHabbo().CurrentRoom;
                if (currentRoom.CheckRights(Session, true))
                {
                    RoomMusicController roomMusicController = currentRoom.GetRoomMusicController();
                    if (roomMusicController.PlaylistSize < roomMusicController.PlaylistCapacity)
                    {
                        int num = Event.PopWiredInt32();
                        UserItem item = Session.GetHabbo().GetInventoryComponent().GetItem((uint)num);
                        if ((item != null) && (item.GetBaseItem().InteractionType == "musicdisc"))
                        {
                            SongItem diskItem = new SongItem(item);
                            if (roomMusicController.AddDisk(diskItem) >= 0)
                            {
                                //diskItem.SaveToDatabase((int)currentRoom.Id); // <-- old
                                diskItem.SaveToDatabase((int)roomMusicController.LinkedItemId); // <-- new
                                Session.GetHabbo().GetInventoryComponent().RemoveItem((uint)num, 0, true);
                                Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                                Session.SendMessage(JukeboxDiscksComposer.Compose(roomMusicController.PlaylistCapacity, roomMusicController.Playlist.Values.ToList<SongInstance>()));
                            }
                        }
                    }
                }
            }
        }
    }
}
