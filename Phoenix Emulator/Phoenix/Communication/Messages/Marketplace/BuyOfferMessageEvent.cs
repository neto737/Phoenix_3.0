using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.Storage;
using Phoenix.Util;
namespace Phoenix.Communication.Messages.Marketplace
{
	internal class BuyOfferMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint ItemId = Event.PopWiredUInt();
			DataRow Row = null;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				Row = adapter.ReadDataRow("SELECT state, timestamp, total_price, extra_data, item_id, furni_id FROM catalog_marketplace_offers WHERE offer_id = '" + ItemId + "' LIMIT 1");
			}
			if (Row == null || (string)Row["state"] != "1" || (double)Row["timestamp"] <= PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().FormatTimestamp())
			{
				Session.SendNotif(TextManager.GetText("marketplace_error_expired"));
			}
			else
			{
				Item Item = PhoenixEnvironment.GetGame().GetItemManager().GetItem((uint)Row["item_id"]);
				if (Item != null)
				{
					if ((int)Row["total_price"] >= 1)
					{
						if (Session.GetHabbo().Credits < (int)Row["total_price"])
						{
							Session.SendNotif(TextManager.GetText("marketplace_error_credits"));
							return;
						}
						Session.GetHabbo().Credits -= (int)Row["total_price"];
						Session.GetHabbo().UpdateCreditsBalance(true);
					}
					PhoenixEnvironment.GetGame().GetCatalog().DeliverItems(Session, Item, 1, (string)Row["extra_data"], false, (uint)Row["furni_id"]);
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.ExecuteQuery("UPDATE catalog_marketplace_offers SET state = '2' WHERE offer_id = '" + ItemId + "' LIMIT 1");
						int num2 = 0;
						try
						{
							num2 = adapter.ReadInt32("SELECT Id FROM catalog_marketplace_data WHERE daysago = 0 AND sprite = " + Item.SpriteId + " LIMIT 1;");
						}
						catch
						{
						}
						if (num2 > 0)
						{
							adapter.ExecuteQuery(string.Concat(new object[]
							{
								"UPDATE catalog_marketplace_data SET sold = sold + 1, avgprice = (avgprice + ",
								(int)Row["total_price"],
								") WHERE Id = ",
								num2,
								" LIMIT 1;"
							}));
						}
						else
						{
							adapter.ExecuteQuery(string.Concat(new object[]
							{
								"INSERT INTO catalog_marketplace_data (sprite, sold, avgprice, daysago) VALUES ('",
								Item.SpriteId,
								"', 1, ",
								(int)Row["total_price"],
								", 0)"
							}));
						}
						if (PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketAverages.ContainsKey(Item.SpriteId) && PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketCounts.ContainsKey(Item.SpriteId))
						{
							int num3 = PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketCounts[Item.SpriteId];
							int num4 = PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketAverages[Item.SpriteId];
							num4 += (int)Row["total_price"];
							PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketAverages.Remove(Item.SpriteId);
							PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketAverages.Add(Item.SpriteId, num4);
							PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketCounts.Remove(Item.SpriteId);
							PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketCounts.Add(Item.SpriteId, num3 + 1);
						}
						else
						{
							if (!PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketAverages.ContainsKey(Item.SpriteId))
							{
								PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketAverages.Add(Item.SpriteId, (int)Row["total_price"]);
							}
							if (!PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketCounts.ContainsKey(Item.SpriteId))
							{
								PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().MarketCounts.Add(Item.SpriteId, 1);
							}
						}
					}
					ServerMessage Message = new ServerMessage(67);
					Message.AppendUInt(Item.ItemId);
					Message.AppendStringWithBreak(Item.Name);
					Message.AppendInt32((int)Row["total_price"]);
					Message.AppendInt32(0);
					Message.AppendInt32(0);
					Message.AppendInt32(1);
					Message.AppendStringWithBreak(Item.Type.ToString());
					Message.AppendInt32(Item.SpriteId);
					Message.AppendStringWithBreak("");
					Message.AppendInt32(1);
					Message.AppendInt32(-1);
					Message.AppendStringWithBreak("");
					Session.SendMessage(Message);
					Session.SendMessage(PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().SerializeOffersNew(-1, -1, "", 1));
				}
			}
		}
	}
}
