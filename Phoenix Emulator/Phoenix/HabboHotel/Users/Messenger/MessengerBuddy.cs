using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Users.Messenger
{
	class MessengerBuddy
    {
        #region Fields
        private uint UserId;
		internal bool UpdateNeeded;
		private string mUsername;
		private string mLook;
		private string mMotto;
		private string mLastOnline;
        #endregion

        #region Return values
        public uint Id
		{
			get
			{
				return UserId;
			}
		}

		internal string Username
		{
			get
			{
				return mUsername;
			}
		}

		internal string RealName
		{
			get
			{
				GameClient clientByHabbo = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(UserId);
				if (clientByHabbo != null)
				{
					return clientByHabbo.GetHabbo().RealName;
				}
				else
				{
					using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
					{
						return dbClient.ReadString("SELECT real_name FROM users WHERE Id = '" + UserId + "' LIMIT 1");
					}
				}
			}
		}

		internal string Look
		{
			get
			{
				return mLook;
			}
		}

		internal string Motto
		{
			get
			{
				return mMotto;
			}
		}

		internal string LastOnline
		{
			get
			{
				return mLastOnline;
			}
		}

		internal Boolean IsOnline
		{
			get
			{
				GameClient clientByHabbo = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(UserId);
				return clientByHabbo != null && clientByHabbo.GetHabbo() != null && clientByHabbo.GetHabbo().GetMessenger() != null && !clientByHabbo.GetHabbo().GetMessenger().AppearOffline && !clientByHabbo.GetHabbo().HideOnline;
			}
		}

		internal Boolean InRoom
		{
			get
			{
				GameClient clientByHabbo = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(UserId);
				return clientByHabbo != null && (clientByHabbo.GetHabbo().InRoom && !clientByHabbo.GetHabbo().HideInRom);
			}
		}
        #endregion

        #region Constructor
        public MessengerBuddy(uint UserId, string pUsername, string pLook, string pMotto, string pLastOnline)
		{
			this.UserId = UserId;
			this.mUsername = pUsername;
			this.mLook = pLook;
			this.mMotto = pMotto;
			this.mLastOnline = PhoenixEnvironment.UnixTimeStampToDateTime(Convert.ToDouble(pLastOnline)).ToString();
			this.UpdateNeeded = false;
		}
        #endregion

        #region Methods
        public void Serialize(ServerMessage reply, bool Search)
		{
			if (Search)
			{
				reply.AppendUInt(UserId);
				reply.AppendStringWithBreak(mUsername);
				reply.AppendStringWithBreak(mMotto);
				bool isOnline = IsOnline;
				reply.AppendBoolean(isOnline);
				if (isOnline)
				{
					reply.AppendBoolean(InRoom);
				}
				else
				{
					reply.AppendBoolean(false);
				}
				reply.AppendStringWithBreak("");
				reply.AppendBoolean(false);
				reply.AppendStringWithBreak(mLook);
				reply.AppendStringWithBreak(mLastOnline);
				reply.AppendStringWithBreak("");
			}
			else
			{
				reply.AppendUInt(UserId);
				reply.AppendStringWithBreak(mUsername);
				reply.AppendBoolean(true);
				if (UserId == 0)
				{
					reply.AppendBoolean(true);
					reply.AppendBoolean(false);
				}
				else
				{
					bool isOnline = IsOnline;
					reply.AppendBoolean(isOnline);
					if (isOnline)
					{
						reply.AppendBoolean(InRoom);
					}
					else
					{
						reply.AppendBoolean(false);
					}
				}
				reply.AppendStringWithBreak(mLook);
				reply.AppendBoolean(false);
				reply.AppendStringWithBreak(mMotto);
				reply.AppendStringWithBreak(mLastOnline);
				reply.AppendStringWithBreak("");
				reply.AppendStringWithBreak("");
			}
        }
        #endregion
    }
}
