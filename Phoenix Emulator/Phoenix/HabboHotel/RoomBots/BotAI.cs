using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.HabboHotel.RoomBots;
namespace Phoenix.HabboHotel.RoomBots
{
	internal abstract class BotAI
	{
		public int BaseId;
		private int RoomUserId;
		private uint RoomId;

        public BotAI() { }

		public void Init(int mBaseId, int mRoomUserId, uint mRoomId)
		{
			this.BaseId = mBaseId;
			this.RoomUserId = mRoomUserId;
			this.RoomId = mRoomId;
		}

        public Room GetRoom()
        {
            return PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
        }

		public RoomUser GetRoomUser()
		{
			return this.GetRoom().GetRoomUserByVirtualId(RoomUserId);
		}

		public RoomBot GetBotData()
		{
            return GetRoomUser().BotData;
		}

		public abstract void OnSelfEnterRoom();
		public abstract void OnSelfLeaveRoom(bool Kicked);
		public abstract void OnUserEnterRoom(RoomUser User);
		public abstract void OnUserLeaveRoom(GameClient Client);
		public abstract void OnUserSay(RoomUser User, string Message);
		public abstract void OnUserShout(RoomUser User, string Message);
		public abstract void OnTimerTick();
	}
}
