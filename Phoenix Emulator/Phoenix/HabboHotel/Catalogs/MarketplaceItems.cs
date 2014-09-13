using System;
namespace Phoenix.HabboHotel.Catalogs
{
	internal class MarketplaceItems
	{
		public uint OfferID;
		public int Sprite;
		public int TotalPrice;
		public int ItemType;

		public MarketplaceItems(uint mOfferID, int mSprite, int mTotalPrice, int mItemType)
		{
			OfferID = mOfferID;
			Sprite = mSprite;
			TotalPrice = mTotalPrice;
			ItemType = mItemType;
		}
	}
}
