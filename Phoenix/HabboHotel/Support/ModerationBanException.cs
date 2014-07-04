using System;
namespace Phoenix.HabboHotel.Support
{
	public sealed class ModerationBanException : Exception
	{
		public ModerationBanException(string Reason) : base(Reason)
		{
		}
	}
}
