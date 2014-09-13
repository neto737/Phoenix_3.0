using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Phoenix.HabboHotel.Users.Subscriptions;
using Phoenix.HabboHotel.Users.Inventory;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.HabboHotel.Users.Messenger;
using Phoenix.HabboHotel.Users.Badges;
using Phoenix.Storage;
using Phoenix.HabboHotel.Groups;
namespace Phoenix.HabboHotel.Users
{
	internal class Habbo
	{
		public uint Id;
		public string Username;
		public string RealName;
        public bool isAaron;
		public bool Visible;
		public bool AcceptTrading;
		public string SSO;
		public string LastIp;
		public uint Rank;
		public string Motto;
		public string Look;
		public string Gender;
		public int GroupID;
		public DataTable GroupMemberships;
		public List<int> GroupReqs;
		public int Rigger;
		public int Credits;
		public int ActivityPoints;
		public double LastActivityPointsUpdate;
		public bool Muted;
		public int MuteLength;
		//internal bool bool_4 = false;
		public uint LoadingRoom;
		public bool LoadingChecksPassed;
		public bool Waitingfordoorbell;
		public uint CurrentRoomId;
		public uint HomeRoom;
		public bool IsTeleporting;
		public uint TeleporterId;
		public List<uint> FavoriteRooms;
		public List<uint> MutedUsers;
		public List<string> Tags;
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
		public int LevelBuilder;
		public int LevelSocial;
		public int LevelIdentity;
		public int LevelExplorer;
		public uint LastQuestId;
		public int NewbieStatus;
		public bool SpectatorMode;
		public bool Disconnected;
		public bool CalledGuideBot;
		public bool BlockNewFriends;
		public bool HideInRom;
		public bool HideOnline;
		public bool Vip;
		public int Volume;
		public int shells;
		public int AchievementScore;
		public int RoomVisits;
		public int Stat_OnlineTime;
		public int Stat_LoginTime;
		public int Respect;
		public int RespectGiven;
		public int GiftsGiven;
		public int GiftsReceived;
		public int DailyRespectPoints;
		public int DailyPetRespectPoints;
		private HabboData HabboData;
		internal List<RoomData> UsersRooms;
		public int FloodCount;
		public DateTime FloodTime;
		public bool Flooded;
		public int BuyCount;
		private bool HabboInfoSaved = false;
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
					return PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(CurrentRoomId);
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

