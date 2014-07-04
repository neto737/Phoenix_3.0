using System;
namespace Phoenix.Core
{
	public sealed class Message13 : Message12
	{
		private static readonly uint uint_0 = 1099087573u;
		private ulong ulong_0;
		public Message13() : this(Convert.ToInt32(DateTime.Now.Ticks & 2147483647L))
		{
		}
		public Message13(int int_0) : base(int_0)
		{
			this.ulong_0 = Convert.ToUInt64(base.method_0());
		}
		public override int Next()
		{
			this.ulong_0 = (ulong)Message13.uint_0 * this.ulong_0;
			return Message12.smethod_2(this.ulong_0);
		}
	}
}
