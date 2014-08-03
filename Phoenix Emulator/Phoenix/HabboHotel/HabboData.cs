using System;
using System.Data;
using Phoenix.Util;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Users.UserDataManagement
{
	internal class HabboData
	{
		private bool mUserFound;
		private DataRow mUserInformation;
		private DataTable mAchievementData;
		private DataTable mUserFavouriteRooms;
		private DataTable mUserIgnores;
		private DataTable mUsertags;
		private DataTable mSubscriptionData;
		private DataTable mUserBadges;
		private DataTable mUserInventory;
		private DataTable mUserEffects;
		private DataTable mUserFriends;
		private DataTable mUserRequests;
		private DataTable mUsersRooms;
		private DataTable mUserPets;
        private DataTable mFriendStream; //FriendStream fix

        public HabboData(string Username, bool LoadFull)
        {
            using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
            {
                adapter.AddParamWithValue("username", Username);
                this.mUserInformation = adapter.ReadDataRow("SELECT * FROM users WHERE username = @username LIMIT 1;");
                if (this.mUserInformation != null)
                {
                    this.mUserFound = true;
                    uint num = (uint)this.mUserInformation["Id"];
                    if (LoadFull)
                    {
                        this.mAchievementData = adapter.ReadDataTable("SELECT achievement_id,achievement_level FROM user_achievements WHERE user_id = '" + num + "'");
                        this.mUserFavouriteRooms = adapter.ReadDataTable("SELECT room_id FROM user_favorites WHERE user_id = '" + num + "'");
                        this.mUserIgnores = adapter.ReadDataTable("SELECT ignore_id FROM user_ignores WHERE user_id = '" + num + "'");
                        this.mUsertags = adapter.ReadDataTable("SELECT tag FROM user_tags WHERE user_id = '" + num + "'");
                        this.mSubscriptionData = adapter.ReadDataTable("SELECT subscription_id, timestamp_activated, timestamp_expire FROM user_subscriptions WHERE user_id = '" + num + "'");
                        this.mUserBadges = adapter.ReadDataTable("SELECT user_badges.badge_id,user_badges.badge_slot FROM user_badges WHERE user_id = " + num + " ORDER BY badge_slot ASC");
                        this.mUserInventory = adapter.ReadDataTable("SELECT Id,base_item,extra_data FROM items WHERE room_id = 0 AND user_id = " + num);
                        this.mUserEffects = adapter.ReadDataTable("SELECT user_effects.effect_id,user_effects.total_duration,user_effects.is_activated,user_effects.activated_stamp FROM user_effects WHERE user_id =  " + num);
                        this.mUserFriends = adapter.ReadDataTable("SELECT users.Id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.Id = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = '" + num + "'");
                        this.mUserRequests = adapter.ReadDataTable("SELECT messenger_requests.Id,messenger_requests.from_id,users.username FROM users JOIN messenger_requests ON users.Id = messenger_requests.from_id WHERE messenger_requests.to_id = '" + num + "'");
                        adapter.AddParamWithValue("name", (string)this.mUserInformation["username"]);
                        this.mUsersRooms = adapter.ReadDataTable("SELECT * FROM rooms WHERE owner = @name ORDER BY Id ASC LIMIT " + GlobalClass.MaxRoomsPerUser);
                        this.mFriendStream = adapter.ReadDataTable("SELECT friend_stream.id, friend_stream.type, friend_stream.userid, friend_stream.gender, friend_stream.look, friend_stream.time, friend_stream.data, friend_stream.data_extra FROM friend_stream JOIN messenger_friendships ON friend_stream.userid = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = '" + num + "' ORDER BY friend_stream.time DESC LIMIT 15"); //FriendStream fix
                    }
                }
                else
                {
                    this.mUserFound = false;
                }
            }
        }

		public HabboData(string pSSOTicket, string pIPAddress, bool LoadFull)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("auth_ticket", pSSOTicket);
				string str = "";
				if (GlobalClass.SecureSessions)
				{
					str = "AND ip_last = '" + pIPAddress + "' ";
				}
				try
				{
					if (int.Parse(PhoenixEnvironment.GetConfig().data["debug"]) == 1)
					{
						str = "";
					}
				}
				catch
				{
				}
				this.mUserInformation = adapter.ReadDataRow("SELECT * FROM users WHERE auth_ticket = @auth_ticket " + str + " LIMIT 1;");
				if (this.mUserInformation != null)
				{
					this.mUserFound = true;
					uint num = (uint)this.mUserInformation["Id"];
					if (LoadFull)
					{
						this.mAchievementData = adapter.ReadDataTable("SELECT achievement_id,achievement_level FROM user_achievements WHERE user_id = '" + num + "'");
						this.mUserFavouriteRooms = adapter.ReadDataTable("SELECT room_id FROM user_favorites WHERE user_id = '" + num + "'");
						this.mUserIgnores = adapter.ReadDataTable("SELECT ignore_id FROM user_ignores WHERE user_id = '" + num + "'");
						this.mUsertags = adapter.ReadDataTable("SELECT tag FROM user_tags WHERE user_id = '" + num + "'");
						this.mSubscriptionData = adapter.ReadDataTable("SELECT subscription_id, timestamp_activated, timestamp_expire FROM user_subscriptions WHERE user_id = '" + num + "'");
                        this.mUserBadges = adapter.ReadDataTable("SELECT user_badges.badge_id,user_badges.badge_slot FROM user_badges WHERE user_id = " + num + " ORDER BY badge_slot ASC");
						this.mUserInventory = adapter.ReadDataTable("SELECT Id,base_item,extra_data FROM items WHERE room_id = 0 AND user_id = " + num);
						this.mUserEffects = adapter.ReadDataTable("SELECT user_effects.effect_id,user_effects.total_duration,user_effects.is_activated,user_effects.activated_stamp FROM user_effects WHERE user_id =  " + num);
						this.mUserFriends = adapter.ReadDataTable("SELECT users.Id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.Id = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = '" + num + "'");
						this.mUserRequests = adapter.ReadDataTable("SELECT messenger_requests.Id,messenger_requests.from_id,users.username FROM users JOIN messenger_requests ON users.Id = messenger_requests.from_id WHERE messenger_requests.to_id = '" + num + "'");
						adapter.AddParamWithValue("name", this.mUserInformation["username"]);
						this.mUsersRooms = adapter.ReadDataTable("SELECT * FROM rooms WHERE owner = @name ORDER BY Id ASC LIMIT " + GlobalClass.MaxRoomsPerUser);
						this.mUserPets = adapter.ReadDataTable("SELECT Id, user_id, room_id, name, type, race, color, expirience, energy, nutrition, respect, createstamp, x, y, z FROM user_pets WHERE user_id = " + num + " AND room_id = 0");
                        this.mFriendStream = adapter.ReadDataTable("SELECT friend_stream.id, friend_stream.type, friend_stream.userid, friend_stream.gender, friend_stream.look, friend_stream.time, friend_stream.data, friend_stream.data_extra FROM friend_stream JOIN messenger_friendships ON friend_stream.userid = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = '" + num + "' ORDER BY friend_stream.time DESC LIMIT 15"); //FriendStream fix
                        adapter.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE users SET online = '1'" + /*auth_ticket = ''*/ "WHERE Id = '",
							num,
							"' LIMIT 1; UPDATE user_info SET login_timestamp = '",
							PhoenixEnvironment.GetUnixTimestamp(),
							"' WHERE user_id = '",
							num,
							"' LIMIT 1;"
						}));
					}
				}
				else
				{
					this.mUserFound = false;
				}
			}
		}

        public void UpdateFriendStream()
        {
            using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
            {
                uint id = (uint)this.mUserInformation["Id"];
                this.mFriendStream = dbClient.ReadDataTable("SELECT friend_stream.id, friend_stream.type, friend_stream.userid, friend_stream.gender, friend_stream.look, friend_stream.time, friend_stream.data, friend_stream.data_extra FROM friend_stream JOIN messenger_friendships ON friend_stream.userid = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = '" + id + "' ORDER BY friend_stream.id DESC LIMIT 15");
            }
        }

        internal bool UserFound
        {
            get
            {
                return this.mUserFound;
            }
        }

        internal DataRow GetHabboDataRow
        {
            get
            {
                return this.mUserInformation;
            }
        }

        internal DataTable GetAchievementData
        {
            get
            {
                return this.mAchievementData;
            }
        }

        internal DataTable GetUserFavouriteRooms
        {
            get
            {
                return this.mUserFavouriteRooms;
            }
        }

        internal DataTable GetUserIgnores
        {
            get
            {
                return this.mUserIgnores;
            }
        }

        internal DataTable GetUserTags
        {
            get
            {
                return this.mUsertags;
            }
        }

        internal DataTable GetSupscriptionData
        {
            get
            {
                return this.mSubscriptionData;
            }
        }

        internal DataTable GetUserBadges
        {
            get
            {
                return this.mUserBadges;
            }
        }

        internal DataTable GetUserInventory
        {
            get
            {
                return this.mUserInventory;
            }
        }

        internal DataTable GetUserEffects
        {
            get
            {
                return this.mUserEffects;
            }
        }

        internal DataTable GetUserFriends
        {
            get
            {
                return this.mUserFriends;
            }
        }

        internal DataTable GetUserRequests
        {
            get
            {
                return this.mUserRequests;
            }
        }

        internal DataTable GetUsersRooms
        {
            get
            {
                return this.mUsersRooms;
            }
            set
            {
                this.mUsersRooms = value;
            }
        }

        internal DataTable GetUserPets
        {
            get
            {
                return this.mUserPets;
            }
        }

        internal DataTable GetFriendStream //FriendStream fix
        {
            get
            {
                return this.mFriendStream;
            }
        }
	}
}