		internal string GetQueryString
		{
			get
			{
				this.HabboInfoSaved = true;
				int num = (int)PhoenixEnvironment.GetUnixTimestamp() - this.Stat_LoginTime;
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
				PhoenixEnvironment.GetGame().GetClientManager().RegisterClientShit(Id, Username, Session);
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
			this.AcceptTrading = AcceptTrading;
			this.Muted = Muted;
			this.LoadingRoom = 0;
			this.LoadingChecksPassed = false;
			this.Waitingfordoorbell = false;
			this.CurrentRoomId = 0;
			this.HomeRoom = HomeRoom;
			this.FavoriteRooms = new List<uint>();
			this.MutedUsers = new List<uint>();
			this.Tags = new List<string>();
			this.Achievements = new Dictionary<uint, int>();
			this.RatedRooms = new List<uint>();
			this.NewbieStatus = NewbieStatus;
			this.CalledGuideBot = false;
			this.BlockNewFriends = BlockNewFriends;
			this.HideInRom = HideInRoom;
			this.HideOnline = HideOnline;
			this.Vip = Vip;
			this.Volume = Volume;
			this.Rigger = 0;
			this.BuyCount = 1;
			this.LastIp = LastIp;
			this.IsTeleporting = false;
			this.TeleporterId = 0;
			this.Session = Session;
			this.HabboData = HabboData;
			this.UsersRooms = new List<RoomData>();
			this.GroupReqs = new List<int>();
            this.FriendStreamEnabled = FriendStream;
			DataRow dataRow = null;

			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("user_id", Id);
				dataRow = adapter.ReadDataRow("SELECT * FROM user_stats WHERE Id = @user_id LIMIT 1");
				if (dataRow == null)
				{
					adapter.ExecuteQuery("INSERT INTO user_stats (Id) VALUES ('" + Id + "')");
					dataRow = adapter.ReadDataRow("SELECT * FROM user_stats WHERE Id = @user_id LIMIT 1");
				}
				this.GroupMemberships = adapter.ReadDataTable("SELECT * FROM group_memberships WHERE userid = @user_id");
				IEnumerator enumerator;
				if (this.GroupMemberships != null)
				{
					enumerator = this.GroupMemberships.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow2 = (DataRow)enumerator.Current;
                            Group class2 = GroupManager.GetGroup((int)dataRow2["groupid"]);
							if (class2 == null)
							{
								DataTable dataTable = adapter.ReadDataTable("SELECT * FROM groups WHERE Id = " + (int)dataRow2["groupid"] + " LIMIT 1;");
								IEnumerator enumerator2 = dataTable.Rows.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										DataRow dataRow3 = (DataRow)enumerator2.Current;
                                        if (!GroupManager.GroupList.ContainsKey((int)dataRow3["Id"]))
										{
                                            GroupManager.GroupList.Add((int)dataRow3["Id"], new Group((int)dataRow3["Id"], dataRow3, adapter));
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
                    Group class3 = GroupManager.GetGroup(num);
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
				DataTable dataTable2 = adapter.ReadDataTable("SELECT groupid FROM group_requests WHERE userid = '" + Id + "';");
				enumerator = dataTable2.Rows.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						DataRow dataRow2 = (DataRow)enumerator.Current;
						this.GroupReqs.Add((int)dataRow2["groupid"]);
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
			this.Stat_LoginTime = (int)PhoenixEnvironment.GetUnixTimestamp();
			this.Stat_OnlineTime = (int)dataRow["OnlineTime"];
			this.Respect = (int)dataRow["Respect"];
			this.RespectGiven = (int)dataRow["RespectGiven"];
			this.GiftsGiven = (int)dataRow["GiftsGiven"];
			this.GiftsReceived = (int)dataRow["GiftsReceived"];
			this.DailyRespectPoints = (int)dataRow["DailyRespectPoints"];
			this.DailyPetRespectPoints = (int)dataRow["DailyPetRespectPoints"];
			this.AchievementScore = (int)dataRow["AchievementScore"];
			this.CompletedQuests = new List<uint>();
			this.LastQuestId = 0u;
			this.CurrentQuestId = (uint)dataRow["quest_id"];
			this.CurrentQuestProgress = (int)dataRow["quest_progress"];
			this.LevelBuilder = (int)dataRow["lev_builder"];
			this.LevelIdentity = (int)dataRow["lev_identity"];
			this.LevelSocial = (int)dataRow["lev_social"];
			this.LevelExplorer = (int)dataRow["lev_explore"];
			if (Session != null)
			{
				this.SubscriptionManager = new SubscriptionManager(Id, HabboData);
				this.BadgeComponent = new BadgeComponent(Id, HabboData);
				this.InventoryComponent = new InventoryComponent(Id, Session, HabboData);
				this.AvatarEffectsInventoryComponent = new AvatarEffectsInventoryComponent(Id, Session, HabboData);
				this.SpectatorMode = false;
				this.Disconnected = false;
				foreach (DataRow dataRow3 in HabboData.GetUsersRooms.Rows)
				{
					this.UsersRooms.Add(PhoenixEnvironment.GetGame().GetRoomManager().FetchRoomData((uint)dataRow3["Id"], dataRow3));
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

		public void UpdateGroups(DatabaseClient dbClient)
		{
			this.GroupMemberships = dbClient.ReadDataTable("SELECT * FROM group_memberships WHERE userid = " + Id);
			if (this.GroupMemberships != null)
			{
				foreach (DataRow dataRow in this.GroupMemberships.Rows)
				{
                    Group group = GroupManager.GetGroup((int)dataRow["groupid"]);
					if (group == null)
					{
						DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM groups WHERE Id = " + (int)dataRow["groupid"] + " LIMIT 1;");
						IEnumerator enumerator2 = dataTable.Rows.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								DataRow dataRow2 = (DataRow)enumerator2.Current;
                                if (!GroupManager.GroupList.ContainsKey((int)dataRow2["Id"]))
								{
                                    GroupManager.GroupList.Add((int)dataRow2["Id"], new Group((int)dataRow2["Id"], dataRow2, dbClient));
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
					if (!group.List.Contains((int)this.Id))
					{
						group.AddMember((int)this.Id);
					}
				}
				int num = dbClient.ReadInt32("SELECT groupid FROM user_stats WHERE Id = " + this.Id + " LIMIT 1");
                Group class2 = GroupManager.GetGroup(num);
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

		internal void UpdateRooms(DatabaseClient dbClient)
		{
			this.UsersRooms.Clear();
			dbClient.AddParamWithValue("name", Username);
			DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM rooms WHERE owner = @name ORDER BY Id ASC");
			foreach (DataRow dataRow in dataTable.Rows)
			{
				this.UsersRooms.Add(PhoenixEnvironment.GetGame().GetRoomManager().FetchRoomData((uint)dataRow["Id"], dataRow));
			}
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

		public void LoadFavorites(HabboData UserData)
		{
			this.FavoriteRooms.Clear();
            foreach (DataRow dataRow in UserData.GetUserFavouriteRooms.Rows)
			{
				FavoriteRooms.Add((uint)dataRow["room_id"]);
			}
		}

		public void LoadMutedUsers(HabboData UserData)
		{
            foreach (DataRow Row in UserData.GetUserIgnores.Rows)
			{
				MutedUsers.Add((uint)Row["ignore_id"]);
			}
		}

		public void LoadTags(HabboData UserData)
		{
			this.Tags.Clear();
			foreach (DataRow Row in UserData.GetUserTags.Rows)
			{
				Tags.Add((string)Row["tag"]);
			}
			if (Tags.Count >= 5 && GetClient() != null)
			{
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(GetClient(), 7, 1);
			}
		}

		public void LoadAchievements(HabboData UserData)
		{
			DataTable getAchievementData = UserData.GetAchievementData;
			if (getAchievementData != null)
			{
				foreach (DataRow Row in getAchievementData.Rows)
				{
					this.Achievements.Add((uint)Row["achievement_id"], (int)Row["achievement_level"]);
				}
			}
		}

        public void LoadData(HabboData UserData)
        {
            this.LoadAchievements(UserData);
            this.LoadFavorites(UserData);
            this.LoadMutedUsers(UserData);
            this.LoadTags(UserData);
            this.LoadQuests();
        }

		public void OnDisconnect()
		{
			if (!this.Disconnected)
			{
				this.Disconnected = true;
				PhoenixEnvironment.GetGame().GetClientManager().NullClientShit(this.Id, this.Username);
				if (!this.HabboInfoSaved)
				{
					this.HabboInfoSaved = true;
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
						int num = (int)PhoenixEnvironment.GetUnixTimestamp() - this.Stat_LoginTime;
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
					this.Messenger.OnStatusChanged(true);
					this.Messenger = null;
				}
				if (this.SubscriptionManager != null)
				{
					this.SubscriptionManager.Clear();
					this.SubscriptionManager = null;
				}
				this.InventoryComponent.RunDBUpdate();
			}
		}

		internal void OnEnterRoom(uint RoomId)
		{
			if (GlobalClass.RecordRoomVisits)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO user_roomvisits (user_id,room_id,entry_timestamp,exit_timestamp,hour,minute) VALUES ('",
						Id,
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
			if (CurrentRoom.Owner != Username && CurrentQuestId == 15)
			{
				PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(15, GetClient());
			}
			this.Messenger.OnStatusChanged(false);
		}

		public void OnLeaveRoom()
		{
			try
			{
				if (GlobalClass.RecordRoomVisits)
				{
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE user_roomvisits SET exit_timestamp = UNIX_TIMESTAMP() WHERE room_id = '",
							CurrentRoomId,
							"' AND user_id = '",
							Id,
							"' ORDER BY entry_timestamp DESC LIMIT 1"
						}));
					}
				}
			}
            catch { }
			this.CurrentRoomId = 0;
			if (Messenger != null)
			{
				Messenger.OnStatusChanged(false);
			}
		}

		public void InitMessenger()
		{
			if (GetMessenger() == null)
			{
				this.Messenger = new HabboMessenger(Id);
				this.Messenger.LoadBuddies(HabboData);
				this.Messenger.method_1(HabboData);

				GameClient client = GetClient();
				if (client != null)
				{
					client.SendMessage(this.Messenger.SerializeFriends());
					client.SendMessage(this.Messenger.SerializeRequests());
					this.Messenger.OnStatusChanged(true);
				}
			}
		}

		public void UpdateCreditsBalance(bool InDatabase)
		{
			ServerMessage Message = new ServerMessage(6);
			Message.AppendStringWithBreak(Credits + ".0");
			this.Session.SendMessage(Message);
			if (InDatabase)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET credits = '",
						Credits,
						"' WHERE Id = '",
						Id,
						"' LIMIT 1;"
					}));
				}
			}
		}

		public void UpdateShellsBalance(bool FromDatabase, bool ToDatabase)
		{
			if (FromDatabase)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					this.shells = adapter.ReadInt32("SELECT vip_points FROM users WHERE Id = '" + Id + "' LIMIT 1;");
				}
			}
			if (ToDatabase)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery("UPDATE users SET vip_points = '" + shells + "' WHERE Id = '" + Id + "' LIMIT 1;");
				}
			}
			UpdateActivityPointsBalance(0);
		}

