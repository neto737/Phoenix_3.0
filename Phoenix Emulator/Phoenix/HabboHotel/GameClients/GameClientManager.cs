using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Support;
using Phoenix.HabboHotel.Achievements;
using Phoenix.Net;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.GameClients
{
	internal sealed class GameClientManager
    {
        #region Fields
        private Task task_0;
		private GameClient[] Session;
		private Hashtable hashtable_0;
		private Hashtable hashtable_1;
		private Timer timer_0;
		private List<TcpConnection> list_0;
        #endregion

        public int connectionCount
		{
			get
			{
				if (this.Session == null)
				{
					return 0;
				}
				else
				{
					int clients = 0;
					for (int i = 0; i < this.Session.Length; i++)
					{
						if (this.Session[i] != null && this.Session[i].GetHabbo() != null && !string.IsNullOrEmpty(this.Session[i].GetHabbo().Username))
						{
							clients++;
						}
					}
					clients++;
					return clients;
				}
			}
		}

		public GameClientManager(int int_0)
		{
			this.hashtable_0 = new Hashtable();
			this.hashtable_1 = new Hashtable();
			this.Session = new GameClient[int_0];
			this.list_0 = new List<TcpConnection>();
			this.timer_0 = new Timer(new TimerCallback(this.method_4), null, 500, 500);
		}

		public void method_0(uint uint_0, string string_0, GameClient class16_1)
		{
			this.hashtable_0[uint_0] = class16_1;
			this.hashtable_1[string_0.ToLower()] = class16_1;
		}
		public void method_1(uint uint_0, string string_0)
		{
			this.hashtable_0[uint_0] = null;
			this.hashtable_1[string_0.ToLower()] = null;
		}
		public GameClient GetClientByHabbo(uint uint_0)
		{
			if (this.Session == null || this.hashtable_0 == null)
			{
				return null;
			}
			else
			{
				if (this.hashtable_0.ContainsKey(uint_0))
				{
					return (GameClient)this.hashtable_0[uint_0];
				}
				else
				{
					return null;
				}
			}
		}
		public GameClient GetClientByHabbo(string string_0)
		{
			GameClient result;
			if (this.Session == null || this.hashtable_1 == null)
			{
				result = null;
			}
			else
			{
				if (this.hashtable_1.ContainsKey(string_0.ToLower()))
				{
					result = (GameClient)this.hashtable_1[string_0.ToLower()];
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
		private void method_4(object object_0)
		{
			try
			{
				List<TcpConnection> list = this.list_0;
				this.list_0 = new List<TcpConnection>();
				if (list != null)
				{
					foreach (TcpConnection current in list)
					{
						if (current != null)
						{
							current.ConnectionDead();
						}
					}
				}
			}
			catch (Exception ex)
			{
                Logging.LogThreadException(ex.ToString(), "Disconnector task");
			}
		}

		internal void AddGameClient(TcpConnection Message1_0)
		{
			if (!this.list_0.Contains(Message1_0))
			{
				this.list_0.Add(Message1_0);
			}
		}
		public void method_6()
		{
		}
		public GameClient method_7(uint uint_0)
		{
			try
			{
				return Session[(int)((UIntPtr)uint_0)];
			}
			catch
			{
				return null;
			}
		}
		internal void CreateAndStartClient(uint uint_0, ref TcpConnection Connection)
		{
			this.Session[(int)((UIntPtr)uint_0)] = new GameClient(uint_0, ref Connection);
			this.Session[(int)((UIntPtr)uint_0)].StartConnection();
		}
		public void StopClient(uint uint_0)
		{
			GameClient Session = this.method_7(uint_0);
			if (Session != null)
			{
				PhoenixEnvironment.GetConnectionManager().DropConnection(uint_0);
				Session.Stop();
				this.Session[(int)((UIntPtr)uint_0)] = null;
			}
		}
		public void StartConnectionChecker()
		{
			if (this.task_0 == null)
			{
				this.task_0 = new Task(new Action(this.method_12));
				this.task_0.Start();
			}
		}
		public void method_11()
		{
			if (this.task_0 != null)
			{
				this.task_0 = null;
			}
		}
		private void method_12()
		{
			int pingInterval = int.Parse(PhoenixEnvironment.GetConfig().data["client.ping.interval"]);
			if (pingInterval <= 100)
			{
				throw new ArgumentException("Invalid configuration value for ping interval! Must be above 100 miliseconds.");
			}
			while (true)
			{
				try
				{
					ServerMessage Message = new ServerMessage(50u);
					List<GameClient> list = new List<GameClient>();
					List<GameClient> list2 = new List<GameClient>();
					for (int i = 0; i < this.Session.Length; i++)
					{
						GameClient Session = this.Session[i];
						if (Session != null)
						{
							if (Session.PongOK)
							{
								Session.PongOK = false;
								list2.Add(Session);
							}
							else
							{
								list.Add(Session);
							}
						}
					}
					foreach (GameClient Session in list)
					{
						try
						{
							Session.Disconnect();
						}
						catch
						{
						}
					}
					byte[] byte_ = Message.GetBytes();
					foreach (GameClient @class in list2)
					{
						try
						{
							@class.GetConnection().SendData(byte_);
						}
						catch
						{
						}
					}
				}
				catch (Exception ex)
				{
                    Logging.LogThreadException(ex.ToString(), "Connection checker task");
				}
				Thread.Sleep(pingInterval);
			}
		}
		internal void method_13()
		{
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null)
				{
					try
					{
						Session.SendMessage(AchievementManager.SerializeAchievementList(Session));
					}
					catch
					{
					}
				}
			}
		}
		internal void QueueBroadcaseMessage(ServerMessage message)
		{
			byte[] byte_ = message.GetBytes();
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null)
				{
					try
					{
						Session.GetConnection().SendData(byte_);
					}
					catch
					{
					}
				}
			}
		}
		internal void BroadcastMessage(ServerMessage Message5_0, ServerMessage Message5_1)
		{
			byte[] byte_ = Message5_0.GetBytes();
			byte[] byte_2 = Message5_1.GetBytes();
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null)
				{
					try
					{
						if (Session.GetHabbo().InRoom)
						{
							Session.GetConnection().SendData(byte_);
						}
						else
						{
							Session.GetConnection().SendData(byte_2);
						}
					}
					catch
					{
					}
				}
			}
		}
		internal void BroadcastMessageToStaff(ServerMessage Message5_0, ServerMessage Message5_1)
		{
			byte[] byte_ = Message5_0.GetBytes();
			byte[] byte_2 = Message5_1.GetBytes();
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null)
				{
					try
					{
						if (Session.GetHabbo().HasRole("receive_sa"))
						{
							if (Session.GetHabbo().InRoom)
							{
								Session.GetConnection().SendData(byte_);
							}
							else
							{
								Session.GetConnection().SendData(byte_2);
							}
						}
					}
					catch
					{
					}
				}
			}
		}
		internal void method_17(GameClient Session, ServerMessage Message5_0)
		{
			byte[] byte_ = Message5_0.GetBytes();
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient session = this.Session[i];
				if (session != null && session != Session)
				{
					try
					{
						if (session.GetHabbo().HasRole("receive_sa"))
						{
							session.GetConnection().SendData(byte_);
						}
					}
					catch
					{
					}
				}
			}
		}
		internal void GiveCredits(int int_0)
		{
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null && Session.GetHabbo() != null)
				{
					try
					{
						Session.GetHabbo().Credits += int_0;
						Session.GetHabbo().UpdateCreditsBalance(true);
						Session.SendNotif("You just received " + int_0 + " credits from staff!");
					}
					catch
					{
					}
				}
			}
		}
		internal void GivePixels(int int_0, bool bool_0)
		{
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null && Session.GetHabbo() != null)
				{
					try
					{
						Session.GetHabbo().ActivityPoints += int_0;
						Session.GetHabbo().UpdateActivityPointsBalance(bool_0);
						Session.SendNotif("You just received " + int_0 + " pixels from staff!");
					}
					catch
					{
					}
				}
			}
		}
		internal void GivePoints(int int_0, bool bool_0)
		{
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null && Session.GetHabbo() != null)
				{
					try
					{
						Session.GetHabbo().shells += int_0;
						Session.GetHabbo().UpdateShellsBalance(false, bool_0);
						Session.SendNotif("You just received " + int_0 + " points from staff!");
					}
					catch
					{
					}
				}
			}
		}
		internal void GiveMassBadge(string string_0)
		{
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null && Session.GetHabbo() != null)
				{
					try
					{
						Session.GetHabbo().GetBadgeComponent().GiveBadge(Session, string_0, true);
						Session.SendNotif("You just received a badge from hotel staff!");
					}
					catch
					{
					}
				}
			}
		}
		public void method_22(ServerMessage Message5_0, string string_0)
		{
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null)
				{
					try
					{
						if (string_0.Length <= 0 || (Session.GetHabbo() != null && Session.GetHabbo().HasRole(string_0)))
						{
							Session.SendMessage(Message5_0);
						}
					}
					catch
					{
					}
				}
			}
		}
		public void CheckEffects()
		{
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null && (Session.GetHabbo() != null && Session.GetHabbo().GetAvatarEffectsInventoryComponent() != null))
				{
					Session.GetHabbo().GetAvatarEffectsInventoryComponent().CheckExpired();
				}
			}
		}
		internal void CloseAll()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				for (int i = 0; i < this.Session.Length; i++)
				{
					GameClient Session = this.Session[i];
					if (Session != null && Session.GetHabbo() != null)
					{
						try
						{
							Session.GetHabbo().GetInventoryComponent().method_19(adapter, true);
							stringBuilder.Append(Session.GetHabbo().String_0);
							flag = true;
						}
						catch
						{
						}
					}
				}
				if (flag)
				{
					try
					{
						adapter.ExecuteQuery(stringBuilder.ToString());
					}
					catch (Exception ex)
					{
						Logging.smethod_8(ex.ToString());
					}
				}
			}
			Console.WriteLine("Done saving users inventory!");
			Console.WriteLine("Closing server connections...");
			try
			{
				for (int i = 0; i < this.Session.Length; i++)
				{
					GameClient Session = this.Session[i];
					if (Session != null && Session.GetConnection() != null)
					{
						try
						{
							Session.GetConnection().Dispose();
						}
						catch
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logging.smethod_8(ex.ToString());
			}
			Array.Clear(this.Session, 0, this.Session.Length);
			Console.WriteLine("Connections closed!");
		}
		public void LogClonesOut(uint uint_0)
		{
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient Session = this.Session[i];
				if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().Id == uint_0)
				{
					Session.Disconnect();
				}
			}
		}
		public string GetNameById(uint uint_0)
		{
			GameClient Session = this.GetClientByHabbo(uint_0);
			string result;
			if (Session != null)
			{
				result = Session.GetHabbo().Username;
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					dataRow = class2.ReadDataRow("SELECT username FROM users WHERE Id = '" + uint_0 + "' LIMIT 1");
				}
				if (dataRow == null)
				{
					result = "Unknown User";
				}
				else
				{
					result = (string)dataRow[0];
				}
			}
			return result;
		}
		public uint method_27(string string_0)
		{
			GameClient Session = this.GetClientByHabbo(string_0);
			uint result;
			if (Session != null)
			{
				result = Session.GetHabbo().Id;
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					dataRow = adapter.ReadDataRow("SELECT Id FROM users WHERE username = '" + string_0 + "' LIMIT 1");
				}
				if (dataRow == null)
				{
					result = 0u;
				}
				else
				{
					result = (uint)dataRow[0];
				}
			}
			return result;
		}
		public void CheckForAllBanConflicts()
		{
			Dictionary<GameClient, ModerationBanException> dictionary = new Dictionary<GameClient, ModerationBanException>();
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient @class = this.Session[i];
				if (@class != null)
				{
					try
					{
						PhoenixEnvironment.GetGame().GetBanManager().CheckForBanConflicts(@class);
					}
					catch (ModerationBanException value)
					{
						dictionary.Add(@class, value);
					}
				}
			}
			foreach (KeyValuePair<GameClient, ModerationBanException> current in dictionary)
			{
				current.Key.SendBanMessage(current.Value.Message);
				current.Key.Disconnect();
			}
		}
		public void method_29()
		{
			try
			{
				if (this.Session != null)
				{
					for (int i = 0; i < this.Session.Length; i++)
					{
						GameClient @class = this.Session[i];
						if (@class != null && (@class.GetHabbo() != null && PhoenixEnvironment.GetGame().GetPixelManager().NeedsUpdate(@class)))
						{
							PhoenixEnvironment.GetGame().GetPixelManager().GivePixels(@class);
						}
					}
				}
			}
			catch (Exception ex)
			{
                Logging.LogThreadException(ex.ToString(), "GCMExt.CheckPixelUpdates task");
			}
		}
		internal List<ServerMessage> method_30()
		{
			List<ServerMessage> list = new List<ServerMessage>();
			int num = 0;
			ServerMessage Message = new ServerMessage();
			Message.Init(161u);
			Message.AppendStringWithBreak("Users online:\r");
			for (int i = 0; i < this.Session.Length; i++)
			{
				GameClient @class = this.Session[i];
				if (@class != null && @class.GetHabbo() != null)
				{
					if (num > 20)
					{
						list.Add(Message);
						num = 0;
						Message = new ServerMessage();
						Message.Init(161u);
					}
					num++;
					Message.AppendStringWithBreak(string.Concat(new object[]
					{
						@class.GetHabbo().Username,
						" {",
						@class.GetHabbo().Rank,
						"}\r"
					}));
				}
			}
			list.Add(Message);
			return list;
		}
		internal void RecordCmdLogs(GameClient Session, string Command, string ExtraData)
		{
            if (GlobalClass.RecordCmdlogs)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.AddParamWithValue("extra_data", ExtraData);
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO cmdlogs (user_id,user_name,command,extra_data,timestamp) VALUES ('",
						Session.GetHabbo().Id,
						"','",
						Session.GetHabbo().Username,
						"','",
						Command,
						"', @extra_data, UNIX_TIMESTAMP())"
					}));
				}
			}
		}
	}
}
