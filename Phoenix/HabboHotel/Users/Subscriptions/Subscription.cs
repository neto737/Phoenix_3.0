using System;
namespace Phoenix.HabboHotel.Users.Subscriptions
{
	internal sealed class Subscription
	{
		private string Caption;
		private int TimeActivated;
		private int TimeExpire;

		public string SubscriptionId
		{
			get
			{
				return this.Caption;
			}
		}

        public int ExpireTime
        {
            get
            {
                return this.TimeExpire;
            }
        }

        public Subscription(string Caption, int TimeActivated, int TimeExpire)
        {
            this.Caption = Caption;
            this.TimeActivated = TimeActivated;
            this.TimeExpire = TimeExpire;
        }

        public bool IsValid()
        {
            if (this.TimeExpire <= PhoenixEnvironment.GetUnixTimestamp())
            {
                return false;
            }
            return true;
        }

		public void ExtendSubscription(int Time)
		{
			if (this.TimeExpire + Time < 2147483647)
			{
				this.TimeExpire += Time;
			}
		}
	}
}