        public void UpdateActivityPointsBalance(bool InDatabase)
        {
            UpdateActivityPointsBalance(0);
            if (InDatabase)
            {
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.ExecuteQuery("UPDATE users SET activity_points = '" + ActivityPoints + "' WHERE Id = '" + Id + "' LIMIT 1;");
                }
            }
        }

		public void UpdateActivityPointsBalance(int NotifAmount)
		{
			ServerMessage Message = new ServerMessage(438);
			Message.AppendInt32(ActivityPoints);
			Message.AppendInt32(NotifAmount);
			Message.AppendInt32(0);
			ServerMessage Message2 = new ServerMessage(438);
			Message2.AppendInt32(shells);
			Message2.AppendInt32(0);
			Message2.AppendInt32(1);
			ServerMessage Message3 = new ServerMessage(438);
			Message3.AppendInt32(shells);
			Message3.AppendInt32(0);
			Message3.AppendInt32(2);
			ServerMessage Message4 = new ServerMessage(438);
			Message4.AppendInt32(shells);
			Message4.AppendInt32(0);
			Message4.AppendInt32(3);
			ServerMessage Message5 = new ServerMessage(438);
			Message5.AppendInt32(shells);
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
			if (!Muted)
			{
				GetClient().SendNotif("You have been muted by a moderator.");
				Muted = true;
                using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery("UPDATE users SET is_muted = '1' WHERE Id = '" + Id + "' LIMIT 1;");
                }
			}
		}

        public void Unmute()
        {
            if (Muted)
            {
                Muted = false;
                using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery("UPDATE users SET is_muted = '0' WHERE Id = '" + Id + "' LIMIT 1;");
                }
            }
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

		public void method_26(bool FromDB, GameClient Session)
		{
			ServerMessage Message = new ServerMessage(266);
			Message.AppendInt32(-1);
			Message.AppendStringWithBreak(Session.GetHabbo().Look);
			Message.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
			Message.AppendStringWithBreak(Session.GetHabbo().Motto);
			Message.AppendInt32(Session.GetHabbo().AchievementScore);
			Message.AppendStringWithBreak("");
			Session.SendMessage(Message);
			if (Session.GetHabbo().InRoom)
			{
				Room currentRoom = Session.GetHabbo().CurrentRoom;
				if (currentRoom != null)
				{
					RoomUser roomUserByHabbo = currentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (roomUserByHabbo != null)
					{
						if (FromDB)
						{
							DataRow dataRow = null;
							using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
							{
								adapter.AddParamWithValue("userid", Session.GetHabbo().Id);
								dataRow = adapter.ReadDataRow("SELECT * FROM users WHERE Id = @userid LIMIT 1");
							}
							Session.GetHabbo().Motto = PhoenixEnvironment.FilterInjectionChars((string)dataRow["motto"]);
							Session.GetHabbo().Look = PhoenixEnvironment.FilterInjectionChars((string)dataRow["look"]);
						}
						ServerMessage Message2 = new ServerMessage(266);
						Message2.AppendInt32(roomUserByHabbo.VirtualId);
						Message2.AppendStringWithBreak(Session.GetHabbo().Look);
						Message2.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
						Message2.AppendStringWithBreak(Session.GetHabbo().Motto);
						Message2.AppendInt32(Session.GetHabbo().AchievementScore);
						Message2.AppendStringWithBreak("");
						currentRoom.SendMessage(Message2, null);
					}
				}
			}
		}

		public void UpdateVIP()
		{
			DataRow dataRow;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				dataRow = adapter.ReadDataRow("SELECT vip FROM users WHERE Id = '" + this.Id + "' LIMIT 1;");
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
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(CurrentRoomId);
			if (room != null)
			{
				RoomUser class2 = room.GetRoomUserByHabbo(Id);
				ServerMessage Message = new ServerMessage(25);
				Message.AppendInt32(class2.VirtualId);
				Message.AppendStringWithBreak(string_7);
				Message.AppendBoolean(false);
				this.GetClient().SendMessage(Message);
			}
		}
	}
}
