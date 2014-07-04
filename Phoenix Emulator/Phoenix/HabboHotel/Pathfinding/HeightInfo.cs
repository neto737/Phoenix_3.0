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
            if ((x >= this.mMaxX) || (x < 0))
            {
                return 0.0;
            }
            if ((y >= this.mMaxY) || (y < 0))
            {
                return 0.0;
            }
            if ((this.fMap[x, y] > this.mMap[x, y]) && (this.sMap[x, y] == 0.0))
            {
                return this.fMap[x, y];
            }
            if (this.sMap[x, y] == 0.0)
            {
                return this.mMap[x, y];
            }
            return this.sMap[x, y];
        }
	}
}
