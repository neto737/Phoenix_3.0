using System;
using System.Text;
namespace Phoenix.Util
{
	public static class OldEncoding
	{
        public static int decodeVL64(string data)
        {
            return decodeVL64(data.ToCharArray());
        }

        public static int decodeVL64(char[] raw)
        {
            try
            {
                int index = 0;
                int num2 = 0;
                bool flag = (raw[index] & '\x0004') == 4;
                int num3 = (raw[index] >> 3) & '\a';
                num2 = raw[index] & '\x0003';
                index++;
                int num4 = 2;
                for (int i = 1; i < num3; i++)
                {
                    num2 |= (raw[index] & 0x3f) << num4;
                    num4 = 2 + (6 * i);
                    index++;
                }
                if (flag)
                {
                    num2 *= -1;
                }
                return num2;
            }
            catch
            {
                return 0;
            }
        }

		public static string encodeVL64(int i)
		{
			byte[] bytes = new byte[6];
			int index = 0;
			int num2 = index;
			int num3 = 1;
			int num4 = (i >= 0) ? 0 : 4;
			i = Math.Abs(i);
			bytes[index++] = (byte)(64 + (i & 3));
			for (i >>= 2; i != 0; i >>= 6)
			{
				num3++;
				bytes[index++] = (byte)(64 + (i & 63));
			}
			bytes[num2] = (byte)((int)bytes[num2] | num3 << 3 | num4);
			ASCIIEncoding encoding = new ASCIIEncoding();
			return encoding.GetString(bytes).Replace("\0", "");
		}
	}
}
