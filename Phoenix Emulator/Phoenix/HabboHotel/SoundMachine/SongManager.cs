using Phoenix.Storage;
using System;
using System.Collections.Generic;
using System.Data;

namespace Phoenix.HabboHotel.SoundMachine
{
    internal class SongManager
    {
        private const int CACHE_LIFETIME = 180;
        private static Dictionary<int, double> cacheTimer;
        private static Dictionary<int, SongData> songs;

        public static SongData GetSong(int SongId)
        {
            if (songs != null)
            {
                if (songs.ContainsKey(SongId))
                {
                    return songs[SongId];
                }
                else
                {
                    return new SongData(-1, "Error", "Error", "", 0);
                }
            }
            else
            {
                return new SongData(-1, "Error", "Error", "", 0);
            }
        }

        public static SongData GetSongFromDataRow(DataRow dRow)
        {
            return new SongData((int)(dRow["id"]), (string)dRow["name"], (string)dRow["author"], (string)(dRow["track"]), (int)(dRow["length"]));
        }

        public static void Initialize()
        {
            songs = new Dictionary<int, SongData>();
            cacheTimer = new Dictionary<int, double>();
            using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
            {
                DataTable table = dbClient.ReadDataTable("SELECT * FROM soundtracks");
                foreach (DataRow row in table.Rows)
                {
                    SongData songFromDataRow = GetSongFromDataRow(row);
                    songs.Add(songFromDataRow.Id, songFromDataRow);
                }
            }
        }

        public bool SongExits(int SongID)
        {
            return songs.ContainsKey(SongID);
        }

        public SongData FindSong(int SongID)
        {
            if (this.SongExits(SongID))
            {
                return songs[SongID];
            }
            else
            {
                return null;
            }
        }
    }
}