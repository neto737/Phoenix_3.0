using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Pets;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Users.Inventory
{
	internal sealed class InventoryComponent
	{
        private Hashtable discs;
		public List<UserItem> InventoryItems;
		private Hashtable InventoryPets;
		private Hashtable hashtable_1;
		public List<uint> list_1;
		private GameClient Session;
		public uint UserId;

		public int ItemCount
		{
			get
			{
				return InventoryItems.Count;
			}
		}

		public int PetCount
		{
			get
			{
				return InventoryPets.Count;
			}
		}

        public InventoryComponent(uint mUserId, GameClient Session, HabboData Data)
        {
            this.Session = Session;
            this.UserId = mUserId;
            this.InventoryItems = new List<UserItem>();
            this.InventoryPets = new Hashtable();
            this.hashtable_1 = new Hashtable();
            this.discs = new Hashtable();
            this.list_1 = new List<uint>();
            this.InventoryItems.Clear();
            DataTable Table = Data.GetUserInventory;
            foreach (DataRow Row in Table.Rows)
            {
                this.InventoryItems.Add(new UserItem((uint)Row["Id"], (uint)Row["base_item"], (string)Row["extra_data"]));
            }
            this.InventoryPets.Clear();
            DataTable dataTable_2 = Data.GetUserPets;
            foreach (DataRow dataRow in Table.Rows)
            {
                string str;
                uint id = Convert.ToUInt32(dataRow["Id"]);
                uint baseItem = Convert.ToUInt32(dataRow["base_item"]);
                if (!DBNull.Value.Equals(dataRow["extra_data"]))
                {
                    str = (string)dataRow["extra_data"];
                }
                else
                {
                    str = string.Empty;
                }

                InventoryItems.Add(new UserItem(id, baseItem, str));
                UserItem item = new UserItem(id, baseItem, str);

                if (item.GetBaseItem().InteractionType == "musicdisc")
                {
                    this.discs.Add(item.Id, item);
                }
            }
        }

		public void ClearItems()
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
                adapter.AddParamWithValue("userid", UserId);
				adapter.ExecuteQuery("DELETE FROM items WHERE room_id = 0 AND user_id = @userid;");
			}

            this.discs.Clear();
			this.hashtable_1.Clear();
			this.list_1.Clear();
			this.InventoryItems.Clear();
			ServerMessage Message = new ServerMessage(101);
			this.GetClient().SendMessage(Message);
		}

		public void RedeemCredits(GameClient class16_1)
		{
			int num = 0;
			List<UserItem> list = new List<UserItem>();
			foreach (UserItem current in this.InventoryItems)
			{
				if (current != null && (current.GetBaseItem().Name.StartsWith("CF_") || current.GetBaseItem().Name.StartsWith("CFC_")))
				{
					string[] array = current.GetBaseItem().Name.Split(new char[]
					{
						'_'
					});
					int num2 = int.Parse(array[1]);
					if (!this.list_1.Contains(current.Id))
					{
						if (num2 > 0)
						{
							num += num2;
						}
						list.Add(current);
					}
				}
			}
			foreach (UserItem current in list)
			{
				this.RemoveItem(current.Id, 0u, false);
			}
			class16_1.GetHabbo().Credits += num;
			class16_1.GetHabbo().UpdateCreditsBalance(true);
			class16_1.SendNotif("All coins in your inventory have been converted back into " + num + " credits!");
		}

		public void ClearPets()
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.ExecuteQuery("DELETE FROM user_pets WHERE user_id = " + UserId + " AND room_id = 0;");
			}
			foreach (Pet Pet in InventoryPets.Values)
			{
				ServerMessage Message = new ServerMessage(604);
				Message.AppendUInt(Pet.PetId);
				this.GetClient().SendMessage(Message);
			}
			this.InventoryPets.Clear();
		}

		public void method_3(bool bool_0)
		{
			if (bool_0)
			{
				this.method_8();
			}
			this.GetClient().SendMessage(this.SerializePetInventory());
		}

		public Pet GetPet(uint Id)
		{
			return InventoryPets[Id] as Pet;
		}

		public bool RemovePet(uint PetId)
		{
			ServerMessage Message = new ServerMessage(604);
			Message.AppendUInt(PetId);
			GetClient().SendMessage(Message);
			InventoryPets.Remove(PetId);
			return true;
		}

		public void MovePetToRoom(uint PetId, uint RoomId)
		{
			RemovePet(PetId);
		}

        public void AddPet(Pet Pet)
        {
            if (Pet != null)
            {
                Pet.PlacedInRoom = false;
                if (!this.InventoryPets.ContainsKey(Pet.PetId))
                {
                    this.InventoryPets.Add(Pet.PetId, Pet);
                }
                ServerMessage AddMessage = new ServerMessage(603);
                Pet.SerializeInventory(AddMessage);
                GetClient().SendMessage(AddMessage);
            }
        }

		public void method_8()
		{
			using (TimedLock.Lock(this.InventoryItems))
			{
				this.InventoryItems.Clear();
				this.hashtable_1.Clear();
				this.list_1.Clear();
				DataTable dataTable;
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					dataTable = @class.ReadDataTable("SELECT Id,base_item,extra_data,user_id FROM items WHERE room_id = 0 AND user_id = " + this.UserId);
				}
				if (dataTable != null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						this.InventoryItems.Add(new UserItem((uint)dataRow["Id"], (uint)dataRow["base_item"], (string)dataRow["extra_data"]));
					}
				}
				using (TimedLock.Lock(this.InventoryPets))
				{
					this.InventoryPets.Clear();
					DataTable dataTable2;
					using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
					{
						@class.AddParamWithValue("userid", this.UserId);
						dataTable2 = @class.ReadDataTable("SELECT Id, user_id, room_id, name, type, race, color, expirience, energy, nutrition, respect, createstamp, x, y, z FROM user_pets WHERE user_id = @userid AND room_id <= 0");
					}
					if (dataTable2 != null)
					{
						foreach (DataRow dataRow in dataTable2.Rows)
						{
							Pet class2 = PhoenixEnvironment.GetGame().GetCatalog().method_12(dataRow);
							this.InventoryPets.Add(class2.PetId, class2);
						}
					}
				}
			}
		}

		public void UpdateItems(bool FromDatabase)
		{
			if (FromDatabase)
			{
				this.method_8();
				this.method_18();
			}
			if (this.GetClient() != null)
			{
				this.GetClient().SendMessage(new ServerMessage(101));
			}
		}

		public UserItem GetItem(uint Id)
		{
			List<UserItem>.Enumerator enumerator = InventoryItems.GetEnumerator();
			while (enumerator.MoveNext())
			{
				UserItem current = enumerator.Current;
				if (current.Id == Id)
				{
					return current;
				}
			}
			return null;
		}

		public void AddItem(uint uint_1, uint uint_2, string string_0, bool bool_0)
		{
			UserItem item = new UserItem(uint_1, uint_2, string_0);
			this.InventoryItems.Add(item);
			if (this.list_1.Contains(uint_1))
			{
				this.list_1.Remove(uint_1);
			}
			if (!this.hashtable_1.ContainsKey(uint_1))
			{
				if (bool_0)
				{
					using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
					{
						@class.AddParamWithValue("extra_data", string_0);
						@class.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items (Id,user_id,base_item,extra_data,wall_pos) VALUES ('",
							uint_1,
							"','",
							this.UserId,
							"','",
							uint_2,
							"',@extra_data, '')"
						}));
						return;
					}
				}
                if (item.GetBaseItem().InteractionType == "musicdisc")
                {
                    if (this.discs.ContainsKey(item.Id))
                    {
                        this.discs.Add(item.Id, item);
                    }
                }
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE items SET room_id = 0, user_id = '",
						this.UserId,
						"' WHERE Id = '",
						uint_1,
						"'"
					}));
				}
			}
		}

		public void RemoveItem(uint Id, uint uint_2, bool PlacedInroom)
		{
			ServerMessage Message = new ServerMessage(99);
			Message.AppendUInt(Id);
			this.GetClient().SendMessage(Message);
			if (this.hashtable_1.ContainsKey(Id))
			{
				this.hashtable_1.Remove(Id);
			}
			if (!this.list_1.Contains(Id))
			{
				this.InventoryItems.Remove(this.GetItem(Id));
				this.list_1.Add(Id);
                this.discs.Remove(Id);
				if (PlacedInroom)
				{
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE items SET user_id = '",
							uint_2,
							"' WHERE Id = '",
							Id,
							"' LIMIT 1"
						}));
						return;
					}
				}
				if (uint_2 == 0 && !PlacedInroom)
				{
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.ExecuteQuery("DELETE FROM items WHERE Id = '" + Id + "' LIMIT 1");
					}
				}
			}
		}
        public ServerMessage SerializeFloorItemInventory()
        {
            ServerMessage Message = new ServerMessage(140);
            Message.AppendStringWithBreak("S");
            Message.AppendInt32(1);
            Message.AppendInt32(1);
            Message.AppendInt32(this.ItemCount);
            List<UserItem>.Enumerator enumerator = InventoryItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Serialize(Message, true);
            }
            return Message;
        }
		

		public ServerMessage SerializeWallItemInventory()
		{
			ServerMessage Message = new ServerMessage(140);
			Message.AppendStringWithBreak("I");
			Message.AppendString("II");
			Message.AppendInt32(0);
			return Message;
		}

		public ServerMessage SerializePetInventory()
		{
			ServerMessage Message = new ServerMessage(600);
			Message.AppendInt32(InventoryPets.Count);
			foreach (Pet Pet in InventoryPets.Values)
			{
				Pet.SerializeInventory(Message);
			}
			return Message;
		}

		private GameClient GetClient()
		{
			return PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(this.UserId);
		}

		public void AddItemArray(List<RoomItem> list_2)
		{
			foreach (RoomItem current in list_2)
			{
				this.AddItem(current.Id, current.BaseItem, current.ExtraData, false);
			}
		}
		internal void method_18()
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				this.method_19(adapter, false);
			}
		}
		internal void method_19(DatabaseClient queries, bool bool_0)
		{
			try
			{
				if (this.list_1.Count > 0 || this.hashtable_1.Count > 0 || this.InventoryPets.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (Pet pet in this.InventoryPets.Values)
					{
						if (pet.DBState == DatabaseUpdateState.NeedsInsert)
						{
							queries.AddParamWithValue("petname" + pet.PetId, pet.Name);
							queries.AddParamWithValue("petcolor" + pet.PetId, pet.Color);
							queries.AddParamWithValue("petrace" + pet.PetId, pet.Race);
							stringBuilder.Append(string.Concat(new object[]
							{
								"INSERT INTO `user_pets` VALUES ('",
								pet.PetId,
								"', '",
								pet.OwnerId,
								"', '",
								pet.RoomId,
								"', @petname",
								pet.PetId,
								", @petrace",
								pet.PetId,
								", @petcolor",
								pet.PetId,
								", '",
								pet.Type,
								"', '",
								pet.Expirience,
								"', '",
								pet.Energy,
								"', '",
								pet.Nutrition,
								"', '",
								pet.Respect,
								"', '",
								pet.CreationStamp,
								"', '",
								pet.X,
								"', '",
								pet.Y,
								"', '",
								pet.Z,
								"');"
							}));
						}
						else
						{
							if (pet.DBState == DatabaseUpdateState.NeedsUpdate)
							{
								stringBuilder.Append(string.Concat(new object[]
								{
									"UPDATE user_pets SET room_id = '",
									pet.RoomId,
									"', expirience = '",
									pet.Expirience,
									"', energy = '",
									pet.Energy,
									"', nutrition = '",
									pet.Nutrition,
									"', respect = '",
									pet.Respect,
									"', x = '",
									pet.X,
									"', y = '",
									pet.Y,
									"', z = '",
									pet.Z,
									"' WHERE Id = '",
									pet.PetId,
									"' LIMIT 1; "
								}));
							}
						}
						pet.DBState = DatabaseUpdateState.Updated;
					}
					if (stringBuilder.Length > 0)
					{
						queries.ExecuteQuery(stringBuilder.ToString());
					}
				}
				if (bool_0)
				{
					Console.WriteLine("Inventory for user: " + this.GetClient().GetHabbo().Username + " saved.");
				}
			}
			catch (Exception ex)
			{
                Logging.LogCacheError("FATAL ERROR DURING DB UPDATE: " + ex.ToString());
			}
		}

        internal Hashtable songDisks
        {
            get
            {
                return this.discs;
            }
        }

        internal void method_20(GameClient Session)
        {
            int num1 = 0;
            List<UserItem> list = new List<UserItem>();
            foreach (UserItem userItem in this.InventoryItems)
            {
                if (userItem != null && userItem.GetBaseItem().Name.StartsWith("PixEx_"))
                {
                    int num2 = int.Parse(userItem.GetBaseItem().Name.Split(new char[1]
                    {
                        '_'
                    })[1]);
                    if (!this.list_1.Contains(userItem.Id))
                    {
                        if (num2 > 0)
                            num1 += num2;
                        list.Add(userItem);
                    }
                }    
            }      
            foreach (UserItem userItem in list)
            this.RemoveItem(userItem.Id, 0U, false);
            Session.GetHabbo().ActivityPoints += num1;
            Session.GetHabbo().UpdateActivityPointsBalance(true);
            Session.SendNotif("All pixels ingots in your inventory were in " + num1 + " Pixel converted!");
        }

        public void method_21(GameClient Session)
        {
            int num1 = 0;
            List<UserItem> list = new List<UserItem>();
            foreach (UserItem userItem in this.InventoryItems)
            {
                if (userItem != null && userItem.GetBaseItem().Name.StartsWith("PntEx_"))
                {
                    int num2 = int.Parse(userItem.GetBaseItem().Name.Split(new char[1]
                    {
                        '_'
                    })[1]);
                    if (!this.list_1.Contains(userItem.Id))
                    {
                        if (num2 > 0)
                            num1 += num2;
                        list.Add(userItem);
                    }
                }
            }
            foreach (UserItem userItem in list)
                this.RemoveItem(userItem.Id, 0U, false);
            Session.GetHabbo().shells += num1;
            Session.GetHabbo().UpdateShellsBalance(false, true);
            Session.SendNotif("All mussel bars in your inventory were in " + num1 + " Mussels converted!");
        }
    }	
}

