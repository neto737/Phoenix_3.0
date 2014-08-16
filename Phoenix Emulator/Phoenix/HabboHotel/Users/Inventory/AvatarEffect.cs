using System;
namespace Phoenix.HabboHotel.Users.Inventory
{
	class AvatarEffect
	{
		public int EffectId;
		public int TotalDuration;
		public bool Activated;
		public double StampActivated;

        public int TimeLeft
        {
            get
            {
                if (!Activated)
                {
                    return -1;
                }
                double diff = PhoenixEnvironment.GetUnixTimestamp() - StampActivated;
                if (diff >= this.TotalDuration)
                {
                    return 0;
                }
                return (TotalDuration - ((int)diff));
            }
        }

        public bool HasExpired
        {
            get
            {
                if (TimeLeft == -1)
                {
                    return false;
                }
                return (TimeLeft <= 0);
            }
        }

        public AvatarEffect(int EffectId, int TotalDuration, bool Activated, double ActivateTimestamp)
        {
            this.EffectId = EffectId;
            this.TotalDuration = TotalDuration;
            this.Activated = Activated;
            this.StampActivated = ActivateTimestamp;
        }

		public void Activate()
		{
			this.Activated = true;
			this.StampActivated = PhoenixEnvironment.GetUnixTimestamp();
		}
	}
}
