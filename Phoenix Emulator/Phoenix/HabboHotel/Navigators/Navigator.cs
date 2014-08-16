using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.HabboHotel.Users.Messenger;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Navigators
{
	internal sealed class Navigator
	{
		private sealed class Class219
		{
			public int int_0;
			public bool method_0(RoomData class27_0)
			{
				return class27_0.Category == this.int_0;
			}
			public bool method_1(Room class14_0)
			{
				return class14_0.Category == this.int_0;
			}
		}
		private List<FlatCat> PrivateCategories;
		private Dictionary<int, PublicItem> PublicItems;
		private Dictionary<int, PublicItem> dictionary_1;

		private static Comparison<KeyValuePair<string, int>> comparison_0;

		private static Func<RoomData, int> func_0;

		private static Func<Room, int> func_1;

		private static Func<Room, int> func_2;
		public Navigator()
		{
			this.PrivateCategories = new List<FlatCat>();
			this.PublicItems = new Dictionary<int, PublicItem>();
			this.dictionary_1 = new Dictionary<int, PublicItem>();
		}
		public void Initialize(DatabaseClient adapter)
		{
			Logging.Write("Loading Navigator..");
			this.PrivateCategories.Clear();
			this.PublicItems.Clear();
			this.dictionary_1.Clear();
			DataTable dataTable = adapter.ReadDataTable("SELECT Id,caption,min_rank,cantrade FROM navigator_flatcats WHERE enabled = '1'");
			DataTable dataTable2 = adapter.ReadDataTable("SELECT Id,bannertype,caption,image,image_type,room_id,category,category_parent_id FROM navigator_publics ORDER BY ordernum ASC;");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.PrivateCategories.Add(new FlatCat((int)dataRow["Id"], (string)dataRow["caption"], (int)dataRow["min_rank"], PhoenixEnvironment.EnumToBool(dataRow["cantrade"].ToString())));
				}
			}
			if (dataTable2 != null)
			{
				foreach (DataRow dataRow in dataTable2.Rows)
				{
					this.PublicItems.Add((int)dataRow["Id"], new PublicItem((int)dataRow["Id"], int.Parse(dataRow["bannertype"].ToString()), (string)dataRow["caption"], (string)dataRow["image"], (dataRow["image_type"].ToString().ToLower() == "internal") ? PublicImageType.INTERNAL : PublicImageType.EXTERNAL, (uint)dataRow["room_id"], PhoenixEnvironment.EnumToBool(dataRow["category"].ToString()), (int)dataRow["category_parent_id"]));
					if (!this.PublicItems[(int)dataRow["Id"]].Category)
					{
						this.dictionary_1.Add((int)dataRow["Id"], this.PublicItems[(int)dataRow["Id"]]);
					}
				}
			}
			Logging.WriteLine("completed!");
		}

		public int GetCountForParent(int ParentId)
		{
			int i = 0;
			foreach (PublicItem current in PublicItems.Values)
			{
				if (current.Recommended == ParentId || ParentId == -1)
				{
					i++;
				}
			}
			return i;
		}

        public FlatCat GetFlatCat(int Id)
        {
            foreach (FlatCat current in PrivateCategories)
            {
                if (current.Id == Id)
                {
                    return current;
                }
            }
            return null;
        }

		public ServerMessage SerializeFlatCategories()
		{
			ServerMessage Message = new ServerMessage(221);
			Message.AppendInt32(this.PrivateCategories.Count);
			foreach (FlatCat FlatCat in this.PrivateCategories)
			{
				if (FlatCat.Id > 0)
				{
					Message.AppendBoolean(true);
				}
				if (FlatCat.Id != 15)
				{
					Message.AppendInt32(FlatCat.Id);
				}
				Message.AppendStringWithBreak(FlatCat.Caption);
			}
			Message.AppendStringWithBreak("");
			return Message;
		}

		public void SerializeItemsFromCata(int Id, ServerMessage Message)
		{
			foreach (PublicItem Item in PublicItems.Values)
			{
				if (Item.Recommended == Id && !Item.Category)
				{
					Item.Serialize(Message);
				}
			}
		}

		public ServerMessage SerializePublicRooms()
		{
			ServerMessage Frontpage = new ServerMessage(450);
			Frontpage.AppendInt32(PublicItems.Count);
			foreach (PublicItem Pub in PublicItems.Values)
			{
				if (Pub.Category)
				{
					Pub.Serialize(Frontpage);
					SerializeItemsFromCata(Pub.Id, Frontpage);
				}
				if (!Pub.Category && (Pub.Recommended == 0 || Pub.Recommended == -1))
				{
					Pub.Serialize(Frontpage);
				}
			}
			return Frontpage;
		}

		public ServerMessage SerializeFavoriteRooms(GameClient Session)
		{
			ServerMessage Rooms = new ServerMessage(451);
			Rooms.AppendInt32(0);
			Rooms.AppendInt32(6);
			Rooms.AppendStringWithBreak("");
			Rooms.AppendInt32(Session.GetHabbo().FavoriteRooms.Count);
            foreach (uint Id in Session.GetHabbo().FavoriteRooms)
            {
                PhoenixEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(Id).Serialize(Rooms, false, false);
            }
			return Rooms;
		}

		public ServerMessage SerializeRecentRooms(GameClient Session)
		{
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable dataTable = @class.ReadDataTable("SELECT * FROM rooms JOIN user_roomvisits ON rooms.Id = user_roomvisits.room_id WHERE user_roomvisits.user_id = '" + Session.GetHabbo().Id + "' ORDER BY entry_timestamp DESC LIMIT 50;");
				List<RoomData> list = new List<RoomData>();
				List<uint> list2 = new List<uint>();
				if (dataTable != null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						RoomData class2 = PhoenixEnvironment.GetGame().GetRoomManager().FetchRoomData((uint)dataRow["Id"], dataRow);
						class2.method_1(dataRow);
						list.Add(class2);
						list2.Add(class2.Id);
					}
				}
				ServerMessage Message = new ServerMessage(451);
				Message.AppendInt32(0);
				Message.AppendInt32(7);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(list.Count);
				foreach (RoomData current in list)
				{
					current.Serialize(Message, false, false);
				}
				return Message;
			}
		}
		public ServerMessage SerializeEventListing(GameClient Session, int Category)
		{
			ServerMessage Message = new ServerMessage(451);
			Message.AppendInt32(Category);
			Message.AppendInt32(12);
			Message.AppendStringWithBreak("");

			List<Room> EventRooms = PhoenixEnvironment.GetGame().GetRoomManager().GetEventRoomsForCategory(Category);
			Message.AppendInt32(EventRooms.Count);
			foreach (Room Room in EventRooms)
			{
				RoomData Data = Room.RoomData;
				Data.Serialize(Message, true, false);
			}
			return Message;
		}
		public ServerMessage SerializePopularRoomTags()
		{
			Dictionary<string, int> Tags = new Dictionary<string, int>();
			//ServerMessage result;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable dataTable = adapter.ReadDataTable("SELECT tags,users_now FROM rooms WHERE roomtype = 'private' AND users_now > 0 ORDER BY users_now DESC LIMIT 50;");
				if (dataTable != null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						List<string> RoomTags = new List<string>();
						string[] array = dataRow["tags"].ToString().Split(new char[]
						{
							','
						});
						for (int i = 0; i < array.Length; i++)
						{
							string text = array[i];
							RoomTags.Add(text);
						}
						foreach (string Tag in RoomTags)
						{
							if (Tags.ContainsKey(Tag))
							{
								Dictionary<string, int> dictionary2;
								string key;
								(dictionary2 = Tags)[key = Tag] = dictionary2[key] + (int)dataRow["users_now"];
							}
							else
							{
								Tags.Add(Tag, (int)dataRow["users_now"]);
							}
						}
					}
				}
				List<KeyValuePair<string, int>> list2 = new List<KeyValuePair<string, int>>(Tags);
				List<KeyValuePair<string, int>> arg_163_0 = list2;
				if (Navigator.comparison_0 == null)
				{
					Navigator.comparison_0 = new Comparison<KeyValuePair<string, int>>(Navigator.smethod_0);
				}
				arg_163_0.Sort(Navigator.comparison_0);
				ServerMessage Message = new ServerMessage(452);
				Message.AppendInt32(list2.Count);
				foreach (KeyValuePair<string, int> current in list2)
				{
					Message.AppendStringWithBreak(current.Key);
					Message.AppendInt32(current.Value);
				}
				return Message;
			}
		}
		public ServerMessage SerializeSearchResults(string SearchQuery)
		{
			DataTable table = null;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				SearchQuery = PhoenixEnvironment.FilterInjectionChars(SearchQuery.ToLower()).Trim();
				if (SearchQuery.Length > 0)
				{
					if (SearchQuery.StartsWith("owner:"))
					{
						SearchQuery = SearchQuery.Replace(" ", "");
						adapter.AddParamWithValue("query", SearchQuery.Substring(6));
						table = adapter.ReadDataTable("SELECT * FROM rooms WHERE owner = @query AND roomtype = 'private' ORDER BY users_now DESC LIMIT " + GlobalClass.MaxRoomsPerUser);
					}
					else
					{
						SearchQuery = SearchQuery.Replace("%", "\\%");
						SearchQuery = SearchQuery.Replace("_", "\\_");
						adapter.AddParamWithValue("query", SearchQuery + "%");
						adapter.AddParamWithValue("tags_query", "%" + SearchQuery + "%");
						table = adapter.ReadDataTable("SELECT * FROM rooms WHERE caption LIKE @query AND roomtype = 'private' OR owner LIKE @query AND roomtype = 'private' ORDER BY users_now DESC LIMIT 40");
					}
				}
			}
			List<RoomData> list = new List<RoomData>();
			if (table != null)
			{
				foreach (DataRow dataRow in table.Rows)
				{
					RoomData item = PhoenixEnvironment.GetGame().GetRoomManager().FetchRoomData((uint)dataRow["Id"], dataRow);
					list.Add(item);
				}
			}
			ServerMessage Message = new ServerMessage(451);
			Message.AppendInt32(1);
			Message.AppendInt32(9);
			Message.AppendStringWithBreak(SearchQuery);
			Message.AppendInt32(list.Count);
			foreach (RoomData current in list)
			{
				current.Serialize(Message, false, false);
			}
			return Message;
		}
		internal byte[] SerializeNavigator(GameClient Session, int mode)
		{
			if (mode != -2)
			{
				return this.GetDynamicNavigatorPacket(Session, mode).GetBytes();
			}
			else
			{
				byte[] array = PhoenixEnvironment.GetGame().GetNavigatorCache().GetPacket(mode);
				if (array != null)
				{
					return array;
				}
				else
				{
					return this.GetDynamicNavigatorPacket(null, mode).GetBytes();
				}
			}
		}
		public ServerMessage GetDynamicNavigatorPacket(GameClient Session, int int_0)
		{
			Func<RoomData, bool> func = null;
			Func<Room, bool> func2 = null;
			Navigator.Class219 @class = new Navigator.Class219();
			@class.int_0 = int_0;
			ServerMessage Message = new ServerMessage(451u);
			if (@class.int_0 >= -1)
			{
				Message.AppendInt32(@class.int_0);
				Message.AppendInt32(1);
			}
			else
			{
				if (@class.int_0 == -2)
				{
					Message.AppendInt32(0);
					Message.AppendInt32(2);
				}
				else
				{
					if (@class.int_0 == -3)
					{
						Message.AppendInt32(0);
						Message.AppendInt32(5);
					}
					else
					{
						if (@class.int_0 == -4)
						{
							Message.AppendInt32(0);
							Message.AppendInt32(3);
						}
						else
						{
							if (@class.int_0 == -5)
							{
								Message.AppendInt32(0);
								Message.AppendInt32(4);
							}
						}
					}
				}
			}
			Message.AppendStringWithBreak("");
			List<RoomData> list = new List<RoomData>();
			switch (@class.int_0)
			{
			case -5:
			case -4:
				break;
			case -3:
				goto IL_3A2;
			case -2:
				goto IL_3E5;
			case -1:
				goto IL_47E;
			default:
			{
				Dictionary<Room, int> dictionary = PhoenixEnvironment.GetGame().GetRoomManager().method_21();
				IEnumerable<RoomData> arg_11F_0 = PhoenixEnvironment.GetGame().GetRoomManager().list_3;
				if (func == null)
				{
					func = new Func<RoomData, bool>(@class.method_0);
				}
				IEnumerable<RoomData> enumerable = arg_11F_0.Where(func);
				IEnumerable<Room> arg_13E_0 = dictionary.Keys;
				if (func2 == null)
				{
					func2 = new Func<Room, bool>(@class.method_1);
				}
				IEnumerable<Room> arg_160_0 = arg_13E_0.Where(func2);
				if (Navigator.func_2 == null)
				{
					Navigator.func_2 = new Func<Room, int>(Navigator.smethod_3);
				}
				IOrderedEnumerable<Room> orderedEnumerable = arg_160_0.OrderByDescending(Navigator.func_2);
				new List<RoomData>();
				int num = 0;
				foreach (Room current in orderedEnumerable)
				{
					if (num > 40)
					{
						break;
					}
					list.Add(current.RoomData);
					num++;
				}
				using (IEnumerator<RoomData> enumerator2 = enumerable.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						RoomData current2 = enumerator2.Current;
						if (num > 40)
						{
							break;
						}
						if (list.Contains(current2))
						{
							list.Remove(current2);
						}
						list.Add(current2);
						num++;
					}
					goto IL_508;
				}
			}
			}
			List<string> list2 = new List<string>();
			Dictionary<int, Room> dictionary2 = new Dictionary<int, Room>();
			Hashtable hashtable = Session.GetHabbo().GetMessenger().method_26().Clone() as Hashtable;
			Dictionary<RoomData, int> dictionary3 = new Dictionary<RoomData, int>();
			foreach (MessengerBuddy class2 in hashtable.Values)
			{
				if (class2.IsOnline && class2.InRoom)
				{
					GameClient class3 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(class2.Id);
					if (class3 != null && class3.GetHabbo() != null && class3.GetHabbo().CurrentRoom != null)
					{
						RoomData class27_ = class3.GetHabbo().CurrentRoom.RoomData;
						if (!dictionary3.ContainsKey(class27_))
						{
							dictionary3.Add(class27_, class27_.UsersNow);
						}
					}
				}
			}
			IEnumerable<RoomData> arg_344_0 = dictionary3.Keys;
			if (Navigator.func_0 == null)
			{
				Navigator.func_0 = new Func<RoomData, int>(Navigator.smethod_1);
			}
			IOrderedEnumerable<RoomData> orderedEnumerable2 = arg_344_0.OrderByDescending(Navigator.func_0);
			list2.Clear();
			dictionary2.Clear();
			hashtable.Clear();
			dictionary3.Clear();
			using (IEnumerator<RoomData> enumerator2 = orderedEnumerable2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RoomData current3 = enumerator2.Current;
					list.Add(current3);
				}
				goto IL_508;
			}
			IL_3A2:
			using (List<RoomData>.Enumerator enumerator4 = Session.GetHabbo().list_6.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					RoomData current3 = enumerator4.Current;
					list.Add(current3);
				}
				goto IL_508;
			}
			IL_3E5:
			DataTable dataTable;
			using (DatabaseClient class4 = PhoenixEnvironment.GetDatabase().GetClient())
			{
				dataTable = class4.ReadDataTable("SELECT * FROM rooms WHERE score > 0 AND roomtype = 'private' ORDER BY score DESC LIMIT 40");
			}
			IEnumerator enumerator3 = dataTable.Rows.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					DataRow dataRow = (DataRow)enumerator3.Current;
					list.Add(PhoenixEnvironment.GetGame().GetRoomManager().FetchRoomData((uint)dataRow["Id"], dataRow));
				}
				goto IL_508;
			}
			finally
			{
				IDisposable disposable = enumerator3 as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			IL_47E:
			Dictionary<Room, int> dictionary4 = PhoenixEnvironment.GetGame().GetRoomManager().method_21();
			IEnumerable<Room> arg_4B3_0 = dictionary4.Keys;
			if (Navigator.func_1 == null)
			{
				Navigator.func_1 = new Func<Room, int>(Navigator.smethod_2);
			}
			IOrderedEnumerable<Room> orderedEnumerable3 = arg_4B3_0.OrderByDescending(Navigator.func_1);
			int num2 = 0;
			foreach (Room current4 in orderedEnumerable3)
			{
				if (num2 >= 40)
				{
					break;
				}
				num2++;
				list.Add(current4.RoomData);
			}
			IL_508:
			Message.AppendInt32(list.Count);
			foreach (RoomData current5 in list)
			{
				current5.Serialize(Message, false, false);
			}
			Random random = new Random();
			Message.AppendStringWithBreak("");
			this.dictionary_1.ElementAt(random.Next(0, this.dictionary_1.Count)).Value.Serialize(Message);
			return Message;
		}
		private static int smethod_0(KeyValuePair<string, int> keyValuePair_0, KeyValuePair<string, int> keyValuePair_1)
		{
			return keyValuePair_0.Value.CompareTo(keyValuePair_1.Value);
		}
		private static int smethod_1(RoomData class27_0)
		{
			return class27_0.UsersNow;
		}
		private static int smethod_2(Room class14_0)
		{
			return class14_0.UserCount;
		}
		private static int smethod_3(Room class14_0)
		{
			return class14_0.UserCount;
		}
	}
}
