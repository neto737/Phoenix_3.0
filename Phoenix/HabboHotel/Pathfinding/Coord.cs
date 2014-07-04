using System;
namespace Phoenix.HabboHotel.Pathfinding
{
	public struct Coord
	{
		internal int X;
		internal int Y;

		internal Coord(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

        public static bool operator ==(Coord a, Coord b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }
            if ((object)a == null || (object)b == null)
            {
                return false;
            }
            return ((a.X == b.X) && (a.Y == b.Y));
        }

        public static bool operator !=(Coord a, Coord b)
        {
            return !(a == b);
        }

		public override int GetHashCode()
		{
			return X ^ Y;
		}

		public override bool Equals(object obj)
		{
			return base.GetHashCode().Equals(obj.GetHashCode());
		}
	}
}
