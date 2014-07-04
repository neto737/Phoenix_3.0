using System;
namespace Phoenix.Core
{
	public abstract class Message12 : Random
	{
		public Message12()
		{
		}
		public Message12(int int_0) : base(int_0)
		{
		}
		protected int method_0()
		{
			return base.Next();
		}
		protected uint method_1()
		{
			return Message12.smethod_0(base.Next());
		}
		protected double method_2()
		{
			return base.NextDouble();
		}
		public abstract override int Next();
		public override int Next(int maxValue)
		{
			return this.Next(0, maxValue);
		}
		public override int Next(int minValue, int maxValue)
		{
			return Convert.ToInt32((double)(maxValue - minValue) * this.Sample() + (double)minValue);
		}
		public override double NextDouble()
		{
			return this.Sample();
		}
		public override void NextBytes(byte[] buffer)
		{
			int i;
			int num;
			for (i = 0; i < buffer.Length - 4; i += 4)
			{
				num = this.Next();
				buffer[i] = Convert.ToByte(num & 255);
				buffer[i + 1] = Convert.ToByte((num & 65280) >> 8);
				buffer[i + 2] = Convert.ToByte((num & 16711680) >> 16);
				buffer[i + 3] = Convert.ToByte(((long)num & (long)((long)-16777216)) >> 24);
			}
			num = this.Next();
			for (int j = 0; j < buffer.Length % 4; j++)
			{
				buffer[i + j] = Convert.ToByte((num & 255 << 8 * j) >> 8 * j);
			}
		}
		protected override double Sample()
		{
			return Convert.ToDouble(this.Next()) / 2147483648.0;
		}
		protected static uint smethod_0(int int_0)
		{
			return BitConverter.ToUInt32(BitConverter.GetBytes(int_0), 0);
		}
		protected static int smethod_1(uint uint_0)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(uint_0), 0);
		}
		protected static int smethod_2(ulong ulong_0)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(ulong_0 & 2147483647uL), 0);
		}
	}
}
