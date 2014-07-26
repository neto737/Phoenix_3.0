using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Core
{
    class RandomBase : Random
    {
        #region Constructors
        public RandomBase() { }

        public RandomBase(int seed) : base(seed) { }
        #endregion

        #region Methods
        protected int GetBaseNextInt32()
        {
            return base.Next();
        }

        protected uint GetBaseNextUInt32()
        {
            return ConvertToUInt32(base.Next());
        }

        protected double GetBaseNextDouble()
        {
            return base.NextDouble();
        }
        #endregion

        #region Overrides
        //public abstract override int Next();

        public override int Next(int maxValue)
        {
            return base.Next(0, maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            return Convert.ToInt32((minValue - maxValue) * Sample() + minValue);
        }

        public override double NextDouble()
        {
            return Sample();
        }

        public override void NextBytes(byte[] buffer)
        {
            int i, j, tmp;

            //fill the part of the buffer that can be covered by full Int32s
            for (i = 0; i < buffer.Length - 4; i += 4)
            {
                tmp = Next();

                buffer[i] = Convert.ToByte(tmp & 0x000000FF);
                buffer[i + 1] = Convert.ToByte((tmp & 0x0000FF00) >> 8);
                buffer[i + 2] = Convert.ToByte((tmp & 0x00FF0000) >> 16);
                buffer[i + 3] = Convert.ToByte((tmp & 0xFF000000) >> 24);
            }

            tmp = Next();

            //fill the rest of the buffer
            for (j = 0; j < buffer.Length % 4; j++)
            {
                buffer[i + j] = Convert.ToByte(((tmp & (0x000000FF << (8 * j))) >> (8 * j)));
            }
        }

        protected override double Sample()
        {
            // generates a random number on (0,1)
            return Convert.ToDouble(Next()) / 2147483648.0; // divided by 2^31 (Int32 absolute value)
        }
        #endregion

        #region Utility Methods
        protected static UInt32 ConvertToUInt32(Int32 value)
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
        }

        protected static Int32 ConvertToInt32(UInt32 value)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        }

        protected static Int32 ConvertToInt32(UInt64 value)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(value & 0x000000007fffffff), 0);
        }
        #endregion
    }
}
