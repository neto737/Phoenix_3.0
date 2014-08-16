using System;
namespace Phoenix.HabboHotel.Support
{
	class HelpCategory
	{
		private uint Id;
		public string Caption;

		public HelpCategory(uint Id, string Caption)
		{
			this.Id = Id;
			this.Caption = Caption;
		}

        public uint CategoryId
        {
            get
            {
                return this.Id;
            }
        }
	}
}
