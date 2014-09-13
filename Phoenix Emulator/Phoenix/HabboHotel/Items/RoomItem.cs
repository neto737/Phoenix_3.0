using System;
using System.Collections.Generic;
using System.Linq;
using Phoenix.Core;
using Phoenix.HabboHotel.Pathfinding;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.HabboHotel.Items.Interactors;
using Phoenix.Util;
using Phoenix.HabboHotel.SoundMachine;
namespace Phoenix.HabboHotel.Items
{
	internal class RoomItem
	{
		/*internal enum Enum5
		{
			const_0,
			const_1,
			const_2
		}*/

		internal uint Id;
		internal uint RoomId;
		internal uint BaseItem;
		internal string ExtraData;
		internal bool TimerRunning;
		internal string string_1;
		internal string Extra1;
		internal string Extra2;
		internal string Extra3;
		internal string Extra4;
		internal string Extra5;
		internal int int_0;
		private Dictionary<int, AffectedTile> mAffectedPoints;
		private int mX;
		private int mY;
		private double mZ;
		//internal RoomItem.Enum5 enum5_0;
		internal int Rot;
		internal string string_7;
		internal bool UpdateNeeded;
		internal int UpdateCounter;
		internal uint InteractingUser;
		internal uint InteractingUser2;
		internal Dictionary<RoomUser, int> dictionary_1;
		private Item mBaseItem;
		private Room mRoom;
		private bool mIsWallItem;
		private bool mIsFloorItem;
		private bool mIsRoller;

		internal Dictionary<int, AffectedTile> GetAffectedTiles
		{
			get
			{
				return mAffectedPoints;
			}
		}

		internal int GetX
		{
			get
			{
				return mX;
			}
		}

		internal int GetY
		{
			get
			{
				return mY;
			}
		}

		internal double GetZ
		{
			get
			{
				return mZ;
			}
		}

		internal bool IsRoller
		{
			get
			{
				return mIsRoller;
			}
		}

		internal Coord Coordinate
		{
			get
			{
				return new Coord(mX, mY);
			}
		}

		internal double TotalHeight
		{
			get
			{
				if (GetBaseItem().Height_Adjustable.Count > 1)
				{
					int i;
					if (int.TryParse(ExtraData, out i))
					{
						return mZ + GetBaseItem().Height_Adjustable[i];
					}
					else
					{
						return mZ + GetBaseItem().Height;
					}
				}
				else
				{
				    return mZ + GetBaseItem().Height;
				}
			}
		}

		internal bool IsWallItem
		{
			get
			{
				return this.mIsWallItem;
			}
		}
		internal bool IsFloorItem
		{
			get
			{
				return this.mIsFloorItem;
			}
		}

		internal Coord SquareInFront
        {
            get
            {
                Coord Sq = new Coord(mX, mY);

                if (Rot == 0)
                {
                    Sq.Y--;
                }
                else if (Rot == 2)
                {
                    Sq.X++;
                }
                else if (Rot == 4)
                {
                    Sq.Y++;
                }
                else if (Rot == 6)
                {
                    Sq.X--;
                }

                return Sq;
            }
        }

		internal Coord SquareBehind
        {
            get
            {
                Coord Sq = new Coord(mX, mY);

                if (Rot == 0)
                {
                    Sq.Y++;
                }
                else if (Rot == 2)
                {
                    Sq.X--;
                }
                else if (Rot == 4)
                {
                    Sq.Y--;
                }
                else if (Rot == 6)
                {
                    Sq.X++;
                }

                return Sq;
            }
        }

