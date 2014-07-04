using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.Users.UserDataManagement;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.HabboHotel.Users.Messenger;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Users.Messenger
{
	internal sealed class HabboMessenger
	{
		private uint UserId;
        private Hashtable mBuddies = new Hashtable();
        private Hashtable mRequests = new Hashtable();
		internal bool AppearOffline;

		public HabboMessenger(uint UserId)
		{
			this.UserId = UserId;
		}
		internal void LoadBuddies(HabboData UserData)
		{
			//this.mBuddies = new Hashtable();
			DataTable getFriendList = UserData.GetUserFriends;
			if (getFriendList != null)
			{
				foreach (DataRow dataRow in getFriendList.Rows)
				{
					this.mBuddies.Add((uint)dataRow["Id"], new MessengerBuddy((uint)dataRow["Id"], dataRow["username"] as string, dataRow["look"] as string, dataRow["motto"] as string, dataRow["last_online"] as string));
				}
				try
				{
					if (this.GetClient().GetHabbo().HasRole("receive_sa"))
					{
						this.mBuddies.Add(0, new MessengerBuddy(0, "Staff Chat", this.GetClient().GetHabbo().Look, "Staff Chat Room", "0"));
					}
				}
				catch
				{
				}
			}
		}
		internal void method_1(HabboData class12_0)
		{
			//this.mRequests = new Hashtable();
			DataTable dataTable_ = class12_0.GetUserRequests;
			if (dataTable_ != null)
			{
				foreach (DataRow dataRow in dataTable_.Rows)
				{
					this.mRequests.Add((uint)dataRow["from_id"], new MessengerRequest((uint)dataRow["Id"], this.UserId, (uint)dataRow["from_id"], dataRow["username"] as string));
				}
			}
		}
		internal void ClearBuddies()
		{
			this.mBuddies.Clear();
		}
		public void ClearRequests()
		{
			this.mRequests.Clear();
		}
		internal MessengerRequest GetRequest(uint RequestId)
		{
			return this.mRequests[RequestId] as MessengerRequest;
		}
		internal void method_5(bool bool_1)
		{
			Hashtable hashtable = this.mBuddies.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				try
				{
					GameClient class2 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(@class.Id);
					if (class2 != null && class2.GetHabbo() != null && class2.GetHabbo().GetMessenger() != null)
					{
						class2.GetHabbo().GetMessenger().method_6(this.UserId);
						if (bool_1)
						{
							class2.GetHabbo().GetMessenger().method_7();
						}
					}
				}
				catch
				{
				}
			}
			hashtable.Clear();
			hashtable = null;
		}
		internal bool method_6(uint uint_1)
		{
			Hashtable hashtable = this.mBuddies.Clone() as Hashtable;
			bool result;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				if (@class.Id == uint_1)
				{
					@class.UpdateNeeded = true;
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		internal void method_7()
		{
			this.GetClient().SendMessage(this.SerializeUpdates());
		}
		internal bool method_8(uint uint_1, uint uint_2)
		{
			bool result;
			if (uint_1 == uint_2)
			{
				result = true;
			}
			else
			{
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					if (@class.ReadDataRow(string.Concat(new object[]
					{
						"SELECT Id FROM messenger_requests WHERE to_id = '",
						uint_1,
						"' AND from_id = '",
						uint_2,
						"' LIMIT 1"
					})) != null)
					{
						result = true;
						return result;
					}
					if (@class.ReadDataRow(string.Concat(new object[]
					{
						"SELECT Id FROM messenger_requests WHERE to_id = '",
						uint_2,
						"' AND from_id = '",
						uint_1,
						"' LIMIT 1"
					})) != null)
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}
		internal bool method_9(uint uint_1, uint uint_2)
		{
			bool result;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				if (@class.ReadDataRow(string.Concat(new object[]
				{
					"SELECT user_one_id FROM messenger_friendships WHERE user_one_id = '",
					uint_1,
					"' AND user_two_id = '",
					uint_2,
					"' LIMIT 1"
				})) != null)
				{
					result = true;
					return result;
				}
				if (@class.ReadDataRow(string.Concat(new object[]
				{
					"SELECT user_one_id FROM messenger_friendships WHERE user_one_id = '",
					uint_2,
					"' AND user_two_id = '",
					uint_1,
					"' LIMIT 1"
				})) != null)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		internal void method_10()
		{
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				@class.ExecuteQuery("DELETE FROM messenger_requests WHERE to_id = '" + this.UserId + "'");
			}
			this.ClearRequests();
		}
		internal void method_11(uint uint_1)
		{
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("userid", this.UserId);
				@class.AddParamWithValue("fromid", uint_1);
				@class.ExecuteQuery("DELETE FROM messenger_requests WHERE to_id = @userid AND from_id = @fromid LIMIT 1");
			}
			if (this.GetRequest(uint_1) != null)
			{
				this.mRequests.Remove(uint_1);
			}
		}
		internal void method_12(uint uint_1)
		{
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("toid", uint_1);
				@class.AddParamWithValue("userid", this.UserId);
				@class.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@userid,@toid)");
				@class.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@toid,@userid)");
			}
			this.method_14(uint_1);
			GameClient class2 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_1);
			if (class2 != null && class2.GetHabbo().GetMessenger() != null)
			{
				class2.GetHabbo().GetMessenger().method_14(this.UserId);
			}
		}
		internal void method_13(uint uint_1)
		{
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("toid", uint_1);
				@class.AddParamWithValue("userid", this.UserId);
				@class.ExecuteQuery("DELETE FROM messenger_friendships WHERE user_one_id = @toid AND user_two_id = @userid LIMIT 1");
				@class.ExecuteQuery("DELETE FROM messenger_friendships WHERE user_one_id = @userid AND user_two_id = @toid LIMIT 1");
			}
			this.method_15(uint_1);
			GameClient class2 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_1);
			if (class2 != null && class2.GetHabbo().GetMessenger() != null)
			{
				class2.GetHabbo().GetMessenger().method_15(this.UserId);
			}
		}
		internal void method_14(uint uint_1)
		{
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataRow dataRow = @class.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + uint_1 + "' LIMIT 1");
				MessengerBuddy class2 = new MessengerBuddy(uint_1, dataRow["username"] as string, dataRow["look"] as string, dataRow["motto"] as string, dataRow["last_online"] as string);
				class2.UpdateNeeded = true;
				if (!this.mBuddies.Contains(class2.Id))
				{
					this.mBuddies.Add(class2.Id, class2);
				}
				this.method_7();
			}
		}
		internal void method_15(uint uint_1)
		{
			this.mBuddies.Remove(uint_1);
			ServerMessage Message = new ServerMessage(13u);
			Message.AppendInt32(0);
			Message.AppendInt32(1);
			Message.AppendInt32(-1);
			Message.AppendUInt(uint_1);
			this.GetClient().SendMessage(Message);
		}
		internal void method_16(string string_0)
		{
			DataRow dataRow = null;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("query", string_0.ToLower());
				dataRow = @class.ReadDataRow("SELECT Id,block_newfriends FROM users WHERE username = @query LIMIT 1");
			}
			if (dataRow != null)
			{
				if (PhoenixEnvironment.EnumToBool(dataRow["block_newfriends"].ToString()) && !this.GetClient().GetHabbo().HasRole("ignore_friendsettings"))
				{
					ServerMessage Message = new ServerMessage(260u);
					Message.AppendInt32(39);
					Message.AppendInt32(3);
					this.GetClient().SendMessage(Message);
				}
				else
				{
					uint num = (uint)dataRow["Id"];
					if (!this.method_8(this.UserId, num))
					{
						using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
						{
							@class.AddParamWithValue("toid", num);
							@class.AddParamWithValue("userid", this.UserId);
							@class.ExecuteQuery("INSERT INTO messenger_requests (to_id,from_id) VALUES (@toid,@userid)");
						}
						GameClient class2 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(num);
						if (class2 != null && class2.GetHabbo() != null)
						{
							uint num2 = 0u;
							using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
							{
								@class.AddParamWithValue("toid", num);
								@class.AddParamWithValue("userid", this.UserId);
								num2 = @class.ReadUInt32("SELECT Id FROM messenger_requests WHERE to_id = @toid AND from_id = @userid ORDER BY Id DESC LIMIT 1");
							}
							MessengerRequest class3 = new MessengerRequest(num2, num, this.UserId, PhoenixEnvironment.GetGame().GetClientManager().GetNameById(this.UserId));
							class2.GetHabbo().GetMessenger().method_17(num2, num, this.UserId);
							ServerMessage Message5_ = new ServerMessage(132u);
							class3.Serialize(Message5_);
							class2.SendMessage(Message5_);
						}
					}
				}
			}
		}
		internal void method_17(uint uint_1, uint uint_2, uint uint_3)
		{
			if (!this.mRequests.ContainsKey(uint_3))
			{
				this.mRequests.Add(uint_3, new MessengerRequest(uint_1, uint_2, uint_3, PhoenixEnvironment.GetGame().GetClientManager().GetNameById(uint_3)));
			}
		}
		internal void method_18(uint uint_1, string string_0)
		{
			if (!this.method_9(uint_1, this.UserId))
			{
				this.method_20(6, uint_1);
			}
			else
			{
				GameClient @class = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_1);
				if (@class == null || @class.GetHabbo().GetMessenger() == null)
				{
					this.method_20(5, uint_1);
				}
				else
				{
					if (this.GetClient().GetHabbo().Muted)
					{
						this.method_20(4, uint_1);
					}
					else
					{
						if (@class.GetHabbo().Muted)
						{
							this.method_20(3, uint_1);
						}
						if (this.GetClient().GetHabbo().MaxFloodTime() > 0)
						{
							TimeSpan timeSpan = DateTime.Now - this.GetClient().GetHabbo().FloodTime;
							if (timeSpan.Seconds > 4)
							{
								this.GetClient().GetHabbo().FloodCount = 0;
							}
							if (timeSpan.Seconds < 4 && this.GetClient().GetHabbo().FloodCount > 5)
							{
								this.method_20(4, uint_1);
								return;
							}
							this.GetClient().GetHabbo().FloodTime = DateTime.Now;
							this.GetClient().GetHabbo().FloodCount++;
						}
						string_0 = ChatCommandHandler.ApplyWordFilter(string_0);
						if (GlobalClass.RecordChatlogs && !this.GetClient().GetHabbo().isAaron)
						{
							using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								class2.AddParamWithValue("message", "<PM to " + @class.GetHabbo().Username + ">: " + string_0);
								class2.ExecuteQuery(string.Concat(new object[]
								{
									"INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
									this.GetClient().GetHabbo().Id,
									"','0','",
									DateTime.Now.Hour,
									"','",
									DateTime.Now.Minute,
									"',UNIX_TIMESTAMP(),@message,'",
									this.GetClient().GetHabbo().Username,
									"','",
									DateTime.Now.ToLongDateString(),
									"')"
								}));
							}
						}
						@class.GetHabbo().GetMessenger().method_19(string_0, this.UserId);
					}
				}
			}
		}
		internal void method_19(string string_0, uint uint_1)
		{
			ServerMessage Message = new ServerMessage(134u);
			Message.AppendUInt(uint_1);
			Message.AppendString(string_0);
			this.GetClient().SendMessage(Message);
		}
		internal void method_20(int int_0, uint uint_1)
		{
			ServerMessage Message = new ServerMessage(261u);
			Message.AppendInt32(int_0);
			Message.AppendUInt(uint_1);
			this.GetClient().SendMessage(Message);
		}
		internal ServerMessage method_21()
		{
			ServerMessage Message = new ServerMessage(12u);
			Message.AppendInt32(6000);
			Message.AppendInt32(200);
			Message.AppendInt32(6000);
			Message.AppendInt32(900);
			Message.AppendBoolean(false);
			Message.AppendInt32(this.mBuddies.Count);
			Hashtable hashtable = this.mBuddies.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				@class.Serialize(Message, false);
			}
			return Message;
		}
		internal ServerMessage SerializeUpdates()
		{
			List<MessengerBuddy> list = new List<MessengerBuddy>();
			int num = 0;
			Hashtable hashtable = this.mBuddies.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				if (@class.UpdateNeeded)
				{
					num++;
					list.Add(@class);
					@class.UpdateNeeded = false;
				}
			}
			hashtable.Clear();
			ServerMessage Message = new ServerMessage(13u);
			Message.AppendInt32(0);
			Message.AppendInt32(num);
			Message.AppendInt32(0);
			foreach (MessengerBuddy @class in list)
			{
				@class.Serialize(Message, false);
				Message.AppendBoolean(false);
			}
			return Message;
		}
		internal ServerMessage method_23()
		{
			ServerMessage Message = new ServerMessage(314u);
			Message.AppendInt32(this.mRequests.Count);
			Message.AppendInt32(this.mRequests.Count);
			Hashtable hashtable = this.mRequests.Clone() as Hashtable;
			foreach (MessengerRequest @class in hashtable.Values)
			{
				@class.Serialize(Message);
			}
			return Message;
		}
		internal ServerMessage method_24(string string_0)
		{
			DataTable dataTable = null;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("query", string_0 + "%");
				dataTable = @class.ReadDataTable("SELECT Id FROM users WHERE username LIKE @query LIMIT 50");
			}
			List<DataRow> list = new List<DataRow>();
			List<DataRow> list2 = new List<DataRow>();
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					if (this.method_9(this.UserId, (uint)dataRow["Id"]))
					{
						list.Add(dataRow);
					}
					else
					{
						list2.Add(dataRow);
					}
				}
			}
			ServerMessage Message = new ServerMessage(435u);
			Message.AppendInt32(list.Count);
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				foreach (DataRow dataRow in list)
				{
					uint num = (uint)dataRow["Id"];
					DataRow dataRow2 = @class.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + num + "' LIMIT 1");
					new MessengerBuddy(num, dataRow2["username"] as string, dataRow2["look"] as string, dataRow2["motto"] as string, dataRow2["last_online"] as string).Serialize(Message, true);
				}
				Message.AppendInt32(list2.Count);
				foreach (DataRow dataRow in list2)
				{
					uint num = (uint)dataRow["Id"];
					DataRow dataRow2 = @class.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + num + "' LIMIT 1");
					new MessengerBuddy(num, dataRow2["username"] as string, dataRow2["look"] as string, dataRow2["motto"] as string, dataRow2["last_online"] as string).Serialize(Message, true);
				}
			}
			return Message;
		}
		private GameClient GetClient()
		{
			return PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(this.UserId);
		}
		internal Hashtable method_26()
		{
			return this.mBuddies.Clone() as Hashtable;
		}
	}
}
