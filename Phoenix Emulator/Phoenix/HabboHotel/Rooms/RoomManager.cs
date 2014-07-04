using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Collections;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Rooms
{
	internal sealed class RoomManager
	{
		private readonly object MAX_PETS_PER_ROOM = new object();
		private Sandbox Rooms;
		private List<uint> list_0;
		private Dictionary<string, RoomModel> Models;
		private Hashtable hashtable_0;
		private List<TeleUserData> list_1;
		private Task task_0;
		private DateTime dateTime_0;
		private List<uint> list_2;
		internal List<RoomData> list_3;
		internal int LoadedRoomsCount
		{
			get
			{
				return this.Rooms.Count;
			}
		}
		internal int Int32_1
		{
			get
			{
				int result = 0;
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					result = int.Parse(@class.ReadString("SELECT COUNT(*) FROM rooms"));
				}
				return result;
			}
		}
		public RoomManager()
		{
			this.Rooms = new Sandbox();
			this.list_0 = new List<uint>();
			this.Models = new Dictionary<string, RoomModel>();
			this.list_1 = new List<TeleUserData>();
			this.list_2 = new List<uint>();
			this.hashtable_0 = new Hashtable();
			this.task_0 = new Task(new Action(this.method_7));
			this.task_0.Start();
		}
		internal void LoadCache()
		{
			Logging.Write("Loading Room Cache..");
			this.list_3 = new List<RoomData>();
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				for (int i = 0; i < 12; i++)
				{
					DataTable dataTable = @class.ReadDataTable("SELECT * FROM rooms WHERE category = '" + i + "' AND roomtype = 'private' ORDER BY users_now DESC LIMIT 40");
					foreach (DataRow dataRow in dataTable.Rows)
					{
						this.list_3.Add(this.method_17((uint)dataRow["Id"], dataRow));
					}
				}
			}
			Logging.WriteLine("completed!");
		}
		private bool method_1(uint uint_0)
		{
			bool result;
			foreach (RoomData current in this.list_3)
			{
				if (current.Id == uint_0)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		internal void method_2(uint uint_0)
		{
			if (this.method_1(uint_0))
			{
				this.LoadCache();
			}
		}
		internal void method_3(string string_0, uint uint_0, uint uint_1, string string_1)
		{
		}
		internal void RemoveAllRooms()
		{
			using (ClonedTable class26_ = this.Rooms.GetThreadSafeTable)
			{
				IEnumerator enumerator;
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					enumerator = class26_.Values.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Room class2 = (Room)enumerator.Current;
							class2.method_65(@class);
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
				if (PhoenixEnvironment.GetConfig().data["emu.messages.roommgr"] == "1")
				{
					Console.WriteLine("[RoomMgr] Done with furniture saving, disposing rooms");
				}
				enumerator = class26_.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Room class2 = (Room)enumerator.Current;
						try
						{
							class2.method_62();
						}
						catch
						{
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
				if (PhoenixEnvironment.GetConfig().data["emu.messages.roommgr"] == "1")
				{
					Console.WriteLine("[RoomMgr] Done disposing rooms!");
				}
			}
		}
		public void method_5(TeleUserData TeleUserData_0)
		{
			this.list_1.Add(TeleUserData_0);
		}
		public List<Room> method_6(int int_0)
		{
			List<Room> list = new List<Room>();
			try
			{
				using (ClonedTable class26_ = this.Rooms.GetThreadSafeTable)
				{
					foreach (Room @class in class26_.Values)
					{
						if (@class.Event != null && (int_0 <= 0 || @class.Event.Category == int_0))
						{
							list.Add(@class);
						}
					}
				}
			}
			catch
			{
			}
			return list;
		}
		private void method_7()
		{
			Thread.Sleep(5000);
			while (true)
			{
				try
				{
					if (this.list_1.Count > 0)
					{
						DateTime now = DateTime.Now;
						try
						{
							try
							{
								this.dateTime_0 = DateTime.Now;
								List<TeleUserData> list = null;
								using (TimedLock.Lock(this.list_1))
								{
									list = this.list_1;
									this.list_1 = new List<TeleUserData>();
								}
								if (list != null)
								{
									foreach (TeleUserData current in list)
									{
										if (current != null)
										{
											current.method_0();
										}
									}
								}
							}
							catch (Exception ex)
							{
                                Logging.LogException("Tele code error: " + ex.ToString());
							}
							continue;
						}
						finally
						{
							DateTime now2 = DateTime.Now;
							double num = 500.0 - (now2 - now).TotalMilliseconds;
							if (num < 0.0)
							{
								num = 0.0;
							}
							if (num > 500.0)
							{
								num = 500.0;
							}
							Thread.Sleep((int)Math.Floor(num));
						}
					}
					Thread.Sleep(500);
				}
				catch (Exception ex)
				{
                    Logging.LogThreadException(ex.ToString(), "Room manager task (Process engine)");
					try
					{
						if (this.list_1 != null)
						{
							this.list_1.Clear();
						}
					}
					catch
					{
					}
					Thread.Sleep(500);
				}
			}
		}
		public void LoadModels(DatabaseClient class6_0)
		{
			Logging.Write("Loading Room Models..");
			this.Models.Clear();
			DataTable dataTable = class6_0.ReadDataTable("SELECT Id,door_x,door_y,door_z,door_dir,heightmap,public_items,club_only FROM room_models");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					string text = (string)dataRow["Id"];
					this.Models.Add(text, new RoomModel(text, (int)dataRow["door_x"], (int)dataRow["door_y"], (double)dataRow["door_z"], (int)dataRow["door_dir"], (string)dataRow["heightmap"], (string)dataRow["public_items"], PhoenixEnvironment.EnumToBool(dataRow["club_only"].ToString())));
				}
				Logging.WriteLine("completed!");
			}
		}
		private RoomModel method_9(uint uint_0)
		{
			DataRow dataRow;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				dataRow = @class.ReadDataRow("SELECT doorx,doory,height,modeldata FROM room_models_customs WHERE roomid = '" + uint_0 + "'");
			}
			return new RoomModel("custom", (int)dataRow["doorx"], (int)dataRow["doory"], (double)dataRow["height"], 2, (string)dataRow["modeldata"], "", false);
		}
		public RoomModel GetModel(string Model, uint uint_0)
		{
			RoomModel result;
			if (Model == "custom")
			{
				result = this.method_9(uint_0);
			}
			else
			{
				if (this.Models.ContainsKey(Model))
				{
					result = this.Models[Model];
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
		public RoomData method_11(uint uint_0)
		{
			RoomData result;
			if (this.GenerateRoomData(uint_0) != null)
			{
				result = this.GenerateRoomData(uint_0);
			}
			else
			{
				RoomData @class = new RoomData();
				@class.FillNull(uint_0);
				result = @class;
			}
			return result;
		}
		public RoomData GenerateRoomData(uint uint_0)
		{
			RoomData @class = new RoomData();
			RoomData result;
			lock (this.hashtable_0)
			{
				if (this.hashtable_0.ContainsKey(uint_0))
				{
					result = (this.hashtable_0[uint_0] as RoomData);
					return result;
				}
				if (this.IsRoomLoaded(uint_0))
				{
					result = this.GetRoom(uint_0).Class27_0;
					return result;
				}
				DataRow dataRow = null;
				using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					dataRow = class2.ReadDataRow("SELECT * FROM rooms WHERE Id = '" + uint_0 + "' LIMIT 1");
				}
				if (dataRow == null)
				{
					result = null;
					return result;
				}
				@class.method_1(dataRow);
			}
			if (!this.hashtable_0.ContainsKey(uint_0))
			{
				this.hashtable_0.Add(uint_0, @class);
			}
			result = @class;
			return result;
		}
		public bool IsRoomLoaded(uint uint_0)
		{
			return this.Rooms.ContainsKey(uint_0);
		}
		public bool IsRoomLoading(uint uint_0)
		{
			return this.list_0.Contains(uint_0);
		}
		internal Room LoadRoom(uint uint_0)
		{
			Room @class = null;
			Room result;
			try
			{
				lock (this.MAX_PETS_PER_ROOM)
				{
					if (this.IsRoomLoaded(uint_0))
					{
						result = this.GetRoom(uint_0);
						return result;
					}
					RoomData class2 = this.GenerateRoomData(uint_0);
					if (class2 == null)
					{
						result = null;
						return result;
					}
					@class = new Room(class2.Id, class2.Name, class2.Description, class2.Type, class2.Owner, class2.Category, class2.State, class2.UsersMax, class2.ModelName, class2.CCTs, class2.Score, class2.Tags, class2.AllowPet, class2.AllowPetsEating, class2.AllowWalkthrough, class2.Hidewall, class2.Icon, class2.Password, class2.Wallpaper, class2.Floor, class2.Landscape, class2, class2.bool_3, class2.Wallthick, class2.Floorthick, class2.Achievement);
					this.Rooms.Add(@class.RoomId, @class);
				}
			}
			catch
			{
				Logging.WriteLine("Error while loading room " + uint_0 + ", we crashed out..");
				result = null;
				return result;
			}
			@class.method_0();
			@class.method_1();
			result = @class;
			return result;
		}
		internal void UnloadRoom(Room class14_0)
		{
			if (class14_0 != null)
			{
				this.Rooms.Remove(class14_0.RoomId);
				this.method_18(class14_0.RoomId);
				class14_0.method_62();
				if (PhoenixEnvironment.GetConfig().data["emu.messages.roommgr"] == "1")
				{
					Logging.WriteLine("[RoomMgr] Unloaded room [ID: " + class14_0.RoomId + "]");
				}
			}
		}
		public RoomData method_17(uint uint_0, DataRow dataRow_0)
		{
			RoomData result;
			if (this.hashtable_0.ContainsKey(uint_0))
			{
				result = (this.hashtable_0[uint_0] as RoomData);
			}
			else
			{
				RoomData @class = new RoomData();
				if (this.IsRoomLoaded(uint_0))
				{
					@class = this.GetRoom(uint_0).Class27_0;
				}
				else
				{
					@class.method_1(dataRow_0);
				}
				this.hashtable_0.Add(uint_0, @class);
				result = @class;
			}
			return result;
		}
		public void method_18(uint uint_0)
		{
			this.hashtable_0.Remove(uint_0);
		}
		public Room GetRoom(uint uint_0)
		{
			Room result;
			if (this.Rooms.ContainsKey(uint_0))
			{
				result = (this.Rooms[uint_0] as Room);
			}
			else
			{
				result = null;
			}
			return result;
		}
		public RoomData method_20(GameClient Session, string string_0, string string_1)
		{
			string_0 = PhoenixEnvironment.FilterInjectionChars(string_0);
			RoomData result;
			if (!this.Models.ContainsKey(string_1))
			{
				Session.SendNotif("Sorry, this room model has not been added yet. Try again later.");
				result = null;
			}
			else
			{
				if (this.Models[string_1].bool_0 && !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") && !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip"))
				{
					Session.SendNotif("You must be an Phoenix Club member to use that room layout.");
					result = null;
				}
				else
				{
					if (string_0.Length < 3)
					{
						Session.SendNotif("Room name is too short for room creation!");
						result = null;
					}
					else
					{
						uint uint_ = 0u;
						using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
						{
							@class.AddParamWithValue("caption", string_0);
							@class.AddParamWithValue("model", string_1);
							@class.AddParamWithValue("username", Session.GetHabbo().Username);
							@class.ExecuteQuery("INSERT INTO rooms (roomtype,caption,owner,model_name) VALUES ('private',@caption,@username,@model)");
							Session.GetHabbo().GetHabboData.GetUsersRooms = @class.ReadDataTable("SELECT * FROM rooms WHERE owner = @username ORDER BY Id ASC");
							uint_ = (uint)@class.ReadDataRow("SELECT Id FROM rooms WHERE owner = @username AND caption = @caption ORDER BY Id DESC")[0];
							Session.GetHabbo().UpdateRooms(@class);
						}
						result = this.GenerateRoomData(uint_);
					}
				}
			}
			return result;
		}
		internal Dictionary<Room, int> method_21()
		{
			Dictionary<Room, int> dictionary = new Dictionary<Room, int>();
			using (ClonedTable class26_ = this.Rooms.GetThreadSafeTable)
			{
				foreach (Room @class in class26_.Values)
				{
					if (@class != null && @class.UserCount > 0 && !@class.IsPublic)
					{
						dictionary.Add(@class, @class.UserCount);
					}
				}
			}
			return dictionary;
		}
		internal Dictionary<Room, int> method_22()
		{
			Dictionary<Room, int> dictionary = new Dictionary<Room, int>();
			using (ClonedTable class26_ = this.Rooms.GetThreadSafeTable)
			{
				foreach (Room @class in class26_.Values)
				{
					if (@class != null)
					{
						dictionary.Add(@class, @class.UserCount);
					}
				}
			}
			return dictionary;
		}
	}
}
