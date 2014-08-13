using System;
namespace Phoenix.HabboHotel.Users.Badges
{
	class Badge
	{
		internal string Code;
        internal int Slot;

        internal Badge(string Code, int Slot)
		{
			this.Code = Code;
			this.Slot = Slot;
		}
	}
}
