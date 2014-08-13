using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Users.Badges
{
	class BadgeComponent
	{
		private List<Badge> Badges;
		private uint UserId;

		public int Count
		{
			get
			{
				return this.Badges.Count;
			}
		}

		public int EquippedCount
		{
			get
			{
				int i = 0;
				foreach (Badge Badge in Badges)
				{
					if (Badge.Slot > 0)
					{
						i++;
					}
				}
				return i;
			}
		}

		public List<Badge> BadgeList
		{
			get
			{
				return this.Badges;
			}
		}

		public BadgeComponent(uint mUserId, HabboData Data)
		{
			Badges = new List<Badge>();
			UserId = mUserId;
			DataTable data = Data.GetUserBadges;
			if (data != null)
			{
				foreach (DataRow Row in data.Rows)
				{
					Badges.Add(new Badge((string)Row["badge_id"], (int)Row["badge_slot"]));
				}
			}
		}

		public Badge GetBadge(string Badge)
		{
			foreach (Badge B in Badges)
			{
				if (Badge.ToLower() == B.Code.ToLower())
				{
					return B;
				}
			}
			return null;
		}

        public Boolean HasBadge(string Badge)
		{
			return this.GetBadge(Badge) != null;
		}

        public void GiveBadge(GameClient Session, string Badge, Boolean InDatabase)
		{
			this.GiveBadge(Badge, 0, InDatabase);
			ServerMessage Message = new ServerMessage(832);
			Message.AppendInt32(1);
			Message.AppendInt32(4);
			Message.AppendInt32(1);
			Message.AppendUInt(PhoenixEnvironment.GetGame().GetAchievementManager().BadgeID(Badge));
			Session.SendMessage(Message);
		}

        public void GiveBadge(string Badge, int Slot, bool InDatabase)
        {
            if (HasBadge(Badge))
            {
                return;
            }
            if (InDatabase)
            {
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.AddParamWithValue("badge", Badge);
                    adapter.ExecuteQuery("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('" + UserId + "',@badge,'" + Slot + "')");
                }
            }
            Badges.Add(new Badge(Badge, Slot));
        }

		public void SetBadgeSlot(string Badge, int Slot)
		{
			Badge B = GetBadge(Badge);
			if (B != null)
			{
				B.Slot = Slot;
			}
		}

		public void ResetSlots()
		{
			foreach (Badge Badge in this.Badges)
			{
				Badge.Slot = 0;
			}
		}

		public void RemoveBadge(string Badge)
		{
			if (HasBadge(Badge))
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.AddParamWithValue("badge", Badge);
					adapter.ExecuteQuery("DELETE FROM user_badges WHERE badge_id = @badge AND user_id = '" + UserId + "' LIMIT 1");
				}
				this.Badges.Remove(this.GetBadge(Badge));
			}
		}

		public ServerMessage Serialize()
		{
			List<Badge> EquippedBadges = new List<Badge>();
			ServerMessage Message = new ServerMessage(229);
			Message.AppendInt32(Count);
			foreach (Badge Badge in Badges)
			{
				Message.AppendUInt(PhoenixEnvironment.GetGame().GetAchievementManager().BadgeID(Badge.Code));
				Message.AppendStringWithBreak(Badge.Code);
				if (Badge.Slot > 0)
				{
					EquippedBadges.Add(Badge);
				}
			}
			Message.AppendInt32(EquippedBadges.Count);
			foreach (Badge Badge in EquippedBadges)
			{
				Message.AppendInt32(Badge.Slot);
				Message.AppendStringWithBreak(Badge.Code);
			}
			return Message;
		}
	}
}
