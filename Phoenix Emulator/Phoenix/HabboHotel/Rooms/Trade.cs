using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phoenix.Core;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Rooms
{
	internal class Trade
	{
		private TradeUser[] Users;
		private int TradeStage;
		private uint RoomId;

		private uint oneId;
		private uint twoId;

		public bool AllUsersAccepted
		{
			get
			{
				for (int i = 0; i < this.Users.Length; i++)
				{
					if (Users[i] != null && !Users[i].HasAccepted)
					{
						return false;
					}
				}
				return true;
			}
		}

		public Trade(uint mUserOneId, uint mUserTwoId, uint mRoomId)
		{
			this.oneId = mUserOneId;
			this.twoId = mUserTwoId;

			this.Users = new TradeUser[2];
			this.Users[0] = new TradeUser(mUserOneId, mRoomId);
			this.Users[1] = new TradeUser(mUserTwoId, mRoomId);
			this.TradeStage = 1;
			this.RoomId = mRoomId;

            foreach (TradeUser User in Users)
            {
                if (!User.GetRoomUser().Statusses.ContainsKey("trd"))
                {
                    User.GetRoomUser().AddStatus("trd", "");
                    User.GetRoomUser().UpdateNeeded = true;
                }
            }

			ServerMessage Message = new ServerMessage(104);
			Message.AppendUInt(mUserOneId);
			Message.AppendBoolean(true);
			Message.AppendUInt(mUserTwoId);
			Message.AppendBoolean(true);
			this.SendMessageToUsers(Message);
		}

		public bool ContainsUser(uint Id)
		{
			for (int i = 0; i < Users.Length; i++)
			{
				if (Users[i] != null && Users[i].UserId == Id)
				{
					return true;
				}
			}
			return false;
		}

		public TradeUser GetTradeUser(uint Id)
		{
			for (int i = 0; i < Users.Length; i++)
			{
				if (Users[i] != null && Users[i].UserId == Id)
				{
					return Users[i];
				}
			}
            return null;
		}

		public void OfferItem(uint UserId, UserItem Item)
		{
            TradeUser User = GetTradeUser(UserId);

            if (User == null || Item == null || !Item.GetBaseItem().AllowTrade || User.HasAccepted || TradeStage != 1)
            {
                return;
            }

            ClearAccepted();

            User.OfferedItems.Add(Item);
            UpdateTradeWindow();
		}

		public void TakeBackItem(uint UserId, UserItem Item)
		{
            TradeUser User = GetTradeUser(UserId);

            if (User == null || Item == null || User.HasAccepted || TradeStage != 1)
            {
                return;
            }

            ClearAccepted();

            User.OfferedItems.Remove(Item);
            UpdateTradeWindow();
		}

        public void Accept(uint UserId)
        {
            TradeUser User = GetTradeUser(UserId);

            if (User == null || TradeStage != 1)
            {
                return;
            }

            User.HasAccepted = true;

            ServerMessage Message = new ServerMessage(109);
            Message.AppendUInt(UserId);
            Message.AppendBoolean(true);
            SendMessageToUsers(Message);

            if (AllUsersAccepted)
            {
                SendMessageToUsers(new ServerMessage(111));
                TradeStage++;
                ClearAccepted();
            }
        }

        public void Unaccept(uint UserId)
        {
            TradeUser User = GetTradeUser(UserId);

            if (User == null || TradeStage != 1 || AllUsersAccepted)
            {
                return;
            }

            User.HasAccepted = false;

            ServerMessage Message = new ServerMessage(109);
            Message.AppendUInt(UserId);
            Message.AppendBoolean(false);
            SendMessageToUsers(Message);
        }

        public void CompleteTrade(uint UserId)
        {
            TradeUser User = GetTradeUser(UserId);

            if (User == null || TradeStage != 2)
            {
                return;
            }

            User.HasAccepted = true;

            ServerMessage Message = new ServerMessage(109);
            Message.AppendUInt(UserId);
            Message.AppendBoolean(true);
            SendMessageToUsers(Message);

            if (this.AllUsersAccepted)
            {
                TradeStage = 999;
                Task Trade = new Task(new Action(TradeTask));
                Trade.Start();
            }
        }

		private void TradeTask()
		{
			try
			{
				this.DeliverItems();
				this.CloseTradeClean();
			}
			catch (Exception ex)
			{
                Logging.LogThreadException(ex.ToString(), "Trade task");
			}
		}

        public void ClearAccepted()
        {
            foreach (TradeUser User in Users)
            {
                User.HasAccepted = false;
            }
        }

        public void UpdateTradeWindow()
        {
            ServerMessage Message = new ServerMessage(108);

            foreach (TradeUser User in Users)
            {
                Message.AppendUInt(User.UserId);
                Message.AppendInt32(User.OfferedItems.Count);

                foreach (UserItem Item in User.OfferedItems)
                {
                    Message.AppendUInt(Item.Id);
                    Message.AppendStringWithBreak(Item.GetBaseItem().Type.ToString().ToLower());
                    Message.AppendUInt(Item.Id);
                    Message.AppendInt32(Item.GetBaseItem().SpriteId);
                    Message.AppendBoolean(true);
                    Message.AppendBoolean(true);
                    Message.AppendStringWithBreak("");
                    Message.AppendBoolean(false);
                    Message.AppendBoolean(false);
                    Message.AppendBoolean(false);

                    if (Item.GetBaseItem().Type == 's')
                    {
                        Message.AppendInt32(-1);
                    }
                }
            }

            SendMessageToUsers(Message);
        }

		public void DeliverItems()
		{
			List<UserItem> ItemsOne = this.GetTradeUser(this.oneId).OfferedItems;
			List<UserItem> ItemsTwo = this.GetTradeUser(this.twoId).OfferedItems;
			foreach (UserItem I in ItemsOne)
			{
				if (this.GetTradeUser(this.oneId).GetClient().GetHabbo().GetInventoryComponent().GetItem(I.Id) == null)
				{
					this.GetTradeUser(this.oneId).GetClient().SendNotif("Trade failed.");
					this.GetTradeUser(this.twoId).GetClient().SendNotif("Trade failed.");
					return;
				}
			}

			foreach (UserItem I in ItemsTwo)
			{
				if (this.GetTradeUser(this.twoId).GetClient().GetHabbo().GetInventoryComponent().GetItem(I.Id) == null)
				{
					this.GetTradeUser(this.oneId).GetClient().SendNotif("Trade failed.");
					this.GetTradeUser(this.twoId).GetClient().SendNotif("Trade failed.");
					return;
				}
			}

			this.GetTradeUser(this.twoId).GetClient().GetHabbo().GetInventoryComponent().method_18();
			this.GetTradeUser(this.oneId).GetClient().GetHabbo().GetInventoryComponent().method_18();
			foreach (UserItem I in ItemsOne)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE items SET room_id = '0', user_id = '",
						this.GetTradeUser(this.twoId).GetClient().GetHabbo().Id,
						"' WHERE Id = '",
						I.Id,
						"' LIMIT 1"
					}));
				}
				this.GetTradeUser(this.oneId).GetClient().GetHabbo().GetInventoryComponent().RemoveItem(I.Id, this.GetTradeUser(this.twoId).GetClient().GetHabbo().Id, true);
				this.GetTradeUser(this.twoId).GetClient().GetHabbo().GetInventoryComponent().AddItem(I.Id, I.BaseItem, I.ExtraData, false);
			}

			foreach (UserItem I in ItemsTwo)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE items SET room_id = '0', user_id = '",
						this.GetTradeUser(this.oneId).GetClient().GetHabbo().Id,
						"' WHERE Id = '",
						I.Id,
						"' LIMIT 1"
					}));
				}
				this.GetTradeUser(this.twoId).GetClient().GetHabbo().GetInventoryComponent().RemoveItem(I.Id, this.GetTradeUser(this.oneId).GetClient().GetHabbo().Id, true);
				this.GetTradeUser(this.oneId).GetClient().GetHabbo().GetInventoryComponent().AddItem(I.Id, I.BaseItem, I.ExtraData, false);
			}
			this.GetTradeUser(this.oneId).GetClient().GetHabbo().GetInventoryComponent().UpdateItems(true);
			this.GetTradeUser(this.twoId).GetClient().GetHabbo().GetInventoryComponent().UpdateItems(true);

		}

		public void CloseTradeClean()
		{
			for (int i = 0; i < this.Users.Length; i++)
			{
				TradeUser User = this.Users[i];
				if (User != null && User.GetRoomUser() != null)
				{
					User.GetRoomUser().RemoveStatus("trd");
					User.GetRoomUser().UpdateNeeded = true;
				}
			}
			this.SendMessageToUsers(new ServerMessage(112));
			this.GetRoom().ActiveTrades.Remove(this);
		}

		public void CloseTrade(uint UserId)
		{
			for (int i = 0; i < this.Users.Length; i++)
			{
				TradeUser User = this.Users[i];
				if (User != null && User.GetRoomUser() != null)
				{
					User.GetRoomUser().RemoveStatus("trd");
					User.GetRoomUser().UpdateNeeded = true;
				}
			}
			ServerMessage Message = new ServerMessage(110);
			Message.AppendUInt(UserId);
			this.SendMessageToUsers(Message);
		}

		public void SendMessageToUsers(ServerMessage Message)
		{
            foreach (TradeUser User in Users)
            {
                User.GetClient().SendMessage(Message);
            }
		}

		private Room GetRoom()
		{
			return PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(this.RoomId);
		}
	}
}
