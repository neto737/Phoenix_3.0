using System;
using System.Collections.Generic;
using System.Data;
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
                List<string> List = new List<string>();
                foreach (Subscription Subscription in Subscriptions.Values)
                {
                    List.Add(Subscription.SubscriptionId);
                }
                return List;
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
                    Subscriptions.Add((string)dataRow["subscription_id"], new Subscription((string)dataRow["subscription_id"], (int)dataRow["timestamp_activated"], (int)dataRow["timestamp_expire"]));
                }
            }
        }

        public void Clear()
        {
            this.Subscriptions.Clear();
        }

        public Subscription GetSubscription(string SubscriptionId)
        {
            if (Subscriptions.ContainsKey(SubscriptionId))
            {
                return Subscriptions[SubscriptionId];
            }
            return null;
        }

        public Boolean HasSubscription(string SubscriptionId)
        {
            if (!Subscriptions.ContainsKey(SubscriptionId))
            {
                return false;
            }
            Subscription Sub = Subscriptions[SubscriptionId];
            return Sub.IsValid();
        }

        public void AddOrExtendSubscription(string SubscriptionId, int DurationSeconds)
        {
            SubscriptionId = SubscriptionId.ToLower();
            if (Subscriptions.ContainsKey(SubscriptionId))
            {
                Subscription Sub = Subscriptions[SubscriptionId];
                Sub.ExtendSubscription(DurationSeconds);
                if (Sub.ExpireTime <= 0 || Sub.ExpireTime >= 2147483647)
                {
                    return;
                }
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.AddParamWithValue("subcrbr", SubscriptionId);
                    adapter.ExecuteQuery("UPDATE user_subscriptions SET timestamp_expire = '" + Sub.ExpireTime + "' WHERE user_id = '" + UserId + "' AND subscription_id = @subcrbr LIMIT 1");
                    return;
                }
            }
            if (!Subscriptions.ContainsKey("habbo_vip"))
            {
                int TimeCreated = (int)PhoenixEnvironment.GetUnixTimestamp();
                int TimeExpire = (int)PhoenixEnvironment.GetUnixTimestamp() + DurationSeconds;

                Subscription NewSub = new Subscription(SubscriptionId, TimeCreated, TimeExpire);

                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.AddParamWithValue("subcrbr", SubscriptionId);
                    adapter.ExecuteQuery("INSERT INTO user_subscriptions (user_id,subscription_id,timestamp_activated,timestamp_expire) VALUES ('" + UserId + "',@subcrbr,'" + TimeCreated + "','" + TimeExpire + "')");
                }
                Subscriptions.Add(NewSub.SubscriptionId.ToLower(), NewSub);
            }
        }
    }
}
