using System;
using System.Collections.Generic;
using Phoenix.Messages;
namespace Phoenix.HabboHotel.Rooms
{
	internal sealed class RoomIcon
	{
		public int BackgroundImage;
		public int ForegroundImage;
		public Dictionary<int, int> Items;
		public RoomIcon(int mBackgroundImage, int mForegroundImage, Dictionary<int, int> mItems)
		{
			this.BackgroundImage = mBackgroundImage;
			this.ForegroundImage = mForegroundImage;
			this.Items = mItems;
		}
		public void Serialize(ServerMessage Message)
		{
			Message.AppendInt32(BackgroundImage);
			Message.AppendInt32(ForegroundImage);
			Message.AppendInt32(Items.Count);
			foreach (KeyValuePair<int, int> Item in Items)
			{
				Message.AppendInt32(Item.Key);
				Message.AppendInt32(Item.Value);
			}
		}
	}
}
