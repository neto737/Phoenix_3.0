using System;
using System.Collections.Generic;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.SoundMachine;
namespace Phoenix.Catalogs
{
    internal class CatalogItem
    {
        public uint Id;
        public List<uint> ItemIds;
        public string Name;
        public int CreditsCost;
        public int PixelsCost;
        public int SnowCost;
        public int Amount;
        internal int PageID;
        internal uint MinRank;
        internal int VIP;
        internal uint Achievement;
        internal int song_id; //JukeBox (Don't work correctly)

        public bool IsDeal
        {
            get
            {
                return ItemIds.Count > 1;
            }
        }

        public CatalogItem(uint mId, string mName, string ItemIds, int mCreditsCost, int mPixelsCost, int mSnowCost, int mAmount, int mPageID, int mVIP, uint mAchievement, int song_id)
        {
            this.Id = mId;
            this.Name = mName;
            this.ItemIds = new List<uint>();
            this.PageID = mPageID;
            foreach (string str in ItemIds.Split(new char[] { ',' }))
            {
                this.ItemIds.Add(uint.Parse(str));
            }
            this.CreditsCost = mCreditsCost;
            this.PixelsCost = mPixelsCost;
            this.SnowCost = mSnowCost;
            this.Amount = mAmount;
            this.VIP = mVIP;
            this.Achievement = mAchievement;
            this.MinRank = 7;
            this.song_id = song_id; //JukeBox (Don't work correctly)
        }

        public Item GetBaseItem()
        {
            if (IsDeal)
            {
                return null;
            }
            else
            {
                return PhoenixEnvironment.GetGame().GetItemManager().GetItem(ItemIds[0]);
            }
        }

        public void Serialize(ServerMessage Message)
        {
            if (IsDeal)
            {
                throw new NotImplementedException("Multipile item ids set for catalog item #" + this.Id + ", but this is usupported at this point");
            }
            Message.AppendUInt(Id);
            if (Name.StartsWith("disc_"))
            {
                Message.AppendStringWithBreak(SongManager.GetSong(Convert.ToInt32(Name.Split(new char[] { '_' })[1])).Name);
            }
            else
            {
                Message.AppendStringWithBreak(Name);
            }
            Message.AppendInt32(CreditsCost);
            Message.AppendInt32(PixelsCost);
            Message.AppendInt32(SnowCost);
            Message.AppendInt32(1);
            Message.AppendStringWithBreak(GetBaseItem().Type.ToString());
            Message.AppendInt32(GetBaseItem().SpriteId);
            string s = "";
            if (Name.Contains("wallpaper_single") || Name.Contains("floor_single") || Name.Contains("landscape_single"))
            {
                s = Name.Split(new char[] { '_' })[2];
            }
            else if (Name.StartsWith("disc_"))
            {
                s = Name.Split(new char[] { '_' })[1];
            }
            else if (GetBaseItem().Name.StartsWith("poster_"))
            {
                s = GetBaseItem().Name.Split(new char[] { '_' })[1];
            }
            Message.AppendStringWithBreak(s);
            Message.AppendInt32(Amount);
            Message.AppendInt32(-1);
            Message.AppendInt32(VIP);
        }
    }
}
