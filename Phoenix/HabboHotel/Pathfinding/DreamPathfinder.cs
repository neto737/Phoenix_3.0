using System;
namespace Phoenix.HabboHotel.Pathfinding
{
	internal sealed class DreamPathfinder
	{

		private static SquarePoint GetClosetSqare(SquareInformation pInfo, HeightInfo Height, bool CalculateDiagonal)
		{
			double getDistance = pInfo.Point.GetDistance;
			SquarePoint point = pInfo.Point;
			double state = Height.GetState(pInfo.Point.X, pInfo.Point.Y);
			for (int i = 0; i < 8; i++)
			{
				SquarePoint point2 = pInfo.Pos(i);
                if ((point2.InUse && point2.CanWalk) && (((Height.GetState(point2.X, point2.Y) - state) < 2.0)))
				{
					double num4 = point2.GetDistance;
					if (getDistance > num4)
					{
						getDistance = num4;
						point = point2;
					}
				}
			}
			return point;
		}

		internal static double GetDistance(int x1, int y1, int x2, int y2)
		{
			return Math.Sqrt(Math.Pow((double)(x1 - x2), 2.0) + Math.Pow((double)(y1 - y2), 2.0));
		}

        internal static SquarePoint GetNextStep(int pUserX, int pUserY, int pUserTargetX, int pUserTargetY, byte[,] pGameMap, double[,] pHeight, double[,] floorHeight, double[,] sitHeight, int MaxX, int MaxY, bool pUserOverride, bool pDiagonal)
        {
            ModelInfo pMap = new ModelInfo(MaxX, MaxY, pGameMap);
            SquarePoint pTarget = new SquarePoint(pUserTargetX, pUserTargetY, pUserTargetX, pUserTargetY, pMap.GetState(pUserTargetX, pUserTargetY), pUserOverride);
            if (pUserX == pUserTargetX && pUserY == pUserTargetY)
            {
                return pTarget;
            }
            SquareInformation pInfo = new SquareInformation(pUserX, pUserY, pTarget, pMap, pUserOverride, pDiagonal);
            return GetClosetSqare(pInfo, new HeightInfo(MaxX, MaxY, pHeight, floorHeight, sitHeight), pDiagonal);
        }
	}
}
