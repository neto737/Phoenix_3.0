using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Users;
using Phoenix.HabboHotel.Pets;
using Phoenix.HabboHotel.Pathfinding;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.HabboHotel.RoomBots;
using Phoenix.HabboHotel.Items;
using Phoenix.Storage;
using System.Drawing;
namespace Phoenix.HabboHotel.Rooms
{
	internal sealed class RoomUser
	{
		public uint HabboId;
		public int VirtualId;
		public uint RoomId;
		public int IdleTime;
		internal int Steps;
		public int X;
		public int Y;
		public double Z;
		internal byte SqState;
		public int CarryItemID;
		public int CarryTimer;
		public int RotHead;
		public int RotBody;
		public bool CanWalk;
		public bool AllowOverride;
		public bool TeleportMode;
		public int int_9;
		public int GoalX;
		public int GoalY;
		public bool WalkBackwards;
		public bool SetStep;
		public int SetX;
		public int SetY;
		public double SetZ;
		public RoomBot BotData;
		public BotAI BotAI;
		internal byte byte_1;
		internal bool bool_5;
		public RoomUser Target;
		public RoomItem Item;
		public RoomBot Riding;
		public Pet PetData;
		public bool IsWalking;
		public bool UpdateNeeded;
		public bool IsAsleep;
		public int int_14;
		public Dictionary<string, string> Statusses;
		public int DanceId;
		public int PathStep;
		public bool PathRecalcNeeded;
		public int PathRecalcX;
		public int PathRecalcY;
		public int TeleDelay;
		internal bool IsSpectator;
		internal bool Visible;
		internal string ChangedClothes;
		internal int CurrentFurniFX;

		public Coord Coordinate
		{
			get
			{
				return new Coord(X, Y);
			}
		}

		public bool IsPet
		{
			get
			{
				return IsBot && BotData.IsPet;
			}
		}

		internal bool IsDancing
		{
			get
			{
				return DanceId >= 1;
			}
		}

		internal bool NeedsAutokick
		{
			get
			{
				return !IsBot && IdleTime >= GlobalClass.IdleKick;
			}
		}

		internal bool IsTrading
		{
			get
			{
				return !IsBot && Statusses.ContainsKey("trd");
			}
		}

		internal bool IsBot
		{
			get
			{
				return BotData != null;
			}
		}

		public RoomUser(uint UserId, uint RoomId, int VirtualId, bool Invisible)
		{
			this.HabboId = UserId;
			this.RoomId = RoomId;
			this.VirtualId = VirtualId;
			this.IdleTime = 0;
			this.X = 0;
			this.Y = 0;
			this.Z = 0.0;
			this.RotHead = 0;
			this.RotBody = 0;
			this.UpdateNeeded = true;
			this.Statusses = new Dictionary<string, string>();
			this.PathStep = 0;
			this.TeleDelay = -1;
			this.Target = null;
			this.AllowOverride = false;
			this.CanWalk = true;
			this.IsSpectator = false;
			this.SqState = 3;
			this.Steps = 0;
			this.CurrentFurniFX = 0;
			this.Visible = Invisible;
			this.ChangedClothes = "";
		}

		public void Unidle()
		{
			IdleTime = 0;
			if (IsAsleep)
			{
				IsAsleep = false;
				ServerMessage Message = new ServerMessage(486);
				Message.AppendInt32(VirtualId);
				Message.AppendBoolean(false);
				GetRoom().SendMessage(Message, null);
			}
		}

