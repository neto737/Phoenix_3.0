using System;
using Phoenix.HabboHotel.Navigators;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.HabboHotel.Navigators
{
	internal sealed class PublicItem
	{
		private int BannerId;
		public int Type;
		public string Caption;
		public string Description;
		public PublicImageType ImageType;
		public uint RoomId;
		public int Recommended;
		public bool Category;
		public int Id
		{
			get {   return BannerId;    }
		}
		public RoomData RoomData
		{
			get
			{
				if (RoomId == 0)
				{
					return new RoomData();
				}
				else
				{
					return PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
				}
			}
		}
		public PublicItem(int mId, int mType, string mCaption, string mDescription, PublicImageType mImageType, uint mRoomId, bool mCategory, int mRecommended)
		{
			BannerId = mId;
			Type = mType;
			Caption = mCaption;
			Description = mDescription;
			ImageType = mImageType;
			RoomId = mRoomId;
			Category = mCategory;
			Recommended = mRecommended;
		}
		public void Serialize(ServerMessage Message)
		{
			if (!Category)
			{
				Message.AppendInt32(Id);
				Message.AppendStringWithBreak((Type == 1) ? Caption : RoomData.Name);
				Message.AppendStringWithBreak(RoomData.Description);
				Message.AppendInt32(Type);
				Message.AppendStringWithBreak(Caption);
				Message.AppendStringWithBreak((ImageType == PublicImageType.EXTERNAL) ? Description : "");
				Message.AppendInt32(Recommended);
				Message.AppendInt32(RoomData.UsersNow);
				Message.AppendInt32(3);
				Message.AppendStringWithBreak((ImageType == PublicImageType.INTERNAL) ? Description : "");
				Message.AppendUInt(1337);
				Message.AppendBoolean(true);
				Message.AppendStringWithBreak(RoomData.CCTs);
				Message.AppendInt32(RoomData.UsersMax);
				Message.AppendUInt(RoomId);
			}
			else
			{
				if (Category)
				{
					Message.AppendInt32(Id);
					Message.AppendStringWithBreak(Caption);
					Message.AppendStringWithBreak("");
					Message.AppendBoolean(true);
					Message.AppendStringWithBreak("");
					Message.AppendStringWithBreak((ImageType == PublicImageType.EXTERNAL) ? Description : "");
					Message.AppendBoolean(false);
					Message.AppendBoolean(false);
					Message.AppendInt32(4);
					Message.AppendBoolean(false);
				}
			}
		}
	}
}
