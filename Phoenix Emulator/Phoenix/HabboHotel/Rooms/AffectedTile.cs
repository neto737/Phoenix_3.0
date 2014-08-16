using System;
namespace Phoenix.HabboHotel.Rooms
{
	class AffectedTile
	{
		private int mX;
		private int mY;
		private int mI;

		public int X
		{
			get
			{
				return mX;
			}
		}

		public int Y
		{
			get
            {
				return mY;
			}
		}

		public int I
		{
			get
			{
            	return mI;
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