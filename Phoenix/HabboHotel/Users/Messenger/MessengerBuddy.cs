using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Users.Messenger
{
	internal sealed class MessengerBuddy
	{
		private uint UserId;
		internal bool UpdateNeeded;
		private string mUsername;
		private string mLook;
		private string mMotto;
		private string mLastOnline;

		public uint Id
		{
			get
			{
				return this.UserId;
			}
		}
		internal string Username
		{
			get
			{
				return this.mUsername;
			}
		}
		internal string RealName
		{
			get
			{
				GameClient clientByHabbo = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(this.UserId);
				if (clientByHabbo != null)
				{
					return clientByHabbo.GetHabbo().RealName;
				}
				else
				{
					using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
					{
						return dbClient.ReadString("SELECT real_name FROM users WHERE Id = '" + this.UserId + "' LIMIT 1");
					}
				}
			}
		}
		internal string Look
		{
			get
			{
				return this.mLook;
			}
		}
		internal string Motto
		{
			get
			{
				return this.mMotto;
			}
		}
		internal string LastOnline
		{
			get
			{
				return this.mLastOnline;
			}
		}
		internal bool IsOnline
		{
			get
			{
				GameClient clientByHabbo = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(this.UserId);
				return clientByHabbo != null && clientByHabbo.GetHabbo() != null && clientByHabbo.GetHabbo().GetMessenger() != null && !clientByHabbo.GetHabbo().GetMessenger().AppearOffline && !clientByHabbo.GetHabbo().HideOnline;
			}
		}
		internal bool InRoom
		{
			get
			{
				GameClient clientByHabbo = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(this.UserId);
				return clientByHabbo != null && (clientByHabbo.GetHabbo().InRoom && !clientByHabbo.GetHabbo().HideInRom);
			}
		}
		public MessengerBuddy(uint UserId, string pUsername, string pLook, string pMotto, string pLastOnline)
		{
			this.UserId = UserId;
			this.mUsername = pUsername;
			this.mLook = pLook;
			this.mMotto = pMotto;
			this.mLastOnline = PhoenixEnvironment.UnixTimeStampToDateTime(Convert.ToDouble(pLastOnline)).ToString();
			this.UpdateNeeded = false;
		}
		public void Serialize(ServerMessage Message, bool Search)
		{
			if (Search)
			{
				Message.AppendUInt(this.UserId);
				Message.AppendStringWithBreak(this.mUsername);
				Message.AppendStringWithBreak(this.mMotto);
				bool isOnline = this.IsOnline;
				Message.AppendBoolean(isOnline);
				if (isOnline)
				{
					Message.AppendBoolean(this.InRoom);
				}
				else
				{
					Message.AppendBoolean(false);
				}
				Message.AppendStringWithBreak("");
				Message.AppendBoolean(false);
				Message.AppendStringWithBreak(this.mLook);
				Message.AppendStringWithBreak(this.mLastOnline);
				Message.AppendStringWithBreak("");
			}
			else
			{
				Message.AppendUInt(this.UserId);
				Message.AppendStringWithBreak(this.mUsername);
				Message.AppendBoolean(true);
				if (this.UserId == 0u)
				{
					Message.AppendBoolean(true);
					Message.AppendBoolean(false);
				}
				else
				{
					bool isOnline = this.IsOnline;
					Message.AppendBoolean(isOnline);
					if (isOnline)
					{
						Message.AppendBoolean(this.InRoom);
					}
					else
					{
						Message.AppendBoolean(false);
					}
				}
				Message.AppendStringWithBreak(this.mLook);
				Message.AppendBoolean(false);
				Message.AppendStringWithBreak(this.mMotto);
				Message.AppendStringWithBreak(this.mLastOnline);
				Message.AppendStringWithBreak("");
				Message.AppendStringWithBreak("");
			}
		}
	}
}
