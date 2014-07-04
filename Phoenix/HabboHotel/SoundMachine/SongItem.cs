using Phoenix.HabboHotel.Items;
using Phoenix.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phoenix.HabboHotel.SoundMachine
{
    class SongItem
    {
        public Item baseItem;
        public int itemID;
        public int songID;

        public SongItem(UserItem item)
        {
            this.itemID = (int)item.Id;
            this.songID = Convert.ToInt32(item.ExtraData);
            this.baseItem = item.GetBaseItem();
        }

        public SongItem(int itemID, int songID, int baseItem)
        {
            this.itemID = itemID;
            this.songID = songID;
            this.baseItem = PhoenixEnvironment.GetGame().GetItemManager().GetItem((uint)baseItem);
        }

        public void RemoveFromDatabase()
        {
            using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
            {
                @class.ExecuteQuery("DELETE FROM items_rooms_songs WHERE itemid = " + itemID); // <-- old
                @class.ExecuteQuery("DELETE FROM items_jukebox_songs WHERE itemid = " + itemID); // <-- new
                //@class.ExecuteQuery(string.Concat(new object[] { "INSERT INTO items (id, base_item) VALUES ('", itemID, "','", baseItem.UInt32_0, "')" }));
            }
        }

        //public void SaveToDatabase(int roomID) // <-- old
        public void SaveToDatabase(int JukeboxID) // <-- new
        {
            using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
            {
                //@class.ExecuteQuery(string.Concat(new object[] { "INSERT INTO items_rooms_songs VALUES (", itemID, ",", roomID, ",", this.songID, ",", this.baseItem.UInt32_0, ")" })); // <-- old
                @class.ExecuteQuery(string.Concat(new object[] { "INSERT INTO items_jukebox_songs VALUES (", itemID, ",", JukeboxID, ",", this.songID, ",", this.baseItem.UInt32_0, ")" })); // <-- new
                //@class.ExecuteQuery("DELETE FROM items WHERE id = '" + itemID + "'");
            }
        }
    }
}