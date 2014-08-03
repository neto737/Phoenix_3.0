using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.RoomBots;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.RoomBots
{
	internal class RoomBot
	{
		public uint BotId;
		public uint RoomId;

		public AIType AiType;
		public string WalkingMode;

		public string Name;
		public string Motto;
		public string Look;
		public int EffectId;

		public bool bool_0;
		public int X;
		public int Y;
		public double Z;
		public int Rot;

		public int minX;
		public int maxX;
		public int minY;
		public int maxY;

		public List<RandomSpeech> RandomSpeech;
		public List<BotResponse> Responses;
		public RoomUser RoomUser;

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
			this.BotId = Id;
			this.RoomId = RoomId;
			this.AiType = AiType;
			this.WalkingMode = WalkingMode;
			this.Name = Name;
			this.Motto = Motto;
			this.Look = Look;
			this.X = X;
			this.Y = Y;
			this.Z = (double)Z;
			this.Rot = Rotation;
			this.minX = Min_X;
			this.minY = Min_Y;
			this.maxX = Max_X;
			this.maxY = Max_Y;
			this.EffectId = EffectId;
			this.bool_0 = true;
			this.RoomUser = null;
			this.LookRandomSpeech(BotSpeeches);
			this.LoadResponses(BotResponses);
		}

		public void LookRandomSpeech(List<RandomSpeech> RandomSpeech)
		{
			this.RandomSpeech = new List<RandomSpeech>();
			foreach (RandomSpeech Speech in RandomSpeech)
			{
				if (Speech.Id == this.BotId)
				{
					this.RandomSpeech.Add(Speech);
				}
			}
		}

		public void LoadResponses(List<BotResponse> Responses)
		{
			this.Responses = new List<BotResponse>();
			foreach (BotResponse Response in Responses)
			{
				if (Response.BotId == this.BotId)
				{
					this.Responses.Add(Response);
				}
			}
		}

        public BotResponse GetResponse(string Message)
        {
            foreach (BotResponse Response in Responses)
            {
                if (Response.KeywordMatched(Message))
                {
                    return Response;
                }
            }
            return null;
        }

		public RandomSpeech GetRandomSpeech()
		{
			return RandomSpeech[PhoenixEnvironment.GetRandomNumber(0, RandomSpeech.Count - 1)];
		}

		public BotAI GenerateBotAI(int VirtualId)
		{
			switch (this.AiType)
			{
			case AIType.Pet:
				return new PetBot(VirtualId);
			case AIType.Guide:
				return new GuideBot();
			case AIType.const_3:
				return new GuideBotMovement(VirtualId);
			}
			return new GenericBot(VirtualId);
		}
	}
}
