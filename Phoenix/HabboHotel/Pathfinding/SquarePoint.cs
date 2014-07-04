using System;
namespace Phoenix.HabboHotel.Pathfinding
{
	internal struct SquarePoint
	{
		private int mX;
		private int mY;
		private double mDistance;
		private byte mSquareData;
		private bool mInUse;
		private bool mOverride;
		private bool mLastStep;

		public SquarePoint(int pX, int pY, int pTargetX, int pTargetY, byte SquareData, bool pOverride)
		{
			this.mX = pX;
			this.mY = pY;
			this.mSquareData = SquareData;
			this.mInUse = true;
			this.mOverride = pOverride;
			this.mDistance = 0.0;
			this.mLastStep = (pX == pTargetX && pY == pTargetY);
			this.mDistance = DreamPathfinder.GetDistance(pX, pY, pTargetX, pTargetY);
		}

        internal int X
        {
            get
            {
                return this.mX;
            }
            set
            {
                this.mX = value;
            }
        }
        internal int Y
        {
            get
            {
                return this.mY;
            }
            set
            {
                this.mY = value;
            }
        }
        internal double GetDistance
        {
            get
            {
                return this.mDistance;
            }
        }
        internal bool CanWalk
        {
            get
            {
                if (!this.mLastStep)
                {
                    if (!this.mOverride)
                    {
                        return ((this.mSquareData == 1) || (this.mSquareData == 4));
                    }
                    return true;
                }
                if (!this.mOverride)
                {
                    if (this.mSquareData == 3)
                    {
                        return true;
                    }
                    if (this.mSquareData == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
                return false;
            }
        }
        internal bool InUse
        {
            get
            {
                return this.mInUse;
            }
        }
	}
}
