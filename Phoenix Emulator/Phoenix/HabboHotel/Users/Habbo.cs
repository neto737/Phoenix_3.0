using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Phoenix.HabboHotel.Users.UserDataManagement;
using Phoenix.HabboHotel.Users.Subscriptions;
using Phoenix.HabboHotel.Users.Inventory;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.HabboHotel.Users.Messenger;
using Phoenix.HabboHotel.Users.Badges;
using Phoenix.Storage;
using Phoenix.HabboHotel.Guilds;
namespace Phoenix.HabboHotel.Users
{
	internal sealed class Habbo
	{
		public uint Id;
		public string Username;
		public string RealName;
        public bool isAaron;
		public bool Visible;
		public bool bool_2;
		public string SSO;
		public string LastIp;
		public uint Rank;
		public string Motto;
		public string Look;
		public string Gender;
		public int GroupID;
		public DataTable GroupMemberships;
		public List<int> list_0;
		public int int_1;
		public int Credits;
		public int ActivityPoints;
		public double LastActivityPointsUpdate;
		public bool Muted;
		public int MuteLength;
		internal bool bool_4 = false;
		public uint LoadingRoom;
		public bool LoadingChecksPassed;
		public bool Waitingfordoorbell;
		public uint CurrentRoomId;
		public uint uint_4;
		public bool IsTeleporting;
		public uint TeleporterId;
		public List<uint> FavoriteRooms;
		public List<uint> MutedUsers;
		public List<string> list_3;
		public Dictionary<uint, int> Achievements;
		public List<uint> RatedRooms;
		private SubscriptionManager SubscriptionManager;
		private HabboMessenger Messenger;
		private BadgeComponent BadgeComponent;
		private InventoryComponent InventoryComponent;
		private AvatarEffectsInventoryComponent AvatarEffectsInventoryComponent;
		private GameClient Session;
		public List<uint> CompletedQuests;
		public uint CurrentQuestId;
		public int CurrentQuestProgress;
		public int int_6;
		public int int_7;
		public int int_8;
		public int int_9;
		public uint uint_7;
		public int NewbieStatus;
		public bool SpectatorMode;
		public bool bool_9;
		public bool bool_10;
		public bool BlockNewFriends;
		public bool HideInRom;
		public bool HideOnline;
		public bool Vip;
		public int Volume;
		public int shells;
		public int AchievementScore;
		public int RoomVisits;
		public int Stat_OnlineTime;
		public int int_16;
		public int Respect;
		public int RespectGiven;
		public int GiftsGiven;
		public int GiftsReceived;
		public int DailyRespectPoints;
		public int DailyPetRespectPoints;
		private HabboData HabboData;
		internal List<RoomData> list_6;
		public int FloodCount;
		public DateTime FloodTime;
		public bool bool_15;
		public int BuyCount;
		private bool bool_16 = false;
        public bool FriendStreamEnabled;

