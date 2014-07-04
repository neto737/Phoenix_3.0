using System;
using System.Collections.Generic;
namespace Phoenix.HabboHotel.RoomBots
{
	internal sealed class BotResponse
	{
		private uint Id;
		public uint BotId;
		public List<string> Keywords;
		public string Response;
		public string ResponseType;
		public int ServeId;
		public BotResponse(uint Id, uint BotId, string Keywords, string ResponseText, string ResponseType, int ServeId)
		{
			this.Id = Id;
			this.BotId = BotId;
			this.Keywords = new List<string>();
			this.Response = ResponseText;
			this.ResponseType = ResponseType;
			this.ServeId = ServeId;
			string[] keywordsArray = Keywords.Split(new char[]	{ ';' });
			for (int i = 0; i < keywordsArray.Length; i++)
			{
				string text = keywordsArray[i];
				this.Keywords.Add(text.ToLower());
			}
		}
		public bool method_0(string string_2)
		{
			using (TimedLock.Lock(this.Keywords))
			{
				foreach (string current in this.Keywords)
				{
					if (string_2.ToLower().Contains(current.ToLower()))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
