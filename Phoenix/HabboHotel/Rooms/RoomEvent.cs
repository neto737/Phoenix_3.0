using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.HabboHotel.Rooms
{
	internal sealed class RoomEvent
	{
		public string Name;
		public string Description;
		public int Category;
		public List<string> Tags;
		public string StartTime;
		public uint RoomId;

		public RoomEvent(uint mRoomId, string mName, string mDescription, int mCategory, List<string> mTags)
		{
			this.RoomId = mRoomId;
			this.Name = mName;
			this.Description = mDescription;
			this.Category = mCategory;
			this.Tags = mTags;
			this.StartTime = DateTime.Now.ToShortTimeString();
		}
		public ServerMessage Serialize(GameClient Session)
		{
			ServerMessage Message = new ServerMessage(370);
			Message.AppendStringWithBreak(string.Concat(Session.GetHabbo().Id));
			Message.AppendStringWithBreak(Session.GetHabbo().Username);
			Message.AppendStringWithBreak(string.Concat(RoomId));
			Message.AppendInt32(Category);
			Message.AppendStringWithBreak(Name);
			Message.AppendStringWithBreak(Description);
			Message.AppendStringWithBreak(StartTime);
			Message.AppendInt32(Tags.Count);

			using (TimedLock.Lock(this.Tags))
			{
				foreach (string Tag in Tags)
				{
					Message.AppendStringWithBreak(Tag);
				}
			}
			return Message;
		}
	}
}
