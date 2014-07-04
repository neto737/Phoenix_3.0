using System;
using Phoenix.Messages;
namespace Phoenix.HabboHotel.Users.Messenger
{
	internal sealed class MessengerRequest
	{
		private uint xRequestId;
		private uint ToUser;
		private uint FromUser;
		private string SenderUsername;
		internal uint RequestId
		{
			get
			{
				return this.FromUser;
			}
		}
		internal uint To
		{
			get
			{
				return this.ToUser;
			}
		}
		internal uint From
		{
			get
			{
				return this.FromUser;
			}
		}
		internal string senderUsername
		{
			get
			{
				return this.SenderUsername;
			}
		}
		public MessengerRequest(uint RequestId, uint ToUser, uint FromUser, string SenderUsername)
		{
			this.xRequestId = RequestId;
			this.ToUser = ToUser;
			this.FromUser = FromUser;
			this.SenderUsername = SenderUsername;
		}
		public void Serialize(ServerMessage Request)
		{
			Request.AppendUInt(this.FromUser);
			Request.AppendStringWithBreak(this.SenderUsername);
			Request.AppendStringWithBreak(this.FromUser.ToString());
		}
	}
}
