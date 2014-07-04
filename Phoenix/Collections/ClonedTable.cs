using System;
using System.Collections;
namespace Phoenix.Collections
{
	internal sealed class ClonedTable : Hashtable, IDisposable
	{
		private bool mDisposed;

		public ClonedTable(Hashtable CloneTable) : base(CloneTable)
		{
			this.mDisposed = false;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool Disposing)
		{
			if (!this.mDisposed)
			{
				this.mDisposed = true;
				if (Disposing)
				{
					base.Clear();
				}
			}
		}
	}
}
