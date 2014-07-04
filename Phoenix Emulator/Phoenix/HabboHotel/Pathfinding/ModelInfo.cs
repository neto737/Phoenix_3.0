using System;
namespace Phoenix.HabboHotel.Pathfinding
{
	internal struct ModelInfo
	{
		private byte[,] mMap;
		private int mMaxX;
		private int mMaxY;
		public ModelInfo(int MaxX, int MaxY, byte[,] Map)
		{
			this.mMap = Map;
			this.mMaxX = MaxX;
			this.mMaxY = MaxY;
		}
		internal byte GetState(int x, int y)
		{
			if ((x >= mMaxX) || (x < 0))
			{
                return 0;
			}
			if (y >= mMaxY || y < 0)
			{
                return 0;
			}
            return mMap[x, y];
		}
	}
}
