using System;
namespace Phoenix.HabboHotel.Users.Badges
{
	internal sealed class Badge
	{
		public string Code;
		public int Slot;
		public Badge(string Code, int Slot)
		{
			this.Code = Code;
			this.Slot = Slot;
		}
	}
}
