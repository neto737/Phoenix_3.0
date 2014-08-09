using Phoenix.HabboHotel.SoundMachine;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phoenix.Messages;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Storage;

namespace Phoenix.HabboHotel.SoundMachine
{
    class RoomMusicController
    {
        private static bool mBroadcastNeeded;
        private bool mIsPlaying;
        private Dictionary<int, SongItem> mLoadedDisks = new Dictionary<int, SongItem>();
        private SortedDictionary<int, SongInstance> mPlaylist = new SortedDictionary<int, SongInstance>();
        private RoomItem mRoomOutputItem;
        private SongInstance mSong;
        private int mSongQueuePosition;
        private double mSongLength;
        private double mStartedPlayingTimestamp;

        public int AddDisk(SongItem DiskItem)
        {
            int songID = DiskItem.songID;
            if (songID == 0)
            {
                return -1;
            }
            SongData song = SongManager.GetSong(songID);
            if (song == null)
            {
                return -1;
            }
            if (this.mLoadedDisks.ContainsKey(DiskItem.itemID))
            {
                return -1;
            }
            this.mLoadedDisks.Add(DiskItem.itemID, DiskItem);
            int count = this.mPlaylist.Count;
            lock (this.mPlaylist)
            {
                this.mPlaylist.Add(count, new SongInstance(DiskItem, song));
            }
            return count;
        }

        public SongItem RemoveDisk(int PlaylistIndex)
        {
            SongInstance instance = null;
            lock (this.mPlaylist)
            {
                if (!this.mPlaylist.ContainsKey(PlaylistIndex))
                {
                    return null;
                }
                instance = this.mPlaylist[PlaylistIndex];
                this.mPlaylist.Remove(PlaylistIndex);
            }
            lock (this.mLoadedDisks)
            {
                this.mLoadedDisks.Remove(instance.DiskItem.itemID);
            }
            this.RepairPlaylist();
            if (PlaylistIndex == this.mSongQueuePosition)
            {
                this.PlaySong();
            }
            return instance.DiskItem;
        }

        public void Update(Room Instance)
        {
            if (this.mIsPlaying && ((this.mSong == null) || (this.TimePlaying >= this.mSongLength)))
            {
                if (this.mPlaylist.Count == 0)
                {
                    this.Stop();
                    this.mRoomOutputItem.ExtraData = "0";
                    this.mRoomOutputItem.UpdateState(true, true);
                }
                else
                {
                    this.SetNextSong();
                }
                mBroadcastNeeded = true;
            }
            if (mBroadcastNeeded)
            {
                this.BroadcastCurrentSongData(Instance);
                mBroadcastNeeded = false;
            }
        }

        internal void BroadcastCurrentSongData(Room Instance)
        {
            if (this.mSong != null)
            {
                Instance.SendMessage(JukeboxDiscksComposer.ComposePlayingComposer(this.mSong.SongData.Id, this.mSongQueuePosition, 0), null);
            }
            else
            {
                Instance.SendMessage(JukeboxDiscksComposer.ComposePlayingComposer(0, 0, 0), null);
            }
        }

        public void RepairPlaylist()
        {
            List<SongItem> list = null;
            lock (this.mLoadedDisks)
            {
                list = this.mLoadedDisks.Values.ToList<SongItem>();
                this.mLoadedDisks.Clear();
            }
            lock (this.mPlaylist)
            {
                this.mPlaylist.Clear();
            }
            foreach (SongItem item in list)
            {
                this.AddDisk(item);
            }
        }

        public void PlaySong()
        {
            if (this.mSongQueuePosition >= this.mPlaylist.Count)
            {
                this.mSongQueuePosition = 0;
            }
            if (this.mPlaylist.Count == 0)
            {
                this.Stop();
            }
            else
            {
                if (!this.mPlaylist.ContainsKey(this.mSongQueuePosition))
                {
                    this.mSongQueuePosition = 0;
                }
                this.mSong = this.mPlaylist[this.mSongQueuePosition];
                this.mSongLength = this.mSong.SongData.Length / 1000;
                this.mStartedPlayingTimestamp = PhoenixEnvironment.GetUnixTimestamp();
                mBroadcastNeeded = true;
            }
        }

        public void LinkRoomOutputItem(RoomItem Item)
        {
            this.mRoomOutputItem = Item;
        }

        public void UnLinkRoomOutputItem()
        {
            this.mRoomOutputItem = null;
        }

        public void LinkRoomOutputItemIfNotAlreadyExits(RoomItem Item)
        {
            if (this.mRoomOutputItem != null)
            {
            }
            else
            {
                this.mRoomOutputItem = Item;
            }
        }

        public void Start(int SongRequest)
        {
            if (!(SongRequest >= 0))
            {
                SongRequest = 0;
            }

            this.mIsPlaying = true;
            this.mSongQueuePosition = SongRequest - 1;
            this.SetNextSong();
            if (this.mRoomOutputItem != null)
            {
                this.mRoomOutputItem.ExtraData = "1";
                this.mRoomOutputItem.TimerRunning = true;
                this.mRoomOutputItem.UpdateNeeded = true;
                this.mRoomOutputItem.UpdateState(true, true);
            }
        }

        public void SetNextSong()
        {
            this.mSongQueuePosition++;
            this.PlaySong();
        }

        public void Stop()
        {
            this.mSong = null;
            this.mIsPlaying = false;
            this.mSongQueuePosition = -1;
            mBroadcastNeeded = true;
            if (this.mRoomOutputItem != null)
            {
                this.mRoomOutputItem.ExtraData = "0";
                this.mRoomOutputItem.TimerRunning = false;
                this.mRoomOutputItem.UpdateState(true, true);
            }
        }

        public SortedDictionary<int, SongInstance> Playlist
        {
            get
            {
                SortedDictionary<int, SongInstance> dictionary = new SortedDictionary<int, SongInstance>();
                lock (this.mPlaylist)
                {
                    foreach (KeyValuePair<int, SongInstance> pair in this.mPlaylist)
                    {
                        dictionary.Add(pair.Key, pair.Value);
                    }
                }
                return dictionary;
            }
        }

        internal void OnNewUserEnter(RoomUser user)
        {
            if ((user.GetClient() != null) && (this.mSong != null))
            {
                this.mSongQueuePosition++;
                user.GetClient().SendMessage(JukeboxDiscksComposer.ComposePlayingComposer(this.mSong.SongData.Id, this.mSongQueuePosition, this.SongSyncTimestamp));
            }

            if (HasLinkedItem)
            {
                if (this.mIsPlaying != true && this.mSong == null && this.mRoomOutputItem.ExtraData == "1" && this.mPlaylist.Count >= 1)
                {
                    this.Start(0);
                }
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this.mIsPlaying;
            }
        }

        public int PlaylistCapacity
        {
            get
            {
                return 20;
            }
        }

        public int PlaylistSize
        {
            get
            {
                return this.mPlaylist.Count;
            }
        }

        public int SongSyncTimestamp
        {
            get
            {
                if (!(this.mIsPlaying && (this.mSong != null)))
                {
                    return 0;
                }
                return (int)(mSong.SongData.Length);
            }
        }

        public double TimePlaying
        {
            get
            {
                return (PhoenixEnvironment.GetUnixTimestamp() - this.mStartedPlayingTimestamp);
            }
        }

        public bool HasLinkedItem
        {
            get
            {
                return (this.mRoomOutputItem != null);
            }
        }

        public int LinkedItemId
        {
            get
            {
                return ((this.mRoomOutputItem != null) ? ((int)this.mRoomOutputItem.Id) : 0);
            }
        }
    }
}