		internal void Chat(GameClient Session, string Message, bool Shout)
		{
			string val = Message;
			if (Session == null || (Session.GetHabbo().HasRole("ignore_roommute") || !GetRoom().RoomMuted))
			{
				this.Unidle();
				if (!this.IsBot && this.GetClient().GetHabbo().Muted)
				{
					this.GetClient().SendNotif(TextManager.GetText("error_muted"));
				}
				else
				{
					if (!Message.StartsWith(":") || Session == null || !ChatCommandHandler.Parse(Session, Message.Substring(1)))
					{
						uint num = 24;
						if (Shout)
						{
							num = 26;
						}
						if (!this.IsBot && Session.GetHabbo().MaxFloodTime() > 0)
						{
							TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().FloodTime;
							if (timeSpan.Seconds > 4)
							{
								Session.GetHabbo().FloodCount = 0;
							}
							if (timeSpan.Seconds < 4 && Session.GetHabbo().FloodCount > 5 && !this.IsBot)
							{
								ServerMessage message = new ServerMessage(27);
								message.AppendInt32(Session.GetHabbo().MaxFloodTime());
								this.GetClient().SendMessage(message);
								this.GetClient().GetHabbo().Muted = true;
								this.GetClient().GetHabbo().MuteLength = Session.GetHabbo().MaxFloodTime();
								return;
							}
							Session.GetHabbo().FloodTime = DateTime.Now;
							Session.GetHabbo().FloodCount++;
						}
						if (!this.IsBot && !Session.GetHabbo().isAaron)
						{
							Message = ChatCommandHandler.ApplyWordFilter(Message);
						}
						if (!this.GetRoom().WF_OnUserSay(this, Message))
						{
							ServerMessage Message2 = new ServerMessage(num);
							Message2.AppendInt32(this.VirtualId);
							if (Message.Contains("http://") || Message.Contains("www.") || Message.Contains("https://"))
							{
								string[] array = Message.Split(new char[]
								{
									' '
								});
								int num2 = 0;
								string text = "";
								string text2 = "";
								string[] array2 = array;
								for (int i = 0; i < array2.Length; i++)
								{
									string text3 = array2[i];
									if (ChatCommandHandler.CheckExternalLink(text3))
									{
										if (num2 > 0)
										{
											text += ",";
										}
										text += text3;
										object obj = text2;
										text2 = string.Concat(new object[]
										{
											obj,
											" {",
											num2,
											"}"
										});
										num2++;
									}
									else
									{
										text2 = text2 + " " + text3;
									}
								}
								Message = text2;
								string[] array3 = text.Split(new char[]
								{
									','
								});
								Message2.AppendStringWithBreak(Message);
								if (array3.Length > 0)
								{
									Message2.AppendBoolean(false);
									Message2.AppendInt32(num2);
									array2 = array3;
									for (int i = 0; i < array2.Length; i++)
									{
										string text4 = array2[i];
										string text5 = ChatCommandHandler.ApplyAdfly(text4.Replace("http://", "").Replace("https://", ""));
										Message2.AppendStringWithBreak(text5.Replace("http://", "").Replace("https://", ""));
										Message2.AppendStringWithBreak(text4);
										Message2.AppendBoolean(false);
									}
								}
							}
							else
							{
								Message2.AppendStringWithBreak(Message);
							}
							Message2.AppendInt32(this.GetSpeechEmotion(Message));
							Message2.AppendBoolean(false);
							if (!this.IsBot)
							{
								this.GetRoom().method_58(Message2, Session.GetHabbo().MutedUsers, Session.GetHabbo().Id);
							}
							else
							{
								this.GetRoom().SendMessage(Message2, null);
							}
						}
						else
						{
							if (!this.IsBot)
							{
								Session.GetHabbo().Sendselfwhisper(Message);
							}
						}
						if (!this.IsBot)
						{
							this.GetRoom().OnUserSay(this, Message, Shout);
							if (Session.GetHabbo().CurrentQuestId == 3)
							{
								PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(3, Session);
							}
						}
                        if (GlobalClass.RecordChatlogs && !this.IsBot)
						{
							using (DatabaseClient client = PhoenixEnvironment.GetDatabase().GetClient())
							{
								client.AddParamWithValue("message", val);
								client.ExecuteQuery(string.Concat(new object[]
								{
									"INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
									Session.GetHabbo().Id,
									"','",
									this.GetRoom().RoomId,
									"','",
									DateTime.Now.Hour,
									"','",
									DateTime.Now.Minute,
									"',UNIX_TIMESTAMP(),@message,'",
									Session.GetHabbo().Username,
									"','",
									DateTime.Now.ToLongDateString(),
									"')"
								}));
							}
						}
					}
				}
			}
		}

		internal int GetSpeechEmotion(string Message)
		{
			Message = Message.ToLower();
			if (Message.Contains(":)") || Message.Contains(":d") || Message.Contains("=]") || Message.Contains("=d") || Message.Contains(":>"))
			{
				return 1;
			}
			if (Message.Contains(">:(") || Message.Contains(":@"))
			{
				return 2;
			}
			if (Message.Contains(":o") || Message.Contains(";o"))
			{
				return 3;
			}
			if (Message.Contains(":(") || Message.Contains(";<") || Message.Contains("=[") || Message.Contains(":'(") || Message.Contains("='["))
			{
				return 4;
			}
			return 0;
		}

		internal void ClearMovement(bool Update)
		{
			this.IsWalking = false;
			this.PathRecalcNeeded = false;
			this.Statusses.Remove("mv");
			this.GoalX = 0;
			this.GoalY = 0;
			this.SetStep = false;
			this.SetX = 0;
			this.SetY = 0;
			this.SetZ = 0.0;
			if (Update)
			{
				this.UpdateNeeded = true;
			}
		}

		internal void MoveTo(Coord c)
		{
			this.MoveTo(c.X, c.Y);
		}

		internal void MoveTo(int X, int Y)
		{
			if (this.GetRoom().ValidTile(X, Y) && !this.GetRoom().SquareHasUsers(X, Y))
			{
				this.Unidle();
				this.IsWalking = true;
				this.PathRecalcNeeded = true;
				this.PathRecalcX = X;
				this.PathRecalcY = Y;
				if (X >= this.GetRoom().Model.MapSizeX || Y >= this.GetRoom().Model.MapSizeY)
				{
					this.GoalX = X;
					this.GoalY = Y;
				}
				else
				{
					this.GoalX = this.GetRoom().mBedMap[X, Y].X;
					this.GoalY = this.GetRoom().mBedMap[X, Y].Y;
				}
			}
		}

