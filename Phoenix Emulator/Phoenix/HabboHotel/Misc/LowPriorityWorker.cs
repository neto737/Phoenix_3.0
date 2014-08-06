using System;
using System.Diagnostics;
using System.Threading;
using Phoenix.Core;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Misc
{
    public class LowPriorityWorker
    {
        internal static uint UserCountCache;

        public static void Process()
        {
            using (DatabaseClient client = PhoenixEnvironment.GetDatabase().GetClient())
            {
                UserCountCache = (uint)client.ReadInt32("SELECT users FROM system_stats ORDER BY ID DESC LIMIT 1");
            }
            while (true)
            {
                try
                {
                    TimeSpan span = (TimeSpan)(DateTime.Now - PhoenixEnvironment.ServerStarted);
                    int Status = 1;
                    int UsersCount = PhoenixEnvironment.GetGame().GetClientManager().connectionCount + -1;
                    int loadedRoomsCount = PhoenixEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;
                    bool flag = true;
                    try
                    {
                        if (int.Parse(PhoenixEnvironment.GetConfig().data["debug"]) == 1)
                        {
                            flag = false;
                        }
                    }
                    catch
                    {
                    }
                    if (flag)
                    {
                        using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                        {
                            adapter.ExecuteQuery(string.Concat(new object[] { "UPDATE server_status SET stamp = UNIX_TIMESTAMP(), status = '", Status, "', users_online = '", UsersCount, "', rooms_loaded = '", loadedRoomsCount, "', server_ver = '", PhoenixEnvironment.PrettyVersion, "' LIMIT 1" }));
                            if (UsersCount > UserCountCache)
                            {
                                adapter.ExecuteQuery(string.Concat(new object[] { "UPDATE system_stats SET users = '", UsersCount, "', rooms = '", loadedRoomsCount, "' ORDER BY ID DESC LIMIT 1" }));
                            }
                        }
                    }
                    PhoenixEnvironment.GetGame().GetClientManager().CheckEffects();
                    Console.Title = string.Concat(new object[] { "Phoenix 3.0 | Online Users: ", UsersCount, " | Rooms Loaded: ", loadedRoomsCount, " | Uptime: ", span.Days, " days, ", span.Hours, " hours and ", span.Minutes, " minutes" });
                }
                catch (Exception exception)
                {
                    Logging.LogThreadException(exception.ToString(), "Server status update task");
                }
                Thread.Sleep(5000);
            }
        }
    }
}
