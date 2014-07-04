using System;
namespace Phoenix.HabboHotel.Catalogs
{
	internal sealed class MarketplaceOffers
	{
		public uint uint_0;
		public int int_0;
		public int int_1;
		public int int_2;
		public MarketplaceOffers(uint OfferID, int sprite, int totalprice, int itemType)
		{
			this.uint_0 = OfferID;
			this.int_0 = sprite;
			this.int_1 = totalprice;
			this.int_2 = itemType;
		}
	}
}