		internal void UnlockWalking()
		{
			this.AllowOverride = false;
			this.CanWalk = true;
		}

		internal void SetPos(int pX, int pY, double pZ)
		{
			this.X = pX;
			this.Y = pY;
			this.Z = pZ;
		}

		public void CarryItem(int Item)
		{
			this.CarryItemID = Item;
			if (Item > 1000)
			{
				this.CarryTimer = 5000;
			}
			else if (Item > 0)
		    {
				this.CarryTimer = 240;
			}
			else
			{
				this.CarryTimer = 0;
			}
			ServerMessage Message = new ServerMessage(482);
			Message.AppendInt32(this.VirtualId);
			Message.AppendInt32(Item);
			this.GetRoom().SendMessage(Message, null);
		}

		public void SetRot(int Rotation)
		{
			this.SetRot(Rotation, false);
		}

        public void SetRot(int Rotation, bool HeadOnly)
        {
            if (!Statusses.ContainsKey("lay") && !IsWalking)
            {
                int diff = RotBody - Rotation;
                RotHead = RotBody;
                if (Statusses.ContainsKey("sit") || HeadOnly)
                {
                    if (RotBody == 2 || RotBody == 4)
                    {
                        if (diff > 0)
                        {
                            RotHead = RotBody - 1;
                        }
                        else if (diff < 0)
                        {
                            RotHead = RotBody + 1;
                        }
                    }
                    else
                    {
                        if (RotBody == 0 || RotBody == 6)
                        {
                            if (diff > 0)
                            {
                                RotHead = RotBody - 1;
                            }
                            else if (diff < 0)
                            {
                                RotHead = RotBody + 1;
                            }
                        }
                    }
                }
                else if (diff <= -2 || diff >= 2)
                {
                    RotHead = Rotation;
                    RotBody = Rotation;
                }
                else
                {
                    RotHead = Rotation;
                }
            }
            UpdateNeeded = true;
        }

        public void AddStatus(string Key, string Value)
        {
            Statusses[Key] = Value;
        }

		public void RemoveStatus(string Key)
		{
			if (Statusses.ContainsKey(Key))
			{
				Statusses.Remove(Key);
			}
		}

		public void ResetStatus()
		{
			Statusses = new Dictionary<string, string>();
		}

		public void Serialize(ServerMessage Message)
		{
			if (Message != null && !IsSpectator)
			{
				if (!IsBot)
				{
					if (GetClient() != null && GetClient().GetHabbo() != null)
					{
						Habbo habbo = GetClient().GetHabbo();
						Message.AppendUInt(habbo.Id);
						Message.AppendStringWithBreak(habbo.Username);
						Message.AppendStringWithBreak(habbo.Motto);
						Message.AppendStringWithBreak(habbo.Look);
						Message.AppendInt32(VirtualId);
						Message.AppendInt32(X);
						Message.AppendInt32(Y);
						Message.AppendStringWithBreak(Z.ToString().Replace(',', '.'));
						Message.AppendInt32(2);
						Message.AppendInt32(1);
						Message.AppendStringWithBreak(habbo.Gender.ToLower());
						Message.AppendInt32(-1);
						if (habbo.GroupID > 0)
						{
							Message.AppendInt32(habbo.GroupID);
						}
						else
						{
							Message.AppendInt32(-1);
						}
						Message.AppendInt32(-1);
						Message.AppendStringWithBreak("");
						Message.AppendInt32(habbo.AchievementScore);
					}
				}
				else
				{
					Message.AppendInt32(BotAI.BaseId);
					Message.AppendStringWithBreak(BotData.Name);
					Message.AppendStringWithBreak(BotData.Motto);
					Message.AppendStringWithBreak(BotData.Look);
					Message.AppendInt32(VirtualId);
					Message.AppendInt32(X);
					Message.AppendInt32(Y);
					Message.AppendStringWithBreak(Z.ToString().Replace(',', '.'));
					Message.AppendInt32(4);
					Message.AppendInt32((BotData.AiType == AIType.Pet) ? 2 : 3);
					if (BotData.AiType == AIType.Pet)
					{
						Message.AppendInt32(0);
					}
				}
			}
		}

		public void SerializeStatus(ServerMessage Message)
		{
			if (!IsSpectator)
			{
				Message.AppendInt32(VirtualId);
				Message.AppendInt32(X);
				Message.AppendInt32(Y);
				Message.AppendStringWithBreak(Z.ToString().Replace(',', '.'));
				Message.AppendInt32(RotHead);
				Message.AppendInt32(RotBody);
				Message.AppendString("/");
				try
				{
					foreach (KeyValuePair<string, string> current in Statusses)
					{
						Message.AppendString(current.Key);
						Message.AppendString(" ");
						Message.AppendString(current.Value);
						Message.AppendString("/");
					}
				}
                catch { }
				Message.AppendStringWithBreak("/");
			}
		}

		public GameClient GetClient()
		{
			if (IsBot)
			{
				return null;
			}
			return PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(HabboId);
		}

		private Room GetRoom()
		{
			return PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
		}
	}
}
