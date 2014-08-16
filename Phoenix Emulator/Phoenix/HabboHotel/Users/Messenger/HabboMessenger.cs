using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Phoenix.HabboHotel.Misc;
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
        private Hashtable friends = new Hashtable();
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
					this.friends.Add((uint)dataRow["Id"], new MessengerBuddy((uint)dataRow["Id"], dataRow["username"] as string, dataRow["look"] as string, dataRow["motto"] as string, dataRow["last_online"] as string));
				}
				try
				{
					if (this.GetClient().GetHabbo().HasRole("receive_sa"))
					{
						this.friends.Add(0, new MessengerBuddy(0, "Staff Chat", this.GetClient().GetHabbo().Look, "Staff Chat Room", "0"));
					}
				}
				catch
				{
				}
			}
		}

		internal void method_1(HabboData UserData)
		{
			//this.mRequests = new Hashtable();
			DataTable dataTable_ = UserData.GetUserRequests;
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
			this.friends.Clear();
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
			Hashtable hashtable = this.friends.Clone() as Hashtable;
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
							class2.GetHabbo().GetMessenger().UpdateFriend();
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
			Hashtable hashtable = this.friends.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				if (@class.Id == uint_1)
				{
					@class.UpdateNeeded = true;
					return true;
				}
			}
			return false;
		}

		internal void UpdateFriend()
		{
			GetClient().SendMessage(SerializeUpdates());
		}

		internal bool method_8(uint uint_1, uint uint_2)
		{
			if (uint_1 == uint_2)
			{
				return true;
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
						return true;
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
						return true;
					}
				}
			}
            return false;
		}

		internal bool method_9(uint uint_1, uint uint_2)
		{
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
					return true;
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
					return true;
				}
			}
			return false;
		}

		internal void HandleAllRequests()
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
                adapter.AddParamWithValue("userid", UserId);
                adapter.ExecuteQuery("DELETE FROM messenger_requests WHERE to_id = @userid");
			}
			this.ClearRequests();
		}

		internal void HandleRequest(uint sender)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("userid", UserId);
				adapter.AddParamWithValue("fromid", sender);
				adapter.ExecuteQuery("DELETE FROM messenger_requests WHERE to_id = @userid AND from_id = @fromid LIMIT 1");
			}
			if (this.GetRequest(sender) != null)
			{
				this.mRequests.Remove(sender);
			}
		}

		internal void method_12(uint ToId)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("toid", ToId);
				adapter.AddParamWithValue("userid", UserId);
				adapter.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@userid,@toid)");
				adapter.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@toid,@userid)");
			}
			this.method_14(ToId);
			GameClient ToUser = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(ToId);
			if (ToUser != null && ToUser.GetHabbo().GetMessenger() != null)
			{
				ToUser.GetHabbo().GetMessenger().method_14(this.UserId);
			}
		}

		internal void method_13(uint ToId)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("toid", ToId);
				adapter.AddParamWithValue("userid", this.UserId);
				adapter.ExecuteQuery("DELETE FROM messenger_friendships WHERE user_one_id = @toid AND user_two_id = @userid LIMIT 1");
				adapter.ExecuteQuery("DELETE FROM messenger_friendships WHERE user_one_id = @userid AND user_two_id = @toid LIMIT 1");
			}
			this.OnDestroyFriendship(ToId);
			GameClient class2 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(ToId);
			if (class2 != null && class2.GetHabbo().GetMessenger() != null)
			{
				class2.GetHabbo().GetMessenger().OnDestroyFriendship(this.UserId);
			}
		}

		internal void method_14(uint uint_1)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataRow dataRow = adapter.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + uint_1 + "' LIMIT 1");
				MessengerBuddy class2 = new MessengerBuddy(uint_1, dataRow["username"] as string, dataRow["look"] as string, dataRow["motto"] as string, dataRow["last_online"] as string);
				class2.UpdateNeeded = true;
				if (!this.friends.Contains(class2.Id))
				{
					this.friends.Add(class2.Id, class2);
				}
				this.UpdateFriend();
			}
		}

		internal void OnDestroyFriendship(uint Friend)
		{
			friends.Remove(Friend);

			ServerMessage Message = new ServerMessage(13);
			Message.AppendInt32(0);
			Message.AppendInt32(1);
			Message.AppendInt32(-1);
			Message.AppendUInt(Friend);
			GetClient().SendMessage(Message);
		}

		internal void RequestBuddy(string UserQuery)
		{
			DataRow dataRow = null;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("query", UserQuery.ToLower());
				dataRow = adapter.ReadDataRow("SELECT Id,block_newfriends FROM users WHERE username = @query LIMIT 1");
			}
			if (dataRow != null)
			{
				if (PhoenixEnvironment.EnumToBool(dataRow["block_newfriends"].ToString()) && !this.GetClient().GetHabbo().HasRole("ignore_friendsettings"))
				{
					ServerMessage Message = new ServerMessage(260);
					Message.AppendInt32(39);
					Message.AppendInt32(3);
					this.GetClient().SendMessage(Message);
				}
				else
				{
					uint ToId = (uint)dataRow["Id"];
					if (!this.method_8(this.UserId, ToId))
					{
						using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
						{
							adapter.AddParamWithValue("toid", ToId);
							adapter.AddParamWithValue("userid", UserId);
							adapter.ExecuteQuery("INSERT INTO messenger_requests (to_id,from_id) VALUES (@toid,@userid)");
						}
						GameClient ToUser = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(ToId);
						if (ToUser != null && ToUser.GetHabbo() != null)
						{
							uint num2 = 0;
							using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
							{
								adapter.AddParamWithValue("toid", ToId);
								adapter.AddParamWithValue("userid", UserId);
								num2 = adapter.ReadUInt32("SELECT Id FROM messenger_requests WHERE to_id = @toid AND from_id = @userid ORDER BY Id DESC LIMIT 1");
							}
							MessengerRequest Request = new MessengerRequest(num2, ToId, UserId, PhoenixEnvironment.GetGame().GetClientManager().GetNameById(this.UserId));
							ToUser.GetHabbo().GetMessenger().OnNewRequest(num2, ToId, UserId);
							ServerMessage Message = new ServerMessage(132);
							Request.Serialize(Message);
							ToUser.SendMessage(Message);
						}
					}
				}
			}
		}

		internal void OnNewRequest(uint uint_1, uint uint_2, uint uint_3)
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
				this.DeliverInstantMessageError(6, uint_1);
			}
			else
			{
				GameClient @class = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_1);
				if (@class == null || @class.GetHabbo().GetMessenger() == null)
				{
					this.DeliverInstantMessageError(5, uint_1);
				}
				else
				{
					if (this.GetClient().GetHabbo().Muted)
					{
						this.DeliverInstantMessageError(4, uint_1);
					}
					else
					{
						if (@class.GetHabbo().Muted)
						{
							this.DeliverInstantMessageError(3, uint_1);
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
								this.DeliverInstantMessageError(4, uint_1);
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
						@class.GetHabbo().GetMessenger().DeliverInstantMessage(string_0, this.UserId);
					}
				}
			}
		}

		internal void DeliverInstantMessage(string message, uint convoID)
		{
			ServerMessage InstantMessage = new ServerMessage(134);
			InstantMessage.AppendUInt(convoID);
			InstantMessage.AppendString(message);
			GetClient().SendMessage(InstantMessage);
		}

		internal void DeliverInstantMessageError(int ErrorId, UInt32 ConversationId)
		{
/*
3                =     Your friend is muted and cannot reply.
4                =     Your message was not sent because you are muted.
5                =     Your friend is not online.
6                =     Receiver is not your friend anymore.
7                =     Your friend is busy.
8                =     Your friend is wanking
*/

			ServerMessage reply = new ServerMessage(261);
			reply.AppendInt32(ErrorId);
			reply.AppendUInt(ConversationId);
			GetClient().SendMessage(reply);
		}

		internal ServerMessage SerializeFriends()
		{
			ServerMessage reply = new ServerMessage(12);
			reply.AppendInt32(6000);
			reply.AppendInt32(200);
			reply.AppendInt32(6000);
			reply.AppendInt32(900);
			reply.AppendBoolean(false);
			reply.AppendInt32(friends.Count);

			Hashtable hashtable = friends.Clone() as Hashtable;

			foreach (MessengerBuddy friend in hashtable.Values)
			{
				friend.Serialize(reply, false);
			}

			return reply;
		}

		internal ServerMessage SerializeUpdates()
		{
			List<MessengerBuddy> list = new List<MessengerBuddy>();
			int num = 0;
			Hashtable hashtable = this.friends.Clone() as Hashtable;
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
			ServerMessage Message = new ServerMessage(13);
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

		internal ServerMessage SerializeRequests()
		{
			ServerMessage reply = new ServerMessage(314);
			reply.AppendInt32(mRequests.Count);
			reply.AppendInt32(mRequests.Count);
			Hashtable requests = mRequests.Clone() as Hashtable;
			foreach (MessengerRequest request in requests.Values)
			{
				request.Serialize(reply);
			}
			return reply;
		}

		internal ServerMessage PerformSearch(string query)
		{
			DataTable dataTable = null;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("query", query + "%");
				dataTable = adapter.ReadDataTable("SELECT Id FROM users WHERE username LIKE @query LIMIT 50");
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
			return PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(UserId);
		}

		internal Hashtable method_26()
		{
			return friends.Clone() as Hashtable;
		}
	}
}
