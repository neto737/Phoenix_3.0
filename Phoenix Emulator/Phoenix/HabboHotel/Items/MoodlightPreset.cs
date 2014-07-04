using System;
namespace Phoenix.HabboHotel.Items
{
	internal sealed class MoodlightPreset
	{
		public string ColorCode;
		public int ColorIntensity;
		public bool BackgroundOnly;
		public MoodlightPreset(string ColorCode, int ColorIntensity, bool BackgroundOnly)
		{
			this.ColorCode = ColorCode;
			this.ColorIntensity = ColorIntensity;
			this.BackgroundOnly = BackgroundOnly;
		}
	}
}
