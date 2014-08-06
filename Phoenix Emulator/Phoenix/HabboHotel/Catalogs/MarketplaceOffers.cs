using System;
namespace Phoenix.HabboHotel.Catalogs
{
	internal sealed class MarketplaceOffers
	{
		public uint OfferID;
		public int Sprite;
		public int TotalPrice;
		public int ItemType;

		public MarketplaceOffers(uint mOfferID, int mSprite, int mTotalPrice, int mItemType)
		{
			OfferID = mOfferID;
			Sprite = mSprite;
			TotalPrice = mTotalPrice;
			ItemType = mItemType;
		}
	}
}
