using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Phoenix.Core;
namespace Phoenix.HabboHotel.Navigators
{
	internal sealed class NavigatorCache
	{
		private Task mWorker;
		private bool mTaskEnded;
		private Hashtable mCacheList;

		public NavigatorCache()
		{
			mTaskEnded = false;
			mCacheList = new Hashtable();
			mWorker = new Task(CycleRooms);
			mWorker.Start();
		}

		private void CycleRooms()
		{
			while (!mTaskEnded)
			{
				try
				{
					Hashtable NewTable = new Hashtable();
                    int i = -2;

                    NewTable.Add(i, PhoenixEnvironment.GetGame().GetNavigator().GetDynamicNavigatorPacket(null, i).GetBytes());
					//NewTable.Add(-2, Phoenix.GetGame().GetNavigator().method_12(null, -2).GetBytes());
					Hashtable CurrentTable = mCacheList;
					mCacheList = NewTable;
					CurrentTable.Clear();
				}
				catch (Exception ex)
				{
                    Logging.LogThreadException(ex.ToString(), "Navigator cache task");
				}
				Thread.Sleep(100000);
			}
		}
		internal byte[] GetPacket(int Mode)
		{
			try
			{
				return mCacheList[Mode] as byte[];
			}
			catch
			{
				return null;
			}
		}
	}
}
