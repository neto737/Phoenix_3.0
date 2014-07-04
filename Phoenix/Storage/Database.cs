using System;
namespace Phoenix.Storage
{
    public class Database
    {
        private readonly uint mMaxPoolSize;
        private readonly uint mMinPoolSize;
        private readonly string mName;

        public Database(string sName, uint minPoolSize, uint maxPoolSize)
        {
            if ((sName == null) || (sName.Length == 0))
            {
                throw new ArgumentException(sName);
            }
            this.mName = sName;
            this.mMinPoolSize = minPoolSize;
            this.mMaxPoolSize = maxPoolSize;
        }

        public uint maxPoolSize
        {
            get
            {
                return this.mMaxPoolSize;
            }
        }

        public uint minPoolSize
        {
            get
            {
                return this.mMinPoolSize;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }
    }
}
