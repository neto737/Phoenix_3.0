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
	internal sealed class Catalog
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
			DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM catalog_pages ORDER BY order_num ASC");
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
			Hashtable hashtable = new Hashtable();
			DataTable dataTable3 = dbClient.ReadDataTable("SELECT * FROM catalog_items");
			if (dataTable3 != null)
			{
				foreach (DataRow dataRow in dataTable3.Rows)
				{
					if (!(dataRow["item_ids"].ToString() == "") && (int)dataRow["amount"] > 0)
					{
                        hashtable.Add((uint)dataRow["Id"], new CatalogItem((uint)dataRow["Id"], (string)dataRow["catalog_name"], (string)dataRow["item_ids"], (int)dataRow["cost_credits"], (int)dataRow["cost_pixels"], (int)dataRow["cost_snow"], (int)dataRow["amount"], (int)dataRow["page_id"], PhoenixEnvironment.EnumToInt(dataRow["vip"].ToString()), (uint)dataRow["achievement"], (int)dataRow["song_id"]));
					}
				}
			}
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					bool bool_ = false;
					bool bool_2 = false;
					if (dataRow["visible"].ToString() == "1")
					{
						bool_ = true;
					}
					if (dataRow["enabled"].ToString() == "1")
					{
						bool_2 = true;
					}
					this.Pages.Add((int)dataRow["Id"], new CatalogPage((int)dataRow["Id"], (int)dataRow["parent_id"], (string)dataRow["caption"], bool_, bool_2, (uint)dataRow["min_rank"], PhoenixEnvironment.EnumToBool(dataRow["club_only"].ToString()), (int)dataRow["icon_color"], (int)dataRow["icon_image"], (string)dataRow["page_layout"], (string)dataRow["page_headline"], (string)dataRow["page_teaser"], (string)dataRow["page_special"], (string)dataRow["page_text1"], (string)dataRow["page_text2"], (string)dataRow["page_text_details"], (string)dataRow["page_text_teaser"], (string)dataRow["page_link_description"], (string)dataRow["page_link_pagename"], ref hashtable));
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
				this.mCataIndexCache[i] = this.method_17(i);
			}
			foreach (CatalogPage current in this.Pages.Values)
			{
				current.InitMsg();
			}
			Logging.WriteLine("completed!");
		}

		public CatalogItem FindItem(uint uint_1)
		{
			foreach (CatalogPage current in this.Pages.Values)
			{
				foreach (CatalogItem current2 in current.Items)
				{
					if (current2.Id == uint_1)
					{
						return current2;
					}
				}
			}
			return null;
		}
		public bool IsItemInCatalog(uint BaseId)
		{
			DataRow dataRow = null;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				dataRow = @class.ReadDataRow("SELECT Id FROM catalog_items WHERE item_ids = '" + BaseId + "' LIMIT 1");
			}
			return dataRow != null;
		}
		public int GetTreeSize(int int_0, int int_1)
		{
			int num = 0;
			foreach (CatalogPage current in this.Pages.Values)
			{
				if ((ulong)current.MinRank <= (ulong)((long)int_0) && current.ParentId == int_1)
				{
					num++;
				}
			}
			return num;
		}
		public CatalogPage method_5(int int_0)
		{
			if (!this.Pages.ContainsKey(int_0))
			{
				return null;
			}
			else
			{
				return this.Pages[int_0];
			}
		}
		public bool HandlePurchase(GameClient Session, int int_0, uint uint_1, string string_0, bool bool_0, string string_1, string string_2, bool bool_1)
		{
			CatalogPage @class = this.method_5(int_0);
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
									Session.GetHabbo().bool_15 = true;
									return false;
								}
								if (Session.GetHabbo().bool_15 && timeSpan.Seconds < Session.GetHabbo().MaxFloodTime())
								{
									return false;
								}
								Session.GetHabbo().bool_15 = false;
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
											if (!this.method_8(string_3))
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
									Item class4 = this.method_10();
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
									if (class2.uint_2 > 0u)
									{
										PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, class2.uint_2, 1);
									}
									return true;
								}
							}
						}
					}
				}
			}
		}
		public void GiveGift(string string_0, uint uint_1, uint uint_2, int int_0)
		{
			CatalogPage @class = this.method_5(int_0);
			CatalogItem class2 = @class.GetItem(uint_2);
			uint num = this.GenerateItemId();
			Item class3 = this.method_10();
			using (DatabaseClient class4 = PhoenixEnvironment.GetDatabase().GetClient())
			{
				class4.AddParamWithValue("gift_message", "!" + ChatCommandHandler.ApplyWordFilter(PhoenixEnvironment.FilterInjectionChars(string_0, true, true)));
				class4.ExecuteQuery(string.Concat(new object[]
				{
					"INSERT INTO items (Id,user_id,base_item,extra_data,wall_pos) VALUES ('",
					num,
					"','",
					uint_1,
					"','",
					class3.ItemId,
					"',@gift_message,'')"
				}));
				class4.ExecuteQuery(string.Concat(new object[]
				{
					"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
					num,
					"','",
					class2.GetBaseItem().ItemId,
					"','",
					class2.Amount,
					"','')"
				}));
			}
			GameClient class5 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_1);
			if (class5 != null)
			{
				class5.SendNotif("You have received a gift! Check your inventory.");
				class5.GetHabbo().GetInventoryComponent().UpdateItems(true);
			}
		}
		public bool method_8(string string_0)
		{
			return string_0.Length >= 1 && string_0.Length <= 16 && PhoenixEnvironment.IsValidAlphaNumeric(string_0) && !(string_0 != ChatCommandHandler.ApplyWordFilter(string_0));
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
							Pet class15_ = this.method_11(Session.GetHabbo().Id, array[0], Convert.ToInt32(Item.Name.Split(new char[]
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
		public Item method_10()
		{
			switch (PhoenixEnvironment.GetRandomNumber(0, 6))
			{
			case 0:
			{
                return PhoenixEnvironment.GetGame().GetItemManager().GetItem(164u);
			}
			case 1:
			{
				return PhoenixEnvironment.GetGame().GetItemManager().GetItem(165u);
			}
			case 2:
			{
				return PhoenixEnvironment.GetGame().GetItemManager().GetItem(166u);
			}
			case 3:
			{
				return PhoenixEnvironment.GetGame().GetItemManager().GetItem(167u);
			}
			case 4:
			{
                return PhoenixEnvironment.GetGame().GetItemManager().GetItem(168u);
			}
			case 5:
			{
                return PhoenixEnvironment.GetGame().GetItemManager().GetItem(169u);
			}
			case 6:
			{
                return PhoenixEnvironment.GetGame().GetItemManager().GetItem(170u);
			}
            default:
            {
                return null;
            }
			}
		}
		public Pet method_11(uint uint_1, string string_0, int int_0, string string_1, string string_2)
		{
			return new Pet(this.GenerateItemId(), uint_1, 0u, string_0, (uint)int_0, string_1, string_2, 0, 100, 100, 0, PhoenixEnvironment.GetUnixTimestamp(), 0, 0, 0.0)
			{
				DBState = DatabaseUpdateState.NeedsInsert
			};
		}
		public Pet method_12(DataRow dataRow_0)
		{
			if (dataRow_0 == null)
			{
				return null;
			}
			else
			{
				return new Pet((uint)dataRow_0["Id"], (uint)dataRow_0["user_id"], (uint)dataRow_0["room_id"], (string)dataRow_0["name"], (uint)dataRow_0["type"], (string)dataRow_0["race"], (string)dataRow_0["color"], (int)dataRow_0["expirience"], (int)dataRow_0["energy"], (int)dataRow_0["nutrition"], (int)dataRow_0["respect"], (double)dataRow_0["createstamp"], (int)dataRow_0["x"], (int)dataRow_0["y"], (double)dataRow_0["z"]);
			}
		}
		internal Pet method_13(DataRow dataRow_0, uint uint_1)
		{
			if (dataRow_0 == null)
			{
				return null;
			}
			else
			{
				return new Pet(uint_1, (uint)dataRow_0["user_id"], (uint)dataRow_0["room_id"], (string)dataRow_0["name"], (uint)dataRow_0["type"], (string)dataRow_0["race"], (string)dataRow_0["color"], (int)dataRow_0["expirience"], (int)dataRow_0["energy"], (int)dataRow_0["nutrition"], (int)dataRow_0["respect"], (double)dataRow_0["createstamp"], (int)dataRow_0["x"], (int)dataRow_0["y"], (double)dataRow_0["z"]);
			}
		}
		internal uint GenerateItemId()
		{
			lock (this.ItemIDCacheProtect)
			{
				return this.mCacheID++;
			}
		}
		public EcotronReward GetRandomEcotronReward()
		{
			uint uint_ = 1u;
			if (PhoenixEnvironment.GetRandomNumber(1, 2000) == 2000)
			{
				uint_ = 5u;
			}
			else
			{
				if (PhoenixEnvironment.GetRandomNumber(1, 200) == 200)
				{
					uint_ = 4u;
				}
				else
				{
					if (PhoenixEnvironment.GetRandomNumber(1, 40) == 40)
					{
						uint_ = 3u;
					}
					else
					{
						if (PhoenixEnvironment.GetRandomNumber(1, 4) == 4)
						{
							uint_ = 2u;
						}
					}
				}
			}
			List<EcotronReward> list = this.GetEcotronRewardsForLevel(uint_);
			if (list != null && list.Count >= 1)
			{
				return list[PhoenixEnvironment.GetRandomNumber(0, list.Count - 1)];
			}
			else
			{
				return new EcotronReward(0u, 0u, 1479u, 0u);
			}
		}
		public List<EcotronReward> GetEcotronRewardsForLevel(uint uint_1)
		{
			List<EcotronReward> list = new List<EcotronReward>();
			foreach (EcotronReward current in this.EcotronRewards)
			{
				if (current.RewardLevel == uint_1)
				{
					list.Add(current);
				}
			}
			return list;
		}
		public ServerMessage method_17(int int_0)
		{
			ServerMessage Message = new ServerMessage(126u);
			Message.AppendBoolean(true);
			Message.AppendInt32(0);
			Message.AppendInt32(0);
			Message.AppendInt32(-1);
			Message.AppendStringWithBreak("");
			Message.AppendInt32(this.GetTreeSize(int_0, -1));
			Message.AppendBoolean(true);
			foreach (CatalogPage current in this.Pages.Values)
			{
				if (current.ParentId == -1)
				{
					current.Serialize(int_0, Message);
					foreach (CatalogPage current2 in this.Pages.Values)
					{
						if (current2.ParentId == current.PageId)
						{
							current2.Serialize(int_0, Message);
						}
					}
				}
			}
			return Message;
		}
		internal ServerMessage GetIndexMessageForRank(uint uint_1)
		{
			if (uint_1 < 1u)
			{
				uint_1 = 1u;
			}
			if ((ulong)uint_1 > (ulong)((long)PhoenixEnvironment.GetGame().GetRoleManager().RankBadge.Count))
			{
				uint_1 = (uint)PhoenixEnvironment.GetGame().GetRoleManager().RankBadge.Count;
			}
			return this.mCataIndexCache[(int)((UIntPtr)uint_1)];
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
