using System;
using System.Collections.Generic;
namespace Phoenix.HabboHotel.Items
{
	internal sealed class Item
	{
		private uint Id;
		internal int Sprite;
		internal string PublicName;
		internal string Name;
		internal char Type;
		public int Width;
		public int Length;
		public double Height;
		public List<double> Height_Adjustable;
		public bool Stackable;
		public bool Walkable;
		public bool IsSeat;
		public bool AllowRecycle;
		public bool AllowTrade;
		public bool AllowMarketplaceSell;
		public bool AllowGift;
		public bool AllowInventoryStack;
		public string InteractionType;
		public byte EffectM;
		public byte EffectF;
		public List<int> VendingIds;
		public int Modes;
		public uint UInt32_0
		{
			get
			{
				return this.Id;
			}
		}
		public Item(uint Id, int Sprite, string PublicName, string Name, string Type, int Width, int Length, double Height, bool Stackable, bool Walkable, bool IsSeat, bool AllowRecycle, bool AllowTrade, bool AllowMarketplaceSell, bool AllowGift, bool AllowInventoryStack, string InteractionType, int Modes, string VendingIds, string Height_Adjustable, byte EffectF, byte EffectM)
		{
			this.Id = Id;
			this.Sprite = Sprite;
			this.PublicName = PublicName;
			this.Name = Name;
			this.Type = char.Parse(Type);
			this.Width = Width;
			this.Length = Length;
			this.Height = Height;
			this.Stackable = Stackable;
			this.Walkable = Walkable;
			this.IsSeat = IsSeat;
			this.AllowRecycle = AllowRecycle;
			this.AllowTrade = AllowTrade;
			this.AllowMarketplaceSell = AllowMarketplaceSell;
			this.AllowGift = AllowGift;
			this.AllowInventoryStack = AllowInventoryStack;
			this.InteractionType = InteractionType;
			this.Modes = Modes;
			this.VendingIds = new List<int>();
			this.Height_Adjustable = new List<double>();
			this.EffectF = EffectF;
			this.EffectM = EffectM;
			string[] array = VendingIds.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string s = array[i];
				this.VendingIds.Add(int.Parse(s));
			}
			array = Height_Adjustable.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string height = array[i];
				this.Height_Adjustable.Add(double.Parse(height));
			}
		}
	}
}
