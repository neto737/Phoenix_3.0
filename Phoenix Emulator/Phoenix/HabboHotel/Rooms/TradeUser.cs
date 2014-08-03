using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.Rooms
{
	internal sealed class TradeUser
	{
		public uint UserId;
		private uint RoomId;
		private bool Accepted;
		public List<UserItem> OfferedItems;

		public bool HasAccepted
		{
			get
			{
				return this.Accepted;
			}
			set
			{
				this.Accepted = value;
			}
		}

		public TradeUser(uint mUserId, uint mRoomId)
		{
			this.UserId = mUserId;
			this.RoomId = mRoomId;
			this.Accepted = false;
			this.OfferedItems = new List<UserItem>();
		}

		public RoomUser GetRoomUser()
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
			if (Room == null)
			{
				return null;
			}
			else
			{
				return Room.GetRoomUserByHabbo(UserId);
			}
		}

		public GameClient GetClient()
		{
			return PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(UserId);
		}
	}
}
