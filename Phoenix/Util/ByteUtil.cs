using System;
namespace Phoenix.Util
{
    public static class ByteUtil
	{
		public static byte[] ChompBytes(byte[] bzBytes, int Offset, int numBytes)
		{
			int End = Offset + numBytes;
			if (End > bzBytes.Length)
				End = bzBytes.Length;

            if (numBytes > bzBytes.Length)
                numBytes = bzBytes.Length;

			if (numBytes < 0)
				numBytes = 0;

			byte[] bzChunk = new byte[numBytes];
			for (int x = 0; x < numBytes; x++)
			{
				bzChunk[x] = bzBytes[Offset++];
			}
			return bzChunk;
		}
	}
}
