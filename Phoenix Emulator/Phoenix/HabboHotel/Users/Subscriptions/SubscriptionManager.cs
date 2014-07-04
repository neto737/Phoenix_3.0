using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.HabboHotel.Users.UserDataManagement;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Users.Subscriptions
{
	internal class SubscriptionManager
	{
		private uint UserId;
		private Dictionary<string, Subscription> Subscriptions;
		public List<string> SubList
		{
			get
			{
				List<string> list = new List<string>();
				using (TimedLock.Lock(this.Subscriptions.Values))
				{
					foreach (Subscription current in this.Subscriptions.Values)
					{
						list.Add(current.SubscriptionId);
					}
				}
				return list;
			}
		}
		public SubscriptionManager(uint UserId, HabboData UserData)
		{
			this.UserId = UserId;
			this.Subscriptions = new Dictionary<string, Subscription>();
			DataTable getSupscriptionData = UserData.GetSupscriptionData;
			if (getSupscriptionData != null)
			{
				foreach (DataRow dataRow in getSupscriptionData.Rows)
				{
					this.Subscriptions.Add((string)dataRow["subscription_id"], new Subscription((string)dataRow["subscription_id"], (int)dataRow["timestamp_activated"], (int)dataRow["timestamp_expire"]));
				}
			}
		}
		public void Clear()
		{
			this.Subscriptions.Clear();
		}
		public Subscription GetSubscription(string SubscriptionId)
		{
			if (this.Subscriptions.ContainsKey(SubscriptionId))
			{
				return Subscriptions[SubscriptionId];
			}
			return null;
		}
		public bool HasSubscription(string SubscriptionId)
		{
			if (!this.Subscriptions.ContainsKey(SubscriptionId))
			{
				return false;
			}
			Subscription subscription = this.Subscriptions[SubscriptionId];
			return subscription.IsValid();
		}
		public void AddOrExtendSubscription(string SubscriptionId, int DurationSeconds)
		{
			SubscriptionId = SubscriptionId.ToLower();
			if (this.Subscriptions.ContainsKey(SubscriptionId))
			{
				Subscription subscription = this.Subscriptions[SubscriptionId];
				subscription.ExtendSubscription(DurationSeconds);
				if (subscription.ExpireTime <= 0 || subscription.ExpireTime >= 2147483647)
				{
					return;
				}
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.AddParamWithValue("subcrbr", SubscriptionId);
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE user_subscriptions SET timestamp_expire = '",
						subscription.ExpireTime,
						"' WHERE user_id = '",
						this.UserId,
						"' AND subscription_id = @subcrbr LIMIT 1"
					}));
					return;
				}
			}
			if (!this.Subscriptions.ContainsKey("habbo_vip"))
			{
				int unixTimestamp = (int)PhoenixEnvironment.GetUnixTimestamp();
				int timeExpire = (int)PhoenixEnvironment.GetUnixTimestamp() + DurationSeconds;
				Subscription subscription2 = new Subscription(SubscriptionId, unixTimestamp, timeExpire);
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.AddParamWithValue("subcrbr", SubscriptionId);
					adapter.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO user_subscriptions (user_id,subscription_id,timestamp_activated,timestamp_expire) VALUES ('",
						this.UserId,
						"',@subcrbr,'",
						unixTimestamp,
						"','",
						timeExpire,
						"')"
					}));
				}
				this.Subscriptions.Add(subscription2.SubscriptionId.ToLower(), subscription2);
			}
		}
	}
}
