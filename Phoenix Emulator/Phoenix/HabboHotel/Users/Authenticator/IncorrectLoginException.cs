using System;
namespace Phoenix.HabboHotel.Users.Authenticator
{
	public class IncorrectLoginException : Exception
	{
        public IncorrectLoginException(string Reason) : base(Reason) { }
	}
}
