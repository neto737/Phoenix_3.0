using System;
using System.Collections;
namespace Phoenix.Collections
{
	internal class Sandbox : Hashtable, IDisposable
	{
		private bool mDisposed = false;

		internal void Clear()
		{
			Dispose();
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

        internal ClonedTable GetThreadSafeTable
        {
            get
            {
                return new ClonedTable(base.Clone() as Hashtable);
            }
        }
	}
}
