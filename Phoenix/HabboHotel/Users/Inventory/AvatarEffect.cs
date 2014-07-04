using System;
namespace Phoenix.HabboHotel.Users.Inventory
{
	internal sealed class AvatarEffect
	{
		public int EffectId;
		public int TotalDuration;
		public bool Activated;
		public double StampActivated;

        public int TimeLeft
        {
            get
            {
                if (!this.Activated)
                {
                    return -1;
                }
                double num = PhoenixEnvironment.GetUnixTimestamp() - this.StampActivated;
                if (num >= this.TotalDuration)
                {
                    return 0;
                }
                return (this.TotalDuration - ((int)num));
            }
        }

        public bool HasExpired
        {
            get
            {
                if (this.TimeLeft == -1)
                {
                    return false;
                }
                return (this.TimeLeft <= 0);
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
