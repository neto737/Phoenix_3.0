using System;
namespace Phoenix.HabboHotel.Support
{
	class ModerationBanException : Exception
	{
        public ModerationBanException(string Reason) : base(Reason) { }
	}
}
