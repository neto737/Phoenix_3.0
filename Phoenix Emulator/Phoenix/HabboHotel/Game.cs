using System;
using System.Data;
using System.Threading.Tasks;
using Phoenix.HabboHotel.Misc;
using Phoenix.Core;
using Phoenix.HabboHotel.Navigators;
using Phoenix.HabboHotel.Catalogs;
using Phoenix.HabboHotel.Support;
using Phoenix.HabboHotel.Roles;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
using Phoenix.HabboHotel.Advertisements;
using Phoenix.HabboHotel.Achievements;
using Phoenix.HabboHotel.RoomBots;
using Phoenix.HabboHotel.Quests;
using Phoenix.Util;
using Phoenix.Storage;
using Phoenix.HabboHotel.Groups;
using Phoenix.HabboHotel.SoundMachine;
namespace Phoenix.HabboHotel
{
	internal class Game
	{
		private GameClientManager ClientManager;
		private ModerationBanManager BanManager;
		private RoleManager RoleManager;
		private HelpTool HelpTool;
		private Catalog Catalog;
		private Navigator Navigator;
		private ItemManager ItemManager;
		private RoomManager RoomManager;
		private AdvertisementManager AdvertisementManager;
		private PixelManager PixelManager;
		private AchievementManager AchievementManager;
		private ModerationTool ModerationTool;
		private BotManager BotManager;
		private Task Task;
		private NavigatorCache NavigatorCache;
		private Marketplace Marketplace;
		private QuestManager QuestManager;
		private TextManager TextManage;
		private GroupManager Guilds;
		public Game(int conns)
		{
			ClientManager = new GameClientManager(conns);
			if (PhoenixEnvironment.GetConfig().data["client.ping.enabled"] == "1")
			{
				ClientManager.StartConnectionChecker();
			}
			DateTime Now = DateTime.Now;
			Logging.Write("Connecting to database...");
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				Logging.WriteLine("completed!");
				PhoenixEnvironment.GameInstance = this;
				LoadSettings(adapter);
				BanManager = new ModerationBanManager();
				RoleManager = new RoleManager();
				HelpTool = new HelpTool();
				Catalog = new Catalog();
				Navigator = new Navigator();
				ItemManager = new ItemManager();
				RoomManager = new RoomManager();
				AdvertisementManager = new AdvertisementManager();
				PixelManager = new PixelManager();
				AchievementManager = new AchievementManager();
				ModerationTool = new ModerationTool();
				BotManager = new BotManager();
				Marketplace = new Marketplace();
				QuestManager = new QuestManager();
				TextManage = new TextManager();
                Guilds = new GroupManager();
				TextManager.LoadTexts(adapter);
				BanManager.LoadBans(adapter);
                RoleManager.LoadRoles(adapter);
				HelpTool.LoadCategories(adapter);
				HelpTool.LoadTopics(adapter);
				ModerationTool.LoadMessagePresets(adapter);
				ModerationTool.LoadPendingTickets(adapter);
				ItemManager.LoadItems(adapter);
				Catalog.Initialize(adapter);
				Catalog.InitCache();
				Navigator.Initialize(adapter);
				RoomManager.LoadModels(adapter);
				RoomManager.LoadCache();
				NavigatorCache = new NavigatorCache();
				AdvertisementManager.LoadRoomAdvertisements(adapter);
				BotManager.LoadBots(adapter);
				AchievementManager.LoadAchievements(adapter);
				PixelManager.Start();
				ChatCommandHandler.InitFilter(adapter);
				QuestManager.InitQuests();
				GroupManager.LoadGroups(adapter);
				DatabaseCleanup(adapter, 1);
			}
			Task = new Task(new Action(LowPriorityWorker.Process));
			Task.Start();
		}
		public void DatabaseCleanup(DatabaseClient adapter, int serverStatus)
		{
			Logging.Write(TextManager.GetText("emu_cleandb"));
			bool debug = true;
			try
			{
				if (int.Parse(PhoenixEnvironment.GetConfig().data["debug"]) == 1)
				{
					debug = false;
				}
			}
			catch
			{
			}
			if (debug)
			{
				adapter.ExecuteQuery("UPDATE users SET online = '0' WHERE online != '0'");
				adapter.ExecuteQuery("UPDATE rooms SET users_now = '0' WHERE users_now != '0'");
				adapter.ExecuteQuery("UPDATE user_roomvisits SET exit_timestamp = UNIX_TIMESTAMP() WHERE exit_timestamp <= 0");
				adapter.ExecuteQuery(string.Concat(new object[] { "UPDATE server_status SET status = '", serverStatus, "', users_online = '0', rooms_loaded = '0', server_ver = '", PhoenixEnvironment.PrettyVersion, "', stamp = UNIX_TIMESTAMP() LIMIT 1;" }));
			}
			Logging.WriteLine("completed!");
		}

