using System;
using System.Collections.Generic;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.SoundMachine;
namespace Phoenix.Catalogs
{
	internal sealed class CatalogItem
	{
		public uint Id;
		public List<uint> ItemIds;
		public string Name;
		public int CreditsCost;
		public int PixelsCost;
		public int SnowCost;
		public int Amount;
		internal int PageID;
		internal uint uint_1;
		internal int VIP;
		internal uint uint_2;
        internal int song_id;

		public bool IsDeal
		{
			get
			{
				return this.ItemIds.Count > 1;
			}
		}

        public CatalogItem(uint mId, string mName, string string_2, int mCreditsCost, int mPixelsCost, int mAmount, int int_9, int int_10, int int_11, uint uint_4, int song_id)
		{
			this.Id = mId;
			this.Name = mName;
			this.ItemIds = new List<uint>();
			this.PageID = int_10;
			string[] array = string_2.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string s = array[i];
				this.ItemIds.Add(uint.Parse(s));
			}
			this.CreditsCost = mCreditsCost;
			this.PixelsCost = mPixelsCost;
			this.SnowCost = mAmount;
			this.Amount = int_9;
			this.VIP = int_11;
			this.uint_2 = uint_4;
			this.uint_1 = 7;
            this.song_id = song_id;
		}
		public Item GetBaseItem()
		{
			if (this.IsDeal)
			{
				return null;
			}
			else
			{
				return PhoenixEnvironment.GetGame().GetItemManager().GetItem(this.ItemIds[0]);
			}
		}
		public void Serialize(ServerMessage Message)
		{
			if (this.IsDeal)
			{
				throw new NotImplementedException("Multipile item ids set for catalog item #" + this.Id + ", but this is usupported at this point");
			}
			Message.AppendUInt(this.Id);
			if (this.Name.StartsWith("disc_"))
			{
                Message.AppendStringWithBreak(SongManager.GetSong(Convert.ToInt32(this.Name.Split(new char[]
				{
					'_'
				})[1])).Name);
			}
			else
			{
				Message.AppendStringWithBreak(this.Name);
			}
			Message.AppendInt32(this.CreditsCost);
			Message.AppendInt32(this.PixelsCost);
			Message.AppendInt32(this.SnowCost);
			Message.AppendInt32(1);
			Message.AppendStringWithBreak(this.GetBaseItem().Type.ToString());
			Message.AppendInt32(this.GetBaseItem().SpriteId);
			string text = "";
			if (this.Name.Contains("wallpaper_single") || this.Name.Contains("floor_single") || this.Name.Contains("landscape_single"))
			{
				string[] array = this.Name.Split(new char[]
				{
					'_'
				});
				text = array[2];
			}
			else
			{
				if (this.Name.StartsWith("disc_"))
				{
					text = this.Name.Split(new char[]
					{
						'_'
					})[1];
				}
				else
				{
					if (this.GetBaseItem().Name.StartsWith("poster_"))
					{
						text = this.GetBaseItem().Name.Split(new char[]
						{
							'_'
						})[1];
					}
				}
			}
			Message.AppendStringWithBreak(text);
			Message.AppendInt32(this.Amount);
			Message.AppendInt32(-1);
			Message.AppendInt32(this.VIP);
		}
	}
}
