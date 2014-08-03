using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Pets;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
using Phoenix.HabboHotel.Pathfinding;
using Phoenix.Util;
namespace Phoenix.HabboHotel.RoomBots
{
	internal class PetBot : BotAI
	{
		private int int_2;
		private int int_3;

		public PetBot(int VirtualId)
		{
			this.int_2 = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(25, 60);
			this.int_3 = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 60);
		}

		private int method_4()
		{
			RoomUser @class = base.GetRoomUser();
			int result = 5;
			if (@class.PetData.Level >= 1)
			{
				result = PhoenixEnvironment.GetRandomNumber(1, 8);
			}
			else
			{
				if (@class.PetData.Level >= 5)
				{
					result = PhoenixEnvironment.GetRandomNumber(1, 7);
				}
				else
				{
					if (@class.PetData.Level >= 10)
					{
						result = PhoenixEnvironment.GetRandomNumber(1, 6);
					}
				}
			}
			return result;
		}
		private void method_5(int int_4, int int_5, bool bool_0)
		{
			RoomUser @class = base.GetRoomUser();
			if (bool_0)
			{
				int int_6 = PhoenixEnvironment.GetRandomNumber(0, base.GetRoom().Model.MapSizeX);
				int int_7 = PhoenixEnvironment.GetRandomNumber(0, base.GetRoom().Model.MapSizeY);
				@class.MoveTo(int_6, int_7);
			}
			else
			{
				if (int_4 < base.GetRoom().Model.MapSizeX && int_5 < base.GetRoom().Model.MapSizeY && int_4 >= 0 && int_5 >= 0)
				{
					@class.MoveTo(int_4, int_5);
				}
			}
		}

		public override void OnSelfEnterRoom()
		{
			if (base.GetRoomUser().PetData.X > 0 && base.GetRoomUser().PetData.Y > 0)
			{
				base.GetRoomUser().X = base.GetRoomUser().PetData.X;
				base.GetRoomUser().Y = base.GetRoomUser().PetData.Y;
			}
			this.method_5(0, 0, true);
		}