		public bool InRoom
		{
			get
			{
				return this.CurrentRoomId >= 1;
			}
		}
		public Room CurrentRoom
		{
			get
			{
				if (this.CurrentRoomId <= 0)
				{
					return null;
				}
				else
				{
					return PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(this.CurrentRoomId);
				}
			}
		}
		internal HabboData GetHabboData
		{
			get
			{
				return this.HabboData;
			}
		}
		internal string String_0
		{
			get
			{
				this.bool_16 = true;
				int num = (int)PhoenixEnvironment.GetUnixTimestamp() - this.int_16;
				string text = string.Concat(new object[]
				{
					"UPDATE users SET last_online = UNIX_TIMESTAMP(), online = '0', activity_points_lastupdate = '",
					this.LastActivityPointsUpdate,
					"' WHERE Id = '",
					this.Id,
					"' LIMIT 1; "
				});
				object obj = text;
				return string.Concat(new object[]
				{
					obj,
					"UPDATE user_stats SET RoomVisits = '",
					this.RoomVisits,
					"', OnlineTime = OnlineTime + ",
					num,
					", Respect = '",
					this.Respect,
					"', RespectGiven = '",
					this.RespectGiven,
					"', GiftsGiven = '",
					this.GiftsGiven,
					"', GiftsReceived = '",
					this.GiftsReceived,
					"' WHERE Id = '",
					this.Id,
					"' LIMIT 1; "
				});
			}
		}
        public Habbo(uint Id, string Username, string RealName, string SSO, uint Rank, string Motto, string Look, string Gender, int Credits, int Pixels, double Activity_Points_LastUpdate, bool Muted, uint HomeRoom, int NewbieStatus, bool BlockNewFriends, bool HideInRoom, bool HideOnline, bool Vip, int Volume, int Points, bool AcceptTrading, string LastIp, GameClient Session, HabboData HabboData, bool FriendStream)
		{
			if (Session != null)
			{
				PhoenixEnvironment.GetGame().GetClientManager().method_0(Id, Username, Session);
			}
			this.Id = Id;
			this.Username = Username;
			this.RealName = RealName;
			this.isAaron = false;
            this.Visible = true;
			this.SSO = SSO;
			this.Rank = Rank;
			this.Motto = Motto;
			this.Look = PhoenixEnvironment.FilterInjectionChars(Look.ToLower());
			this.Gender = Gender.ToLower();
			this.Credits = Credits;
			this.shells = Points;
			this.ActivityPoints = Pixels;
			this.LastActivityPointsUpdate = Activity_Points_LastUpdate;
			this.bool_2 = AcceptTrading;
			this.Muted = Muted;
			this.LoadingRoom = 0u;
			this.LoadingChecksPassed = false;
			this.Waitingfordoorbell = false;
			this.CurrentRoomId = 0u;
			this.uint_4 = HomeRoom;
			this.FavoriteRooms = new List<uint>();
			this.MutedUsers = new List<uint>();
			this.list_3 = new List<string>();
			this.Achievements = new Dictionary<uint, int>();
			this.RatedRooms = new List<uint>();
			this.NewbieStatus = NewbieStatus;
			this.bool_10 = false;
			this.BlockNewFriends = BlockNewFriends;
			this.HideInRom = HideInRoom;
			this.HideOnline = HideOnline;
			this.Vip = Vip;
			this.Volume = Volume;
			this.int_1 = 0;
			this.BuyCount = 1;
			this.LastIp = LastIp;
			this.IsTeleporting = false;
			this.TeleporterId = 0u;
			this.Session = Session;
			this.HabboData = HabboData;
			this.list_6 = new List<RoomData>();
			this.list_0 = new List<int>();
            this.FriendStreamEnabled = FriendStream;
			DataRow dataRow = null;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("user_id", Id);
				dataRow = @class.ReadDataRow("SELECT * FROM user_stats WHERE Id = @user_id LIMIT 1");
				if (dataRow == null)
				{
					@class.ExecuteQuery("INSERT INTO user_stats (Id) VALUES ('" + Id + "')");
					dataRow = @class.ReadDataRow("SELECT * FROM user_stats WHERE Id = @user_id LIMIT 1");
				}
				this.GroupMemberships = @class.ReadDataTable("SELECT * FROM group_memberships WHERE userid = @user_id");
				IEnumerator enumerator;
				if (this.GroupMemberships != null)
				{
					enumerator = this.GroupMemberships.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow2 = (DataRow)enumerator.Current;
                            Guild class2 = GuildManager.GetGuild((int)dataRow2["groupid"]);
							if (class2 == null)
							{
								DataTable dataTable = @class.ReadDataTable("SELECT * FROM groups WHERE Id = " + (int)dataRow2["groupid"] + " LIMIT 1;");
								IEnumerator enumerator2 = dataTable.Rows.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										DataRow dataRow3 = (DataRow)enumerator2.Current;
                                        if (!GuildManager.GuildList.ContainsKey((int)dataRow3["Id"]))
										{
                                            GuildManager.GuildList.Add((int)dataRow3["Id"], new Guild((int)dataRow3["Id"], dataRow3, @class));
										}
									}
									continue;
								}
								finally
								{
									IDisposable disposable = enumerator2 as IDisposable;
									if (disposable != null)
									{
										disposable.Dispose();
									}
								}
							}
							if (!class2.List.Contains((int)Id))
							{
								class2.AddMember((int)Id);
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					int num = (int)dataRow["groupid"];
                    Guild class3 = GuildManager.GetGuild(num);
					if (class3 != null)
					{
						this.GroupID = num;
					}
					else
					{
						this.GroupID = 0;
					}
				}
				else
				{
					this.GroupID = 0;
				}
				DataTable dataTable2 = @class.ReadDataTable("SELECT groupid FROM group_requests WHERE userid = '" + Id + "';");
				enumerator = dataTable2.Rows.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						DataRow dataRow2 = (DataRow)enumerator.Current;
						this.list_0.Add((int)dataRow2["groupid"]);
					}
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
			this.RoomVisits = (int)dataRow["RoomVisits"];
			this.int_16 = (int)PhoenixEnvironment.GetUnixTimestamp();
			this.Stat_OnlineTime = (int)dataRow["OnlineTime"];
			this.Respect = (int)dataRow["Respect"];
			this.RespectGiven = (int)dataRow["RespectGiven"];
			this.GiftsGiven = (int)dataRow["GiftsGiven"];
			this.GiftsReceived = (int)dataRow["GiftsReceived"];
			this.DailyRespectPoints = (int)dataRow["DailyRespectPoints"];
			this.DailyPetRespectPoints = (int)dataRow["DailyPetRespectPoints"];
			this.AchievementScore = (int)dataRow["AchievementScore"];
			this.CompletedQuests = new List<uint>();
			this.uint_7 = 0u;
			this.CurrentQuestId = (uint)dataRow["quest_id"];
			this.CurrentQuestProgress = (int)dataRow["quest_progress"];
			this.int_6 = (int)dataRow["lev_builder"];
			this.int_8 = (int)dataRow["lev_identity"];
			this.int_7 = (int)dataRow["lev_social"];
			this.int_9 = (int)dataRow["lev_explore"];
			if (Session != null)
			{
				this.SubscriptionManager = new SubscriptionManager(Id, HabboData);
				this.BadgeComponent = new BadgeComponent(Id, HabboData);
				this.InventoryComponent = new InventoryComponent(Id, Session, HabboData);
				this.AvatarEffectsInventoryComponent = new AvatarEffectsInventoryComponent(Id, Session, HabboData);
				this.SpectatorMode = false;
				this.bool_9 = false;
				foreach (DataRow dataRow3 in HabboData.GetUsersRooms.Rows)
				{
					this.list_6.Add(PhoenixEnvironment.GetGame().GetRoomManager().method_17((uint)dataRow3["Id"], dataRow3));
				}
			}
		}
		public void UpdateGroups(DatabaseClient class6_0)
		{
			this.GroupMemberships = class6_0.ReadDataTable("SELECT * FROM group_memberships WHERE userid = " + this.Id);
			if (this.GroupMemberships != null)
			{
				foreach (DataRow dataRow in this.GroupMemberships.Rows)
				{
                    Guild @class = GuildManager.GetGuild((int)dataRow["groupid"]);
					if (@class == null)
					{
						DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM groups WHERE Id = " + (int)dataRow["groupid"] + " LIMIT 1;");
						IEnumerator enumerator2 = dataTable.Rows.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								DataRow dataRow2 = (DataRow)enumerator2.Current;
                                if (!GuildManager.GuildList.ContainsKey((int)dataRow2["Id"]))
								{
                                    GuildManager.GuildList.Add((int)dataRow2["Id"], new Guild((int)dataRow2["Id"], dataRow2, class6_0));
								}
							}
							continue;
						}
						finally
						{
							IDisposable disposable = enumerator2 as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					if (!@class.List.Contains((int)this.Id))
					{
						@class.AddMember((int)this.Id);
					}
				}
				int num = class6_0.ReadInt32("SELECT groupid FROM user_stats WHERE Id = " + this.Id + " LIMIT 1");
                Guild class2 = GuildManager.GetGuild(num);
				if (class2 != null)
				{
					this.GroupID = num;
				}
				else
				{
					this.GroupID = 0;
				}
			}
			else
			{
				this.GroupID = 0;
			}
		}
		internal void UpdateRooms(DatabaseClient class6_0)
		{
			this.list_6.Clear();
			class6_0.AddParamWithValue("name", this.Username);
			DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM rooms WHERE owner = @name ORDER BY Id ASC");
			foreach (DataRow dataRow in dataTable.Rows)
			{
				this.list_6.Add(PhoenixEnvironment.GetGame().GetRoomManager().method_17((uint)dataRow["Id"], dataRow));
			}
		}
		public void LoadData(HabboData class12_1)
		{
			this.method_8(class12_1);
			this.method_5(class12_1);
			this.method_6(class12_1);
			this.method_7(class12_1);
			this.LoadQuests();
		}
		public bool HasRole(string string_7)
		{
			if (PhoenixEnvironment.GetGame().GetRoleManager().UserHasPersonalPermissions(this.Id))
			{
				return PhoenixEnvironment.GetGame().GetRoleManager().UserHasPermission(this.Id, string_7);
			}
			else
			{
				return PhoenixEnvironment.GetGame().GetRoleManager().RankHasRight(this.Rank, string_7);
			}
		}
		public int MaxFloodTime()
		{
			if (this.isAaron)
			{
				return 0;
			}
			else
			{
				return PhoenixEnvironment.GetGame().GetRoleManager().FloodTime(this.Rank);
			}
		}
		public void method_5(HabboData class12_1)
		{
			this.FavoriteRooms.Clear();
			DataTable dataTable_ = class12_1.GetUserFavouriteRooms;
			foreach (DataRow dataRow in dataTable_.Rows)
			{
				this.FavoriteRooms.Add((uint)dataRow["room_id"]);
			}
		}
		public void method_6(HabboData class12_1)
		{
			DataTable dataTable_ = class12_1.GetUserIgnores;
			foreach (DataRow dataRow in dataTable_.Rows)
			{
				this.MutedUsers.Add((uint)dataRow["ignore_id"]);
			}
		}
		public void method_7(HabboData class12_1)
		{
			this.list_3.Clear();
			DataTable dataTable_ = class12_1.GetUserTags;
			foreach (DataRow dataRow in dataTable_.Rows)
			{
				this.list_3.Add((string)dataRow["tag"]);
			}
			if (this.list_3.Count >= 5 && this.GetClient() != null)
			{
				PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this.GetClient(), 7u, 1);
			}
		}
		public void method_8(HabboData class12_1)
		{
			DataTable dataTable = class12_1.GetAchievementData;
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.Achievements.Add((uint)dataRow["achievement_id"], (int)dataRow["achievement_level"]);
				}
			}
		}
		public void OnDisconnect()
		{
			if (!this.bool_9)
			{
				this.bool_9 = true;
				PhoenixEnvironment.GetGame().GetClientManager().method_1(this.Id, this.Username);
				if (!this.bool_16)
				{
					this.bool_16 = true;
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE users SET last_online = UNIX_TIMESTAMP(), users.online = '0', activity_points = '",
							this.ActivityPoints,
							"', activity_points_lastupdate = '",
							this.LastActivityPointsUpdate,
							"', credits = '",
							this.Credits,
							"' WHERE Id = '",
							this.Id,
							"' LIMIT 1;"
						}));
						int num = (int)PhoenixEnvironment.GetUnixTimestamp() - this.int_16;
						adapter.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE user_stats SET RoomVisits = '",
							this.RoomVisits,
							"', OnlineTime = OnlineTime + ",
							num,
							", Respect = '",
							this.Respect,
							"', RespectGiven = '",
							this.RespectGiven,
							"', GiftsGiven = '",
							this.GiftsGiven,
							"', GiftsReceived = '",
							this.GiftsReceived,
							"' WHERE Id = '",
							this.Id,
							"' LIMIT 1; "
						}));
					}
				}
				if (this.InRoom && this.CurrentRoom != null)
				{
					this.CurrentRoom.RemoveUserFromRoom(this.Session, false, false);
				}
				if (this.Messenger != null)
				{
					this.Messenger.AppearOffline = true;
					this.Messenger.method_5(true);
					this.Messenger = null;
				}
				if (this.SubscriptionManager != null)
				{
					this.SubscriptionManager.Clear();
					this.SubscriptionManager = null;
				}
				this.InventoryComponent.method_18();
			}
		}
		internal void method_10(uint RoomId)
		{
			if (GlobalClass.RecordRoomVisits)
			{
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO user_roomvisits (user_id,room_id,entry_timestamp,exit_timestamp,hour,minute) VALUES ('",
						this.Id,
						"','",
						RoomId,
						"',UNIX_TIMESTAMP(),'0','",
						DateTime.Now.Hour,
						"','",
						DateTime.Now.Minute,
						"')"
					}));
				}
			}
			this.CurrentRoomId = RoomId;
			if (this.CurrentRoom.Owner != this.Username && this.CurrentQuestId == 15u)
			{
				PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(15u, this.GetClient());
			}
			this.Messenger.method_5(false);
		}
		public void method_11()
		{
			try
			{
				if (GlobalClass.RecordRoomVisits)
				{
					using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
					{
						@class.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE user_roomvisits SET exit_timestamp = UNIX_TIMESTAMP() WHERE room_id = '",
							this.CurrentRoomId,
							"' AND user_id = '",
							this.Id,
							"' ORDER BY entry_timestamp DESC LIMIT 1"
						}));
					}
				}
			}
			catch
			{
			}
			this.CurrentRoomId = 0u;
			if (this.Messenger != null)
			{
				this.Messenger.method_5(false);
			}
		}
		public void method_12()
		{
			if (this.GetMessenger() == null)
			{
				this.Messenger = new HabboMessenger(this.Id);
				this.Messenger.LoadBuddies(this.HabboData);
				this.Messenger.method_1(this.HabboData);
				GameClient @class = this.GetClient();
				if (@class != null)
				{
					@class.SendMessage(this.Messenger.method_21());
					@class.SendMessage(this.Messenger.method_23());
					this.Messenger.method_5(true);
				}
			}
		}
		public void UpdateCreditsBalance(bool bool_17)
		{
			ServerMessage Message = new ServerMessage(6u);
			Message.AppendStringWithBreak(this.Credits + ".0");
			this.Session.SendMessage(Message);
			if (bool_17)
			{
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET credits = '",
						this.Credits,
						"' WHERE Id = '",
						this.Id,
						"' LIMIT 1;"
					}));
				}
			}
		}
		public void UpdateShellsBalance(bool bool_17, bool bool_18)
		{
			if (bool_17)
			{
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					this.shells = @class.ReadInt32("SELECT vip_points FROM users WHERE Id = '" + this.Id + "' LIMIT 1;");
				}
			}
			if (bool_18)
			{
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET vip_points = '",
						this.shells,
						"' WHERE Id = '",
						this.Id,
						"' LIMIT 1;"
					}));
				}
			}
			this.UpdateActivityPointsBalance(0);
		}
		public void UpdateActivityPointsBalance(bool bool_17)
		{
			this.UpdateActivityPointsBalance(0);
			if (bool_17)
			{
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET activity_points = '",
						this.ActivityPoints,
						"' WHERE Id = '",
						this.Id,
						"' LIMIT 1;"
					}));
				}
			}
		}
		public void UpdateActivityPointsBalance(int NotifAmount)
		{
			ServerMessage Message = new ServerMessage(438);
			Message.AppendInt32(this.ActivityPoints);
			Message.AppendInt32(NotifAmount);
			Message.AppendInt32(0);
			ServerMessage Message2 = new ServerMessage(438);
			Message2.AppendInt32(this.shells);
			Message2.AppendInt32(0);
			Message2.AppendInt32(1);
			ServerMessage Message3 = new ServerMessage(438);
			Message3.AppendInt32(this.shells);
			Message3.AppendInt32(0);
			Message3.AppendInt32(2);
			ServerMessage Message4 = new ServerMessage(438);
			Message4.AppendInt32(this.shells);
			Message4.AppendInt32(0);
			Message4.AppendInt32(3);
			ServerMessage Message5 = new ServerMessage(438);
			Message5.AppendInt32(this.shells);
			Message5.AppendInt32(0);
			Message5.AppendInt32(4);
			this.Session.SendMessage(Message);
			this.Session.SendMessage(Message2);
			this.Session.SendMessage(Message3);
			this.Session.SendMessage(Message4);
			this.Session.SendMessage(Message5);
		}
		public void Mute()
		{
			if (!this.Muted)
			{
				this.GetClient().SendNotif("You have been muted by a moderator.");
				this.Muted = true;
                using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "UPDATE users SET is_muted = '1' WHERE Id = '", this.Id, "' LIMIT 1;"
                    }));
                }
			}
		}
		public void Unmute()
		{
			if (this.Muted)
			{
				this.Muted = false;
                using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "UPDATE users SET is_muted = '0' WHERE Id = '", this.Id, "' LIMIT 1;"
                    }));
                }
			}
		}
		private GameClient GetClient()
		{
			return PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Id);
		}
		public SubscriptionManager GetSubscriptionManager()
		{
			return SubscriptionManager;
		}
		public HabboMessenger GetMessenger()
		{
			return Messenger;
		}
		public BadgeComponent GetBadgeComponent()
		{
			return BadgeComponent;
		}
		public InventoryComponent GetInventoryComponent()
		{
			return InventoryComponent;
		}
		public AvatarEffectsInventoryComponent GetAvatarEffectsInventoryComponent()
		{
			return AvatarEffectsInventoryComponent;
		}
		public void LoadQuests()
		{
			this.CompletedQuests.Clear();
			DataTable Data = null;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				Data = adapter.ReadDataTable("SELECT quest_id FROM user_quests WHERE user_id = '" + this.Id + "'");
			}
			if (Data != null)
			{
				foreach (DataRow dataRow in Data.Rows)
				{
					this.CompletedQuests.Add((uint)dataRow["quest_Id"]);
				}
			}
		}
		public void method_26(bool bool_17, GameClient class16_1)
		{
			ServerMessage Message = new ServerMessage(266u);
			Message.AppendInt32(-1);
			Message.AppendStringWithBreak(class16_1.GetHabbo().Look);
			Message.AppendStringWithBreak(class16_1.GetHabbo().Gender.ToLower());
			Message.AppendStringWithBreak(class16_1.GetHabbo().Motto);
			Message.AppendInt32(class16_1.GetHabbo().AchievementScore);
			Message.AppendStringWithBreak("");
			class16_1.SendMessage(Message);
			if (class16_1.GetHabbo().InRoom)
			{
				Room class14_ = class16_1.GetHabbo().CurrentRoom;
				if (class14_ != null)
				{
					RoomUser @class = class14_.GetRoomUserByHabbo(class16_1.GetHabbo().Id);
					if (@class != null)
					{
						if (bool_17)
						{
							DataRow dataRow = null;
							using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								class2.AddParamWithValue("userid", class16_1.GetHabbo().Id);
								dataRow = class2.ReadDataRow("SELECT * FROM users WHERE Id = @userid LIMIT 1");
							}
							class16_1.GetHabbo().Motto = PhoenixEnvironment.FilterInjectionChars((string)dataRow["motto"]);
							class16_1.GetHabbo().Look = PhoenixEnvironment.FilterInjectionChars((string)dataRow["look"]);
						}
						ServerMessage Message2 = new ServerMessage(266u);
						Message2.AppendInt32(@class.VirtualId);
						Message2.AppendStringWithBreak(class16_1.GetHabbo().Look);
						Message2.AppendStringWithBreak(class16_1.GetHabbo().Gender.ToLower());
						Message2.AppendStringWithBreak(class16_1.GetHabbo().Motto);
						Message2.AppendInt32(class16_1.GetHabbo().AchievementScore);
						Message2.AppendStringWithBreak("");
						class14_.SendMessage(Message2, null);
					}
				}
			}
		}
		public void UpdateVIP()
		{
			DataRow dataRow;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				dataRow = @class.ReadDataRow("SELECT vip FROM users WHERE Id = '" + this.Id + "' LIMIT 1;");
			}
			this.Vip = PhoenixEnvironment.EnumToBool(dataRow["vip"].ToString());
			ServerMessage Message = new ServerMessage(2u);
			if (this.Vip || GlobalClass.VIPclothesforHCusers)
			{
				Message.AppendInt32(2);
			}
			else
			{
				if (this.GetSubscriptionManager().HasSubscription("habbo_club"))
				{
					Message.AppendInt32(1);
				}
				else
				{
					Message.AppendInt32(0);
				}
			}
			if (this.HasRole("acc_anyroomowner"))
			{
				Message.AppendInt32(7);
			}
			else
			{
				if (this.HasRole("acc_anyroomrights"))
				{
					Message.AppendInt32(5);
				}
				else
				{
					if (this.HasRole("acc_supporttool"))
					{
						Message.AppendInt32(4);
					}
					else
					{
						if (this.Vip || GlobalClass.VIPclothesforHCusers || this.GetSubscriptionManager().HasSubscription("habbo_club"))
						{
							Message.AppendInt32(2);
						}
						else
						{
							Message.AppendInt32(0);
						}
					}
				}
			}
			this.GetClient().SendMessage(Message);
		}
		public void Sendselfwhisper(string string_7)
		{
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(this.CurrentRoomId);
			if (room != null)
			{
				RoomUser class2 = room.GetRoomUserByHabbo(this.Id);
				ServerMessage Message = new ServerMessage(25);
				Message.AppendInt32(class2.VirtualId);
				Message.AppendStringWithBreak(string_7);
				Message.AppendBoolean(false);
				this.GetClient().SendMessage(Message);
			}
		}
	}
}
