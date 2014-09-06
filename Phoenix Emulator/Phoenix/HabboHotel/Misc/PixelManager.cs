using System;
using System.Threading;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.HabboHotel.Rooms;

namespace Phoenix.HabboHotel.Misc
{
	internal sealed class PixelManager
	{
		public bool KeepAlive;
		private Thread WorkerThread;

		public PixelManager()
		{
			this.KeepAlive = true;
			this.WorkerThread = new Thread(new ThreadStart(this.Process));
			this.WorkerThread.Name = "Pixel Manager";
			this.WorkerThread.Priority = ThreadPriority.Lowest;
		}
		public void Start()
		{
			Logging.Write("Starting Reward Timer..");
			this.WorkerThread.Start();
			Logging.WriteLine("completed!");
		}
		private void Process()
		{
			try
			{
				while (this.KeepAlive)
				{
					if (PhoenixEnvironment.GetGame() != null && PhoenixEnvironment.GetGame().GetClientManager() != null)
					{
						PhoenixEnvironment.GetGame().GetClientManager().CheckPixelUpdates();
					}
					Thread.Sleep(15000);
				}
			}
			catch (ThreadAbortException)
			{
			}
		}
		public bool NeedsUpdate(GameClient Client)
		{
            double num = (PhoenixEnvironment.GetUnixTimestamp() - Client.GetHabbo().LastActivityPointsUpdate) / 60.0;
            return (num >= GlobalClass.Timer);
		}
		public void GivePixels(GameClient Session)
		{
			try
			{
				if (Session.GetHabbo().InRoom)
				{
					RoomUser @class = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class.IdleTime <= GlobalClass.IdleSleep)
					{
						double double_ = PhoenixEnvironment.GetUnixTimestamp();
						Session.GetHabbo().LastActivityPointsUpdate = double_;
						if (GlobalClass.Pixels > 0 && (Session.GetHabbo().ActivityPoints < GlobalClass.pixels_max || GlobalClass.pixels_max == 0))
						{
							Session.GetHabbo().ActivityPoints += GlobalClass.Pixels;
							Session.GetHabbo().UpdateActivityPointsBalance(GlobalClass.Pixels);
						}
						if (GlobalClass.Credits > 0 && (Session.GetHabbo().Credits < GlobalClass.credits_max || GlobalClass.credits_max == 0))
						{
							Session.GetHabbo().Credits += GlobalClass.Credits;
							if (Session.GetHabbo().Vip)
							{
								Session.GetHabbo().Credits += GlobalClass.Credits;
							}
							Session.GetHabbo().UpdateCreditsBalance(true);
						}
						if (GlobalClass.Points > 0 && (Session.GetHabbo().shells < GlobalClass.points_max || GlobalClass.points_max == 0))
						{
							Session.GetHabbo().shells += GlobalClass.Points;
							Session.GetHabbo().UpdateShellsBalance(false, true);
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
