using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Catalogs
{
	internal class Marketplace
	{
		public List<uint> MarketItemKeys;
		public List<MarketplaceItems> MarketplaceItems;
		public Dictionary<int, int> MarketAverages;
		public Dictionary<int, int> MarketCounts;

		public Marketplace()
		{
			this.MarketItemKeys = new List<uint>();
			this.MarketplaceItems = new List<MarketplaceItems>();
			this.MarketAverages = new Dictionary<int, int>();
			this.MarketCounts = new Dictionary<int, int>();
		}

		public bool CanSellItem(UserItem Item)
		{
			return Item.GetBaseItem().AllowTrade && Item.GetBaseItem().AllowMarketplaceSell;
		}

		public void SellItem(GameClient Session, uint ItemId, int SellingPrice)
		{
			UserItem Item = Session.GetHabbo().GetInventoryComponent().GetItem(ItemId);
			if (Item == null || SellingPrice > GlobalClass.MaxMarketPlacePrice || !this.CanSellItem(Item))
			{
				ServerMessage Message = new ServerMessage(610u);
				Message.AppendBoolean(false);
				Session.SendMessage(Message);
			}
			else
			{
				int num = this.CalculateComissionPrice((float)SellingPrice);
				int num2 = SellingPrice + num;
				int num3 = 1;
				if (Item.GetBaseItem().Type == 'i')
				{
					num3++;
				}
				using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					class2.AddParamWithValue("public_name", Item.GetBaseItem().PublicName);
					class2.AddParamWithValue("extra_data", Item.ExtraData);
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO catalog_marketplace_offers (furni_id, item_id,user_id,asking_price,total_price,public_name,sprite_id,item_type,timestamp,extra_data) VALUES ('",
						ItemId,
						"','",
						Item.BaseItem,
						"','",
						Session.GetHabbo().Id,
						"','",
						SellingPrice,
						"','",
						num2,
						"',@public_name,'",
						Item.GetBaseItem().SpriteId,
						"','",
						num3,
						"','",
						PhoenixEnvironment.GetUnixTimestamp(),
						"',@extra_data)"
					}));
				}
				Session.GetHabbo().GetInventoryComponent().RemoveItem(ItemId, 0u, true);
				ServerMessage Message2 = new ServerMessage(610u);
				Message2.AppendBoolean(true);
				Session.SendMessage(Message2);
			}
		}

		public int CalculateComissionPrice(float SellingPrice)
		{
			double num = (double)(SellingPrice / 100f);
			return (int)Math.Ceiling((double)((float)(num * (double)GlobalClass.MarketPlaceTax)));
		}

		internal double FormatTimestamp()
		{
			return PhoenixEnvironment.GetUnixTimestamp() - 172800.0;
		}

		internal string FormatTimestampString()
		{
            return this.FormatTimestamp().ToString().Split(new char[] { ',' })[0];
		}

		public ServerMessage SerializeOffersNew(int MinCost, int MaxCost, string SearchQuery, int FilterMode)
		{
			DataTable Table = null;
			StringBuilder Builder = new StringBuilder();
			Builder.Append("WHERE state = '1' AND timestamp >= " + this.FormatTimestampString());
			if (MinCost >= 0)
			{
				Builder.Append(" AND total_price > " + MinCost);
			}
			if (MaxCost >= 0)
			{
				Builder.Append(" AND total_price < " + MaxCost);
			}
			string str;
			switch (FilterMode)
			{
			case 1:
				str = "ORDER BY asking_price DESC";
				goto IL_82;
			}
			str = "ORDER BY asking_price ASC";
			IL_82:
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("search_query", "%" + SearchQuery + "%");
				if (SearchQuery.Length >= 1)
				{
					Builder.Append(" AND public_name LIKE @search_query");
				}
				Table = adapter.ReadDataTable(string.Concat(new string[]
				{
					"SELECT offer_id, item_type, sprite_id, total_price FROM catalog_marketplace_offers ",
					Builder.ToString(),
					" ",
					str,
					" LIMIT 500"
				}));
			}
			ServerMessage Message = new ServerMessage(615u);
			this.MarketplaceItems.Clear();
			this.MarketItemKeys.Clear();
			if (Table != null)
			{
				foreach (DataRow dataRow in Table.Rows)
				{
					if (!this.MarketItemKeys.Contains((uint)dataRow["offer_id"]))
					{
						MarketplaceItems item = new MarketplaceItems((uint)dataRow["offer_id"], (int)dataRow["sprite_id"], (int)dataRow["total_price"], int.Parse(dataRow["item_type"].ToString()));
						this.MarketItemKeys.Add((uint)dataRow["offer_id"]);
						this.MarketplaceItems.Add(item);
					}
				}
				return this.SerializeOffers(MinCost, MaxCost);
			}
			else
			{
				Message.AppendInt32(0);
				return Message;
			}
		}

		public ServerMessage SerializeOffers(int MinCost, int MaxCost)
		{
			Dictionary<int, MarketplaceItems> MarketItems = new Dictionary<int, MarketplaceItems>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			ServerMessage Message = new ServerMessage(615);
			foreach (MarketplaceItems Item in this.MarketplaceItems)
			{
				if (MarketItems.ContainsKey(Item.Sprite))
				{
					if (MarketItems[Item.Sprite].TotalPrice > Item.TotalPrice)
					{
						MarketItems.Remove(Item.Sprite);
						MarketItems.Add(Item.Sprite, Item);
					}
					int num = dictionary2[Item.Sprite];
					dictionary2.Remove(Item.Sprite);
					dictionary2.Add(Item.Sprite, num + 1);
				}
				else
				{
					MarketItems.Add(Item.Sprite, Item);
					dictionary2.Add(Item.Sprite, 1);
				}
			}
			if (MarketItems.Count > 0)
			{
				Message.AppendInt32(MarketItems.Count);
				using (Dictionary<int, MarketplaceItems>.Enumerator enumerator2 = MarketItems.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, MarketplaceItems> current2 = enumerator2.Current;
						Message.AppendUInt(current2.Value.OfferID);
						Message.AppendInt32(1);
						Message.AppendInt32(current2.Value.ItemType);
						Message.AppendInt32(current2.Value.Sprite);
						Message.AppendStringWithBreak("");
						Message.AppendInt32(current2.Value.TotalPrice);
						Message.AppendInt32(current2.Value.Sprite);
						Message.AppendInt32(this.AvgPriceForSprite(current2.Value.Sprite));
						Message.AppendInt32(dictionary2[current2.Value.Sprite]);
					}
					return Message;
				}
			}
			Message.AppendInt32(0);
			return Message;
		}

		public int OfferCountForSprite(int SpriteID)
		{
			Dictionary<int, MarketplaceItems> dictionary = new Dictionary<int, MarketplaceItems>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			foreach (MarketplaceItems current in this.MarketplaceItems)
			{
				if (dictionary.ContainsKey(current.Sprite))
				{
					if (dictionary[current.Sprite].TotalPrice > current.TotalPrice)
					{
						dictionary.Remove(current.Sprite);
						dictionary.Add(current.Sprite, current);
					}
					int num = dictionary2[current.Sprite];
					dictionary2.Remove(current.Sprite);
					dictionary2.Add(current.Sprite, num + 1);
				}
				else
				{
					dictionary.Add(current.Sprite, current);
					dictionary2.Add(current.Sprite, 1);
				}
			}
			if (dictionary2.ContainsKey(SpriteID))
			{
				return dictionary2[SpriteID];
			}
			else
			{
				return 0;
			}
		}

		public int AvgPriceForSprite(int SpriteID)
		{
			int num = 0;
			int num2 = 0;
			if (this.MarketAverages.ContainsKey(SpriteID) && this.MarketCounts.ContainsKey(SpriteID))
			{
				if (this.MarketCounts[SpriteID] > 0)
				{
					return this.MarketAverages[SpriteID] / this.MarketCounts[SpriteID];
				}
				else
				{
					return 0;
				}
			}
			else
			{
				try
				{
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						num = adapter.ReadInt32("SELECT avgprice FROM catalog_marketplace_data WHERE sprite = '" + SpriteID + "' AND daysago = 0 LIMIT 1;");
						num2 = adapter.ReadInt32("SELECT sold FROM catalog_marketplace_data WHERE sprite = '" + SpriteID + "' AND daysago = 0 LIMIT 1;");
					}
				}
                catch { }

				this.MarketAverages.Add(SpriteID, num);
				this.MarketCounts.Add(SpriteID, num2);
				if (num2 > 0)
				{
					return (int)Math.Ceiling((double)((float)(num / num2)));
				}
				else
				{
					return 0;
				}
			}
		}

		public ServerMessage SerializeOwnOffers(uint HabboId)
		{
			int i = 0;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable Table = adapter.ReadDataTable("SELECT timestamp, state, offer_id, item_type, sprite_id, total_price FROM catalog_marketplace_offers WHERE user_id = '" + HabboId + "'");
				string s = adapter.ReadDataRow("SELECT SUM(asking_price) FROM catalog_marketplace_offers WHERE state = '2' AND user_id = '" + HabboId + "'")[0].ToString();
				if (s.Length > 0)
				{
					i = int.Parse(s);
				}
				ServerMessage Message = new ServerMessage(616);
				Message.AppendInt32(i);
				if (Table != null)
				{
					Message.AppendInt32(Table.Rows.Count);
					IEnumerator enumerator = Table.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							int num = (int)Math.Floor(((double)dataRow["timestamp"] + 172800.0 - PhoenixEnvironment.GetUnixTimestamp()) / 60.0);
							int num2 = int.Parse(dataRow["state"].ToString());
							if (num <= 0 && num2 != 2)
							{
								num2 = 3;
								num = 0;
							}
							Message.AppendUInt((uint)dataRow["offer_id"]);
							Message.AppendInt32(num2);
							Message.AppendInt32(int.Parse(dataRow["item_type"].ToString()));
							Message.AppendInt32((int)dataRow["sprite_id"]);
							Message.AppendStringWithBreak("");
							Message.AppendInt32((int)dataRow["total_price"]);
							Message.AppendInt32(num);
							Message.AppendInt32((int)dataRow["sprite_id"]);
						}
						goto IL_1DE;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				Message.AppendInt32(0);
				IL_1DE:
				return Message;
			}
		}
	}
}
