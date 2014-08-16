using System;
namespace Phoenix.HabboHotel.Support
{
	class HelpTopic
	{
		private uint Id;
		public string Caption;
		public string Body;
		public uint CategoryId;

		public HelpTopic(uint Id, string Caption, string Body, uint CategoryId)
		{
			this.Id = Id;
			this.Caption = Caption;
			this.Body = Body;
			this.CategoryId = CategoryId;
		}

        public uint TopicId
        {
            get
            {
                return Id;
            }
        }
	}
}
