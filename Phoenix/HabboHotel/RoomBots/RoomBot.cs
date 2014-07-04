using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.RoomBots;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.RoomBots
{
	internal sealed class RoomBot
	{
		public uint Id;
		public uint RoomId;
		public AIType AiType;
		public string WalkMode;
		public string Name;
		public string Motto;
		public string Look;
		public int EffectId;
		public bool bool_0;
		public int x;
		public int y;
		public double z;
		public int Rotation;
		public int min_x;
		public int max_x;
		public int min_y;
		public int max_y;
		public List<RandomSpeech> list_0;
		public List<BotResponse> list_1;
		public RoomUser RoomUser_0;
		public bool IsPet
		{
			get
			{
				return this.AiType == AIType.Pet;
			}
		}
		public bool Boolean_1
		{
			get
			{
				return this.AiType == AIType.const_3;
			}
		}
		public RoomBot(uint Id, uint RoomId, AIType AiType, string WalkingMode, string Name, string Motto, string Look, int X, int Y, int Z, int Rotation, int Min_X, int Min_Y, int Max_X, int Max_Y, ref List<RandomSpeech> BotSpeeches, ref List<BotResponse> BotResponses, int EffectId)
		{
			this.Id = Id;
			this.RoomId = RoomId;
			this.AiType = AiType;
			this.WalkMode = WalkingMode;
			this.Name = Name;
			this.Motto = Motto;
			this.Look = Look;
			this.x = X;
			this.y = Y;
			this.z = (double)Z;
			this.Rotation = Rotation;
			this.min_x = Min_X;
			this.min_y = Min_Y;
			this.max_x = Max_X;
			this.max_y = Max_Y;
			this.EffectId = EffectId;
			this.bool_0 = true;
			this.RoomUser_0 = null;
			this.LookRandomSpeech(BotSpeeches);
			this.LoadResponses(BotResponses);
		}
		public void LookRandomSpeech(List<RandomSpeech> list_2)
		{
			this.list_0 = new List<RandomSpeech>();
			foreach (RandomSpeech current in list_2)
			{
				if (current.Id == this.Id)
				{
					this.list_0.Add(current);
				}
			}
		}
		public void LoadResponses(List<BotResponse> list_2)
		{
			this.list_1 = new List<BotResponse>();
			foreach (BotResponse current in list_2)
			{
				if (current.BotId == this.Id)
				{
					this.list_1.Add(current);
				}
			}
		}
		public BotResponse method_2(string string_4)
		{
			using (TimedLock.Lock(this.list_1))
			{
				foreach (BotResponse current in this.list_1)
				{
					if (current.method_0(string_4))
					{
						return current;
					}
				}
			}
			return null;
		}
		public RandomSpeech method_3()
		{
			return this.list_0[PhoenixEnvironment.GetRandomNumber(0, this.list_0.Count - 1)];
		}
		public BotAI method_4(int int_8)
		{
			switch (this.AiType)
			{
			case AIType.Pet:
				return new PetBot(int_8);
			case AIType.const_1:
				return new GuideBot();
			case AIType.const_3:
				return new GuideBotMovement(int_8);
			}
			return new GenericBot(int_8);
		}
	}
}
