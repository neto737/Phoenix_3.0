using System;
using System.Data;
using System.Text.RegularExpressions;
using Phoenix.Core;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.Support;
using Phoenix.Messages;
using Phoenix.Util;
using Phoenix.HabboHotel.Users;
using Phoenix.Net;
using Phoenix.HabboHotel.Users.Authenticator;
using Phoenix.Storage;
using Phoenix.Communication;
namespace Phoenix.HabboHotel.GameClients
{
    internal class GameClient
    {
        private uint Id;
        private TcpConnection Connection;
        private GameClientMessageHandler MessageHandler;
        private Habbo Habbo;
        public Boolean PongOK;
        //internal bool bool_1 = false;
        private bool Disconnected = false;

        public GameClient(uint ClientId, ref TcpConnection pConnection)
        {
            this.Id = ClientId;
            this.Connection = pConnection;
        }

        public void Disconnect()
        {
            if (!Disconnected)
            {
                PhoenixEnvironment.GetGame().GetClientManager().StopClient(Id);
                Disconnected = true;
            }
        }

        public TcpConnection GetConnection()
        {
            return Connection;
        }

        public GameClientMessageHandler GetMessageHandler()
        {
            return MessageHandler;
        }

        public Habbo GetHabbo()
        {
            return Habbo;
        }

        internal ServerMessage GenerateUsersRoomNaviPacket()
        {
            return PhoenixEnvironment.GetGame().GetNavigator().GetDynamicNavigatorPacket(this, -3);
        }