		internal FurniInteractor Interactor
		{
			get
			{
				string pType = this.GetBaseItem().InteractionType.ToLower();
				switch (pType)
				{
				case "ball":    
                    return new InteractorFootball();
				case "teleport":
					return new InteractorTeleport();
				case "bottle":
					return new InteractorSpinningBottle();
				case "dice":
					return new InteractorDice();
				case "habbowheel":
					return new InteractorHabboWheel();
				case "loveshuffler":
					return new InteractorLoveShuffler();
				case "onewaygate":
					return new InteractorOneWayGate();
				case "alert":
					return new InteractorAlert();
				case "vendingmachine":
					return new InteractorVendor();
				case "gate":
					return new InteractorGate(this.GetBaseItem().Modes);
				case "scoreboard":
					return new InteractorScoreboard();
				case "counter":
					return new InteractorBanzaiScoreCounter();
				case "wired":
					return new WiredInteractor();
				case "wf_trg_onsay":
					return new InteractorWiredOnSay();
				case "wf_trg_enterroom":
					return new InteractorWiredEnterRoom();
				case "wf_act_saymsg":
				case "wf_act_give_phx":
				case "wf_cnd_phx":
					return new InteractorSuperWired();
				case "wf_trg_furnistate":
				case "wf_trg_onfurni":
				case "wf_trg_offfurni":
				case "wf_act_moveuser":
				case "wf_act_togglefurni":
					return new InteractorWiredTriggerState();
				case "wf_trg_gameend":
				case "wf_trg_gamestart":
					return new InteractorWiredTriggerGame();
				case "wf_trg_timer":
					return new InteractorWiredTriggerTimer();
				case "wf_act_givepoints":
					return new InteractorWiredGivePoints();
				case "wf_trg_attime":
					return new InteractorWiredAtTime();
				case "wf_trg_atscore":
					return new InteractorWiredAtScore();
				case "wf_act_moverotate":
					return new InteractorWiredMoveRotate();
				case "wf_act_matchfurni":
					return new InteractorWiredMatchFurni();
				case "wf_cnd_trggrer_on_frn":
				case "wf_cnd_furnis_hv_avtrs":
				case "wf_cnd_has_furni_on":
					return new InteractorWiredCondition();
				case "puzzlebox":
					return new InteractorPuzzleBox();
                case "jukebox":
                    return new InteractorJukebox();
				}
                return new InteractorDefault(this.GetBaseItem().Modes);
			}
		}

		public RoomItem(uint uint_5, uint uint_6, uint BaseItem, string string_8, int int_5, int int_6, double double_1, int int_7, string string_9, Room class14_1)
		{
			this.Id = uint_5;
			this.RoomId = uint_6;
			this.BaseItem = BaseItem;
			this.ExtraData = string_8;
			this.mX = int_5;
			this.mY = int_6;
			this.mZ = double_1;
			this.Rot = int_7;
			this.string_7 = string_9;
			this.UpdateNeeded = false;
			this.UpdateCounter = 0;
			this.InteractingUser = 0u;
			this.InteractingUser2 = 0u;
			this.TimerRunning = false;
			this.string_1 = "none";
			//this.enum5_0 = RoomItem.Enum5.const_0;
			this.Extra1 = "";
			this.Extra2 = "";
			this.Extra3 = "";
			this.Extra4 = "";
			this.Extra5 = "";
			this.int_0 = 0;
			this.dictionary_1 = new Dictionary<RoomUser, int>();
			this.mBaseItem = PhoenixEnvironment.GetGame().GetItemManager().GetItem(BaseItem);
			this.mRoom = class14_1;
			if (this.GetBaseItem() == null)
			{
                Logging.LogException("Unknown baseID: " + BaseItem);
			}
			string text = this.GetBaseItem().InteractionType.ToLower();
			if (text != null)
			{
				if (!(text == "teleport"))
				{
					if (!(text == "roller"))
					{
						if (!(text == "blue_score"))
						{
							if (!(text == "green_score"))
							{
								if (!(text == "red_score"))
								{
									if (text == "yellow_score")
									{
										this.string_1 = "yellow";
									}
								}
								else
								{
									this.string_1 = "red";
								}
							}
							else
							{
								this.string_1 = "green";
							}
						}
						else
						{
							this.string_1 = "blue";
						}
					}
					else
					{
						this.mIsRoller = true;
						class14_1.Boolean_1 = true;
					}
				}
				else
				{
					this.ReqUpdate(0);
				}
			}
            if (text != null)
            {
                switch (text)
                {
                    case "jukebox":
                        RoomMusicController roomMusicController = this.GetRoom().GetRoomMusicController();
                        roomMusicController.LinkRoomOutputItemIfNotAlreadyExits(this);
                        break;
                }
            }
			this.mIsWallItem = (this.GetBaseItem().Type == 'i');
			this.mIsFloorItem = (this.GetBaseItem().Type == 's');
			this.mAffectedPoints = this.GetRoom().GetAffectedTiles(this.GetBaseItem().Length, this.GetBaseItem().Width, this.mX, this.mY, int_7);
		}

