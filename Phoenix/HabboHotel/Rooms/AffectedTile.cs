using System;
namespace Phoenix.HabboHotel.Rooms
{
	public sealed class AffectedTile
	{
		private int mX;
		private int mY;
		private int mI;

		public int X
		{
			get
			{
				return this.mX;
			}
		}

		public int Y
		{
			get
            {
				return this.mY;
			}
		}

		public int I
		{
			get
			{
            	return this.mI;
			}
		}

		public AffectedTile(int x, int y, int i)
		{
			this.mX = x;
			this.mY = y;
			this.mI = i;
        }
	}
}