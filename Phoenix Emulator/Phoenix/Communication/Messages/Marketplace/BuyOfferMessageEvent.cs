using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.Storage;
using Phoenix.Util;
namespace Phoenix.Communication.Messages.Marketplace
{
	internal sealed class BuyOfferMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			DataRow dataRow = null;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				dataRow = @class.ReadDataRow("SELECT state, timestamp, total_price, extra_data, item_id, furni_id FROM catalog_marketplace_offers WHERE offer_id = '" + num + "' LIMIT 1");
			}
			if (dataRow == null || (string)dataRow["state"] != "1" || (double)dataRow["timestamp"] <= PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().method_3())
			{
				Session.SendNotif(TextManager.GetText("marketplace_error_expired"));
			}
			else
			{
				Item class2 = PhoenixEnvironment.GetGame().GetItemManager().GetItem((uint)dataRow["item_id"]);
				if (class2 != null)
				{
					if ((int)dataRow["total_price"] >= 1)
					{
						if (Session.GetHabbo().Credits < (int)dataRow["total_price"])
						{
							Session.SendNotif(TextManager.GetText("marketplace_error_credits"));
							return;
						}
						Session.GetHabbo().Credits -= (int)dataRow["total_price"];
						Session.GetHabbo().UpdateCreditsBalance(true);
					}
					PhoenixEnvironment.GetGame().GetCatalog().method_9(Session, class2, 1, (string)dataRow["extra_data"], false, (uint)dataRow["furni_id"]);
					using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
					{
						@class.ExecuteQuery("UPDATE catalog_marketplace_offers SET state = '2' WHERE offer_id = '" + num + "' LIMIT 1");
						int num2 = 0;
						try
						{
							num2 = @class.ReadInt32("SELECT Id FROM catalog_marketplace_data WHERE daysago = 0 AND sprite = " + class2.SpriteId + " LIMIT 1;");
						}
						catch
						{
						}
						if (num2 > 0)
						{
							@class.ExecuteQuery(string.Concat(new object[]
							{
								"UPDATE catalog_marketplace_data SET sold = sold + 1, avgprice = (avgprice + ",
								(int)dataRow["total_price"],
								") WHERE Id = ",
								num2,
								" LIMIT 1;"
							}));
						}
						else
						{
							@class.ExecuteQuery(string.Concat(new object[]
							{
								"INSERT INTO catalog_marketplace_data (sprite, sold, avgprice, daysago) VALUES ('",
								class2.SpriteId,
								"', 1, ",
								(int)dataRow["total_price"],
								", 0)"
							}));
						}
						if (PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_0.ContainsKey(class2.SpriteId) && PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_1.ContainsKey(class2.SpriteId))
						{
							int num3 = PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_1[class2.SpriteId];
							int num4 = PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_0[class2.SpriteId];
							num4 += (int)dataRow["total_price"];
							PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_0.Remove(class2.SpriteId);
							PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_0.Add(class2.SpriteId, num4);
							PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_1.Remove(class2.SpriteId);
							PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_1.Add(class2.SpriteId, num3 + 1);
						}
						else
						{
							if (!PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_0.ContainsKey(class2.SpriteId))
							{
								PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_0.Add(class2.SpriteId, (int)dataRow["total_price"]);
							}
							if (!PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_1.ContainsKey(class2.SpriteId))
							{
								PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().dictionary_1.Add(class2.SpriteId, 1);
							}
						}
					}
					ServerMessage Message = new ServerMessage(67u);
					Message.AppendUInt(class2.ItemId);
					Message.AppendStringWithBreak(class2.Name);
					Message.AppendInt32((int)dataRow["total_price"]);
					Message.AppendInt32(0);
					Message.AppendInt32(0);
					Message.AppendInt32(1);
					Message.AppendStringWithBreak(class2.Type.ToString());
					Message.AppendInt32(class2.SpriteId);
					Message.AppendStringWithBreak("");
					Message.AppendInt32(1);
					Message.AppendInt32(-1);
					Message.AppendStringWithBreak("");
					Session.SendMessage(Message);
					Session.SendMessage(PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().method_5(-1, -1, "", 1));
				}
			}
		}
	}
}