        public void HandleConnectionData(ref byte[] data)
        {
            if (data[0] == 64)
            {
                int pos = 0;
                while (pos < data.Length)
                {
                    try
                    {
                        int MessageLength = Base64Encoding.DecodeInt32(new byte[] { data[pos++], data[pos++], data[pos++] });
                        uint MessageId = Base64Encoding.DecodeUInt32(new byte[] { data[pos++], data[pos++] });

                        byte[] Content = new byte[MessageLength - 2];
                        for (int j = 0; j < Content.Length; j++)
                        {
                            Content[j] = data[pos++];
                        }
                        if (this.MessageHandler == null)
                        {
                            this.InitHandler();
                        }
                        ClientMessage Message = new ClientMessage(MessageId, Content);
                        if (Message != null)
                        {
                            try
                            {
                                if (int.Parse(PhoenixEnvironment.GetConfig().data["debug"]) == 1)
                                {
                                    Logging.WriteLine(string.Concat(new object[] { "[", ClientId, "] --> [", Message.Id, "] ", Message.Header, Message.GetBody() }));
                                }
                            }
                            catch
                            {
                            }
                            MessageEvent MessageHandler;
                            if (PhoenixEnvironment.GetPacketManager().Get(Message.Id, out MessageHandler))
                            {
                                MessageHandler.parse(this, Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogException("Error: " + ex.ToString());
                        this.Disconnect();
                    }
                }
            }
            else
            {
                if (true)
                {
                    this.Connection.SendData(CrossdomainPolicy.GetXmlPolicy());
                    this.Connection.Dispose();
                }
            }
        }

        internal void InitHandler()
        {
            MessageHandler = new GameClientMessageHandler(this);
        }

        internal void Login(string AuthTicket)
        {
            try
            {
                HabboData pData = new HabboData(AuthTicket, this.GetConnection().ipAddress, true);
                if (this.GetConnection().ipAddress == "127.0.0.1" && !pData.UserFound)
                {
                    pData = new HabboData(AuthTicket, "::1", true);
                }
                if (!pData.UserFound)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    string str = "";
                    if (GlobalClass.SecureSessions)
                    {
                        str = TextManager.GetText("emu_sso_wrong_secure") + "(" + this.GetConnection().ipAddress + ")";
                    }
                    ServerMessage Message = new ServerMessage(161);
                    Message.AppendStringWithBreak(TextManager.GetText("emu_sso_wrong") + str);
                    this.GetConnection().SendMessage(Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    this.Disconnect();
                    return;
                }
                Habbo habbo = Authenticator.TryLoginHabbo(AuthTicket, this, pData, pData);
                PhoenixEnvironment.GetGame().GetClientManager().LogClonesOut(habbo.Id);
                this.Habbo = habbo;
                this.Habbo.LoadData(pData);
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.ReadString("SELECT ip_last FROM users WHERE Id = " + GetHabbo().Id + " LIMIT 1;");
                }
                this.Habbo.isAaron = false;
                if (Habbo.isAaron)
                {
                    Habbo.Rank = (uint)PhoenixEnvironment.GetGame().GetRoleManager().RankCount();
                    Habbo.Vip = true;
                }
            }
            catch (Exception ex)
            {
                this.SendNotif("Login error: " + ex.Message);
                this.Disconnect();
                return;
            }
            try
            {
                PhoenixEnvironment.GetGame().GetBanManager().CheckForBanConflicts(this);
            }
            catch (ModerationBanException gException)
            {
                this.SendBanMessage(gException.Message);
                this.Disconnect();
                return;
            }
            ServerMessage Message2 = new ServerMessage(2);
            if (this.GetHabbo().Vip || GlobalClass.VIPclothesforHCusers)
            {
                Message2.AppendInt32(2);
            }
            else if (this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                Message2.AppendInt32(1);
            }
            else
            {
                Message2.AppendInt32(0);
            }
            if (this.GetHabbo().HasRole("acc_anyroomowner"))
            {
                Message2.AppendInt32(7);
            }
            else if (this.GetHabbo().HasRole("acc_anyroomrights"))
            {
                Message2.AppendInt32(5);
            }
            else if (this.GetHabbo().HasRole("acc_supporttool"))
            {
                Message2.AppendInt32(4);
            }
            else if (this.GetHabbo().Vip || GlobalClass.VIPclothesforHCusers || this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                Message2.AppendInt32(2);
            }
            else
            {
                Message2.AppendInt32(0);
            }
            this.SendMessage(Message2);

            this.SendMessage(this.GetHabbo().GetAvatarEffectsInventoryComponent().Serialize());

            ServerMessage Message3 = new ServerMessage(290);
            Message3.AppendBoolean(true);
            Message3.AppendBoolean(false);
            this.SendMessage(Message3);

            ServerMessage message4 = new ServerMessage(3);
            this.SendMessage(message4);

            if (this.GetHabbo().HasRole("acc_supporttool"))
            {
                this.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeTool());
                PhoenixEnvironment.GetGame().GetModerationTool().SendOpenTickets(this);
            }

            ServerMessage Logging = new ServerMessage(517);
            Logging.AppendBoolean(true);
            this.SendMessage(Logging);
            if (PhoenixEnvironment.GetGame().GetPixelManager().NeedsUpdate(this))
            {
                PhoenixEnvironment.GetGame().GetPixelManager().GivePixels(this);
            }
            ServerMessage Message5 = new ServerMessage(455);
            Message5.AppendUInt(this.GetHabbo().HomeRoom);
            this.SendMessage(Message5);
            ServerMessage Message6 = new ServerMessage(458);
            Message6.AppendInt32(30);
            Message6.AppendInt32(this.GetHabbo().FavoriteRooms.Count);
            foreach (uint current in this.GetHabbo().FavoriteRooms)
            {
                Message6.AppendUInt(current);
            }
            this.SendMessage(Message6);
            if (this.GetHabbo().Stat_OnlineTime > 8294400)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 10);
            }
            else if (this.GetHabbo().Stat_OnlineTime > 4147200)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 9);
            }
            else if (this.GetHabbo().Stat_OnlineTime > 2073600)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 8);
            }
            else if (this.GetHabbo().Stat_OnlineTime > 1036800)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 7);
            }
            else if (this.GetHabbo().Stat_OnlineTime > 518400)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 6);
            }
            else if (this.GetHabbo().Stat_OnlineTime > 172800)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 5);
            }
            else if (this.GetHabbo().Stat_OnlineTime > 57600)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 4);
            }
            else if (this.GetHabbo().Stat_OnlineTime > 28800)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 3);
            }
            else if (this.GetHabbo().Stat_OnlineTime > 10800)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 2);
            }
            else if (this.GetHabbo().Stat_OnlineTime > 3600)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16, 1);
            }
            if (GlobalClass.Motd != "")
            {
                this.SendNotif(GlobalClass.Motd, 2);
            }
            for (uint i = (uint)PhoenixEnvironment.GetGame().GetRoleManager().RankCount(); i > 1; i -= 1)
            {
                if (PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(i).Length > 0)
                {
                    if (!GetHabbo().GetBadgeComponent().HasBadge(PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(i)) && GetHabbo().Rank == i)
                    {
                        GetHabbo().GetBadgeComponent().GiveBadge(this, PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(i), true);
                    }
                    else if (GetHabbo().GetBadgeComponent().HasBadge(PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(i)) && GetHabbo().Rank < i)
                    {
                        GetHabbo().GetBadgeComponent().RemoveBadge(PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(i));
                    }
                }
            }
            if (GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") && !GetHabbo().GetBadgeComponent().HasBadge("HC1"))
            {
                GetHabbo().GetBadgeComponent().GiveBadge(this, "HC1", true);
            }
            else
            {
                if (!GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") && GetHabbo().GetBadgeComponent().HasBadge("HC1"))
                {
                    GetHabbo().GetBadgeComponent().RemoveBadge("HC1");
                }
            }
            if (GetHabbo().Vip && !GetHabbo().GetBadgeComponent().HasBadge("VIP"))
            {
                this.GetHabbo().GetBadgeComponent().GiveBadge(this, "VIP", true);
            }
            else
            {
                if (!GetHabbo().Vip && GetHabbo().GetBadgeComponent().HasBadge("VIP"))
                {
                    GetHabbo().GetBadgeComponent().RemoveBadge("VIP");
                }
            }
            if (GetHabbo().CurrentQuestId > 0)
            {
                PhoenixEnvironment.GetGame().GetQuestManager().HandleQuest(GetHabbo().CurrentQuestId, this);
            }
            if (!Regex.IsMatch(this.GetHabbo().Username, "^[-a-zA-Z0-9._:,]+$"))
            {
                ServerMessage message8 = new ServerMessage(573);
                this.SendMessage(message8);
            }
            this.GetHabbo().Motto = PhoenixEnvironment.FilterInjectionChars(this.GetHabbo().Motto);
            DataTable table = null;
            using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
            {
                table = adapter.ReadDataTable("SELECT achievement,achlevel FROM achievements_owed WHERE user = '" + GetHabbo().Id + "'");
            }
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, (uint)row["achievement"], (int)row["achlevel"]);
                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        adapter.ExecuteQuery(string.Concat(new object[]
						{
							"DELETE FROM achievements_owed WHERE achievement = '",
							(uint)row["achievement"],
							"' AND user = '",
							this.GetHabbo().Id,
							"' LIMIT 1"
						}));
                    }
                }
            }
        }

        public void SendBanMessage(string Message)
        {
            ServerMessage message = new ServerMessage(35);
            message.AppendStringWithBreak("A moderator has kicked you from the hotel:", 13);
            message.AppendStringWithBreak(Message);
            this.SendMessage(message);
        }

        public void SendMessage(ServerMessage Message)
        {
            if (Message != null && GetConnection() != null)
            {
                GetConnection().SendMessage(Message);
            }
        }

        public void SendNotif(string Message)
        {
            this.SendNotif(Message, 0);
        }

        public void SendNotif(string Message, int Type)
        {
            ServerMessage nMessage = new ServerMessage();
            switch (Type)
            {
                case 0:
                    nMessage.Init(161);
                    break;
                case 1:
                    nMessage.Init(139);
                    break;
                case 2:
                    nMessage.Init(810);
                    nMessage.AppendUInt(1);
                    break;
                default:
                    nMessage.Init(161);
                    break;
            }
            nMessage.AppendStringWithBreak(Message);
            this.GetConnection().SendMessage(nMessage);
        }

        public void SendNotif(string Message, string Url)
        {
            ServerMessage message = new ServerMessage(161);
            message.AppendStringWithBreak(Message);
            message.AppendStringWithBreak(Url);
            this.GetConnection().SendMessage(message);
        }

        public void StartConnection()
        {
            if (this.Connection != null)
            {
                this.PongOK = true;
                TcpConnection.RouteReceivedDataCallback dataRouter = new TcpConnection.RouteReceivedDataCallback(this.HandleConnectionData);
                this.Connection.Start(dataRouter);
            }
        }

        public void Stop()
        {
            if (this.Connection != null)
            {
                this.Connection.Dispose();
                this.Connection = null;
            }
            if (this.GetHabbo() != null)
            {
                this.Habbo.OnDisconnect();
                this.Habbo = null;
            }
            if (this.GetMessageHandler() != null)
            {
                this.MessageHandler.Destroy();
                this.MessageHandler = null;
            }
        }

        public uint ClientId
        {
            get
            {
                return this.Id;
            }
        }

        public bool LoggedIn
        {
            get
            {
                if (Habbo == null)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