		public override void OnSelfLeaveRoom(bool Kicked)
		{
			if (base.GetBotData().RoomUser != null)
			{
				RoomUser User = base.GetBotData().RoomUser;
				if (User.class34_1 != null && User == base.GetBotData().RoomUser)
				{
					base.GetBotData().RoomUser = null;
					User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, true);
					User.class34_1 = null;
					User.Target = null;
				}
			}
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				if (base.GetRoomUser().PetData.DBState == DatabaseUpdateState.NeedsInsert)
				{
					adapter.AddParamWithValue("petname" + base.GetRoomUser().PetData.PetId, base.GetRoomUser().PetData.Name);
					adapter.AddParamWithValue("petcolor" + base.GetRoomUser().PetData.PetId, base.GetRoomUser().PetData.Color);
					adapter.AddParamWithValue("petrace" + base.GetRoomUser().PetData.PetId, base.GetRoomUser().PetData.Race);
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO `user_pets` VALUES ('",
						base.GetRoomUser().PetData.PetId,
						"', '",
						base.GetRoomUser().PetData.OwnerId,
						"', '0', @petname",
						base.GetRoomUser().PetData.PetId,
						", @petrace",
						base.GetRoomUser().PetData.PetId,
						", @petcolor",
						base.GetRoomUser().PetData.PetId,
						", '",
						base.GetRoomUser().PetData.Type,
						"', '",
						base.GetRoomUser().PetData.Expirience,
						"', '",
						base.GetRoomUser().PetData.Energy,
						"', '",
						base.GetRoomUser().PetData.Nutrition,
						"', '",
						base.GetRoomUser().PetData.Respect,
						"', '",
						base.GetRoomUser().PetData.CreationStamp,
						"', '",
						base.GetRoomUser().PetData.X,
						"', '",
						base.GetRoomUser().PetData.Y,
						"', '",
						base.GetRoomUser().PetData.Z,
						"');"
					}));
				}
				else
				{
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE user_pets SET room_id = '0', expirience = '",
						base.GetRoomUser().PetData.Expirience,
						"', energy = '",
						base.GetRoomUser().PetData.Energy,
						"', nutrition = '",
						base.GetRoomUser().PetData.Nutrition,
						"', respect = '",
						base.GetRoomUser().PetData.Respect,
						"' WHERE Id = '",
						base.GetRoomUser().PetData.PetId,
						"' LIMIT 1; "
					}));
				}
				base.GetRoomUser().PetData.DBState = DatabaseUpdateState.Updated;
			}
		}

        public override void OnUserEnterRoom(RoomUser User) { }

		public override void OnUserLeaveRoom(GameClient Session)
		{
			if (Session != null && Session.GetHabbo() != null)
			{
				string string_ = Session.GetHabbo().Username;
				RoomUser @class = base.GetRoom().GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (base.GetBotData().RoomUser != null && @class != null && @class.class34_1 != null && @class == base.GetBotData().RoomUser)
				{
					base.GetBotData().RoomUser = null;
				}
				try
				{
					if (string_.ToLower() == base.GetRoomUser().PetData.OwnerName.ToLower() && string_.ToLower() != base.GetRoom().Owner.ToLower())
					{
						base.GetRoom().RemoveBot(base.GetRoomUser().PetData.VirtualId, false);
						Session.GetHabbo().GetInventoryComponent().method_7(base.GetRoomUser().PetData);
					}
				}
				catch
				{
				}
			}
		}

		public override void OnUserSay(RoomUser User, string Message)
		{
			RoomUser @class = base.GetRoomUser();
			if (@class.BotData.RoomUser == null)
			{
				if (Message.ToLower().Equals(@class.PetData.Name.ToLower()))
				{
					@class.SetRot(Rotation.Calculate(@class.X, @class.Y, User.X, User.Y));
				}
				else
				{
					if (Message.ToLower().StartsWith(@class.PetData.Name.ToLower() + " ") && User.GetClient().GetHabbo().Username.ToLower() == base.GetRoomUser().PetData.OwnerName.ToLower())
					{
						string key = Message.Substring(@class.PetData.Name.ToLower().Length + 1);
						if ((@class.PetData.Energy >= 10 && this.method_4() < 6) || @class.PetData.Level >= 15)
						{
							@class.Statusses.Clear();
							if (!PhoenixEnvironment.GetGame().GetRoleManager().PetCommandsList.ContainsKey(key))
							{
								string[] array = new string[]
								{
									TextManager.GetText("pet_response_confused1"),
									TextManager.GetText("pet_response_confused2"),
									TextManager.GetText("pet_response_confused3"),
									TextManager.GetText("pet_response_confused4"),
									TextManager.GetText("pet_response_confused5"),
									TextManager.GetText("pet_response_confused6"),
									TextManager.GetText("pet_response_confused7")
								};
								Random random = new Random();
								@class.Chat(null, array[random.Next(0, array.Length - 1)], false);
							}
							else
							{
								switch (PhoenixEnvironment.GetGame().GetRoleManager().PetCommandsList[key])
								{
								case 1:
									@class.PetData.AddExpirience(10, -25);
									@class.Chat(null, TextManager.GetText("pet_response_sleep"), false);
									@class.Statusses.Add("lay", @class.Z.ToString());
									break;
								case 2:
									this.method_5(0, 0, true);
									@class.PetData.AddExpirience(5, 5);
									break;
								case 3:
									@class.PetData.AddExpirience(5, 5);
									@class.Statusses.Add("sit", @class.Z.ToString());
									break;
								case 4:
									@class.PetData.AddExpirience(5, 5);
									@class.Statusses.Add("lay", @class.Z.ToString());
									break;
								case 5:
									@class.PetData.AddExpirience(10, 10);
									this.int_3 = 60;
									break;
								case 6:
								{
									int int_ = User.X;
									int int_2 = User.Y;
									if (User.RotBody == 4)
									{
										int_2 = User.Y + 1;
									}
									else
									{
										if (User.RotBody == 0)
										{
											int_2 = User.Y - 1;
										}
										else
										{
											if (User.RotBody == 6)
											{
												int_ = User.X - 1;
											}
											else
											{
												if (User.RotBody == 2)
												{
													int_ = User.X + 1;
												}
												else
												{
													if (User.RotBody == 3)
													{
														int_ = User.X + 1;
														int_2 = User.Y + 1;
													}
													else
													{
														if (User.RotBody == 1)
														{
															int_ = User.X + 1;
															int_2 = User.Y - 1;
														}
														else
														{
															if (User.RotBody == 7)
															{
																int_ = User.X - 1;
																int_2 = User.Y - 1;
															}
															else
															{
																if (User.RotBody == 5)
																{
																	int_ = User.X - 1;
																	int_2 = User.Y + 1;
																}
															}
														}
													}
												}
											}
										}
									}
									@class.PetData.AddExpirience(15, 15);
									this.method_5(int_, int_2, false);
									break;
								}
								case 7:
									@class.PetData.AddExpirience(20, 20);
									@class.Statusses.Add("ded", @class.Z.ToString());
									break;
								case 8:
									@class.PetData.AddExpirience(10, 10);
									@class.Statusses.Add("beg", @class.Z.ToString());
									break;
								case 9:
									@class.PetData.AddExpirience(15, 15);
									@class.Statusses.Add("jmp", @class.Z.ToString());
									break;
								case 10:
									@class.PetData.AddExpirience(25, 25);
									@class.Chat(null, TextManager.GetText("pet_response_silent"), false);
									this.int_2 = 120;
									break;
								case 11:
									@class.PetData.AddExpirience(15, 15);
									this.int_2 = 2;
									break;
								}
							}
						}
						else
						{
							string[] array2 = new string[]
							{
								TextManager.GetText("pet_response_sleeping1"),
								TextManager.GetText("pet_response_sleeping2"),
								TextManager.GetText("pet_response_sleeping3"),
								TextManager.GetText("pet_response_sleeping4"),
								TextManager.GetText("pet_response_sleeping5")
							};
							string[] array3 = new string[]
							{
								TextManager.GetText("pet_response_refusal1"),
								TextManager.GetText("pet_response_refusal2"),
								TextManager.GetText("pet_response_refusal3"),
								TextManager.GetText("pet_response_refusal4"),
								TextManager.GetText("pet_response_refusal5")
							};
							@class.GoalX = @class.SetX;
							@class.GoalY = @class.SetY;
							@class.Statusses.Clear();
							if (@class.PetData.Energy < 10)
							{
								Random random2 = new Random();
								@class.Chat(null, array2[random2.Next(0, array2.Length - 1)], false);
								if (@class.PetData.Type != 13u)
								{
									@class.Statusses.Add("lay", @class.Z.ToString());
								}
								else
								{
									@class.Statusses.Add("lay", (@class.Z - 1.0).ToString());
								}
								this.int_2 = 25;
								this.int_3 = 20;
								base.GetRoomUser().PetData.PetEnergy(-25);
							}
							else
							{
								Random random2 = new Random();
								@class.Chat(null, array3[random2.Next(0, array3.Length - 1)], false);
							}
						}
						@class.UpdateNeeded = true;
					}
				}
			}
		}

        public override void OnUserShout(RoomUser User, string Message) { }

		public override void OnTimerTick()
		{
			if (this.int_2 <= 0)
			{
				RoomUser @class = base.GetRoomUser();
				string[] array = new string[]
				{
					TextManager.GetText("pet_chatter_dog1"),
					TextManager.GetText("pet_chatter_dog2"),
					TextManager.GetText("pet_chatter_dog3"),
					TextManager.GetText("pet_chatter_dog4"),
					TextManager.GetText("pet_chatter_dog5")
				};
				string[] array2 = new string[]
				{
					TextManager.GetText("pet_chatter_cat1"),
					TextManager.GetText("pet_chatter_cat2"),
					TextManager.GetText("pet_chatter_cat3"),
					TextManager.GetText("pet_chatter_cat4"),
					TextManager.GetText("pet_chatter_cat5")
				};
				string[] array3 = new string[]
				{
					TextManager.GetText("pet_chatter_croc1"),
					TextManager.GetText("pet_chatter_croc2"),
					TextManager.GetText("pet_chatter_croc3"),
					TextManager.GetText("pet_chatter_croc4"),
					TextManager.GetText("pet_chatter_croc5")
				};
				string[] array4 = new string[]
				{
					TextManager.GetText("pet_chatter_dog1"),
					TextManager.GetText("pet_chatter_dog2"),
					TextManager.GetText("pet_chatter_dog3"),
					TextManager.GetText("pet_chatter_dog4"),
					TextManager.GetText("pet_chatter_dog5")
				};
				string[] array5 = new string[]
				{
					TextManager.GetText("pet_chatter_bear1"),
					TextManager.GetText("pet_chatter_bear2"),
					TextManager.GetText("pet_chatter_bear3"),
					TextManager.GetText("pet_chatter_bear4"),
					TextManager.GetText("pet_chatter_bear5")
				};
				string[] array6 = new string[]
				{
					TextManager.GetText("pet_chatter_pig1"),
					TextManager.GetText("pet_chatter_pig2"),
					TextManager.GetText("pet_chatter_pig3"),
					TextManager.GetText("pet_chatter_pig4"),
					TextManager.GetText("pet_chatter_pig5")
				};
				string[] array7 = new string[]
				{
					TextManager.GetText("pet_chatter_lion1"),
					TextManager.GetText("pet_chatter_lion2"),
					TextManager.GetText("pet_chatter_lion3"),
					TextManager.GetText("pet_chatter_lion4"),
					TextManager.GetText("pet_chatter_lion5")
				};
				string[] array8 = new string[]
				{
					TextManager.GetText("pet_chatter_rhino1"),
					TextManager.GetText("pet_chatter_rhino2"),
					TextManager.GetText("pet_chatter_rhino3"),
					TextManager.GetText("pet_chatter_rhino4"),
					TextManager.GetText("pet_chatter_rhino5")
				};
				string[] array9 = new string[]
				{
					TextManager.GetText("pet_chatter_spider1"),
					TextManager.GetText("pet_chatter_spider2"),
					TextManager.GetText("pet_chatter_spider3"),
					TextManager.GetText("pet_chatter_spider4"),
					TextManager.GetText("pet_chatter_spider5")
				};
				string[] array10 = new string[]
				{
					TextManager.GetText("pet_chatter_turtle1"),
					TextManager.GetText("pet_chatter_turtle2"),
					TextManager.GetText("pet_chatter_turtle3"),
					TextManager.GetText("pet_chatter_turtle4"),
					TextManager.GetText("pet_chatter_turtle5")
				};
				string[] array11 = new string[]
				{
					TextManager.GetText("pet_chatter_chic1"),
					TextManager.GetText("pet_chatter_chic2"),
					TextManager.GetText("pet_chatter_chic3"),
					TextManager.GetText("pet_chatter_chic4"),
					TextManager.GetText("pet_chatter_chic5")
				};
				string[] array12 = new string[]
				{
					TextManager.GetText("pet_chatter_frog1"),
					TextManager.GetText("pet_chatter_frog2"),
					TextManager.GetText("pet_chatter_frog3"),
					TextManager.GetText("pet_chatter_frog4"),
					TextManager.GetText("pet_chatter_frog5")
				};
				string[] array13 = new string[]
				{
					TextManager.GetText("pet_chatter_dragon1"),
					TextManager.GetText("pet_chatter_dragon2"),
					TextManager.GetText("pet_chatter_dragon3"),
					TextManager.GetText("pet_chatter_dragon4"),
					TextManager.GetText("pet_chatter_dragon5")
				};
				string[] array14 = new string[]
				{
					TextManager.GetText("pet_chatter_horse1"),
					TextManager.GetText("pet_chatter_horse2"),
					TextManager.GetText("pet_chatter_horse3"),
					TextManager.GetText("pet_chatter_horse4"),
					TextManager.GetText("pet_chatter_horse5")
				};
				string[] array15 = new string[]
				{
					TextManager.GetText("pet_chatter_monkey1"),
					TextManager.GetText("pet_chatter_monkey2"),
					TextManager.GetText("pet_chatter_monkey3"),
					TextManager.GetText("pet_chatter_monkey4"),
					TextManager.GetText("pet_chatter_monkey5")
				};
				string[] array16 = new string[]
				{
					TextManager.GetText("pet_chatter_generic1"),
					TextManager.GetText("pet_chatter_generic2"),
					TextManager.GetText("pet_chatter_generic3"),
					TextManager.GetText("pet_chatter_generic4"),
					TextManager.GetText("pet_chatter_generic5")
				};
				string[] array17 = new string[]
				{
					"sit",
					"lay",
					"snf",
					"ded",
					"jmp",
					"snf",
					"sit",
					"snf"
				};
				string[] array18 = new string[]
				{
					"sit",
					"lay"
				};
				string[] array19 = new string[]
				{
					"wng",
					"grn",
					"flm",
					"std",
					"swg",
					"sit",
					"lay",
					"snf",
					"plf",
					"jmp",
					"flm",
					"crk",
					"rlx",
					"flm"
				};
				if (@class != null)
				{
					Random random = new Random();
					int num = PhoenixEnvironment.GetRandomNumber(1, 4);
					if (num == 2)
					{
						@class.Statusses.Clear();
						if (base.GetRoomUser().BotData.RoomUser == null)
						{
							if (@class.PetData.Type == 13u)
							{
								@class.Statusses.Add(array18[random.Next(0, array18.Length - 1)], @class.Z.ToString());
							}
							else
							{
								if (@class.PetData.Type != 12u)
								{
									@class.Statusses.Add(array17[random.Next(0, array17.Length - 1)], @class.Z.ToString());
								}
								else
								{
									@class.Statusses.Add(array19[random.Next(0, array19.Length - 1)], @class.Z.ToString());
								}
							}
						}
					}
					switch (@class.PetData.Type)
					{
					case 0:
						@class.Chat(null, array[random.Next(0, array.Length - 1)], false);
						break;
					case 1:
						@class.Chat(null, array2[random.Next(0, array2.Length - 1)], false);
						break;
					case 2:
						@class.Chat(null, array3[random.Next(0, array3.Length - 1)], false);
						break;
					case 3:
						@class.Chat(null, array4[random.Next(0, array4.Length - 1)], false);
						break;
					case 4:
						@class.Chat(null, array5[random.Next(0, array5.Length - 1)], false);
						break;
					case 5:
						@class.Chat(null, array6[random.Next(0, array6.Length - 1)], false);
						break;
					case 6:
						@class.Chat(null, array7[random.Next(0, array7.Length - 1)], false);
						break;
					case 7:
						@class.Chat(null, array8[random.Next(0, array8.Length - 1)], false);
						break;
					case 8:
						@class.Chat(null, array9[random.Next(0, array9.Length - 1)], false);
						break;
					case 9:
						@class.Chat(null, array10[random.Next(0, array10.Length - 1)], false);
						break;
					case 10:
						@class.Chat(null, array11[random.Next(0, array11.Length - 1)], false);
						break;
					case 11:
						@class.Chat(null, array12[random.Next(0, array12.Length - 1)], false);
						break;
					case 12:
						@class.Chat(null, array13[random.Next(0, array13.Length - 1)], false);
						break;
					case 13:
						@class.Chat(null, array14[random.Next(0, array14.Length - 1)], false);
						break;
					case 14:
						@class.Chat(null, array15[random.Next(0, array15.Length - 1)], false);
						break;
					default:
						@class.Chat(null, array16[random.Next(0, array16.Length - 1)], false);
						break;
					}
				}
				this.int_2 = PhoenixEnvironment.GetRandomNumber(30, 120);
			}
			else
			{
				this.int_2--;
			}
			if (this.int_3 <= 0)
			{
				base.GetRoomUser().PetData.PetEnergy(-10);
				if (base.GetRoomUser().BotData.RoomUser == null)
				{
					this.method_5(0, 0, true);
				}
				this.int_3 = 30;
			}
			else
			{
				this.int_3--;
			}
		}
	}
}
