using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.RoomBots
{
	internal class BotManager
	{
		private List<RoomBot> Bots;

		public BotManager()
		{
			this.Bots = new List<RoomBot>();
		}

		public void LoadBots(DatabaseClient adapter)
		{
            Logging.Write("Loading Bot data..");
			this.Bots = new List<RoomBot>();
			DataTable dataTable = adapter.ReadDataTable("SELECT * FROM bots;");
			DataTable dataTable2 = adapter.ReadDataTable("SELECT Id, bot_id, keywords, response_text, mode, serve_id FROM bots_responses;");
			DataTable dataTable3 = adapter.ReadDataTable("SELECT text, shout, bot_id FROM bots_speech;");
			List<BotResponse> Response = new List<BotResponse>();
			List<RandomSpeech> Speech = new List<RandomSpeech>();
			foreach (DataRow dataRow in dataTable2.Rows)
			{
				Response.Add(new BotResponse((uint)dataRow["Id"], (uint)dataRow["bot_id"], (string)dataRow["keywords"], (string)dataRow["response_text"], dataRow["mode"].ToString(), (int)dataRow["serve_id"]));
			}
			foreach (DataRow dataRow in dataTable3.Rows)
			{
				Speech.Add(new RandomSpeech((string)dataRow["text"], PhoenixEnvironment.EnumToBool(dataRow["shout"].ToString()), (uint)dataRow["bot_id"]));
			}
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					string text = (string)dataRow["ai_type"];
					string text2 = text;
					if (text2 == null)
					{
						goto IL_201;
					}
					AIType enum2_;
					if (!(text2 == "generic"))
					{
						if (!(text2 == "guide"))
						{
							if (!(text2 == "pet"))
							{
								goto IL_201;
							}
							enum2_ = AIType.Pet;
						}
						else
						{
							enum2_ = AIType.Guide;
						}
					}
					else
					{
						enum2_ = AIType.const_2;
					}
					IL_204:
					this.Bots.Add(new RoomBot((uint)dataRow["Id"], (uint)dataRow["room_id"], enum2_, (string)dataRow["walk_mode"], (string)dataRow["name"], (string)dataRow["motto"], (string)dataRow["look"], (int)dataRow["x"], (int)dataRow["y"], (int)dataRow["z"], (int)dataRow["rotation"], (int)dataRow["min_x"], (int)dataRow["min_y"], (int)dataRow["max_x"], (int)dataRow["max_y"], ref Speech, ref Response, (int)dataRow["effect"]));
					continue;
					IL_201:
					enum2_ = AIType.const_2;
					goto IL_204;
				}
				Logging.WriteLine("completed!");
			}
		}

		public bool RoomHasBots(uint RoomId)
		{
			return GetBotsForRoom(RoomId).Count >= 1;
		}

        public List<RoomBot> GetBotsForRoom(uint RoomId)
        {
            List<RoomBot> List = new List<RoomBot>();
            foreach (RoomBot Bot in Bots)
            {
                if (Bot.RoomId == RoomId)
                {
                    List.Add(Bot);
                }
            }
            return List;
        }

        public RoomBot GetBot(uint BotId)
        {
            foreach (RoomBot Bot in Bots)
            {
                if (Bot.BotId == BotId)
                {
                    return Bot;
                }
            }
            return null;
        }
	}
}
