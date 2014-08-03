using System;
using System.Collections.Generic;
namespace Phoenix.HabboHotel.RoomBots
{
	internal sealed class BotResponse
	{
		private uint Id;
		public uint BotId;
		public List<string> Keywords;
		public string ResponseText;
		public string ResponseType;
		public int ServeId;

		public BotResponse(uint mId, uint mBotId, string mKeywords, string mResponseText, string mResponseType, int mServeId)
		{
			this.Id = mId;
			this.BotId = mBotId;
			this.Keywords = new List<string>();
			this.ResponseText = mResponseText;
			this.ResponseType = mResponseType;
			this.ServeId = mServeId;
			string[] keywordsArray = mKeywords.Split(new char[]	{ ';' });
			for (int i = 0; i < keywordsArray.Length; i++)
			{
				string text = keywordsArray[i];
				this.Keywords.Add(text.ToLower());
			}
		}

        public bool KeywordMatched(string Message)
        {
            foreach (string Keyword in Keywords)
            {
                if (Message.ToLower().Contains(Keyword.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
	}
}
