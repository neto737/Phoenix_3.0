using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.HabboHotel.Users.UserDataManagement;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Users.Badges
{
	internal sealed class BadgeComponent
	{
		private List<Badge> Badges;
		private uint uint_0;
		public int Count
		{
			get
			{
				return this.Badges.Count;
			}
		}
		public int Int32_1
		{
			get
			{
				int num = 0;
				foreach (Badge current in this.Badges)
				{
					if (current.Slot > 0)
					{
						num++;
					}
				}
				return num;
			}
		}
		public List<Badge> List_0
		{
			get
			{
				return this.Badges;
			}
		}
		public BadgeComponent(uint uint_1, HabboData class12_0)
		{
			this.Badges = new List<Badge>();
			this.uint_0 = uint_1;
			DataTable dataTable_ = class12_0.GetUserBadges;
			if (dataTable_ != null)
			{
				foreach (DataRow dataRow in dataTable_.Rows)
				{
					this.Badges.Add(new Badge((string)dataRow["badge_id"], (int)dataRow["badge_slot"]));
				}
			}
		}
		public Badge method_0(string string_0)
		{
			Badge result;
			foreach (Badge current in this.Badges)
			{
				if (string_0.ToLower() == current.Code.ToLower())
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}
		public bool HasBadge(string string_0)
		{
			return this.method_0(string_0) != null;
		}
		public void GiveBadge(GameClient Session, string string_0, bool bool_0)
		{
			this.GiveBadge(string_0, 0, bool_0);
			ServerMessage Message = new ServerMessage(832u);
			Message.AppendInt32(1);
			Message.AppendInt32(4);
			Message.AppendInt32(1);
			Message.AppendUInt(PhoenixEnvironment.GetGame().GetAchievementManager().BadgeID(string_0));
			Session.SendMessage(Message);
		}
		public void GiveBadge(string string_0, int int_0, bool bool_0)
		{
			if (!this.HasBadge(string_0))
			{
				if (bool_0)
				{
					using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
					{
						@class.AddParamWithValue("badge", string_0);
						@class.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('",
							this.uint_0,
							"',@badge,'",
							int_0,
							"')"
						}));
					}
				}
				this.Badges.Add(new Badge(string_0, int_0));
			}
		}
		public void method_4(string string_0, int int_0)
		{
			Badge @class = this.method_0(string_0);
			if (@class != null)
			{
				@class.Slot = int_0;
			}
		}
		public void method_5()
		{
			foreach (Badge current in this.Badges)
			{
				current.Slot = 0;
			}
		}
		public void RemoveBadge(string string_0)
		{
			if (this.HasBadge(string_0))
			{
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					@class.AddParamWithValue("badge", string_0);
					@class.ExecuteQuery("DELETE FROM user_badges WHERE badge_id = @badge AND user_id = '" + this.uint_0 + "' LIMIT 1");
				}
				this.Badges.Remove(this.method_0(string_0));
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
			foreach (Badge current in EquippedBadges)
			{
				Message.AppendInt32(current.Slot);
				Message.AppendStringWithBreak(current.Code);
			}
			return Message;
		}
	}
}
