using System;
namespace Phoenix.HabboHotel.Pathfinding
{
	internal struct HeightInfo
	{
		private double[,] mMap;
		private double[,] sMap;
		private double[,] fMap;
		private int mMaxX;
		private int mMaxY;

		public HeightInfo(int MaxX, int MaxY, double[,] Map, double[,] floorMap, double[,] sitHeight)
		{
			this.mMap = Map;
			this.fMap = floorMap;
			this.sMap = sitHeight;
			this.mMaxX = MaxX;
			this.mMaxY = MaxY;
		}

        internal double GetState(int x, int y)
        {
            if ((x >= mMaxX) || (x < 0))
            {
                return 0.0;
            }
            if ((y >= mMaxY) || (y < 0))
            {
                return 0.0;
            }
            if ((fMap[x, y] > mMap[x, y]) && (sMap[x, y] == 0.0))
            {
                return fMap[x, y];
            }
            if (sMap[x, y] == 0.0)
            {
                return mMap[x, y];
            }
            return sMap[x, y];
        }
	}
}
