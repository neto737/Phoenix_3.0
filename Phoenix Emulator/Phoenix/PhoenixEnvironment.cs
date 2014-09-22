using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Phoenix.Core;
using Phoenix.HabboHotel;
using Phoenix.Net;
using Phoenix.Storage;
using Phoenix.Util;
using Phoenix.Communication;
using Phoenix.Messages;
namespace Phoenix
{
    internal class PhoenixEnvironment
    {
        public const int Build = 14986;
        private static MessageHandler Messages;
        private static ConfigurationData Configuration;
        private static DatabaseManager DatabaseManager;
        private static TcpConnectionManager ConnectionManager;
        private static MusSocket MusListener;
        private static Game Game;
        internal static DateTime ServerStarted;
        private static bool ShutdownInitiated = false;

        internal static void BeginShutDown()
        {
            PerformShutDown("", true);
        }

        public static string BoolToEnum(bool Bool)
        {
            if (Bool)
            {
                return "1";
            }
            return "0";
        }

        public static string CheckMD5Crypto(string data)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] array = Encoding.UTF8.GetBytes(data);
            array = mD5CryptoServiceProvider.ComputeHash(array);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x2").ToLower());
            }
            string text = stringBuilder.ToString();
            return text.ToUpper();
        }

        public static void Destroy()
        {
            Logging.WriteLine("Destroying PhoenixEmu environment...");
            if (GetGame() != null)
            {
                GetGame().Destroy();
                Game = null;
            }
            if (GetConnectionManager() != null)
            {
                Logging.WriteLine("Destroying connection manager.");
                GetConnectionManager().GetListener().Destroy();
                GetConnectionManager().DestroyManager();
                ConnectionManager = null;
            }
            if (GetDatabase() != null)
            {
                try
                {
                    Logging.WriteLine("Destroying database manager.");
                    MySqlConnection.ClearAllPools();
                    DatabaseManager = null;
                }
                catch { }
            }
            Logging.WriteLine("Uninitialized successfully. Closing.");
        }

        public static bool EnumToBool(string Enum)
        {
            return (Enum == "1");
        }

        public static int EnumToInt(string Enum)
        {
            return Convert.ToInt32(Enum);
        }

        public static string FilterInjectionChars(string Input)
        {
            return FilterInjectionChars(Input, false, false);
        }

        public static string FilterInjectionChars(string Input, bool AllowLinebreaks, bool ProtectSQL)
        {
            Input = Input.Replace(Convert.ToChar(1), ' ');
            Input = Input.Replace(Convert.ToChar(2), ' ');
            Input = Input.Replace(Convert.ToChar(9), ' ');
            if (!AllowLinebreaks)
            {
                Input = Input.Replace(Convert.ToChar(13), ' ');
            }
            if (ProtectSQL)
            {
                Input = Input.Replace('\'', ' ');
            }
            return Input;
        }

        public static ConfigurationData GetConfig()
        {
            return Configuration;
        }

        public static TcpConnectionManager GetConnectionManager()
        {
            return ConnectionManager;
        }

        public static DatabaseManager GetDatabase()
        {
            return DatabaseManager;
        }

        public static Encoding GetDefaultEncoding()
        {
            return Encoding.Default;
        }

        internal static Game GetGame()
        {
            return Game;
        }

        public static MessageHandler GetPacketManager()
        {
            return Messages;
        }

        public static int GetRandomNumber(int Min, int Max)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] data = new byte[4];
            provider.GetBytes(data);
            return new Random(BitConverter.ToInt32(data, 0)).Next(Min, Max + 1);
        }

        public static double GetUnixTimestamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        public void Initialize()
        {
            ServerStarted = DateTime.Now;
            TextManager.WritePhoenix();
            try
            {
                Configuration = new ConfigurationData("config.conf");
                DateTime now = DateTime.Now;

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;

                DatabaseServer server = new DatabaseServer(GetConfig().data["db.hostname"], uint.Parse(GetConfig().data["db.port"]), GetConfig().data["db.username"], GetConfig().data["db.password"]);
                Database database = new Database(GetConfig().data["db.name"], uint.Parse(GetConfig().data["db.pool.minsize"]), uint.Parse(GetConfig().data["db.pool.maxsize"]));
                DatabaseManager = new DatabaseManager(server, database);
                Game = new Game(int.Parse(GetConfig().data["game.tcp.conlimit"]));

                Messages = new MessageHandler();
                Messages.RegisterHandshake();
                Messages.RegisterMessenger();
                Messages.RegisterNavigator();
                Messages.RegisterRoomsAction();
                Messages.RegisterRoomsAvatar();
                Messages.RegisterRoomsChat();
                Messages.RegisterRoomsEngine();
                Messages.RegisterRoomsFurniture();
                Messages.RegisterRoomsPets();
                Messages.RegisterRoomsSession();
                Messages.RegisterRoomsSettings();
                Messages.RegisterCatalog();
                Messages.RegisterMarketplace();
                Messages.RegisterRecycler();
                Messages.RegisterQuest();
                Messages.RegisterInventoryAchievements();
                Messages.RegisterInventoryAvatarFX();
                Messages.RegisterInventoryBadges();
                Messages.RegisterInventoryFurni();
                Messages.RegisterInventoryPurse();
                Messages.RegisterInventoryTrading();
                Messages.RegisterAvatar();
                Messages.RegisterUsers();
                Messages.RegisterRegister();
                Messages.RegisterHelp();
                Messages.RegisterSound();
                Messages.RegisterWired();
                Messages.RegisterFriendStream(); //NEW!

                MusListener = new MusSocket(GetConfig().data["mus.tcp.bindip"], int.Parse(GetConfig().data["mus.tcp.port"]), GetConfig().data["mus.tcp.allowedaddr"].Split(new char[] { ';' }), 20);
                ConnectionManager = new TcpConnectionManager(GetConfig().data["game.tcp.bindip"], int.Parse(GetConfig().data["game.tcp.port"]), int.Parse(GetConfig().data["game.tcp.conlimit"]));
                ConnectionManager.GetListener().Start();
                TimeSpan span = DateTime.Now - now;
                Logging.WriteLine(string.Concat(new object[] { "Server -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                Console.Beep();
            }
            catch (KeyNotFoundException)
            {
                Logging.WriteLine("Failed to boot, key not found.");
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                Destroy();
            }
            catch (InvalidOperationException ex)
            {
                Logging.WriteLine("Failed to initialize PhoenixEmulator: " + ex.Message);
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                Destroy();
            }
        }

        public static bool IsDivisble(int x, int n)
        {
            return ((x % n) == 0);
        }

        public static bool IsValidAlphaNumeric(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!char.IsLetter(inputStr[i]) && !char.IsNumber(inputStr[i]))
                {
                    return false;
                }
            }
            return true;
        }

        internal static void PerformShutDown(string reason, bool ExitWhenDone)
        {
            GlobalClass.ShuttingDown = true;
            try
            {
                GetPacketManager().UnregisterPackets();
            }
            catch { }
            if (reason != "")
            {
                if (ShutdownInitiated)
                {
                    return;
                }
                Console.WriteLine(reason);
                Logging.DisablePrimaryWriting();
                SendMassMessage("ATTENTION:\r\nThe server is shutting down. All furniture placed in rooms/traded/bought after this message is on your own responsibillity.");
                ShutdownInitiated = true;
                Console.WriteLine("Server shutting down...");
                try
                {
                    Game.GetRoomManager().RemoveAllRooms();
                }
                catch { }
                try
                {
                    GetConnectionManager().GetListener().Stop();
                    GetGame().GetClientManager().CloseAll();
                }
                catch { }
                try
                {
                    using (DatabaseClient adapter = GetDatabase().GetClient())
                    {
                        adapter.ExecuteQuery("UPDATE users SET online = '0'");
                        adapter.ExecuteQuery("UPDATE rooms SET users_now = '0'");
                    }
                    ConnectionManager.Shutdown();
                    Game.Destroy();
                }
                catch { }
                try
                {
                    Console.WriteLine("Destroying database manager.");
                    MySqlConnection.ClearAllPools();
                    DatabaseManager = null;
                }
                catch { }
                Console.WriteLine("System disposed, goodbye!");
            }
            else
            {
                Logging.DisablePrimaryWriting();
                ShutdownInitiated = true;
                try
                {
                    Game.GetRoomManager().RemoveAllRooms();
                }
                catch { }
                try
                {
                    GetConnectionManager().GetListener().Stop();
                    GetGame().GetClientManager().CloseAll();
                }
                catch { }
                ConnectionManager.Shutdown();
                Game.Destroy();
                Console.WriteLine(reason);
            }
            if (ExitWhenDone)
            {
                Environment.Exit(0);
            }
        }

        internal static void SendMassMessage(string Message)
        {
            try
            {
                ServerMessage message = new ServerMessage(139);
                message.AppendStringWithBreak(Message);
                GetGame().GetClientManager().BroadcastMessage(message);
            }
            catch { }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return time.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        internal static Game GameInstance
        {
            get
            {
                return Game;
            }
            set
            {
                Game = value;
            }
        }

        public static string PrettyVersion
        {
            get
            {
                return "Phoenix v3.11.0 (Build " + Build + ")";
            }
        }
    }
}
