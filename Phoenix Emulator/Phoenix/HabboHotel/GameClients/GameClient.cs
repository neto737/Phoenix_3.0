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
        public bool PongOK;
        internal bool bool_1 = false;
        private bool bool_2 = false;
        public uint UInt32_0
        {
            get
            {
                return this.Id;
            }
        }
        public bool Boolean_0
        {
            get
            {
                return this.Habbo != null;
            }
        }
        public GameClient(uint ClientId, ref TcpConnection pConnection)
        {
            this.Id = ClientId;
            this.Connection = pConnection;
        }
        public TcpConnection GetConnection()
        {
            return this.Connection;
        }
        public GameClientMessageHandler GetMessageHandler()
        {
            return this.MessageHandler;
        }
        public Habbo GetHabbo()
        {
            return this.Habbo;
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
        internal void InitHandler()
        {
            this.MessageHandler = new GameClientMessageHandler(this);
        }
        internal ServerMessage GenerateUsersRoomNaviPacket()
        {
            return PhoenixEnvironment.GetGame().GetNavigator().GetDynamicNavigatorPacket(this, -3);
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
                string a;
                using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    a = class3.ReadString("SELECT ip_last FROM users WHERE Id = " + this.GetHabbo().Id + " LIMIT 1;");
                }
                this.Habbo.isAaron = false;//(this.GetConnection().String_0 == Phoenix.string_5 || a == Phoenix.string_5);
                if (this.Habbo.isAaron)
                {
                    this.Habbo.Rank = (uint)PhoenixEnvironment.GetGame().GetRoleManager().RankCount();
                    this.Habbo.Vip = true;
                }
            }
            catch (Exception ex)
            {
                this.SendNotif("Login error: " + ex.Message);
                //Logging.LogException(ex.ToString());
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
            ServerMessage Message2 = new ServerMessage(2u);
            if (this.GetHabbo().Vip || GlobalClass.VIPclothesforHCusers)
            {
                Message2.AppendInt32(2);
            }
            else
            {
                if (this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                {
                    Message2.AppendInt32(1);
                }
                else
                {
                    Message2.AppendInt32(0);
                }
            }
            if (this.GetHabbo().HasRole("acc_anyroomowner"))
            {
                Message2.AppendInt32(7);
            }
            else
            {
                if (this.GetHabbo().HasRole("acc_anyroomrights"))
                {
                    Message2.AppendInt32(5);
                }
                else
                {
                    if (this.GetHabbo().HasRole("acc_supporttool"))
                    {
                        Message2.AppendInt32(4);
                    }
                    else
                    {
                        if (this.GetHabbo().Vip || GlobalClass.VIPclothesforHCusers || this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                        {
                            Message2.AppendInt32(2);
                        }
                        else
                        {
                            Message2.AppendInt32(0);
                        }
                    }
                }
            }
            this.SendMessage(Message2);

            this.SendMessage(this.GetHabbo().GetAvatarEffectsInventoryComponent().Serialize());

            ServerMessage Message3 = new ServerMessage(290u);
            Message3.AppendBoolean(true);
            Message3.AppendBoolean(false);
            this.SendMessage(Message3);

            ServerMessage Message5_ = new ServerMessage(3u);
            this.SendMessage(Message5_);

            if (this.GetHabbo().HasRole("acc_supporttool"))
            {
                this.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeTool());
                PhoenixEnvironment.GetGame().GetModerationTool().SendOpenTickets(this);
            }


            ServerMessage Logging = new ServerMessage(517u);
            Logging.AppendBoolean(true);
            this.SendMessage(Logging);
            if (PhoenixEnvironment.GetGame().GetPixelManager().NeedsUpdate(this))
            {
                PhoenixEnvironment.GetGame().GetPixelManager().GivePixels(this);
            }
            ServerMessage Message5 = new ServerMessage(455u);
            Message5.AppendUInt(this.GetHabbo().uint_4);
            this.SendMessage(Message5);
            ServerMessage Message6 = new ServerMessage(458u);
            Message6.AppendInt32(30);
            Message6.AppendInt32(this.GetHabbo().FavoriteRooms.Count);
            foreach (uint current in this.GetHabbo().FavoriteRooms)
            {
                Message6.AppendUInt(current);
            }
            this.SendMessage(Message6);
            if (this.GetHabbo().Stat_OnlineTime > 8294400)
            {
                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 10);
            }
            else
            {
                if (this.GetHabbo().Stat_OnlineTime > 4147200)
                {
                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 9);
                }
                else
                {
                    if (this.GetHabbo().Stat_OnlineTime > 2073600)
                    {
                        PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 8);
                    }
                    else
                    {
                        if (this.GetHabbo().Stat_OnlineTime > 1036800)
                        {
                            PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 7);
                        }
                        else
                        {
                            if (this.GetHabbo().Stat_OnlineTime > 518400)
                            {
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 6);
                            }
                            else
                            {
                                if (this.GetHabbo().Stat_OnlineTime > 172800)
                                {
                                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 5);
                                }
                                else
                                {
                                    if (this.GetHabbo().Stat_OnlineTime > 57600)
                                    {
                                        PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 4);
                                    }
                                    else
                                    {
                                        if (this.GetHabbo().Stat_OnlineTime > 28800)
                                        {
                                            PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 3);
                                        }
                                        else
                                        {
                                            if (this.GetHabbo().Stat_OnlineTime > 10800)
                                            {
                                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 2);
                                            }
                                            else
                                            {
                                                if (this.GetHabbo().Stat_OnlineTime > 3600)
                                                {
                                                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, 16u, 1);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (GlobalClass.Motd != "")
            {
                this.SendNotif(GlobalClass.Motd, 2);
            }
            for (uint num = (uint)PhoenixEnvironment.GetGame().GetRoleManager().RankCount(); num > 1u; num -= 1u)
            {
                if (PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(num).Length > 0)
                {
                    if (!this.GetHabbo().GetBadgeComponent().HasBadge(PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(num)) && this.GetHabbo().Rank == num)
                    {
                        this.GetHabbo().GetBadgeComponent().GiveBadge(this, PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(num), true);
                    }
                    else
                    {
                        if (this.GetHabbo().GetBadgeComponent().HasBadge(PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(num)) && this.GetHabbo().Rank < num)
                        {
                            this.GetHabbo().GetBadgeComponent().RemoveBadge(PhoenixEnvironment.GetGame().GetRoleManager().RanksBadge(num));
                        }
                    }
                }
            }
            if (this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") && !this.GetHabbo().GetBadgeComponent().HasBadge("HC1"))
            {
                this.GetHabbo().GetBadgeComponent().GiveBadge(this, "HC1", true);
            }
            else
            {
                if (!this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") && this.GetHabbo().GetBadgeComponent().HasBadge("HC1"))
                {
                    this.GetHabbo().GetBadgeComponent().RemoveBadge("HC1");
                }
            }
            if (this.GetHabbo().Vip && !this.GetHabbo().GetBadgeComponent().HasBadge("VIP"))
            {
                this.GetHabbo().GetBadgeComponent().GiveBadge(this, "VIP", true);
            }
            else
            {
                if (!this.GetHabbo().Vip && this.GetHabbo().GetBadgeComponent().HasBadge("VIP"))
                {
                    this.GetHabbo().GetBadgeComponent().RemoveBadge("VIP");
                }
            }
            if (this.GetHabbo().CurrentQuestId > 0u)
            {
                PhoenixEnvironment.GetGame().GetQuestManager().HandleQuest(this.GetHabbo().CurrentQuestId, this);
            }
            if (!Regex.IsMatch(this.GetHabbo().Username, "^[-a-zA-Z0-9._:,]+$"))
            {
                ServerMessage Message5_2 = new ServerMessage(573u);
                this.SendMessage(Message5_2);
            }
            this.GetHabbo().Motto = PhoenixEnvironment.FilterInjectionChars(this.GetHabbo().Motto);
            DataTable dataTable = null;
            using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
            {
                dataTable = class3.ReadDataTable("SELECT achievement,achlevel FROM achievements_owed WHERE user = '" + this.GetHabbo().Id + "'");
            }
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(this, (uint)dataRow["achievement"], (int)dataRow["achlevel"]);
                    using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        class3.ExecuteQuery(string.Concat(new object[]
						{
							"DELETE FROM achievements_owed WHERE achievement = '",
							(uint)dataRow["achievement"],
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
            ServerMessage message = new ServerMessage(35u);
            message.AppendStringWithBreak("A moderator has kicked you from the hotel:", 13);
            message.AppendStringWithBreak(Message);
            this.SendMessage(message);
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
                    nMessage.Init(161u);
                    break;
                case 1:
                    nMessage.Init(139u);
                    break;
                case 2:
                    nMessage.Init(810u);
                    nMessage.AppendUInt(1u);
                    break;
                default:
                    nMessage.Init(161u);
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
        public void Disconnect()
        {
            if (!this.bool_2)
            {
                PhoenixEnvironment.GetGame().GetClientManager().StopClient(this.Id);
                this.bool_2 = true;
            }
        }
        public void HandleConnectionData(ref byte[] byte_0)
        {
            if (byte_0[0] == 64)
            {
                int i = 0;
                while (i < byte_0.Length)
                {
                    try
                    {
                        int num = Base64Encoding.DecodeInt32(new byte[]
						{
							byte_0[i++],
							byte_0[i++],
							byte_0[i++]
						});
                        uint uint_ = Base64Encoding.DecodeUInt32(new byte[]
						{
							byte_0[i++],
							byte_0[i++]
						});
                        byte[] array = new byte[num - 2];
                        for (int j = 0; j < array.Length; j++)
                        {
                            array[j] = byte_0[i++];
                        }
                        if (this.MessageHandler == null)
                        {
                            this.InitHandler();
                        }
                        ClientMessage @class = new ClientMessage(uint_, array);
                        if (@class != null)
                        {
                            try
                            {
                                if (int.Parse(PhoenixEnvironment.GetConfig().data["debug"]) == 1)
                                {
                                    Logging.WriteLine(string.Concat(new object[]
									{
										"[",
										this.UInt32_0,
										"] --> [",
										@class.Id,
										"] ",
										@class.Header,
										@class.GetBody()
									}));
                                }
                            }
                            catch
                            {
                            }
                            MessageEvent Message;
                            if (PhoenixEnvironment.GetPacketManager().Get(@class.Id, out Message))
                            {
                                Message.parse(this, @class);
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
                if (true)//Class13.Boolean_7)
                {
                    this.Connection.SendData(CrossdomainPolicy.GetXmlPolicy());
                    this.Connection.Dispose();
                }
            }
        }
        public void SendMessage(ServerMessage Message5_0)
        {
            if (Message5_0 != null && this.GetConnection() != null)
            {
                this.GetConnection().SendMessage(Message5_0);
            }
        }
    }
}
