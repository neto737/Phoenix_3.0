using System;
namespace Phoenix.HabboHotel.Pathfinding
{
	internal sealed class Rotation
	{
        public static int Calculate(int X1, int Y1, int X2, int Y2)
        {
            int num = 0;
            if ((X1 > X2) && (Y1 > Y2))
            {
                return 7;
            }
            if ((X1 < X2) && (Y1 < Y2))
            {
                return 3;
            }
            if ((X1 > X2) && (Y1 < Y2))
            {
                return 5;
            }
            if ((X1 < X2) && (Y1 > Y2))
            {
                return 1;
            }
            if (X1 > X2)
            {
                return 6;
            }
            if (X1 < X2)
            {
                return 2;
            }
            if (Y1 < Y2)
            {
                return 4;
            }
            if (Y1 > Y2)
            {
                num = 0;
            }
            return num;
        }

        public static int CalculateMoonWalk(int X1, int Y1, int X2, int Y2)
        {
            int num = 0;
            if ((X1 > X2) && (Y1 > Y2))
            {
                return 3;
            }
            if ((X1 < X2) && (Y1 < Y2))
            {
                return 7;
            }
            if ((X1 > X2) && (Y1 < Y2))
            {
                return 1;
            }
            if ((X1 < X2) && (Y1 > Y2))
            {
                return 5;
            }
            if (X1 > X2)
            {
                return 2;
            }
            if (X1 < X2)
            {
                return 6;
            }
            if (Y1 < Y2)
            {
                return 0;
            }
            if (Y1 > Y2)
            {
                num = 4;
            }
            return num;
        }
	}
}
