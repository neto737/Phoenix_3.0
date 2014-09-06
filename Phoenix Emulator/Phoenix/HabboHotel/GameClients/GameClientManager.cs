using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Support;
using Phoenix.HabboHotel.Achievements;
using Phoenix.Net;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.GameClients
{
	internal sealed class GameClientManager
    {
        #region Fields
		private GameClient[] Session;
		private Hashtable ClientIDs = new Hashtable();
        private Hashtable ClientNames = new Hashtable();
        private Task ConnectionChecker;
		private Timer mConnectionKiller;
		private List<TcpConnection> mToDisconnect;
        #endregion

        public GameClientManager(int Count)
        {
            this.Session = new GameClient[Count];
            this.mToDisconnect = new List<TcpConnection>();
            this.mConnectionKiller = new Timer(new TimerCallback(DisconnectorLoop), null, 500, 500);
        }

        internal void AddGameClient(TcpConnection Client)
        {
            if (!mToDisconnect.Contains(Client))
            {
                mToDisconnect.Add(Client);
            }
        }

        internal void BroadcastMessage(ServerMessage Message)
        {
            byte[] bytes = Message.GetBytes();
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null)
                {
                    try
                    {
                        Session.GetConnection().SendData(bytes);
                    }
                    catch { }
                }
            }
        }

        internal void BroadcastMessage(ServerMessage Message, ServerMessage HotelView)
        {
            byte[] bytes = Message.GetBytes();
            byte[] data = HotelView.GetBytes();
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null)
                {
                    try
                    {
                        if (Session.GetHabbo().InRoom)
                        {
                            Session.GetConnection().SendData(bytes);
                        }
                        else
                        {
                            Session.GetConnection().SendData(data);
                        }
                    }
                    catch { }
                }
            }
        }

        public void BroadcastMessage(ServerMessage Message, string FuseRequirement)
        {
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null)
                {
                    try
                    {
                        if (FuseRequirement.Length <= 0 || (Session.GetHabbo() != null && Session.GetHabbo().HasRole(FuseRequirement)))
                        {
                            Session.SendMessage(Message);
                        }
                    }
                    catch { }
                }
            }
        }

        internal void BroadcastMessageToStaff(ServerMessage Message, ServerMessage HotelView)
        {
            byte[] bytes = Message.GetBytes();
            byte[] data = HotelView.GetBytes();
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null)
                {
                    try
                    {
                        if (Session.GetHabbo().HasRole("receive_sa"))
                        {
                            if (Session.GetHabbo().InRoom)
                            {
                                Session.GetConnection().SendData(bytes);
                            }
                            else
                            {
                                Session.GetConnection().SendData(data);
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        internal void BroadcastNewAchievements()
        {
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null)
                {
                    try
                    {
                        Session.SendMessage(AchievementManager.SerializeAchievementList(Session));
                    }
                    catch { }
                }
            }
        }

        public void CheckEffects()
        {
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null && (Session.GetHabbo() != null && Session.GetHabbo().GetAvatarEffectsInventoryComponent() != null))
                {
                    Session.GetHabbo().GetAvatarEffectsInventoryComponent().CheckExpired();
                }
            }
        }

        public void CheckForAllBanConflicts()
        {
            Dictionary<GameClient, ModerationBanException> BanException = new Dictionary<GameClient, ModerationBanException>();
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null)
                {
                    try
                    {
                        PhoenixEnvironment.GetGame().GetBanManager().CheckForBanConflicts(Session);
                    }
                    catch (ModerationBanException ex)
                    {
                        BanException.Add(Session, ex);
                    }
                }
            }
            foreach (KeyValuePair<GameClient, ModerationBanException> pair in BanException)
            {
                pair.Key.SendBanMessage(pair.Value.Message);
                pair.Key.Disconnect();
            }
        }

        public void CheckPixelUpdates()
        {
            try
            {
                if (this.Session != null)
                {
                    for (int i = 0; i < this.Session.Length; i++)
                    {
                        GameClient Session = this.Session[i];
                        if (Session != null && (Session.GetHabbo() != null && PhoenixEnvironment.GetGame().GetPixelManager().NeedsUpdate(Session)))
                        {
                            PhoenixEnvironment.GetGame().GetPixelManager().GivePixels(Session);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogThreadException(ex.ToString(), "GCMExt.CheckPixelUpdates task");
            }
        }

        public void Clear()
        {
        }

        internal void CloseAll()
        {
            StringBuilder QueryBuilder = new StringBuilder();
            bool RunUpdate = false;
            using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
            {
                for (int i = 0; i < this.Session.Length; i++)
                {
                    GameClient Session = this.Session[i];
                    if (Session != null && Session.GetHabbo() != null)
                    {
                        try
                        {
                            Session.GetHabbo().GetInventoryComponent().RunDBUpdate(adapter, true);
                            QueryBuilder.Append(Session.GetHabbo().GetQueryString);
                            RunUpdate = true;
                        }
                        catch { }
                    }
                }
                if (RunUpdate)
                {
                    try
                    {
                        adapter.ExecuteQuery(QueryBuilder.ToString());
                    }
                    catch (Exception ex)
                    {
                        Logging.HandleException(ex.ToString());
                    }
                }
            }
            Console.WriteLine("Done saving users inventory!");
            Console.WriteLine("Closing server connections...");
            try
            {
                for (int i = 0; i < this.Session.Length; i++)
                {
                    GameClient Session = this.Session[i];
                    if (Session != null && Session.GetConnection() != null)
                    {
                        try
                        {
                            Session.GetConnection().Dispose();
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.HandleException(ex.ToString());
            }
            Array.Clear(this.Session, 0, this.Session.Length);
            Console.WriteLine("Connections closed!");
        }

        internal void CreateAndStartClient(uint ClientId, ref TcpConnection Connection)
        {
            this.Session[ClientId] = new GameClient(ClientId, ref Connection);
            this.Session[ClientId].StartConnection();
        }

        private void DisconnectorLoop(object state)
        {
            try
            {
                List<TcpConnection> mToDisconnect = this.mToDisconnect;
                this.mToDisconnect = new List<TcpConnection>();
                if (mToDisconnect != null)
                {
                    foreach (TcpConnection connection in mToDisconnect)
                    {
                        if (connection != null)
                        {
                            connection.ConnectionDead();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogThreadException(ex.ToString(), "Disconnector task");
            }
        }

        internal List<ServerMessage> GenerateUsersOnlineList()
        {
            List<ServerMessage> list = new List<ServerMessage>();
            int num = 0;
            ServerMessage Message = new ServerMessage();
            Message.Init(161);
            Message.AppendStringWithBreak("Users online:\r");
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null && Session.GetHabbo() != null)
                {
                    if (num > 20)
                    {
                        list.Add(Message);
                        num = 0;
                        Message = new ServerMessage();
                        Message.Init(161);
                    }
                    num++;
                    Message.AppendStringWithBreak(string.Concat(new object[] {	Session.GetHabbo().Username, " {", Session.GetHabbo().Rank, "}\r" }));
                }
            }
            list.Add(Message);
            return list;
        }

        public GameClient GetClient(uint ClientId)
        {
            try
            {
                return Session[ClientId];
            }
            catch
            {
                return null;
            }
        }

        public GameClient GetClientByHabbo(uint HabboId)
        {
            if (((this.Session != null) && (this.ClientIDs != null)) && this.ClientIDs.ContainsKey(HabboId))
            {
                return (GameClient)this.ClientIDs[HabboId];
            }
            return null;
        }

        public GameClient GetClientByHabbo(string Name)
        {
            if (((this.Session != null) && (this.ClientNames != null)) && this.ClientNames.ContainsKey(Name.ToLower()))
            {
                return (GameClient)this.ClientNames[Name.ToLower()];
            }
            return null;
        }

        public uint GetIdByName(string Name)
        {
            GameClient clientByHabbo = this.GetClientByHabbo(Name);
            if (clientByHabbo != null)
            {
                return clientByHabbo.GetHabbo().Id;
            }
            else
            {
                DataRow dataRow = null;
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    dataRow = adapter.ReadDataRow("SELECT Id FROM users WHERE username = '" + Name + "' LIMIT 1");
                }
                if (dataRow == null)
                {
                    return 0;
                }
                else
                {
                    return (uint)dataRow[0];
                }
            }
        }

        public string GetNameById(uint Id)
        {
            GameClient clientByHabbo = this.GetClientByHabbo(Id);
            if (clientByHabbo != null)
            {
                return clientByHabbo.GetHabbo().Username;
            }
            else
            {
                DataRow Row = null;
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    Row = adapter.ReadDataRow("SELECT username FROM users WHERE Id = '" + Id + "' LIMIT 1");
                }
                if (Row == null)
                {
                    return "Unknown User";
                }
                else
                {
                    return (string)Row[0];
                }
            }
        }

        internal void GiveMassBadge(string pBadge)
        {
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null && Session.GetHabbo() != null)
                {
                    try
                    {
                        Session.GetHabbo().GetBadgeComponent().GiveBadge(Session, pBadge, true);
                        Session.SendNotif("You just received a badge from hotel staff!");
                    }
                    catch { }
                }
            }
        }

        internal void GiveCredits(int Amount)
        {
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null && Session.GetHabbo() != null)
                {
                    try
                    {
                        Session.GetHabbo().Credits += Amount;
                        Session.GetHabbo().UpdateCreditsBalance(true);
                        Session.SendNotif("You just received " + Amount + " credits from staff!");
                    }
                    catch { }
                }
            }
        }

        internal void GivePixels(int Amount, bool indb)
        {
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null && Session.GetHabbo() != null)
                {
                    try
                    {
                        Session.GetHabbo().ActivityPoints += Amount;
                        Session.GetHabbo().UpdateActivityPointsBalance(indb);
                        Session.SendNotif("You just received " + Amount + " pixels from staff!");
                    }
                    catch { }
                }
            }
        }

        internal void GivePoints(int Amount, bool UpdateDB)
        {
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null && Session.GetHabbo() != null)
                {
                    try
                    {
                        Session.GetHabbo().shells += Amount;
                        Session.GetHabbo().UpdateShellsBalance(false, UpdateDB);
                        Session.SendNotif("You just received " + Amount + " points from staff!");
                    }
                    catch { }
                }
            }
        }

        public void LogClonesOut(uint UserID)
        {
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient Session = this.Session[i];
                if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().Id == UserID)
                {
                    Session.Disconnect();
                }
            }
        }

        public void NullClientShit(uint ID, string Username)
        {
            this.ClientIDs[ID] = null;
            this.ClientNames[Username.ToLower()] = null;
        }

        internal void RecordCmdLogs(GameClient Session, string Command, string ExtraData)
        {
            if (GlobalClass.RecordCmdlogs)
            {
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.AddParamWithValue("extra_data", ExtraData);
                    adapter.ExecuteQuery("INSERT INTO cmdlogs (user_id,user_name,command,extra_data,timestamp) VALUES ('" + Session.GetHabbo().Id + "','" + Session.GetHabbo().Username + "','" + Command + "', @extra_data, UNIX_TIMESTAMP())");
                }
            }
        }

        public void RegisterClientShit(uint ID, string Username, GameClient Session)
        {
            this.ClientIDs[ID] = Session;
            this.ClientNames[Username.ToLower()] = Session;
        }

        internal void SendStaffChat(GameClient Session, ServerMessage Message)
        {
            byte[] bytes = Message.GetBytes();
            for (int i = 0; i < this.Session.Length; i++)
            {
                GameClient session = this.Session[i];
                if (session != null && session != Session)
                {
                    try
                    {
                        if (session.GetHabbo().HasRole("receive_sa"))
                        {
                            session.GetConnection().SendData(bytes);
                        }
                    }
                    catch { }
                }
            }
        }

        public void StartConnectionChecker()
        {
            if (ConnectionChecker == null)
            {
                this.ConnectionChecker = new Task(new Action(TestClientConnections));
                this.ConnectionChecker.Start();
            }
        }

        public void StopClient(uint ClientId)
        {
            GameClient Session = this.GetClient(ClientId);
            if (Session != null)
            {
                PhoenixEnvironment.GetConnectionManager().DropConnection(ClientId);
                Session.Stop();
                this.Session[ClientId] = null;
            }
        }

        public void StopConnectionChecker()
        {
            if (ConnectionChecker != null)
            {
                this.ConnectionChecker = null;
            }
        }

        private void TestClientConnections()
        {
            int millisecondsTimeout = int.Parse(PhoenixEnvironment.GetConfig().data["client.ping.interval"]);
            if (millisecondsTimeout <= 100)
            {
                throw new ArgumentException("Invalid configuration value for ping interval! Must be above 100 miliseconds.");
            }
            while (true)
            {
                try
                {
                    ServerMessage Message = new ServerMessage(50);
                    List<GameClient> SuccessTest = new List<GameClient>();
                    List<GameClient> FailTest = new List<GameClient>();
                    for (int i = 0; i < this.Session.Length; i++)
                    {
                        GameClient Session = this.Session[i];
                        if (Session != null)
                        {
                            if (Session.PongOK)
                            {
                                Session.PongOK = false;
                                FailTest.Add(Session);
                            }
                            else
                            {
                                SuccessTest.Add(Session);
                            }
                        }
                    }
                    foreach (GameClient Session in SuccessTest)
                    {
                        try
                        {
                            Session.Disconnect();
                        }
                        catch { }
                    }
                    byte[] bytes = Message.GetBytes();
                    foreach (GameClient Session in FailTest)
                    {
                        try
                        {
                            Session.GetConnection().SendData(bytes);
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogThreadException(ex.ToString(), "Connection checker task");
                }
                Thread.Sleep(millisecondsTimeout);
            }
        }

        public int ClientCount
        {
            get
            {
                if (Session == null)
                {
                    return 0;
                }
                int clients = 0;
                for (int i = 0; i < Session.Length; i++)
                {
                    if (Session[i] != null && Session[i].GetHabbo() != null && !string.IsNullOrEmpty(Session[i].GetHabbo().Username))
                    {
                        clients++;
                    }
                }
                clients++;
                return clients;
            }
        }
	}
}
