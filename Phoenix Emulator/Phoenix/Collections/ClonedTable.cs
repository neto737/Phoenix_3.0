using System;
using System.Collections;
namespace Phoenix.Collections
{
	internal class ClonedTable : Hashtable, IDisposable
	{
		private bool mDisposed;

		public ClonedTable(Hashtable CloneTable) : base(CloneTable)
		{
			mDisposed = false;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool Disposing)
		{
			if (!mDisposed)
			{
				mDisposed = true;
				if (Disposing)
				{
					base.Clear();
				}
			}
		}
	}
}
