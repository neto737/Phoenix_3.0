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
		private int SpeechTimer;
		private int EnergyTimer;

		public PetBot(int VirtualId)
		{
			this.SpeechTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(25, 60);
			this.EnergyTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 60);
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
			RoomUser RoomUser = base.GetRoomUser();
			if (bool_0)
			{
				int randomX = PhoenixEnvironment.GetRandomNumber(0, base.GetRoom().Model.MapSizeX);
				int randomY = PhoenixEnvironment.GetRandomNumber(0, base.GetRoom().Model.MapSizeY);
				RoomUser.MoveTo(randomX, randomY);
			}
			else
			{
				if (int_4 < base.GetRoom().Model.MapSizeX && int_5 < base.GetRoom().Model.MapSizeY && int_4 >= 0 && int_5 >= 0)
				{
					RoomUser.MoveTo(int_4, int_5);
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
						Session.GetHabbo().GetInventoryComponent().AddPet(base.GetRoomUser().PetData);
					}
				}
				catch
				{
				}
			}
		}

		public override void OnUserSay(RoomUser User, string Message)
		{
			RoomUser Pet = base.GetRoomUser();
			if (Pet.BotData.RoomUser == null)
			{
				if (Message.ToLower().Equals(Pet.PetData.Name.ToLower()))
				{
					Pet.SetRot(Rotation.Calculate(Pet.X, Pet.Y, User.X, User.Y));
				}
				else
				{
					if (Message.ToLower().StartsWith(Pet.PetData.Name.ToLower() + " ") && User.GetClient().GetHabbo().Username.ToLower() == base.GetRoomUser().PetData.OwnerName.ToLower())
					{
						string key = Message.Substring(Pet.PetData.Name.ToLower().Length + 1);
                        if ((Pet.PetData.Energy >= 10 && this.method_4() < 6) || Pet.PetData.Level >= 15)
                        {
                            Pet.Statusses.Clear();
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
                                Pet.Chat(null, array[random.Next(0, array.Length - 1)], false);
                            }
                            else
                            {
                                switch (PhoenixEnvironment.GetGame().GetRoleManager().PetCommandsList[key])
                                {
                                    case 1:
                                        Pet.PetData.AddExpirience(10, -25);
                                        Pet.Chat(null, TextManager.GetText("pet_response_sleep"), false);
                                        Pet.Statusses.Add("lay", Pet.Z.ToString());
                                        break;
                                    case 2:
                                        this.method_5(0, 0, true);
                                        Pet.PetData.AddExpirience(5, 5);
                                        break;
                                    case 3:
                                        Pet.PetData.AddExpirience(5, 5);
                                        Pet.Statusses.Add("sit", Pet.Z.ToString());
                                        break;
                                    case 4:
                                        Pet.PetData.AddExpirience(5, 5);
                                        Pet.Statusses.Add("lay", Pet.Z.ToString());
                                        break;
                                    case 5:
                                        Pet.PetData.AddExpirience(10, 10);
                                        this.EnergyTimer = 60;
                                        break;
                                    case 6:
                                        {
                                            int NewX = User.X;
                                            int NewY = User.Y;
                                            if (User.RotBody == 4)
                                            {
                                                NewY = User.Y + 1;
                                            }
                                            else if (User.RotBody == 0)
                                            {
                                                NewY = User.Y - 1;
                                            }
                                            else if (User.RotBody == 6)
                                            {
                                                NewX = User.X - 1;
                                            }
                                            else if (User.RotBody == 2)
                                            {
                                                NewX = User.X + 1;
                                            }
                                            else if (User.RotBody == 3)
                                            {
                                                NewX = User.X + 1;
                                                NewY = User.Y + 1;
                                            }
                                            else if (User.RotBody == 1)
                                            {
                                                NewX = User.X + 1;
                                                NewY = User.Y - 1;
                                            }
                                            else if (User.RotBody == 7)
                                            {
                                                NewX = User.X - 1;
                                                NewY = User.Y - 1;
                                            }
                                            else if (User.RotBody == 5)
                                            {
                                                NewX = User.X - 1;
                                                NewY = User.Y + 1;
                                            }
                                            Pet.PetData.AddExpirience(15, 15);
                                            this.method_5(NewX, NewY, false);
                                            break;
                                        }
                                    case 7:
                                        Pet.PetData.AddExpirience(20, 20);
                                        Pet.Statusses.Add("ded", Pet.Z.ToString());
                                        break;
                                    case 8:
                                        Pet.PetData.AddExpirience(10, 10);
                                        Pet.Statusses.Add("beg", Pet.Z.ToString());
                                        break;
                                    case 9:
                                        Pet.PetData.AddExpirience(15, 15);
                                        Pet.Statusses.Add("jmp", Pet.Z.ToString());
                                        break;
                                    case 10:
                                        Pet.PetData.AddExpirience(25, 25);
                                        Pet.Chat(null, TextManager.GetText("pet_response_silent"), false);
                                        this.SpeechTimer = 120;
                                        break;
                                    case 11:
                                        Pet.PetData.AddExpirience(15, 15);
                                        this.SpeechTimer = 2;
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
                            Pet.GoalX = Pet.SetX;
                            Pet.GoalY = Pet.SetY;
                            Pet.Statusses.Clear();
                            if (Pet.PetData.Energy < 10)
                            {
                                Random random2 = new Random();
                                Pet.Chat(null, array2[random2.Next(0, array2.Length - 1)], false);
                                if (Pet.PetData.Type != 13)
                                {
                                    Pet.Statusses.Add("lay", Pet.Z.ToString());
                                }
                                else
                                {
                                    Pet.Statusses.Add("lay", (Pet.Z - 1.0).ToString());
                                }
                                this.SpeechTimer = 25;
                                this.EnergyTimer = 20;
                                base.GetRoomUser().PetData.PetEnergy(-25);
                            }
                            else
                            {
                                Random random2 = new Random();
                                Pet.Chat(null, array3[random2.Next(0, array3.Length - 1)], false);
                            }
                        }
						Pet.UpdateNeeded = true;
					}
				}
			}
		}

        public override void OnUserShout(RoomUser User, string Message) { }

		public override void OnTimerTick()
		{
            if (SpeechTimer <= 0)
            {
                RoomUser Pet = base.GetRoomUser();
                string[] PetDog = new string[]
				{
					TextManager.GetText("pet_chatter_dog1"),
					TextManager.GetText("pet_chatter_dog2"),
					TextManager.GetText("pet_chatter_dog3"),
					TextManager.GetText("pet_chatter_dog4"),
					TextManager.GetText("pet_chatter_dog5")
				};
                string[] PetCat = new string[]
				{
					TextManager.GetText("pet_chatter_cat1"),
					TextManager.GetText("pet_chatter_cat2"),
					TextManager.GetText("pet_chatter_cat3"),
					TextManager.GetText("pet_chatter_cat4"),
					TextManager.GetText("pet_chatter_cat5")
				};
                string[] PetCroc = new string[]
				{
					TextManager.GetText("pet_chatter_croc1"),
					TextManager.GetText("pet_chatter_croc2"),
					TextManager.GetText("pet_chatter_croc3"),
					TextManager.GetText("pet_chatter_croc4"),
					TextManager.GetText("pet_chatter_croc5")
				};
                string[] PetDogg = new string[]
				{
					TextManager.GetText("pet_chatter_dog1"),
					TextManager.GetText("pet_chatter_dog2"),
					TextManager.GetText("pet_chatter_dog3"),
					TextManager.GetText("pet_chatter_dog4"),
					TextManager.GetText("pet_chatter_dog5")
				};
                string[] PetBear = new string[]
				{
					TextManager.GetText("pet_chatter_bear1"),
					TextManager.GetText("pet_chatter_bear2"),
					TextManager.GetText("pet_chatter_bear3"),
					TextManager.GetText("pet_chatter_bear4"),
					TextManager.GetText("pet_chatter_bear5")
				};
                string[] PetPig = new string[]
				{
					TextManager.GetText("pet_chatter_pig1"),
					TextManager.GetText("pet_chatter_pig2"),
					TextManager.GetText("pet_chatter_pig3"),
					TextManager.GetText("pet_chatter_pig4"),
					TextManager.GetText("pet_chatter_pig5")
				};
                string[] PetLion = new string[]
				{
					TextManager.GetText("pet_chatter_lion1"),
					TextManager.GetText("pet_chatter_lion2"),
					TextManager.GetText("pet_chatter_lion3"),
					TextManager.GetText("pet_chatter_lion4"),
					TextManager.GetText("pet_chatter_lion5")
				};
                string[] PetRhino = new string[]
				{
					TextManager.GetText("pet_chatter_rhino1"),
					TextManager.GetText("pet_chatter_rhino2"),
					TextManager.GetText("pet_chatter_rhino3"),
					TextManager.GetText("pet_chatter_rhino4"),
					TextManager.GetText("pet_chatter_rhino5")
				};
                string[] PetSpider = new string[]
				{
					TextManager.GetText("pet_chatter_spider1"),
					TextManager.GetText("pet_chatter_spider2"),
					TextManager.GetText("pet_chatter_spider3"),
					TextManager.GetText("pet_chatter_spider4"),
					TextManager.GetText("pet_chatter_spider5")
				};
                string[] PetTurtle = new string[]
				{
					TextManager.GetText("pet_chatter_turtle1"),
					TextManager.GetText("pet_chatter_turtle2"),
					TextManager.GetText("pet_chatter_turtle3"),
					TextManager.GetText("pet_chatter_turtle4"),
					TextManager.GetText("pet_chatter_turtle5")
				};
                string[] PetChic = new string[]
				{
					TextManager.GetText("pet_chatter_chic1"),
					TextManager.GetText("pet_chatter_chic2"),
					TextManager.GetText("pet_chatter_chic3"),
					TextManager.GetText("pet_chatter_chic4"),
					TextManager.GetText("pet_chatter_chic5")
				};
                string[] PetFrog = new string[]
				{
					TextManager.GetText("pet_chatter_frog1"),
					TextManager.GetText("pet_chatter_frog2"),
					TextManager.GetText("pet_chatter_frog3"),
					TextManager.GetText("pet_chatter_frog4"),
					TextManager.GetText("pet_chatter_frog5")
				};
                string[] PetDragon = new string[]
				{
					TextManager.GetText("pet_chatter_dragon1"),
					TextManager.GetText("pet_chatter_dragon2"),
					TextManager.GetText("pet_chatter_dragon3"),
					TextManager.GetText("pet_chatter_dragon4"),
					TextManager.GetText("pet_chatter_dragon5")
				};
                string[] PetHorse = new string[]
				{
					TextManager.GetText("pet_chatter_horse1"),
					TextManager.GetText("pet_chatter_horse2"),
					TextManager.GetText("pet_chatter_horse3"),
					TextManager.GetText("pet_chatter_horse4"),
					TextManager.GetText("pet_chatter_horse5")
				};
                string[] PetMonkey = new string[]
				{
					TextManager.GetText("pet_chatter_monkey1"),
					TextManager.GetText("pet_chatter_monkey2"),
					TextManager.GetText("pet_chatter_monkey3"),
					TextManager.GetText("pet_chatter_monkey4"),
					TextManager.GetText("pet_chatter_monkey5")
				};
                string[] PetGeneric = new string[]
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
                if (Pet != null)
                {
                    Random RandomSpeech = new Random();
                    int num = PhoenixEnvironment.GetRandomNumber(1, 4);
                    if (num == 2)
                    {
                        Pet.Statusses.Clear();
                        if (base.GetRoomUser().BotData.RoomUser == null)
                        {
                            if (Pet.PetData.Type == 13)
                            {
                                Pet.Statusses.Add(array18[RandomSpeech.Next(0, array18.Length - 1)], Pet.Z.ToString());
                            }
                            else
                            {
                                if (Pet.PetData.Type != 12)
                                {
                                    Pet.Statusses.Add(array17[RandomSpeech.Next(0, array17.Length - 1)], Pet.Z.ToString());
                                }
                                else
                                {
                                    Pet.Statusses.Add(array19[RandomSpeech.Next(0, array19.Length - 1)], Pet.Z.ToString());
                                }
                            }
                        }
                    }
                    switch (Pet.PetData.Type)
                    {
                        case 0:
                            Pet.Chat(null, PetDog[RandomSpeech.Next(0, PetDog.Length - 1)], false);
                            break;
                        case 1:
                            Pet.Chat(null, PetCat[RandomSpeech.Next(0, PetCat.Length - 1)], false);
                            break;
                        case 2:
                            Pet.Chat(null, PetCroc[RandomSpeech.Next(0, PetCroc.Length - 1)], false);
                            break;
                        case 3:
                            Pet.Chat(null, PetDogg[RandomSpeech.Next(0, PetDogg.Length - 1)], false);
                            break;
                        case 4:
                            Pet.Chat(null, PetBear[RandomSpeech.Next(0, PetBear.Length - 1)], false);
                            break;
                        case 5:
                            Pet.Chat(null, PetPig[RandomSpeech.Next(0, PetPig.Length - 1)], false);
                            break;
                        case 6:
                            Pet.Chat(null, PetLion[RandomSpeech.Next(0, PetLion.Length - 1)], false);
                            break;
                        case 7:
                            Pet.Chat(null, PetRhino[RandomSpeech.Next(0, PetRhino.Length - 1)], false);
                            break;
                        case 8:
                            Pet.Chat(null, PetSpider[RandomSpeech.Next(0, PetSpider.Length - 1)], false);
                            break;
                        case 9:
                            Pet.Chat(null, PetTurtle[RandomSpeech.Next(0, PetTurtle.Length - 1)], false);
                            break;
                        case 10:
                            Pet.Chat(null, PetChic[RandomSpeech.Next(0, PetChic.Length - 1)], false);
                            break;
                        case 11:
                            Pet.Chat(null, PetFrog[RandomSpeech.Next(0, PetFrog.Length - 1)], false);
                            break;
                        case 12:
                            Pet.Chat(null, PetDragon[RandomSpeech.Next(0, PetDragon.Length - 1)], false);
                            break;
                        case 13:
                            Pet.Chat(null, PetHorse[RandomSpeech.Next(0, PetHorse.Length - 1)], false);
                            break;
                        case 14:
                            Pet.Chat(null, PetMonkey[RandomSpeech.Next(0, PetMonkey.Length - 1)], false);
                            break;
                        default:
                            Pet.Chat(null, PetGeneric[RandomSpeech.Next(0, PetGeneric.Length - 1)], false);
                            break;
                    }
                }
                SpeechTimer = PhoenixEnvironment.GetRandomNumber(30, 120);
            }
            else
            {
                SpeechTimer--;
            }
			if (EnergyTimer <= 0)
			{
				base.GetRoomUser().PetData.PetEnergy(-10);
				if (base.GetRoomUser().BotData.RoomUser == null)
				{
					method_5(0, 0, true);
				}
				EnergyTimer = 30;
			}
			else
			{
				EnergyTimer--;
			}
		}
	}
}
