using System;
using Phoenix.Messages;
namespace Phoenix.HabboHotel.Users.Messenger
{
	class MessengerRequest
	{
        private UInt32 xRequestId;
        private UInt32 ToUser;
        private UInt32 FromUser;
		private string mUsername;

        internal UInt32 RequestId
		{
			get
			{
				return FromUser;
			}
		}

		internal UInt32 To
		{
			get
			{
				return ToUser;
			}
		}

        internal UInt32 From
		{
			get
			{
				return FromUser;
			}
		}

		internal string senderUsername
		{
			get
			{
				return mUsername;
			}
		}

        public MessengerRequest(UInt32 RequestId, UInt32 ToUser, UInt32 FromUser, string pUsername)
		{
			this.xRequestId = RequestId;
			this.ToUser = ToUser;
			this.FromUser = FromUser;
			this.mUsername = pUsername;
		}

		public void Serialize(ServerMessage Request)
		{
			Request.AppendUInt(FromUser);
			Request.AppendStringWithBreak(mUsername);
			Request.AppendStringWithBreak(FromUser.ToString());
		}
	}
}
