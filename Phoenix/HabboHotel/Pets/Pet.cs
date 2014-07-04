using System;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
namespace Phoenix.HabboHotel.Pets
{
	internal sealed class Pet
	{
		public uint PetId;
		public uint OwnerId;
		public int VirtualId;

		public uint Type;
		public string Name;
		public string Race;
		public string Color;

		public int Expirience;
		public int Energy;
		public int Nutrition;

		public uint RoomId;
		public int X;
		public int Y;
		public double Z;

		public int Respect;

		public double CreationStamp;
		public bool PlacedInRoom;

		public int[] experienceLevels = new int[]
		{ 100, 200,	400, 600, 1000,	1300, 1800, 2400, 3200, 4300, 7200,	8500, 10100, 13300, 17500, 23000, 51900, 120000, 240000	};

		internal DatabaseUpdateState DBState;
		public Room Room
		{
			get
			{
				if (!IsInRoom)
				{
					return null;
				}
					return PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
			}
		}
		public bool IsInRoom
		{
			get
			{
				return (RoomId > 0);
			}
		}
		public int Level
		{
			get
			{
				for (int level = 0; level < this.experienceLevels.Length; level++)
				{
					if (this.Expirience < this.experienceLevels[level])
					{
						return level + 1;
					}
				}
				return this.MaxLevel;
			}
		}
		public int MaxLevel
		{
			get
			{
				return 20;
			}
		}
		public int ExpirienceGoal
		{
			get
			{
				if (this.Level != this.MaxLevel)
				{
					return this.experienceLevels[this.Level - 1];
				}
				else
				{
					return 240000;
				}
			}
		}
		public int MaxEnergy
		{
			get
			{
				return 100;
			}
		}
		public int MaxNutrition
		{
			get
			{
				return 150;
			}
		}
		public int Age
		{
			get
			{
				return (int)Math.Floor((PhoenixEnvironment.GetUnixTimestamp() - this.CreationStamp) / 86400.0);
			}
		}
		public string Look
		{
			get
			{
                return Type + " " + Race + " " + Color.ToLower();
			}
		}
		public string UNDEFINED
		{
			get
			{
				return OldEncoding.encodeVL64((int)this.Type) + OldEncoding.encodeVL64(Convert.ToInt32(this.Race)) + this.Color;
			}
		}
		public string OwnerName
		{
			get
			{
				return PhoenixEnvironment.GetGame().GetClientManager().GetNameById(OwnerId);
			}
		}
		public Pet(uint PetId, uint OwnerId, uint RoomId, string Name, uint Type, string Race, string Color, int Expirience, int Energy, int Nutrition, int Respect, double CreationStamp, int X, int Y, double Z)
		{
			this.PetId = PetId;
			this.OwnerId = OwnerId;
			this.RoomId = RoomId;
			this.Name = Name;
			this.Type = Type;
			this.Race = Race;
			this.Color = Color;
			this.Expirience = Expirience;
			this.Energy = Energy;
			this.Nutrition = Nutrition;
			this.Respect = Respect;
			this.CreationStamp = CreationStamp;
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.PlacedInRoom = false;
			this.DBState = DatabaseUpdateState.Updated;
		}
		public void OnRespect()
		{
			this.Respect++;
			if (this.DBState != DatabaseUpdateState.NeedsInsert)
			{
				this.DBState = DatabaseUpdateState.NeedsUpdate;
			}
			if (this.Expirience <= 51900)
			{
				this.AddExpirience(10, 0);
			}
			ServerMessage Message = new ServerMessage(606);
			Message.AppendInt32(this.Respect);
			Message.AppendUInt(this.OwnerId);
			Message.AppendUInt(this.PetId);
			Message.AppendStringWithBreak(this.Name);
			Message.AppendBoolean(false);
			Message.AppendInt32(10);
			Message.AppendBoolean(false);
			Message.AppendInt32(-2);
			Message.AppendBoolean(true);
			Message.AppendStringWithBreak("281");
			this.Room.SendMessage(Message, null);
		}
		public void AddExpirience(int Amount, int int_9)
		{
			this.PetEnergy(int_9);
			if (this.Room != null && this.Expirience + Amount >= this.ExpirienceGoal && this.Level != this.MaxLevel)
			{
				this.Energy = this.MaxEnergy;
				ServerMessage message = new ServerMessage(24u);
				message.AppendInt32(this.VirtualId);
				message.AppendStringWithBreak("*leveled up to level " + (this.Level + 1) + "*");
				message.AppendInt32(0);
				this.Room.SendMessage(message, null);
			}
			this.Expirience += Amount;
			if (this.Expirience < 51900)
			{
				if (this.DBState != DatabaseUpdateState.NeedsInsert)
				{
					this.DBState = DatabaseUpdateState.NeedsUpdate;
				}
				if (this.Room != null)
				{
					ServerMessage message2 = new ServerMessage(609u);
					message2.AppendUInt(this.PetId);
					message2.AppendInt32(this.VirtualId);
					message2.AppendInt32(Amount);
					this.Room.SendMessage(message2, null);
				}
			}
		}
		public void PetEnergy(int Add)
		{
			this.Energy -= Add;
			if (this.Energy > 100)
			{
				this.Energy = 100;
			}
			else
			{
				if (this.Energy < 0)
				{
					this.Energy = 0;
				}
			}
			if (this.DBState != DatabaseUpdateState.NeedsInsert)
			{
				this.DBState = DatabaseUpdateState.NeedsUpdate;
			}
		}
		public void SerializeInventory(ServerMessage Message)
		{
			Message.AppendUInt(PetId);
			Message.AppendStringWithBreak(Name);
			Message.AppendStringWithBreak(UNDEFINED);
		}
		public ServerMessage SerializeInfo()
		{
			ServerMessage Nfo = new ServerMessage(601u);
			Nfo.AppendUInt(PetId);
			Nfo.AppendStringWithBreak(Name);
			Nfo.AppendInt32(Level);
			Nfo.AppendInt32(MaxLevel);
			Nfo.AppendInt32(Expirience);
			Nfo.AppendInt32(ExpirienceGoal);
			Nfo.AppendInt32(Energy);
			Nfo.AppendInt32(MaxEnergy);
			Nfo.AppendInt32(Nutrition);
			Nfo.AppendInt32(MaxNutrition);
			Nfo.AppendStringWithBreak(Look.ToLower());
			Nfo.AppendInt32(Respect);
			Nfo.AppendUInt(OwnerId);
			Nfo.AppendInt32(Age);
			Nfo.AppendStringWithBreak(OwnerName);
			return Nfo;
		}
	}
}
