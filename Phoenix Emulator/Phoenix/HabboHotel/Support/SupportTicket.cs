using System;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Support
{
	class SupportTicket
	{
		private uint Id;
		public int Score;
		public int Type;
		public TicketStatus Status;
		public uint SenderId;
		public uint ReportedId;
		public uint ModeratorId;
		public string Message;
		public uint RoomId;
		public string RoomName;
		public double Timestamp;
		private string SenderName;
		private string ReportedName;
		private string ModName;

		public int TabId
		{
			get
			{
				if (Status == TicketStatus.OPEN)
				{
					return 1;
				}
				if (Status == TicketStatus.PICKED)
				{
					return 2;
				}
				return 0;
			}
		}

		public uint TicketId
		{
			get
			{
				return Id;
			}
		}

		public SupportTicket(uint mId, int mScore, int mType, uint mSenderId, uint mReportedId, string mMessage, uint mRoomId, string mRoomName, double mTimestamp, uint mModeratorId)
		{
			Id = mId;
			Score = mScore;
			Type = mType;
			Status = TicketStatus.OPEN;
			SenderId = mSenderId;
			ReportedId = mReportedId;
			ModeratorId = mModeratorId;
			Message = mMessage;
			RoomId = mRoomId;
			RoomName = mRoomName;
			Timestamp = mTimestamp;
			SenderName = PhoenixEnvironment.GetGame().GetClientManager().GetNameById(mSenderId);
			ReportedName = PhoenixEnvironment.GetGame().GetClientManager().GetNameById(mReportedId);
			ModName = PhoenixEnvironment.GetGame().GetClientManager().GetNameById(mModeratorId);
		}

		public void Pick(uint ModeratorId, bool UpdateInDb)
		{
			Status = TicketStatus.PICKED;
			//ModeratorId = ModeratorId;
			ModName = PhoenixEnvironment.GetGame().GetClientManager().GetNameById(ModeratorId);
			if (UpdateInDb)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery("UPDATE moderation_tickets SET status = 'picked', moderator_id = '" + ModeratorId + "' WHERE Id = '" + Id + "' LIMIT 1");
				}
			}
		}

        public void Close(TicketStatus NewStatus, bool UpdateInDb)
        {
            String dbType = null;
            Status = NewStatus;
            if (UpdateInDb)
            {
                switch (NewStatus)
                {
                    case TicketStatus.RESOLVED:
                        dbType = "resolved";
                        break;

                    case TicketStatus.ABUSIVE:
                        dbType = "abusive";
                        break;

                    case TicketStatus.INVALID:
                        dbType = "invalid";
                        break;
                }

                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.ExecuteQuery("UPDATE moderation_tickets SET status = '" + dbType + "' WHERE Id = '" + Id + "' LIMIT 1");
                }
            }
        }

		public void Release(bool UpdateInDb)
		{
			Status = TicketStatus.OPEN;
			if (UpdateInDb)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery("UPDATE moderation_tickets SET status = 'open' WHERE Id = '" + Id + "' LIMIT 1");
				}
			}
		}

		public void Delete(bool UpdateInDb)
		{
			Status = TicketStatus.DELETED;
			if (UpdateInDb)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery("UPDATE moderation_tickets SET status = 'deleted' WHERE Id = '" + Id + "' LIMIT 1");
				}
			}
		}

		public ServerMessage Serialize()
		{
			ServerMessage Message = new ServerMessage(530);
			Message.AppendUInt(Id);
			Message.AppendInt32(TabId);
			Message.AppendInt32(11);
			Message.AppendInt32(Type);
			Message.AppendInt32(11);
			Message.AppendInt32(Score);
			Message.AppendUInt(SenderId);
			Message.AppendStringWithBreak(SenderName);
			Message.AppendUInt(ReportedId);
			Message.AppendStringWithBreak(ReportedName);
			Message.AppendUInt(ModeratorId);
			Message.AppendStringWithBreak(ModName);
			Message.AppendStringWithBreak(this.Message);
			Message.AppendUInt(RoomId);
			Message.AppendStringWithBreak(RoomName);
			return Message;
		}
	}
}
