using System;
using System.Security.Cryptography;
using System.Text;
namespace Phoenix.Util
{
	internal static class GlobalClass
    {
        #region Fields
        public static int MaxMarketPlacePrice;
		public static int MarketPlaceTax;
		public static int MaxPetsPerRoom;
		public static bool AntiDDoSEnabled;
		public static int pixels_max;
		public static int points_max;
		public static int credits_max;
		private static string ss_motd = "";
		private static int ss_timer = 15;
		private static int ss_credits = 75;
		private static int ss_pixels = 15;
		private static int ss_points = 0;
		private static bool ss_RecordChatlogs = true;
		private static bool ss_RecordCmdlogs = true;
		private static bool roomlogs = true;
		private static bool ss_vipclothesforhcusers = true;
		private static bool ss_allowfurnidrops = true;
		private static int ss_maxroomsperuser = 50;
		private static string ss_ExternalLinkMode = "disabled";
		private static bool ss_SecureSessions = true;
		private static bool cmdPush = false;
		private static bool cmdPull = false;
		private static bool cmdFlagme = false;
		private static bool cmdMimic = false;
		private static bool cmdMoonwalk = false;
        private static bool cmdFaceless = false; //Faceless command
        private static bool cmdEnable = false; //Enable command
		private static bool cmdFollow = false;
		private static bool cmdCredits = false;
		public static bool ShuttingDown = false;
		public static bool UnloadCrashedRooms = true;
		public static bool ShowUsersAndRoomsInAbout = true;
		public static int IdleSleep = 300;
		public static int IdleKick = 1200;
		public static bool UseIP_Last = false;
        #endregion

        #region Return values
        public static bool cmdRedeemCredits
		{
			get
			{
				return cmdCredits;
			}
			set
			{
				cmdCredits = value;
			}
		}

		public static bool AllowFriendlyFurni
		{
			get
			{
				return ss_allowfurnidrops;
			}
			set
			{
				ss_allowfurnidrops = value;
			}
		}

		public static bool SecureSessions
		{
			get
			{
				return ss_SecureSessions;
			}
			set
			{
				ss_SecureSessions = value;
			}
		}

		public static string ExternalLinkMode
		{
			get
			{
				return ss_ExternalLinkMode;
			}
			set
			{
				ss_ExternalLinkMode = value;
			}
		}

		public static string Motd
		{
			get
			{
				return ss_motd;
			}
			set
			{
				ss_motd = value.Replace("\\n", "\n");
			}
		}

		public static bool VIPclothesforHCusers
		{
			get
			{
				return ss_vipclothesforhcusers;
			}
			set
			{
				ss_vipclothesforhcusers = value;
			}
		}

		public static bool RecordChatlogs
		{
			get
			{
				return ss_RecordChatlogs;
			}
			set
			{
				ss_RecordChatlogs = value;
			}
		}

		public static bool RecordCmdlogs
		{
			get
			{
				return ss_RecordCmdlogs;
			}
			set
			{
				ss_RecordCmdlogs = value;
			}
		}

		public static bool RecordRoomVisits
		{
			get
			{
				return roomlogs;
			}
			set
			{
				roomlogs = value;
			}
		}

		public static int Timer
		{
			get
			{
				return ss_timer;
			}
			set
			{
				ss_timer = value;
			}
		}

		public static int Credits
		{
			get
			{
				return ss_credits;
			}
			set
			{
				ss_credits = value;
			}
		}

		public static int Points
		{
			get
			{
				return ss_points;
			}
			set
			{
				ss_points = value;
			}
		}

		public static int Pixels
		{
			get
			{
				return ss_pixels;
			}
			set
			{
				ss_pixels = value;
			}
		}

		public static int MaxRoomsPerUser
		{
			get
			{
				return ss_maxroomsperuser;
			}
			set
			{
				ss_maxroomsperuser = value;
			}
		}

		public static bool cmdPushEnabled
		{
			get
			{
				return cmdPush;
			}
			set
			{
				cmdPush = value;
			}
		}

		public static bool cmdFollowEnabled
		{
			get
			{
				return cmdFollow;
			}
			set
			{
				cmdFollow = value;
			}
		}

		public static bool cmdPullEnabled
		{
			get
			{
				return cmdPull;
			}
			set
			{
				cmdPull = value;
			}
		}

		public static bool cmdFlagmeEnabled
		{
			get
			{
				return cmdFlagme;
			}
			set
			{
				cmdFlagme = value;
			}
		}

		public static bool cmdMimicEnabled
		{
			get
			{
				return cmdMimic;
			}
			set
			{
				cmdMimic = value;
			}
		}

		public static bool cmdMoonwalkEnabled
		{
			get
			{
				return cmdMoonwalk;
			}
			set
			{
				cmdMoonwalk = value;
			}
        }

        public static bool cmdFacelessEnabled
        {
            get
            {
                return cmdFaceless;
            }
            set
            {
                cmdFaceless = value;
            }
        }

        public static bool cmdEnableEnabled
        {
            get
            {
                return cmdEnable;
            }
            set
            {
                cmdEnable = value;
            }
        }
        #endregion
    }
}
