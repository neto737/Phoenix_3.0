using System;

namespace Phoenix.HabboHotel.SoundMachine
{
    internal class SongData
    {
        private int mId;
        private string mName;
        private string mAuthor;
        private string mTrack;
        private int mLength;

        public SongData(int id, string name, string author, string track, int length)
        {
            this.mId = id;
            this.mName = name;
            this.mAuthor = author;
            this.mTrack = track;
            this.mLength = length * 1000;
        }

        public int Id
        {
            get
            {
                return this.mId;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public string Author
        {
            get
            {
                return this.mAuthor;
            }
        }

        public string Track
        {
            get
            {
                return this.mTrack;
            }
        }

        public int Length
        {
            get
            {
                return this.mLength;
            }
        }
    }
}