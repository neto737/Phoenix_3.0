using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Pets;
using Phoenix.Util;
using Phoenix.Catalogs;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Catalogs
{
	internal class Catalog
	{
		public Dictionary<int, CatalogPage> Pages;
		public List<EcotronReward> EcotronRewards;
		private VoucherHandler VoucherHandler = new VoucherHandler();
		private Marketplace Marketplace = new Marketplace();
		private ServerMessage[] mCataIndexCache;
		private uint mCacheID;
		private readonly object ItemIDCacheProtect = new object();

		public void Initialize(DatabaseClient dbClient)
		{
			Logging.Write("Loading Catalogue..");
			this.Pages = new Dictionary<int, CatalogPage>();
			this.EcotronRewards = new List<EcotronReward>();
			DataTable Table = dbClient.ReadDataTable("SELECT * FROM catalog_pages ORDER BY order_num ASC");
			DataTable dataTable2 = dbClient.ReadDataTable("SELECT * FROM ecotron_rewards ORDER BY item_id");
			try
			{
				this.mCacheID = (uint)dbClient.ReadDataRow("SELECT ID FROM items ORDER BY ID DESC LIMIT 1")[0];
			}
			catch
			{
				this.mCacheID = 0;
			}
			this.mCacheID += 1;
			Hashtable cataItems = new Hashtable();
			DataTable dataTable3 = dbClient.ReadDataTable("SELECT * FROM catalog_items");
			if (dataTable3 != null)
			{
				foreach (DataRow dataRow in dataTable3.Rows)
				{
					if (!(dataRow["item_ids"].ToString() == "") && (int)dataRow["amount"] > 0)
					{
                        cataItems.Add((uint)dataRow["Id"], new CatalogItem((uint)dataRow["Id"], (string)dataRow["catalog_name"], (string)dataRow["item_ids"], (int)dataRow["cost_credits"], (int)dataRow["cost_pixels"], (int)dataRow["cost_snow"], (int)dataRow["amount"], (int)dataRow["page_id"], PhoenixEnvironment.EnumToInt(dataRow["vip"].ToString()), (uint)dataRow["achievement"], (int)dataRow["song_id"]));
					}
				}
			}
			if (Table != null)
			{
				foreach (DataRow Row in Table.Rows)
				{
					bool Visible = false;
					bool Enabled = false;
					if (Row["visible"].ToString() == "1")
					{
						Visible = true;
					}
					if (Row["enabled"].ToString() == "1")
					{
						Enabled = true;
					}
					this.Pages.Add((int)Row["Id"], new CatalogPage((int)Row["Id"], (int)Row["parent_id"], (string)Row["caption"], Visible, Enabled, (uint)Row["min_rank"], PhoenixEnvironment.EnumToBool(Row["club_only"].ToString()), (int)Row["icon_color"], (int)Row["icon_image"], (string)Row["page_layout"], (string)Row["page_headline"], (string)Row["page_teaser"], (string)Row["page_special"], (string)Row["page_text1"], (string)Row["page_text2"], (string)Row["page_text_details"], (string)Row["page_text_teaser"], (string)Row["page_link_description"], (string)Row["page_link_pagename"], ref cataItems));
				}
			}
			if (dataTable2 != null)
			{
				foreach (DataRow dataRow in dataTable2.Rows)
				{
					this.EcotronRewards.Add(new EcotronReward((uint)dataRow["Id"], (uint)dataRow["display_id"], (uint)dataRow["item_id"], (uint)dataRow["reward_level"]));
				}
			}
			Logging.WriteLine("completed!");
		}

		internal void InitCache()
		{
			Logging.Write("Loading Catalogue Cache..");
			int num = PhoenixEnvironment.GetGame().GetRoleManager().RankBadge.Count + 1;
			this.mCataIndexCache = new ServerMessage[num];
			for (int i = 1; i < num; i++)
			{
				this.mCataIndexCache[i] = this.SerializeIndexForCache(i);
			}
			foreach (CatalogPage current in this.Pages.Values)
			{
				current.InitMsg();
			}
			Logging.WriteLine("completed!");
		}

		public CatalogItem FindItem(uint ItemId)
		{
			foreach (CatalogPage Page in this.Pages.Values)
			{
				foreach (CatalogItem Item in Page.Items)
				{
					if (Item.Id == ItemId)
					{
						return Item;
					}
				}
			}
			return null;
		}

		public bool IsItemInCatalog(uint BaseId)
		{
			DataRow Row = null;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				Row = adapter.ReadDataRow("SELECT Id FROM catalog_items WHERE item_ids = '" + BaseId + "' LIMIT 1");
			}
			return Row != null;
		}

		public int GetTreeSize(int Rank, int TreeId)
		{
			int num = 0;
			foreach (CatalogPage Page in this.Pages.Values)
			{
				if ((Page.MinRank <= Rank) && (Page.ParentId == TreeId))
				{
					num++;
				}
			}
			return num;
		}

		public CatalogPage GetPage(int Page)
		{
			if (!Pages.ContainsKey(Page))
			{
				return null;
			}
			else
			{
				return Pages[Page];
			}
		}

		public bool HandlePurchase(GameClient Session, int int_0, uint uint_1, string string_0, bool bool_0, string string_1, string string_2, bool bool_1)
		{
			CatalogPage @class = this.GetPage(int_0);
			if (@class == null || !@class.Enabled || !@class.Visible || @class.MinRank > Session.GetHabbo().Rank)
			{
				return false;
			}
			else
			{
				if (@class.ClubOnly && (!Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") || !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip")))
				{
					return false;
				}
				else
				{
					CatalogItem class2 = @class.GetItem(uint_1);
					if (class2 == null)
					{
						return false;
					}
					else
					{
						uint num = 0u;
						if (bool_0)
						{
							if (!class2.GetBaseItem().AllowGift)
							{
								return false;
							}
							if (Session.GetHabbo().MaxFloodTime() > 0)
							{
								TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().FloodTime;
								if (timeSpan.Seconds > 4)
								{
									Session.GetHabbo().FloodCount = 0;
								}
								if (timeSpan.Seconds < 4 && Session.GetHabbo().FloodCount > 3)
								{
									Session.GetHabbo().Flooded = true;
									return false;
								}
								if (Session.GetHabbo().Flooded && timeSpan.Seconds < Session.GetHabbo().MaxFloodTime())
								{
									return false;
								}
								Session.GetHabbo().Flooded = false;
								Session.GetHabbo().FloodTime = DateTime.Now;
								Session.GetHabbo().FloodCount++;
							}
							using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								class3.AddParamWithValue("gift_user", string_1);
								try
								{
									num = (uint)class3.ReadDataRow("SELECT Id FROM users WHERE username = @gift_user LIMIT 1")[0];
								}
								catch (Exception)
								{
								}
							}
							if (num == 0u)
							{
								ServerMessage Message = new ServerMessage(76u);
								Message.AppendBoolean(true);
								Message.AppendStringWithBreak(string_1);
								Session.SendMessage(Message);
								return false;
							}
						}
						bool flag = false;
						bool flag2 = false;
						int int_ = class2.SnowCost;
						if (Session.GetHabbo().Credits < class2.CreditsCost)
						{
							flag = true;
						}
						if ((int_ == 0 && Session.GetHabbo().ActivityPoints < class2.PixelsCost) || (int_ > 0 && Session.GetHabbo().shells < class2.PixelsCost))
						{
							flag2 = true;
						}
						if (flag || flag2)
						{
							ServerMessage Message2 = new ServerMessage(68u);
							Message2.AppendBoolean(flag);
							Message2.AppendBoolean(flag2);
							Session.SendMessage(Message2);
							return false;
						}
						else
						{
							if (bool_0 && class2.GetBaseItem().Type == 'e')
							{
								Session.SendNotif("You can not send this item as a gift.");
								return false;
							}
							else
							{
								string text = class2.GetBaseItem().InteractionType.ToLower();
								if (text != null)
								{
									if (!(text == "pet"))
									{
										if (text == "roomeffect")
										{
											double num2 = 0.0;
											try
											{
												num2 = double.Parse(string_0);
											}
											catch (Exception)
											{
											}
											string_0 = num2.ToString().Replace(',', '.');
											goto IL_4FC;
										}
										if (text == "postit")
										{
											string_0 = "FFFF33";
											goto IL_4FC;
										}
										if (text == "dimmer")
										{
											string_0 = "1,1,1,#000000,255";
											goto IL_4FC;
										}
										if (text == "trophy")
										{
											string_0 = string.Concat(new object[]
											{
												Session.GetHabbo().Username,
												Convert.ToChar(9),
												DateTime.Now.Day,
												"-",
												DateTime.Now.Month,
												"-",
												DateTime.Now.Year,
												Convert.ToChar(9),
												ChatCommandHandler.ApplyWordFilter(PhoenixEnvironment.FilterInjectionChars(string_0, true, true))
											});
											goto IL_4FC;
										}
                                        if (text == "musicdisc")
                                        {
                                            string_0 = class2.song_id.ToString();
                                            goto IL_4FC;
                                        }
									}
									else
									{
										try
										{
											string[] array = string_0.Split(new char[]
											{
												'\n'
											});
											string string_3 = array[0];
											string text2 = array[1];
											string text3 = array[2];
											int.Parse(text2);
											if (!this.CheckPetName(string_3))
											{
												return false;
											}
											if (text2.Length > 2)
											{
												return false;
											}
											if (text3.Length != 6)
											{
												return false;
											}
											goto IL_4FC;
										}
										catch (Exception)
										{
											return false;
										}
									}
								}
								if (class2.Name.StartsWith("disc_"))
								{
									string_0 = class2.Name.Split(new char[]
									{
										'_'
									})[1];
								}
								else
								{
									string_0 = "";
								}
								IL_4FC:
								if (class2.CreditsCost > 0)
								{
									Session.GetHabbo().Credits -= class2.CreditsCost;
									Session.GetHabbo().UpdateCreditsBalance(true);
								}
								if (class2.PixelsCost > 0 && int_ == 0)
								{
									Session.GetHabbo().ActivityPoints -= class2.PixelsCost;
									Session.GetHabbo().UpdateActivityPointsBalance(true);
								}
								else
								{
									if (class2.PixelsCost > 0 && int_ > 0)
									{
										Session.GetHabbo().shells -= class2.PixelsCost;
										Session.GetHabbo().UpdateActivityPointsBalance(0);
										Session.GetHabbo().UpdateShellsBalance(false, true);
									}
								}
								ServerMessage Message3 = new ServerMessage(67u);
								Message3.AppendUInt(class2.GetBaseItem().ItemId);
								Message3.AppendStringWithBreak(class2.GetBaseItem().Name);
								Message3.AppendInt32(class2.CreditsCost);
								Message3.AppendInt32(class2.PixelsCost);
								Message3.AppendInt32(class2.SnowCost);
								if (bool_1)
								{
									Message3.AppendInt32(1);
								}
								else
								{
									Message3.AppendInt32(0);
								}
								Message3.AppendStringWithBreak(class2.GetBaseItem().Type.ToString());
								Message3.AppendInt32(class2.GetBaseItem().SpriteId);
								Message3.AppendStringWithBreak("");
								Message3.AppendInt32(1);
								Message3.AppendInt32(-1);
								Message3.AppendStringWithBreak("");
								Session.SendMessage(Message3);
								if (bool_0)
								{
									uint num3 = this.GenerateItemId();
									Item class4 = this.GeneratePresent();
									using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
									{
										class3.AddParamWithValue("gift_message", "!" + ChatCommandHandler.ApplyWordFilter(PhoenixEnvironment.FilterInjectionChars(string_2, true, true)) + " - " + Session.GetHabbo().Username);
										class3.AddParamWithValue("extra_data", string_0);
										class3.ExecuteQuery(string.Concat(new object[]
										{
											"INSERT INTO items (Id,user_id,base_item,extra_data,wall_pos) VALUES ('",
											num3,
											"','",
											num,
											"','",
											class4.ItemId,
											"',@gift_message,'')"
										}));
										class3.ExecuteQuery(string.Concat(new object[]
										{
											"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
											num3,
											"','",
											class2.GetBaseItem().ItemId,
											"','",
											class2.Amount,
											"',@extra_data)"
										}));
									}
									GameClient class5 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(num);
									int num4;
									if (class5 != null)
									{
										class5.SendNotif("You have received a gift! Check your inventory.");
										class5.GetHabbo().GetInventoryComponent().UpdateItems(true);
										class5.GetHabbo().GiftsReceived++;
										num4 = class5.GetHabbo().GiftsReceived;
										if (num4 <= 60)
										{
											if (num4 <= 15)
											{
												if (num4 != 5)
												{
													if (num4 == 15)
													{
														PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 2);
													}
												}
												else
												{
													PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 1);
												}
											}
											else
											{
												if (num4 != 35)
												{
													if (num4 != 50)
													{
														if (num4 == 60)
														{
															PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 5);
														}
													}
													else
													{
														PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 4);
													}
												}
												else
												{
													PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 3);
												}
											}
										}
										else
										{
											if (num4 <= 120)
											{
												if (num4 != 80)
												{
													if (num4 == 120)
													{
														PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 7);
													}
												}
												else
												{
													PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 6);
												}
											}
											else
											{
												if (num4 != 140)
												{
													if (num4 != 160)
													{
														if (num4 == 200)
														{
															PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 10);
														}
													}
													else
													{
														PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 9);
													}
												}
												else
												{
													PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(class5, 25u, 8);
												}
											}
										}
									}
									Session.GetHabbo().GiftsGiven++;
									num4 = Session.GetHabbo().GiftsGiven;
									if (num4 <= 60)
									{
										if (num4 <= 15)
										{
											if (num4 != 5)
											{
												if (num4 == 15)
												{
													PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 2);
												}
											}
											else
											{
												PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 1);
											}
										}
										else
										{
											if (num4 != 35)
											{
												if (num4 != 50)
												{
													if (num4 == 60)
													{
														PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 5);
													}
												}
												else
												{
													PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 4);
												}
											}
											else
											{
												PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 3);
											}
										}
									}
									else
									{
										if (num4 <= 120)
										{
											if (num4 != 80)
											{
												if (num4 == 120)
												{
													PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 7);
												}
											}
											else
											{
												PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 6);
											}
										}
										else
										{
											if (num4 != 140)
											{
												if (num4 != 160)
												{
													if (num4 == 200)
													{
														PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 10);
													}
												}
												else
												{
													PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 9);
												}
											}
											else
											{
												PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 24u, 8);
											}
										}
									}
									Session.SendNotif("Gift sent successfully!");
									return true;
								}
								else
								{
									this.DeliverItems(Session, class2.GetBaseItem(), class2.Amount, string_0, true, 0u);
									if (class2.Achievement > 0u)
									{
										PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, class2.Achievement, 1);
									}
									return true;
								}
							}
						}
					}
				}
			}
		}

		public void GiveGift(string GiftMessage, uint GiftUserId, uint ItemId, int PageId)
		{
			CatalogItem CataItem = GetPage(PageId).GetItem(ItemId);
			uint num = GenerateItemId();
			Item Item = GeneratePresent();

			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("gift_message", "!" + ChatCommandHandler.ApplyWordFilter(PhoenixEnvironment.FilterInjectionChars(GiftMessage, true, true)));
				adapter.ExecuteQuery(string.Concat(new object[] { "INSERT INTO items (Id,user_id,base_item,extra_data,wall_pos) VALUES ('", num, "','", GiftUserId, "','", Item.ItemId, "',@gift_message,'')" }));
				adapter.ExecuteQuery(string.Concat(new object[] { "INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('", num, "','", CataItem.GetBaseItem().ItemId, "','", CataItem.Amount, "','')" }));
			}
			GameClient clientByHabbo = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(GiftUserId);
			if (clientByHabbo != null)
			{
				clientByHabbo.SendNotif("You have received a gift! Check your inventory.");
				clientByHabbo.GetHabbo().GetInventoryComponent().UpdateItems(true);
			}
		}

		public bool CheckPetName(string PetName)
		{
            if ((PetName.Length < 1) || (PetName.Length > 16))
            {
                return false;
            }
            if (!PhoenixEnvironment.IsValidAlphaNumeric(PetName))
            {
                return false;
            }
            if (PetName != ChatCommandHandler.ApplyWordFilter(PetName))
            {
                return false;
            }
            return true;
		}

		public void DeliverItems(GameClient Session, Item Item, int int_0, string string_0, bool bool_0, uint uint_1)
		{
			string text = Item.Type.ToString();
			if (text != null)
			{
				if (text == "i" || text == "s")
				{
					int i = 0;
					while (i < int_0)
					{
						uint num;
						if (!bool_0 && uint_1 > 0u)
						{
							num = uint_1;
						}
						else
						{
							num = this.GenerateItemId();
						}
						text = Item.InteractionType.ToLower();
						if (text == null)
						{
							goto IL_4CF;
						}
						if (!(text == "pet"))
						{
							if (!(text == "teleport"))
							{
								if (!(text == "dimmer"))
								{
									goto IL_4CF;
								}
								using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
								{
									@class.ExecuteQuery("INSERT INTO room_items_moodlight (item_id,enabled,current_preset,preset_one,preset_two,preset_three) VALUES ('" + num + "','0','1','#000000,255,0','#000000,255,0','#000000,255,0')");
								}
								Session.GetHabbo().GetInventoryComponent().AddItem(num, Item.ItemId, string_0, bool_0);
							}
							else
							{
								uint num2 = this.GenerateItemId();
								using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
								{
									@class.ExecuteQuery(string.Concat(new object[]
									{
										"INSERT INTO tele_links (tele_one_id,tele_two_id) VALUES ('",
										num,
										"','",
										num2,
										"')"
									}));
									@class.ExecuteQuery(string.Concat(new object[]
									{
										"INSERT INTO tele_links (tele_one_id,tele_two_id) VALUES ('",
										num2,
										"','",
										num,
										"')"
									}));
								}
								Session.GetHabbo().GetInventoryComponent().AddItem(num2, Item.ItemId, "0", bool_0);
								Session.GetHabbo().GetInventoryComponent().AddItem(num, Item.ItemId, "0", bool_0);
							}
						}
						else
						{
							string[] array = string_0.Split(new char[]
							{
								'\n'
							});
							Pet class15_ = this.CreatePet(Session.GetHabbo().Id, array[0], Convert.ToInt32(Item.Name.Split(new char[]
							{
								't'
							})[1]), array[1], array[2]);
							Session.GetHabbo().GetInventoryComponent().AddPet(class15_);
							Session.GetHabbo().GetInventoryComponent().AddItem(num, 320u, "0", bool_0);
						}
						IL_4EA:
						ServerMessage Message = new ServerMessage(832u);
						Message.AppendInt32(1);
						if (Item.InteractionType.ToLower() == "pet")
						{
							Message.AppendInt32(3);
						}
						else
						{
							if (Item.Type.ToString() == "i")
							{
								Message.AppendInt32(2);
							}
							else
							{
								Message.AppendInt32(1);
							}
						}
						Message.AppendInt32(1);
						Message.AppendUInt(num);
						Session.SendMessage(Message);
						i++;
						continue;
						IL_4CF:
						Session.GetHabbo().GetInventoryComponent().AddItem(num, Item.ItemId, string_0, bool_0);
						goto IL_4EA;
					}
					Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
					return;
				}
				if (text == "e")
				{
					for (int i = 0; i < int_0; i++)
					{
						Session.GetHabbo().GetAvatarEffectsInventoryComponent().AddEffect(Item.SpriteId, 3600);
					}
					return;
				}
				if (text == "h")
				{
					for (int i = 0; i < int_0; i++)
					{
						Session.GetHabbo().GetSubscriptionManager().AddOrExtendSubscription("habbo_club", 2678400);
					}
					if (!Session.GetHabbo().GetBadgeComponent().HasBadge("HC1"))
					{
						Session.GetHabbo().GetBadgeComponent().GiveBadge(Session, "HC1", true);
					}
					ServerMessage Message2 = new ServerMessage(7u);
					Message2.AppendStringWithBreak("habbo_club");
					if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
					{
						double num3 = (double)Session.GetHabbo().GetSubscriptionManager().GetSubscription("habbo_club").ExpireTime;
						double num4 = num3 - PhoenixEnvironment.GetUnixTimestamp();
						int num5 = (int)Math.Ceiling(num4 / 86400.0);
						int num6 = num5 / 31;
						if (num6 >= 1)
						{
							num6--;
						}
						Message2.AppendInt32(num5 - num6 * 31);
						Message2.AppendBoolean(true);
						Message2.AppendInt32(num6);
					}
					else
					{
						for (int i = 0; i < 3; i++)
						{
							Message2.AppendInt32(0);
						}
					}
					Session.SendMessage(Message2);
					ServerMessage Message3 = new ServerMessage(2u);
					if (Session.GetHabbo().Vip || GlobalClass.VIPclothesforHCusers)
					{
						Message3.AppendInt32(2);
					}
					else
					{
						if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
						{
							Message3.AppendInt32(1);
						}
						else
						{
							Message3.AppendInt32(0);
						}
					}
					if (Session.GetHabbo().HasRole("acc_anyroomowner"))
					{
						Message3.AppendInt32(7);
					}
					else
					{
						if (Session.GetHabbo().HasRole("acc_anyroomrights"))
						{
							Message3.AppendInt32(5);
						}
						else
						{
							if (Session.GetHabbo().HasRole("acc_supporttool"))
							{
								Message3.AppendInt32(4);
							}
							else
							{
								if (Session.GetHabbo().Vip || GlobalClass.VIPclothesforHCusers || Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
								{
									Message3.AppendInt32(2);
								}
								else
								{
									Message3.AppendInt32(0);
								}
							}
						}
					}
					Session.SendMessage(Message3);
					return;
				}
			}
			Session.SendNotif("Something went wrong! The item type could not be processed. Please do not try to buy this item anymore, instead inform support as soon as possible.");
		}

        public Item GeneratePresent()
        {
            switch (PhoenixEnvironment.GetRandomNumber(0, 6))
            {
                case 0:
                    {
                        return PhoenixEnvironment.GetGame().GetItemManager().GetItem(164);
                    }
                case 1:
                    {
                        return PhoenixEnvironment.GetGame().GetItemManager().GetItem(165);
                    }
                case 2:
                    {
                        return PhoenixEnvironment.GetGame().GetItemManager().GetItem(166);
                    }
                case 3:
                    {
                        return PhoenixEnvironment.GetGame().GetItemManager().GetItem(167);
                    }
                case 4:
                    {
                        return PhoenixEnvironment.GetGame().GetItemManager().GetItem(168);
                    }
                case 5:
                    {
                        return PhoenixEnvironment.GetGame().GetItemManager().GetItem(169);
                    }
                case 6:
                    {
                        return PhoenixEnvironment.GetGame().GetItemManager().GetItem(170);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

		public Pet CreatePet(uint UserId, string Name, int Type, string Race, string Color)
		{
			return new Pet(this.GenerateItemId(), UserId, 0, Name, (uint)Type, Race, Color, 0, 100, 100, 0, PhoenixEnvironment.GetUnixTimestamp(), 0, 0, 0.0) {    DBState = DatabaseUpdateState.NeedsInsert   };
		}

		public Pet GeneratePetFromRow(DataRow Row)
		{
			if (Row == null)
			{
				return null;
			}
			else
			{
				return new Pet((uint)Row["Id"], (uint)Row["user_id"], (uint)Row["room_id"], (string)Row["name"], (uint)Row["type"], (string)Row["race"], (string)Row["color"], (int)Row["expirience"], (int)Row["energy"], (int)Row["nutrition"], (int)Row["respect"], (double)Row["createstamp"], (int)Row["x"], (int)Row["y"], (double)Row["z"]);
			}
		}

		internal Pet GeneratePetFromRow(DataRow Row, uint PetID)
		{
			if (Row == null)
			{
				return null;
			}
			else
			{
				return new Pet(PetID, (uint)Row["user_id"], (uint)Row["room_id"], (string)Row["name"], (uint)Row["type"], (string)Row["race"], (string)Row["color"], (int)Row["expirience"], (int)Row["energy"], (int)Row["nutrition"], (int)Row["respect"], (double)Row["createstamp"], (int)Row["x"], (int)Row["y"], (double)Row["z"]);
			}
		}

		internal uint GenerateItemId()
		{
			lock (ItemIDCacheProtect)
			{
				return mCacheID++;
			}
		}

        public EcotronReward GetRandomEcotronReward()
        {
            uint level = 1;
            if (PhoenixEnvironment.GetRandomNumber(1, 2000) == 2000)
            {
                level = 5;
            }
            else if (PhoenixEnvironment.GetRandomNumber(1, 200) == 200)
            {
                level = 4;
            }
            else if (PhoenixEnvironment.GetRandomNumber(1, 40) == 40)
            {
                level = 3;
            }
            else if (PhoenixEnvironment.GetRandomNumber(1, 4) == 4)
            {
                level = 2;
            }
            List<EcotronReward> ecotronRewardsForLevel = this.GetEcotronRewardsForLevel(level);
            if (ecotronRewardsForLevel != null && ecotronRewardsForLevel.Count >= 1)
            {
                return ecotronRewardsForLevel[PhoenixEnvironment.GetRandomNumber(0, ecotronRewardsForLevel.Count - 1)];
            }
            else
            {
                return new EcotronReward(0u, 0, 1479, 0);
            }
        }

		public List<EcotronReward> GetEcotronRewardsForLevel(uint Level)
		{
			List<EcotronReward> list = new List<EcotronReward>();
			foreach (EcotronReward Reward in this.EcotronRewards)
			{
				if (Reward.RewardLevel == Level)
				{
					list.Add(Reward);
				}
			}
			return list;
		}

		public ServerMessage SerializeIndexForCache(int Rank)
		{
			ServerMessage Message = new ServerMessage(126);
			Message.AppendBoolean(true);
			Message.AppendInt32(0);
			Message.AppendInt32(0);
			Message.AppendInt32(-1);
			Message.AppendStringWithBreak("");
			Message.AppendInt32(GetTreeSize(Rank, -1));
			Message.AppendBoolean(true);
			foreach (CatalogPage Page in Pages.Values)
			{
				if (Page.ParentId == -1)
				{
					Page.Serialize(Rank, Message);
					foreach (CatalogPage page in Pages.Values)
					{
						if (page.ParentId == Page.PageId)
						{
							page.Serialize(Rank, Message);
						}
					}
				}
			}
			return Message;
		}

		internal ServerMessage GetIndexMessageForRank(uint Rank)
		{
			if (Rank < 1)
			{
				Rank = 1;
			}
			if (Rank > PhoenixEnvironment.GetGame().GetRoleManager().RankBadge.Count)
			{
				Rank = (uint)PhoenixEnvironment.GetGame().GetRoleManager().RankBadge.Count;
			}
			return mCataIndexCache[Rank];
		}

        public ServerMessage SerializePage(CatalogPage Page)
        {
            ServerMessage message = new ServerMessage(0x7f);
            message.AppendInt32(Page.PageId);
            switch (Page.Layout)
            {
                case "frontpage":
                    message.AppendStringWithBreak("frontpage3");
                    message.AppendInt32(3);
                    message.AppendStringWithBreak(Page.LayoutHeadline);
                    message.AppendStringWithBreak(Page.LayoutTeaser);
                    message.AppendStringWithBreak("");
                    message.AppendInt32(11);
                    message.AppendStringWithBreak(Page.Text1);
                    message.AppendStringWithBreak(Page.TextLinkDesc);
                    message.AppendStringWithBreak(Page.Text2);
                    message.AppendStringWithBreak(Page.TextDetails);
                    message.AppendStringWithBreak(Page.TextLinkPage);
                    message.AppendStringWithBreak("#FAF8CC");
                    message.AppendStringWithBreak("#FAF8CC");
                    message.AppendStringWithBreak("Read More >");
                    message.AppendStringWithBreak("magic.credits");
                    break;

                case "recycler_info":
                    message.AppendStringWithBreak(Page.Layout);
                    message.AppendInt32(2);
                    message.AppendStringWithBreak(Page.LayoutHeadline);
                    message.AppendStringWithBreak(Page.LayoutTeaser);
                    message.AppendInt32(3);
                    message.AppendStringWithBreak(Page.Text1);
                    message.AppendStringWithBreak(Page.Text2);
                    message.AppendStringWithBreak(Page.TextDetails);
                    break;

                case "recycler_prizes":
                    message.AppendStringWithBreak("recycler_prizes");
                    message.AppendInt32(1);
                    message.AppendStringWithBreak("catalog_recycler_headline3");
                    message.AppendInt32(1);
                    message.AppendStringWithBreak(Page.Text1);
                    break;

                case "spaces":
                    message.AppendStringWithBreak("spaces_new");
                    message.AppendInt32(1);
                    message.AppendStringWithBreak(Page.LayoutHeadline);
                    message.AppendInt32(1);
                    message.AppendStringWithBreak(Page.Text1);
                    break;

                case "recycler":
                    message.AppendStringWithBreak(Page.Layout);
                    message.AppendInt32(2);
                    message.AppendStringWithBreak(Page.LayoutHeadline);
                    message.AppendStringWithBreak(Page.LayoutTeaser);
                    message.AppendInt32(1);
                    message.AppendStringWithBreak(Page.Text1, 10);
                    message.AppendStringWithBreak(Page.Text2);
                    message.AppendStringWithBreak(Page.TextDetails);
                    break;

                case "trophies":
                    message.AppendStringWithBreak("trophies");
                    message.AppendInt32(1);
                    message.AppendStringWithBreak(Page.LayoutHeadline);
                    message.AppendInt32(2);
                    message.AppendStringWithBreak(Page.Text1);
                    message.AppendStringWithBreak(Page.TextDetails);
                    break;

                case "pets":
                    message.AppendStringWithBreak("pets");
                    message.AppendInt32(2);
                    message.AppendStringWithBreak(Page.LayoutHeadline);
                    message.AppendStringWithBreak(Page.LayoutTeaser);
                    message.AppendInt32(4);
                    message.AppendStringWithBreak(Page.Text1);
                    message.AppendStringWithBreak("");
                    message.AppendStringWithBreak("Pick a color:");
                    message.AppendStringWithBreak("Pick a race:");
                    break;

                case "club_buy":
                    message.AppendStringWithBreak("club_buy");
                    message.AppendInt32(1);
                    message.AppendStringWithBreak("habboclub_2");
                    message.AppendInt32(1);
                    break;

                case "club_gifts":
                    message.AppendStringWithBreak("club_gifts");
                    message.AppendInt32(1);
                    message.AppendStringWithBreak("habboclub_2");
                    message.AppendInt32(1);
                    message.AppendStringWithBreak("");
                    message.AppendInt32(1);
                    break;

                case "soundmachine":
                    message.AppendStringWithBreak(Page.Layout);
                    message.AppendInt32(2);
                    message.AppendStringWithBreak(Page.LayoutHeadline);
                    message.AppendStringWithBreak(Page.LayoutTeaser);
                    message.AppendInt32(2);
                    message.AppendStringWithBreak(Page.Text1);
                    message.AppendStringWithBreak(Page.TextDetails);
                    break;

                default:
                    message.AppendStringWithBreak(Page.Layout);
                    message.AppendInt32(3);
                    message.AppendStringWithBreak(Page.LayoutHeadline);
                    message.AppendStringWithBreak(Page.LayoutTeaser);
                    message.AppendStringWithBreak(Page.LayoutSpecial);
                    message.AppendInt32(3);
                    message.AppendStringWithBreak(Page.Text1);
                    message.AppendStringWithBreak(Page.TextDetails);
                    message.AppendStringWithBreak(Page.TextTeaser);
                    break;
            }
            message.AppendInt32(Page.Items.Count);
            foreach (CatalogItem item in Page.Items)
            {
                item.Serialize(message);
            }
            return message;
        }

		public ServerMessage ClubPage()
		{
			return new ServerMessage(625);
		}
		public VoucherHandler GetVoucherHandler()
		{
			return this.VoucherHandler;
		}
		public Marketplace GetMarketplace()
		{
			return this.Marketplace;
		}
	}
}
