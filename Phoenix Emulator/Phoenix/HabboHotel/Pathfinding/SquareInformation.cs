using System;
namespace Phoenix.HabboHotel.Pathfinding
{
	internal struct SquareInformation
	{
		private int mX;
		private int mY;
		private SquarePoint[] mPos;
		private SquarePoint mTarget;
		private SquarePoint mPoint;

		public SquareInformation(int pX, int pY, SquarePoint pTarget, ModelInfo pMap, bool pUserOverride, bool CalculateDiagonal)
		{
			this.mX = pX;
			this.mY = pY;
			this.mTarget = pTarget;
			this.mPoint = new SquarePoint(pX, pY, pTarget.X, pTarget.Y, pMap.GetState(pX, pY), pUserOverride);
			this.mPos = new SquarePoint[8];
			if (CalculateDiagonal)
			{
				this.mPos[1] = new SquarePoint(pX - 1, pY - 1, pTarget.X, pTarget.Y, pMap.GetState(pX - 1, pY - 1), pUserOverride);
				this.mPos[3] = new SquarePoint(pX - 1, pY + 1, pTarget.X, pTarget.Y, pMap.GetState(pX - 1, pY + 1), pUserOverride);
				this.mPos[5] = new SquarePoint(pX + 1, pY + 1, pTarget.X, pTarget.Y, pMap.GetState(pX + 1, pY + 1), pUserOverride);
				this.mPos[7] = new SquarePoint(pX + 1, pY - 1, pTarget.X, pTarget.Y, pMap.GetState(pX + 1, pY - 1), pUserOverride);
			}
			this.mPos[0] = new SquarePoint(pX, pY - 1, pTarget.X, pTarget.Y, pMap.GetState(pX, pY - 1), pUserOverride);
			this.mPos[2] = new SquarePoint(pX - 1, pY, pTarget.X, pTarget.Y, pMap.GetState(pX - 1, pY), pUserOverride);
			this.mPos[4] = new SquarePoint(pX, pY + 1, pTarget.X, pTarget.Y, pMap.GetState(pX, pY + 1), pUserOverride);
			this.mPos[6] = new SquarePoint(pX + 1, pY, pTarget.X, pTarget.Y, pMap.GetState(pX + 1, pY), pUserOverride);
		}

		internal SquarePoint Pos(int val)
		{
			return this.mPos[val];
		}

        internal SquarePoint Point
        {
            get
            {
                return this.mPoint;
            }
        }
	}
}
