using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.HabboHotel.RoomBots;
namespace Phoenix.HabboHotel.RoomBots
{
	internal abstract class BotAI
	{
		public int BaseId;
		private int int_1;
		private uint uint_0;
		public BotAI()
		{
		}
		public void Init(int int_2, int int_3, uint uint_1)
		{
			this.BaseId = int_2;
			this.int_1 = int_3;
			this.uint_0 = uint_1;
		}
		public Room method_1()
		{
			return PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(this.uint_0);
		}
		public RoomUser GetRoomUser()
		{
			return this.method_1().method_52(this.int_1);
		}
		public RoomBot method_3()
		{
			RoomUser @class = this.GetRoomUser();
			RoomBot result;
			if (@class == null)
			{
				result = null;
			}
			else
			{
				result = this.GetRoomUser().BotData;
			}
			return result;
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
