using System;
using System.Collections;
namespace Phoenix.Collections
{
	internal sealed class Sandbox : Hashtable, IDisposable
	{
		private bool mDisposed = false;

		internal void Clear()
		{
			this.Dispose();
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

        internal ClonedTable GetThreadSafeTable
        {
            get
            {
                return new ClonedTable(base.Clone() as Hashtable);
            }
        }
	}
}