		internal void method_0(int int_5, int int_6, double double_1)
		{
			this.mX = int_5;
			this.mY = int_6;
			this.mZ = double_1;
			this.mAffectedPoints = this.GetRoom().GetAffectedTiles(this.GetBaseItem().Length, this.GetBaseItem().Width, this.mX, this.mY, this.Rot);
		}

		internal Coord method_1(int int_5)
		{
			Coord result = new Coord(this.mX, this.mY);
			if (int_5 == 0)
			{
				result.Y++;
			}
			else
			{
				if (int_5 == 2)
				{
					result.X--;
				}
				else
				{
					if (int_5 == 4)
					{
						result.Y--;
					}
					else
					{
						if (int_5 == 6)
						{
							result.X++;
						}
					}
				}
			}
			return result;
		}
		internal void ProcessUpdates()
		{
			this.UpdateCounter--;
			if (this.UpdateCounter <= 0)
			{
				this.UpdateNeeded = false;
				this.UpdateCounter = 0;
				if (this.TimerRunning && this.int_0 > 0)
				{
					this.int_0 += 500;
					this.GetRoom().int_13 += 500;
					this.UpdateNeeded = true;
                    if (this.int_0 > SongManager.GetSong(Convert.ToInt32(this.ExtraData)).Length)
					{
						ServerMessage Message = new ServerMessage(327);
						Message.AppendInt32(7);
						Message.AppendInt32(6);
						Message.AppendInt32(7);
						Message.AppendInt32(0);
						Message.AppendInt32(0);
						this.GetRoom().SendMessage(Message, null);
						this.int_0 = 1;
						this.GetRoom().int_13 = 0;
					}
				}
				else
				{
					string text = this.GetBaseItem().InteractionType.ToLower();
					switch (text)
					{
					case "onewaygate":
					{
						RoomUser roomUserByHabbo = null;
						if (this.InteractingUser > 0)
						{
							roomUserByHabbo = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
						}
						if (roomUserByHabbo != null && roomUserByHabbo.X == this.mX && roomUserByHabbo.Y == this.mY && this.Extra1 != "tried")
						{
							this.ExtraData = "1";
							this.Extra1 = "tried";
							roomUserByHabbo.UnlockWalking();
							roomUserByHabbo.MoveTo(this.SquareBehind);
							this.ReqUpdate(0);
							this.UpdateState(false, true);
						}
						else
						{
							if ((roomUserByHabbo != null && (roomUserByHabbo.Coordinate != this.SquareBehind)) || this.Extra1 == "tried")
							{
								this.Extra1 = "";
								this.ExtraData = "0";
                                this.InteractingUser = 0;
								this.UpdateState(false, true);
								this.GetRoom().GenerateMaps();
							}
							else
							{
								if (this.ExtraData == "1")
								{
									this.ExtraData = "0";
									this.UpdateState(false, true);
								}
							}
						}
						if (roomUserByHabbo == null)
						{
							this.InteractingUser = 0u;
						}
						break;
					}
					case "teleport":
					{
						bool flag = false;
						bool flag2 = false;
						if (this.InteractingUser > 0u)
						{
							RoomUser roomUserByHabbo = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
							if (roomUserByHabbo != null)
							{
								if (!(roomUserByHabbo.Coordinate == this.Coordinate))
								{
									roomUserByHabbo.AllowOverride = false;
									if (roomUserByHabbo.TeleDelay == -1)
									{
										roomUserByHabbo.TeleDelay = 1;
									}
									if (TeleHandler.IsTeleLinked(this.Id))
									{
										flag2 = true;
										if (roomUserByHabbo.TeleDelay == 0)
										{
											uint num2 = TeleHandler.GetLinkedTele(this.Id);
											uint num3 = TeleHandler.GetTeleRoomId(num2);
											if (num3 == this.RoomId)
											{
												RoomItem class2 = this.GetRoom().GetItem(num2);
												if (class2 == null)
												{
													roomUserByHabbo.UnlockWalking();
												}
												else
												{
													roomUserByHabbo.SetPos(class2.GetX, class2.GetY, class2.GetZ);
													roomUserByHabbo.SetRot(class2.Rot);
													class2.ExtraData = "2";
													class2.UpdateState(false, true);
													class2.InteractingUser2 = this.InteractingUser;
												}
											}
											else
											{
												if (!roomUserByHabbo.IsBot)
												{
													PhoenixEnvironment.GetGame().GetRoomManager().method_5(new TeleUserData(roomUserByHabbo.GetClient().GetMessageHandler(), roomUserByHabbo.GetClient().GetHabbo(), num3, num2));
												}
											}
											this.InteractingUser = 0u;
										}
										else
										{
											roomUserByHabbo.TeleDelay--;
										}
									}
									else
									{
										roomUserByHabbo.UnlockWalking();
										this.InteractingUser = 0u;
										roomUserByHabbo.MoveTo(this.SquareInFront);
									}
								}
								else
								{
									if ((roomUserByHabbo.Coordinate == this.SquareInFront) && roomUserByHabbo.Item == this)
									{
										roomUserByHabbo.AllowOverride = true;
										flag = true;
										if (roomUserByHabbo.IsWalking && (roomUserByHabbo.GoalX != this.mX || roomUserByHabbo.GoalY != this.mY))
										{
											roomUserByHabbo.ClearMovement(true);
										}
										roomUserByHabbo.CanWalk = false;
										roomUserByHabbo.AllowOverride = true;
										roomUserByHabbo.MoveTo(this.Coordinate);
									}
									else
									{
										this.InteractingUser = 0u;
									}
								}
							}
							else
							{
								this.InteractingUser = 0u;
							}
						}
						if (this.InteractingUser2 > 0u)
						{
							RoomUser user2 = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser2);
							if (user2 != null)
							{
								flag = true;
								user2.UnlockWalking();
								if (user2.Coordinate == this.Coordinate)
								{
									user2.MoveTo(this.SquareInFront);
								}
							}
							this.InteractingUser2 = 0u;
						}
						if (flag)
						{
							if (this.ExtraData != "1")
							{
								this.ExtraData = "1";
								this.UpdateState(false, true);
							}
						}
						else
						{
							if (flag2)
							{
								if (this.ExtraData != "2")
								{
									this.ExtraData = "2";
									this.UpdateState(false, true);
								}
							}
							else
							{
								if (this.ExtraData != "0")
								{
									this.ExtraData = "0";
									this.UpdateState(false, true);
								}
							}
						}
						this.ReqUpdate(1);
						break;
					}
					case "bottle":
					{
						int num = PhoenixEnvironment.GetRandomNumber(0, 7);
						this.ExtraData = num.ToString();
						this.UpdateState();
						break;
					}
					case "dice":
						try
						{
							RoomUser @class = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
							if (@class.GetClient().GetHabbo().Rigger > 0)
							{
								this.ExtraData = @class.GetClient().GetHabbo().Rigger.ToString();
								@class.GetClient().GetHabbo().Rigger = 0;
							}
							else
							{
								int num = PhoenixEnvironment.GetRandomNumber(1, 6);
								this.ExtraData = num.ToString();
							}
						}
						catch
						{
							int num = PhoenixEnvironment.GetRandomNumber(1, 6);
							this.ExtraData = num.ToString();
						}
						this.UpdateState();
						break;
					case "habbowheel":
					{
						int num = PhoenixEnvironment.GetRandomNumber(1, 10);
						this.ExtraData = num.ToString();
						this.UpdateState();
						break;
					}
					case "loveshuffler":
						if (this.ExtraData == "0")
						{
							int num = PhoenixEnvironment.GetRandomNumber(1, 4);
							this.ExtraData = num.ToString();
							this.ReqUpdate(20);
						}
						else
						{
							if (this.ExtraData != "-1")
							{
								this.ExtraData = "-1";
							}
						}
						this.UpdateState(false, true);
						break;
					case "alert":
						if (this.ExtraData == "1")
						{
							this.ExtraData = "0";
							this.UpdateState(false, true);
						}
						break;
					case "vendingmachine":
						if (this.ExtraData == "1")
						{
							RoomUser @class = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
							if (@class != null)
							{
								@class.UnlockWalking();
								int int_ = this.GetBaseItem().VendingIds[PhoenixEnvironment.GetRandomNumber(0, this.GetBaseItem().VendingIds.Count - 1)];
								@class.CarryItem(int_);
							}
							this.InteractingUser = 0u;
							this.ExtraData = "0";
							this.UpdateState(false, true);
						}
						break;
					case "wf_trg_onsay":
					case "wf_trg_enterroom":
					case "wf_trg_furnistate":
					case "wf_trg_onfurni":
					case "wf_trg_offfurni":
					case "wf_trg_gameend":
					case "wf_trg_gamestart":
					case "wf_trg_attime":
					case "wf_trg_atscore":
					case "wf_act_saymsg":
					case "wf_act_togglefurni":
					case "wf_act_givepoints":
					case "wf_act_moverotate":
					case "wf_act_matchfurni":
					case "wf_act_give_phx":
					case "wf_cnd_trggrer_on_frn":
					case "wf_cnd_furnis_hv_avtrs":
					case "wf_cnd_has_furni_on":
					case "wf_cnd_phx":
					case "bb_teleport":
						if (this.ExtraData == "1")
						{
							this.ExtraData = "0";
							this.UpdateState(false, true);
						}
						break;
					case "wf_trg_timer":
						if (this.ExtraData == "1")
						{
							this.ExtraData = "0";
							this.UpdateState(false, true);
						}
						if (this.Extra1.Length > 0)
						{
							this.GetRoom().method_15(this);
							this.ReqUpdate(Convert.ToInt32(Convert.ToDouble(this.Extra1) * 2.0));
						}
						break;
					case "wf_act_moveuser":
						if (this.dictionary_1.Count > 0 && this.GetRoom().UserList != null)
						{
							int num4 = 0;
							RoomUser[] RoomUser_ = this.GetRoom().UserList;
							for (int i = 0; i < RoomUser_.Length; i++)
							{
								RoomUser class4 = RoomUser_[i];
								if (class4 != null)
								{
									if (class4.IsBot)
									{
										this.dictionary_1.Remove(class4);
									}
									if (this.dictionary_1.ContainsKey(class4))
									{
										if (this.dictionary_1[class4] > 0)
										{
											if (this.dictionary_1[class4] == 10 && !class4.IsBot)
											{
												class4.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(4, true);
											}
											Dictionary<RoomUser, int> dictionary;
											RoomUser key;
											(dictionary = this.dictionary_1)[key = class4] = dictionary[key] - 1;
											num4++;
										}
										else
										{
											this.dictionary_1.Remove(class4);
											class4.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(0, true);
										}
									}
								}
							}
							if (num4 > 0)
							{
								this.ReqUpdate(0);
							}
							else
							{
								this.dictionary_1.Clear();
							}
						}
						break;
					case "counter":
						if (this.TimerRunning && this.Extra1 != "1")
						{
							this.ExtraData = Convert.ToString(Convert.ToInt32(this.ExtraData) - 1);
							if (Convert.ToInt32(this.ExtraData) <= 0)
							{
								this.ExtraData = "0";
								this.TimerRunning = false;
								this.GetRoom().method_89(0, this, true);
							}
							this.UpdateState(true, true);
							this.GetRoom().method_16((double)Convert.ToInt32(this.ExtraData));
							this.Extra1 = "1";
							this.ReqUpdate(1);
						}
						else
						{
							if (this.TimerRunning)
							{
								this.GetRoom().method_16((double)Convert.ToInt32(this.ExtraData) - 0.5);
								this.Extra1 = "0";
								this.ReqUpdate(1);
							}
						}
						break;
					}
				}
			}
		}