		public void Destroy()
		{
			if (Task != null)
			{
				Task = null;
			}
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DatabaseCleanup(adapter, 0);
			}
			if (GetClientManager() != null)
			{
				GetClientManager().Clear();
				GetClientManager().StopConnectionChecker();
			}
			if (GetPixelManager() != null)
			{
				PixelManager.KeepAlive = false;
			}
			ClientManager = null;
			BanManager = null;
			RoleManager = null;
			HelpTool = null;
			Catalog = null;
			Navigator = null;
			ItemManager = null;
			RoomManager = null;
			AdvertisementManager = null;
			PixelManager = null;
		}

		public GameClientManager GetClientManager()
		{
			return ClientManager;
		}

		public ModerationBanManager GetBanManager()
		{
			return BanManager;
		}

		public RoleManager GetRoleManager()
		{
			return RoleManager;
		}

		public HelpTool GetHelpTool()
		{
			return HelpTool;
		}

		public Catalog GetCatalog()
		{
			return Catalog;
		}

		public Navigator GetNavigator()
		{
			return Navigator;
		}

		public ItemManager GetItemManager()
		{
			return ItemManager;
		}

		public RoomManager GetRoomManager()
		{
			return RoomManager;
		}

		public AdvertisementManager GetAdvertisementManager()
		{
			return AdvertisementManager;
		}

		public PixelManager GetPixelManager()
		{
			return PixelManager;
		}

		public AchievementManager GetAchievementManager()
		{
			return AchievementManager;
		}

		public ModerationTool GetModerationTool()
		{
			return ModerationTool;
		}

		public BotManager GetBotManager()
		{
			return BotManager;
		}

		internal NavigatorCache GetNavigatorCache()
		{
			return NavigatorCache;
		}

		public QuestManager GetQuestManager()
		{
			return QuestManager;
		}

		public void LoadSettings(DatabaseClient adapter)
		{
			Logging.Write("Loading your settings..");
			DataRow Row = adapter.ReadDataRow("SELECT * FROM server_settings LIMIT 1");
			GlobalClass.MaxRoomsPerUser = (int)Row["MaxRoomsPerUser"];
			GlobalClass.Motd = (string)Row["motd"];
			GlobalClass.Timer = (int)Row["timer"];
			GlobalClass.Credits = (int)Row["credits"];
			GlobalClass.Pixels = (int)Row["pixels"];
			GlobalClass.Points = (int)Row["points"];
			GlobalClass.pixels_max = (int)Row["pixels_max"];
			GlobalClass.credits_max = (int)Row["credits_max"];
			GlobalClass.points_max = (int)Row["points_max"];
			GlobalClass.MaxPetsPerRoom = (int)Row["MaxPetsPerRoom"];
			GlobalClass.MaxMarketPlacePrice = (int)Row["MaxMarketPlacePrice"];
			GlobalClass.MarketPlaceTax = (int)Row["MarketPlaceTax"];
			GlobalClass.AntiDDoSEnabled = PhoenixEnvironment.EnumToBool(Row["enable_antiddos"].ToString());
			GlobalClass.VIPclothesforHCusers = PhoenixEnvironment.EnumToBool(Row["vipclothesforhcusers"].ToString());
			GlobalClass.RecordChatlogs = PhoenixEnvironment.EnumToBool(Row["enable_chatlogs"].ToString());
			GlobalClass.RecordCmdlogs = PhoenixEnvironment.EnumToBool(Row["enable_cmdlogs"].ToString());
			GlobalClass.RecordRoomVisits = PhoenixEnvironment.EnumToBool(Row["enable_roomlogs"].ToString());
			GlobalClass.ExternalLinkMode = (string)Row["enable_externalchatlinks"];
			GlobalClass.SecureSessions = PhoenixEnvironment.EnumToBool(Row["enable_securesessions"].ToString());
			GlobalClass.AllowFriendlyFurni = PhoenixEnvironment.EnumToBool(Row["allow_friendfurnidrops"].ToString());
			GlobalClass.cmdRedeemCredits = PhoenixEnvironment.EnumToBool(Row["enable_cmd_redeemcredits"].ToString());
			GlobalClass.UnloadCrashedRooms = PhoenixEnvironment.EnumToBool(Row["unload_crashedrooms"].ToString());
			GlobalClass.ShowUsersAndRoomsInAbout = PhoenixEnvironment.EnumToBool(Row["ShowUsersAndRoomsInAbout"].ToString());
			GlobalClass.IdleSleep = (int)Row["idlesleep"];
			GlobalClass.IdleKick = (int)Row["idlekick"];
			GlobalClass.UseIP_Last = PhoenixEnvironment.EnumToBool(Row["ip_lastforbans"].ToString());
			Logging.WriteLine("completed!");
		}
	}
}