		internal void ReqUpdate(int Cycles)
		{
			this.UpdateCounter = Cycles;
			this.UpdateNeeded = true;
		}

		internal void UpdateState()
		{
			this.UpdateState(true, true);
		}

		internal void UpdateState(bool inDb, bool inRoom)
		{
			if (inDb)
			{
				this.GetRoom().UpdateItem(this);
			}
			if (inRoom)
			{
				ServerMessage Message = new ServerMessage();
				if (this.IsFloorItem)
				{
					Message.Init(88);
					Message.AppendStringWithBreak(this.Id.ToString());
					Message.AppendStringWithBreak(this.ExtraData);
				}
				else
				{
					Message.Init(85);
					this.Serialize(Message);
				}
				this.GetRoom().SendMessage(Message, null);
			}
		}

		internal void Serialize(ServerMessage Message)
		{
			if (this.IsFloorItem)
			{
				Message.AppendUInt(this.Id);
				Message.AppendInt32(this.GetBaseItem().SpriteId);
				Message.AppendInt32(this.mX);
				Message.AppendInt32(this.mY);
				Message.AppendInt32(this.Rot);
				Message.AppendStringWithBreak(this.mZ.ToString().Replace(',', '.'));
				if (this.GetBaseItem().Name == "song_disk" && this.ExtraData.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(this.ExtraData));
					Message.AppendStringWithBreak("");
				}
				else
				{
					Message.AppendInt32(0);
					Message.AppendStringWithBreak(this.ExtraData);
				}
				Message.AppendInt32(-1);
				Message.AppendBoolean(!(this.GetBaseItem().InteractionType.ToLower() == "default"));
			}
			else
			{
				if (this.IsWallItem)
				{
					Message.AppendStringWithBreak(string.Concat(this.Id));
					Message.AppendInt32(this.GetBaseItem().SpriteId);
					Message.AppendStringWithBreak(this.string_7);
					if (this.GetBaseItem().Name.StartsWith("poster_"))
					{
						Message.AppendString(this.GetBaseItem().Name.Split(new char[]
						{
							'_'
						})[1]);
					}
					string text = this.GetBaseItem().InteractionType.ToLower();
					if (text != null && text == "postit")
					{
						Message.AppendStringWithBreak(this.ExtraData.Split(new char[]
						{
							' '
						})[0]);
					}
					else
					{
						Message.AppendStringWithBreak(this.ExtraData);
					}
					Message.AppendBoolean(!(this.GetBaseItem().InteractionType == "default"));
				}
			}
		}

		internal Item GetBaseItem()
		{
			if (mBaseItem == null)
			{
				mBaseItem = PhoenixEnvironment.GetGame().GetItemManager().GetItem(BaseItem);
			}
			return mBaseItem;
		}

		internal Room GetRoom()
		{
			if (mRoom == null)
			{
				mRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
			}
			return mRoom;
		}

		internal void method_9()
		{
			if (!(this.Extra3 == ""))
			{
				string[] collection = this.Extra3.Split(new char[]
				{
					','
				});
				IEnumerable<string> enumerable = new List<string>(collection);
				List<string> list = enumerable.ToList<string>();
				bool flag = false;
				if (list.Count > 5)
				{
					this.Extra3 = "";
					this.Extra4 = "";
				}
				else
				{
					foreach (string current in enumerable)
					{
						RoomItem @class = null;
						if (current.Length > 0)
						{
							@class = this.GetRoom().GetItem(Convert.ToUInt32(current));
						}
						if (@class == null)
						{
							list.Remove(current);
							flag = true;
						}
					}
					if (flag)
					{
						this.Extra4 = OldEncoding.encodeVL64(list.Count);
						for (int i = 0; i < list.Count; i++)
						{
							int value = Convert.ToInt32(list[i]);
							this.Extra4 += OldEncoding.encodeVL64(value);
							this.Extra3 = this.Extra3 + "," + Convert.ToString(value);
						}
						this.Extra3 = this.Extra3.Substring(1);
					}
				}
			}
		}

		internal void method_10()
		{
			if (!(this.Extra2 == ""))
			{
				string[] collection = this.Extra2.Split(new char[]
				{
					','
				});
				IEnumerable<string> enumerable = new List<string>(collection);
				List<string> list = enumerable.ToList<string>();
				bool flag = false;
				foreach (string current in enumerable)
				{
					RoomItem @class = this.GetRoom().GetItem(Convert.ToUInt32(current));
					if (@class == null)
					{
						list.Remove(current);
						flag = true;
					}
				}
				if (flag)
				{
					this.Extra1 = OldEncoding.encodeVL64(list.Count);
					for (int i = 0; i < list.Count; i++)
					{
						int num = Convert.ToInt32(list[i]);
						this.Extra1 += OldEncoding.encodeVL64(num);
					}
					this.Extra2 = string.Join(",", list.ToArray());
				}
			}
		}
	}
}
