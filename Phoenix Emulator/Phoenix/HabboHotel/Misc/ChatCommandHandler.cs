using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Achievements;
using Phoenix.HabboHotel.Users;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.HabboHotel.Users.Authenticator;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
using System.Net;
namespace Phoenix.HabboHotel.Misc
{
    internal class ChatCommandHandler
    {
        private static List<string> BadWords;
        private static List<string> BadReplacement;
        private static List<bool> BadStrict;
        private static List<string> ExternalLinks;

        public static void InitFilter(DatabaseClient dbClient)
        {
            Logging.Write("Loading Chat Filter..");
            BadWords = new List<string>();
            BadReplacement = new List<string>();
            BadStrict = new List<bool>();
            ExternalLinks = new List<string>();
            UpdateFilters(dbClient);
            Logging.WriteLine("completed!");
        }

        #region Filters

        public static void UpdateFilters(DatabaseClient dbClient)
        {
            ChatCommandHandler.BadWords.Clear();
            ChatCommandHandler.BadReplacement.Clear();
            ChatCommandHandler.BadStrict.Clear();
            ChatCommandHandler.ExternalLinks.Clear();
            DataTable Table = dbClient.ReadDataTable("SELECT * FROM wordfilter ORDER BY word ASC;");
            if (Table != null)
            {
                foreach (DataRow Row in Table.Rows)
                {
                    ChatCommandHandler.BadWords.Add(Row["word"].ToString());
                    ChatCommandHandler.BadReplacement.Add(Row["replacement"].ToString());
                    ChatCommandHandler.BadStrict.Add(PhoenixEnvironment.EnumToBool(Row["strict"].ToString()));
                }
            }
            DataTable Table2 = dbClient.ReadDataTable("SELECT * FROM linkfilter;");
            if (Table2 != null)
            {
                foreach (DataRow Row in Table2.Rows)
                {
                    ChatCommandHandler.ExternalLinks.Add(Row["externalsite"].ToString());
                }
            }
        }

        public static bool CheckExternalLink(string Website)
        {
            if (GlobalClass.ExternalLinkMode == "disabled")
            {
                return false;
            }
            else if ((Website.StartsWith("http://") || Website.StartsWith("www.") || Website.StartsWith("https://")) && ChatCommandHandler.ExternalLinks != null && ChatCommandHandler.ExternalLinks.Count > 0)
            {
                foreach (string current in ChatCommandHandler.ExternalLinks)
                {
                    if (Website.Contains(current))
                    {
                        if (GlobalClass.ExternalLinkMode == "whitelist")
                        {
                            return true;
                        }
                        if (!(GlobalClass.ExternalLinkMode == "blacklist"))
                        {
                        }
                    }
                }
            }
            return (Website.StartsWith("http://") || Website.StartsWith("www.") || (Website.StartsWith("https://") && GlobalClass.ExternalLinkMode == "blacklist") || (GlobalClass.ExternalLinkMode == "whitelist" && false));
        }

        public static string ApplyAdfly(string Input)
        {
            return Input;
        }

        public static string ApplyWordFilter(string Input)
        {
            if ((BadWords != null) && (BadWords.Count > 0))
            {
                int num = -1;
                foreach (string str in BadWords)
                {
                    num++;
                    if (Input.ToLower().Contains(str.ToLower()) && BadStrict[num])
                    {
                        Input = Regex.Replace(Input, str, BadReplacement[num], RegexOptions.IgnoreCase);
                    }
                    else if (Input.ToLower().Contains(" " + str.ToLower() + " "))
                    {
                        Input = Regex.Replace(Input, str, BadReplacement[num], RegexOptions.IgnoreCase);
                    }
                }
            }
            return Input;
        }

        #endregion

        internal static Boolean Parse(GameClient Session, string Input)
        {
            string[] Params = Input.Split(' ');

            string TargetUser = null;
            GameClient TargetClient = null;
            Room TargetRoom = null;
            RoomUser TargetRoomUser = null;
            Habbo TargetHabbo = null;
            if (!PhoenixEnvironment.GetGame().GetRoleManager().CommandsList.ContainsKey(Params[0]))
            {
                return false;
            }
            else
            {
                try
                {
                    int num;
                    if (TargetRoom != null && TargetRoom.CheckRights(Session, true))
                    {
                        num = PhoenixEnvironment.GetGame().GetRoleManager().CommandsList[Params[0]];
                        if (num <= 33)
                        {
                            if (num == 8)
                            {
                                TargetRoom = Session.GetHabbo().CurrentRoom;
                                if (TargetRoom.bool_5)
                                {
                                    TargetRoom.bool_5 = false;
                                }
                                else
                                {
                                    TargetRoom.bool_5 = true;
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (num == 33)
                            {
                                TargetRoom = Session.GetHabbo().CurrentRoom;
                                if (TargetRoom != null && TargetRoom.CheckRights(Session, true))
                                {
                                    List<RoomItem> list = TargetRoom.method_24(Session);
                                    Session.GetHabbo().GetInventoryComponent().method_17(list);
                                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                                    PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input + " " + Session.GetHabbo().CurrentRoomId);
                                    return true;
                                }
                                return false;
                            }
                        }
                        else
                        {
                            if (num == 46)
                            {
                                TargetRoom = Session.GetHabbo().CurrentRoom;
                                try
                                {
                                    int num2 = int.Parse(Params[1]);
                                    if (Session.GetHabbo().Rank >= 6)
                                    {
                                        TargetRoom.UsersMax = num2;
                                    }
                                    else
                                    {
                                        if (num2 > 100 || num2 < 5)
                                        {
                                            Session.SendNotif("ERROR: Use a number between 5 and 100");
                                        }
                                        else
                                        {
                                            TargetRoom.UsersMax = num2;
                                        }
                                    }
                                }
                                catch
                                {
                                    return false;
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (num == 53)
                            {
                                TargetRoom = Session.GetHabbo().CurrentRoom;
                                PhoenixEnvironment.GetGame().GetRoomManager().UnloadRoom(TargetRoom);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        }
                    }
                    switch (PhoenixEnvironment.GetGame().GetRoleManager().CommandsList[Params[0]])
                    {
                        #region Moderation Commands
                        #region CMD Alert
                        case 2: //CMD Alert
                            if (!Session.GetHabbo().HasRole("cmd_alert"))
                            {
                                return false;
                            }

                            TargetUser = Params[1];
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("Could not find user: " + TargetUser);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            TargetClient.SendNotif(MergeParams(Params, 2), 0);
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Award
                        case 3: //CMD Award

                            if (!Session.GetHabbo().HasRole("cmd_award"))
                            {
                                return false;
                            }

                            TargetUser = Params[1];
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("Could not find user: " + TargetUser);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            PhoenixEnvironment.GetGame().GetAchievementManager().UnlockNextAchievement(TargetClient, Convert.ToUInt32(MergeParams(Params, 2)));
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Ban
                        case 4: //CMD Ban
                            if (Session.GetHabbo().HasRole("cmd_ban"))
                            {
                                return false;
                            }

                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("User not found");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                            {
                                Session.SendNotif("You are not allowed to ban that user.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }

                            int BanTime = 0;
                            try
                            {
                                BanTime = int.Parse(Params[2]);
                            }
                            catch (FormatException) { }

                            if (BanTime <= 600)
                            {
                                Session.SendNotif("Ban time is in seconds and must be at least than 600 seconds (ten minutes). For more specific preset ban times, use the mod tool.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            PhoenixEnvironment.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, BanTime, MergeParams(Params, 3), false);
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Coins
                        case 6: //CMD Coins
                            if (!Session.GetHabbo().HasRole("cmd_coins"))
                            {
                                return false;
                            }

                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("User could not be found.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            int creditsToAdd;
                            if (int.TryParse(Params[2], out creditsToAdd))
                            {
                                TargetClient.GetHabbo().Credits = TargetClient.GetHabbo().Credits + creditsToAdd;
                                TargetClient.GetHabbo().UpdateCreditsBalance(true);
                                TargetClient.SendNotif(Session.GetHabbo().Username + " has awarded you " + creditsToAdd.ToString() + " credits.");
                                Session.SendNotif("Credit balance updated successful.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            Session.SendNotif("Please send a valid amount of credits.");
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Freeze
                        case 14: //CMD Freeze
                            if (Session.GetHabbo().HasRole("cmd_freeze"))
                            {

                                TargetRoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Params[1]);
                                if (TargetRoomUser != null)
                                {
                                    TargetRoomUser.bool_5 = !TargetRoomUser.bool_5;
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD GiveBadge
                        case 15: //CMD GiveBadge
                            if (Session.GetHabbo().HasRole("cmd_givebadge"))
                            {
                                TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);

                                if (TargetClient != null)
                                {
                                    TargetClient.GetHabbo().GetBadgeComponent().GiveBadge(TargetClient, PhoenixEnvironment.FilterInjectionChars(Params[2]), true);
                                }
                                else
                                {
                                    Session.SendNotif("User: " + Params[1] + " could not be found in the database.\rPlease try your request again.");
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD GlobalCredits
                        case 16: //CMD GlobalCredits
                            if (Session.GetHabbo().HasRole("cmd_globalcredits"))
                            {
                                try
                                {
                                    int GCreditsToAdd = int.Parse(Params[1]);
                                    PhoenixEnvironment.GetGame().GetClientManager().GiveCredits(GCreditsToAdd);
                                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                    {
                                        adapter.ExecuteQuery("UPDATE users SET credits = credits + " + GCreditsToAdd);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotif("Input must be a number");
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD GlobalPixels
                        case 17: //CMD GlobalPixels
                            if (Session.GetHabbo().HasRole("cmd_globalpixels"))
                            {
                                try
                                {
                                    int pixelsToAdd = int.Parse(Params[1]);
                                    PhoenixEnvironment.GetGame().GetClientManager().GivePixels(pixelsToAdd, false);
                                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                    {
                                        adapter.ExecuteQuery("UPDATE users SET activity_points = activity_points + " + pixelsToAdd);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotif("Input must be a number");
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD GlobalPoints
                        case 18: //CMD GlobalPoints
                            if (Session.GetHabbo().HasRole("cmd_globalpoints"))
                            {
                                try
                                {
                                    int pointsToAdd = int.Parse(Params[1]);
                                    PhoenixEnvironment.GetGame().GetClientManager().GivePoints(pointsToAdd, false);
                                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                    {
                                        adapter.ExecuteQuery("UPDATE users SET vip_points = vip_points + " + pointsToAdd);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotif("Input must be a number");
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD HaL
                        case 19: //CMD HaL
                            if (Session.GetHabbo().HasRole("cmd_hal"))
                            {
                                string msg = Params[1];
                                Input = Input.Substring(4).Replace(msg, "");
                                string url = Input.Substring(1);
                                ServerMessage Message = new ServerMessage(161);
                                Message.AppendStringWithBreak(string.Concat(new string[]
							{
								TextManager.GetText("cmd_hal_title"),
								"\r\n",
								url,
								"\r\n-",
								Session.GetHabbo().Username
							}));
                                Message.AppendStringWithBreak(msg);
                                PhoenixEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(Message);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Ha
                        case 20: //CMD Ha
                            if (Session.GetHabbo().HasRole("cmd_ha"))
                            {
                                string notice = Input.Substring(3);
                                ServerMessage Message2 = new ServerMessage(808);
                                Message2.AppendStringWithBreak(TextManager.GetText("cmd_ha_title"));
                                Message2.AppendStringWithBreak(notice + "\r\n- " + Session.GetHabbo().Username);
                                ServerMessage Message3 = new ServerMessage(161);
                                Message3.AppendStringWithBreak(notice + "\r\n- " + Session.GetHabbo().Username);
                                PhoenixEnvironment.GetGame().GetClientManager().BroadcastMessage(Message2, Message3);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Invisible
                        case 21: //CMD Invisible
                            if (Session.GetHabbo().HasRole("cmd_invisible"))
                            {
                                Session.GetHabbo().Visible = !Session.GetHabbo().Visible;
                                Session.SendNotif("You are now " + (Session.GetHabbo().Visible ? "visible" : "invisible") + "\nTo apply the changes reload the room ;D");
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD IpBan
                        case 22: //CMD IpBan
                            if (!Session.GetHabbo().HasRole("cmd_ipban"))
                            {
                                return false;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("User not found.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().isAaron)
                            {
                                Session.SendNotif("You are not allowed to ban that user.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            PhoenixEnvironment.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), true);
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Kick
                        case 23: //CMD Kick
                            if (!Session.GetHabbo().HasRole("cmd_kick"))
                            {
                                return false;
                            }
                            TargetUser = Params[1];
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("Could not find user: " + TargetUser);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank && !Session.GetHabbo().isAaron)
                            {
                                Session.SendNotif("You are not allowed to kick that user.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (TargetClient.GetHabbo().CurrentRoomId < 1u)
                            {
                                Session.SendNotif("That user is not in a room and can not be kicked.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);
                            if (TargetRoom == null)
                            {
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            TargetRoom.RemoveUserFromRoom(TargetClient, true, false);
                            if (Params.Length > 2)
                            {
                                TargetClient.SendNotif("A moderator has kicked you from the room for the following reason: " + ChatCommandHandler.MergeParams(Params, 2));
                            }
                            else
                            {
                                TargetClient.SendNotif("A moderator has kicked you from the room.");
                            }
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD MassBadge
                        case 24: //CMD MassBadge
                            if (Session.GetHabbo().HasRole("cmd_massbadge"))
                            {
                                PhoenixEnvironment.GetGame().GetClientManager().GiveMassBadge(Params[1]);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD MassCredits
                        case 25: //CMD MassCredits
                            if (Session.GetHabbo().HasRole("cmd_masscredits"))
                            {
                                try
                                {
                                    int MCreditsToAdd = int.Parse(Params[1]);
                                    PhoenixEnvironment.GetGame().GetClientManager().GiveCredits(MCreditsToAdd);
                                }
                                catch
                                {
                                    Session.SendNotif("Input must be a number");
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD MassPixels
                        case 26: //CMD MassPixels
                            if (Session.GetHabbo().HasRole("cmd_masspixels"))
                            {
                                try
                                {
                                    int MPixelsToAdd = int.Parse(Params[1]);
                                    PhoenixEnvironment.GetGame().GetClientManager().GivePixels(MPixelsToAdd, true);
                                }
                                catch
                                {
                                    Session.SendNotif("Input must be a number");
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD MassPoints
                        case 27: //CMD MassPoints
                            if (Session.GetHabbo().HasRole("cmd_masspoints"))
                            {
                                try
                                {
                                    int MPointsToAdd = int.Parse(Params[1]);
                                    PhoenixEnvironment.GetGame().GetClientManager().GivePoints(MPointsToAdd, true);
                                }
                                catch
                                {
                                    Session.SendNotif("Input must be a number");
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Motd
                        case 30: //CMD Motd
                            if (!Session.GetHabbo().HasRole("cmd_motd"))
                            {
                                return false;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("Could not find user: " + Params[1]);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            TargetClient.SendNotif(ChatCommandHandler.MergeParams(Params, 2), 2);
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Mute
                        case 31: //CMD Mute
                            if (!Session.GetHabbo().HasRole("cmd_mute"))
                            {
                                return false;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null || TargetClient.GetHabbo() == null)
                            {
                                Session.SendNotif("Could not find user: " + Params[1]);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().isAaron)
                            {
                                Session.SendNotif("You are not allowed to (un)mute that user.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            TargetClient.GetHabbo().Mute();
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Override
                        case 32: //CMD Override
                            if (!Session.GetHabbo().HasRole("cmd_override"))
                            {
                                return false;
                            }
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            if (TargetRoom == null)
                            {
                                return false;
                            }
                            TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            if (TargetRoomUser == null)
                            {
                                return false;
                            }
                            if (TargetRoomUser.AllowOverride)
                            {
                                TargetRoomUser.AllowOverride = false;
                                Session.SendNotif("Walking override disabled.");
                            }
                            else
                            {
                                TargetRoomUser.AllowOverride = true;
                                Session.SendNotif("Walking override enabled.");
                            }
                            TargetRoom.GenerateMaps();
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Pixels
                        case 34: //CMD Pixels
                            if (!Session.GetHabbo().HasRole("cmd_pixels"))
                            {
                                return false;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("User could not be found.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            int PixelsToAdd;
                            if (int.TryParse(Params[2], out PixelsToAdd))
                            {
                                TargetClient.GetHabbo().ActivityPoints = TargetClient.GetHabbo().ActivityPoints + PixelsToAdd;
                                TargetClient.GetHabbo().UpdateActivityPointsBalance(true);
                                TargetClient.SendNotif(Session.GetHabbo().Username + " has awarded you " + PixelsToAdd.ToString() + " Pixels!");
                                Session.SendNotif("Pixels balance updated successfully.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            Session.SendNotif("Please send a valid amount of pixels.");
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Points
                        case 35: //CMD Points
                            if (!Session.GetHabbo().HasRole("cmd_points"))
                            {
                                return false;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("User could not be found.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            int PointsToAdd;
                            if (int.TryParse(Params[2], out PointsToAdd))
                            {
                                TargetClient.GetHabbo().shells = TargetClient.GetHabbo().shells + PointsToAdd;
                                TargetClient.GetHabbo().UpdateShellsBalance(false, true);
                                TargetClient.SendNotif(Session.GetHabbo().Username + " has awarded you " + PointsToAdd.ToString() + " Points!");
                                Session.SendNotif("Points balance updated successfully.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            Session.SendNotif("Please send a valid amount of points.");
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD RemoveBadge
                        case 39: //CMD RemoveBadge
                            if (Session.GetHabbo().HasRole("cmd_removebadge"))
                            {
                                TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null)
                                {
                                    TargetClient.GetHabbo().GetBadgeComponent().RemoveBadge(PhoenixEnvironment.FilterInjectionChars(Params[2]));
                                }
                                else
                                {
                                    Session.SendNotif("User: " + Params[1] + " could not be found in the database.\rPlease try your request again.");
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD RoomAlert
                        case 41: //CMD RoomAlert
                            if (!Session.GetHabbo().HasRole("cmd_roomalert"))
                            {
                                return false;
                            }
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            if (TargetRoom == null)
                            {
                                return false;
                            }
                            string alert = ChatCommandHandler.MergeParams(Params, 1);
                            for (int i = 0; i < TargetRoom.UserList.Length; i++)
                            {
                                TargetRoomUser = TargetRoom.UserList[i];
                                if (TargetRoomUser != null)
                                {
                                    TargetRoomUser.GetClient().SendNotif(alert);
                                }
                            }
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD RoomBadge
                        case 42: //CMD RoomBadge
                            if (!Session.GetHabbo().HasRole("cmd_roombadge"))
                            {
                                return false;
                            }
                            if (Session.GetHabbo().CurrentRoom == null)
                            {
                                return false;
                            }
                            for (int i = 0; i < Session.GetHabbo().CurrentRoom.UserList.Length; i++)
                            {
                                try
                                {
                                    TargetRoomUser = Session.GetHabbo().CurrentRoom.UserList[i];
                                    if (TargetRoomUser != null)
                                    {
                                        if (!TargetRoomUser.IsBot)
                                        {
                                            if (TargetRoomUser.GetClient() != null)
                                            {
                                                if (TargetRoomUser.GetClient().GetHabbo() != null)
                                                {
                                                    TargetRoomUser.GetClient().GetHabbo().GetBadgeComponent().GiveBadge(TargetRoomUser.GetClient(), Params[1], true);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Session.SendNotif("Error: " + ex.ToString());
                                }
                            }
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD RoomKick
                        case 43: //CMD RoomKick
                            if (!Session.GetHabbo().HasRole("cmd_roomkick"))
                            {
                                return false;
                            }
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            if (TargetRoom == null)
                            {
                                return false;
                            }
                            bool GenericMsg = true;
                            string ModMsg = ChatCommandHandler.MergeParams(Params, 1);
                            if (ModMsg.Length > 0)
                            {
                                GenericMsg = false;
                            }
                            for (int i = 0; i < TargetRoom.UserList.Length; i++)
                            {
                                TargetRoomUser = TargetRoom.UserList[i];
                                if (TargetRoomUser != null && TargetRoomUser.GetClient().GetHabbo().Rank < Session.GetHabbo().Rank)
                                {
                                    if (!GenericMsg)
                                    {
                                        TargetRoomUser.GetClient().SendNotif("You have been kicked by an moderator: " + ModMsg);
                                    }
                                    TargetRoom.RemoveUserFromRoom(TargetRoomUser.GetClient(), true, GenericMsg);
                                }
                            }
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD RoomMute
                        case 44: //CMD RoomMute
                            if (Session.GetHabbo().HasRole("cmd_roommute"))
                            {
                                if (Session.GetHabbo().CurrentRoom.RoomMuted)
                                {
                                    Session.GetHabbo().CurrentRoom.RoomMuted = false;
                                }
                                else
                                {
                                    Session.GetHabbo().CurrentRoom.RoomMuted = true;
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD SA
                        case 45: //CMD SA
                            if (Session.GetHabbo().HasRole("cmd_sa"))
                            {
                                ServerMessage Logging = new ServerMessage(134);
                                Logging.AppendUInt(0);
                                Logging.AppendString(Session.GetHabbo().Username + ": " + Input.Substring(3));
                                PhoenixEnvironment.GetGame().GetClientManager().BroadcastMessageToStaff(Logging, Logging);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD SetSpeed
                        case 47: //CMD SetSpeed
                            if (Session.GetHabbo().HasRole("cmd_setspeed"))
                            {
                                int.Parse(Params[1]);
                                Session.GetHabbo().CurrentRoom.method_102(int.Parse(Params[1]));
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Shutdown
                        case 48: //CMD Shutdown
                            if (Session.GetHabbo().HasRole("cmd_shutdown"))
                            {
                                Logging.LogCriticalException("User " + Session.GetHabbo().Username + " shut down the server " + DateTime.Now.ToString());
                                Task task = new Task(new Action(PhoenixEnvironment.BeginShutDown));
                                task.Start();
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Spull
                        case 49: //CMD Spull
                            if (Session.GetHabbo().HasRole("cmd_spull"))
                            {
                                try
                                {
                                    string a = "down";
                                    TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                    TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (Session == null || TargetClient == null)
                                    {
                                        return false;
                                    }
                                    TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    RoomUser TargetClient2 = TargetRoom.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                    if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                    {
                                        Session.GetHabbo().Sendselfwhisper("You cannot pull yourself");
                                        return true;
                                    }
                                    TargetRoomUser.Chat(Session, "*pulls " + TargetClient.GetHabbo().Username + " to them*", false);
                                    if (TargetRoomUser.RotBody == 0)
                                    {
                                        a = "up";
                                    }
                                    if (TargetRoomUser.RotBody == 2)
                                    {
                                        a = "right";
                                    }
                                    if (TargetRoomUser.RotBody == 4)
                                    {
                                        a = "down";
                                    }
                                    if (TargetRoomUser.RotBody == 6)
                                    {
                                        a = "left";
                                    }
                                    if (a == "up")
                                    {
                                        TargetClient2.MoveTo(TargetRoomUser.X, TargetRoomUser.Y - 1);
                                    }
                                    if (a == "right")
                                    {
                                        TargetClient2.MoveTo(TargetRoomUser.X + 1, TargetRoomUser.Y);
                                    }
                                    if (a == "down")
                                    {
                                        TargetClient2.MoveTo(TargetRoomUser.X, TargetRoomUser.Y + 1);
                                    }
                                    if (a == "left")
                                    {
                                        TargetClient2.MoveTo(TargetRoomUser.X - 1, TargetRoomUser.Y);
                                    }
                                    return true;
                                }
                                catch
                                {
                                    return false;
                                }
                            }
                            return false;
                        #endregion
                        #region CMD Summon
                        case 50: //CMD Summon
                            if (Session.GetHabbo().HasRole("cmd_summon"))
                            {
                                TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null && TargetClient.GetHabbo().CurrentRoom != Session.GetHabbo().CurrentRoom)
                                {
                                    ServerMessage Message5 = new ServerMessage(286u);
                                    Message5.AppendBoolean(Session.GetHabbo().CurrentRoom.IsPublic);
                                    Message5.AppendUInt(Session.GetHabbo().CurrentRoomId);
                                    TargetClient.SendMessage(Message5);
                                    TargetClient.SendNotif(Session.GetHabbo().Username + " has summoned you to them");
                                }
                                else
                                {
                                    Session.GetHabbo().Sendselfwhisper("User: " + Params[1] + " could not be found - Maybe they're not online anymore :(");
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD SuperBan
                        case 51: //CMD SuperBan
                            if (!Session.GetHabbo().HasRole("cmd_superban"))
                            {
                                return false;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("User not found.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().isAaron)
                            {
                                Session.SendNotif("You are not allowed to ban that user.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            PhoenixEnvironment.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), false);
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Teleport
                        case 52: //CMD Teleport
                            if (!Session.GetHabbo().HasRole("cmd_teleport"))
                            {
                                return false;
                            }
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            if (TargetRoom == null)
                            {
                                return false;
                            }
                            TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            if (TargetRoomUser == null)
                            {
                                return false;
                            }
                            if (TargetRoomUser.TeleportMode)
                            {
                                TargetRoomUser.TeleportMode = false;
                                Session.SendNotif("Teleporting disabled.");
                            }
                            else
                            {
                                TargetRoomUser.TeleportMode = true;
                                Session.SendNotif("Teleporting enabled.");
                            }
                            TargetRoom.GenerateMaps();
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD UnMute
                        case 54: //CMD UnMute
                            if (!Session.GetHabbo().HasRole("cmd_unmute"))
                            {
                                return false;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null || TargetClient.GetHabbo() == null)
                            {
                                Session.SendNotif("Could not find user: " + Params[1]);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            TargetClient.GetHabbo().Unmute();
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Update Achievements
                        case 55: //CMD Update Achievements
                            if (Session.GetHabbo().HasRole("cmd_update_achievements"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    AchievementManager.LoadAchievements(adapter);
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Update Bans
                        case 56: //CMD Update Bans
                            if (Session.GetHabbo().HasRole("cmd_update_bans"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    PhoenixEnvironment.GetGame().GetBanManager().LoadBans(adapter);
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().CheckForAllBanConflicts();
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region  CMD Update Bots
                        case 57: //CMD Update Bots
                            if (Session.GetHabbo().HasRole("cmd_update_bots"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    PhoenixEnvironment.GetGame().GetBotManager().LoadBots(adapter);
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Update Catalogue
                        case 58: //CMD Update Catalogue
                            if (Session.GetHabbo().HasRole("cmd_update_catalogue"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    PhoenixEnvironment.GetGame().GetCatalog().Initialize(adapter);
                                }
                                PhoenixEnvironment.GetGame().GetCatalog().InitCache();
                                PhoenixEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(new ServerMessage(441u));
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Update Filter
                        case 59: //CMD Update Filter
                            if (Session.GetHabbo().HasRole("cmd_update_filter"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    ChatCommandHandler.UpdateFilters(adapter);
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Update Items
                        case 60: //CMD Update Items
                            if (Session.GetHabbo().HasRole("cmd_update_items"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    PhoenixEnvironment.GetGame().GetItemManager().LoadItems(adapter);
                                }
                                Session.SendNotif("Item defenitions reloaded successfully.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Update Navigator
                        case 61: //CMD Update Navigator
                            if (Session.GetHabbo().HasRole("cmd_update_navigator"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    PhoenixEnvironment.GetGame().GetNavigator().Initialize(adapter);
                                    PhoenixEnvironment.GetGame().GetRoomManager().LoadModels(adapter);
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Update Permissions
                        case 62: //CMD Update Premissions
                            if (Session.GetHabbo().HasRole("cmd_update_permissions"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    PhoenixEnvironment.GetGame().GetRoleManager().LoadRoles(adapter);
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Update Settings
                        case 63: ///CMD Update Settings
                            if (Session.GetHabbo().HasRole("cmd_update_settings"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    PhoenixEnvironment.GetGame().LoadSettings(adapter);
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Update Texts
                        case 65: //CMD Update Texts
                            if (Session.GetHabbo().HasRole("cmd_update_texts"))
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    TextManager.LoadTexts(adapter);
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD UserInfo
                        case 64: //CMD UserInfo
                            if (!Session.GetHabbo().HasRole("cmd_userinfo"))
                            {
                                return false;
                            }
                            bool flag2 = true;
                            if (string.IsNullOrEmpty(Params[1]))
                            {
                                Session.SendNotif("Please enter a username");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                flag2 = false;
                                TargetHabbo = Authenticator.GetHabboViaUsername(Params[1]);
                            }
                            else
                            {
                                TargetHabbo = TargetClient.GetHabbo();
                            }
                            if (TargetHabbo == null)
                            {
                                Session.SendNotif("Unable to find user " + Params[1]);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            StringBuilder Builderer = new StringBuilder();
                            if (TargetHabbo.CurrentRoom != null)
                            {
                                Builderer.Append(" - ROOM INFORMATION FOR ROOMID: " + TargetHabbo.CurrentRoom.RoomId + " - \r");
                                Builderer.Append("Owner: " + TargetHabbo.CurrentRoom.Owner + "\r");
                                Builderer.Append("Room name: " + TargetHabbo.CurrentRoom.Name + "\r");
                                Builderer.Append(string.Concat(new object[]
							{
								"Users in room: ",
								TargetHabbo.CurrentRoom.UserCount,
								"/",
								TargetHabbo.CurrentRoom.UsersMax
							}));
                            }
                            uint UserRank = TargetHabbo.Rank;
                            string UserIP = "";
                            if (Session.GetHabbo().HasRole("cmd_userinfo_viewip"))
                            {
                                UserIP = "UserIP: " + TargetHabbo.LastIp + " \r";
                            }
                            Session.SendNotif(string.Concat(new object[]
						{
							"User information for user: ",
							Params[1],
							":\rRank: ",
							UserRank,
							" \rUser online: ",
							flag2.ToString(),
							" \rUserID: ",
							TargetHabbo.Id,
							" \r",
							UserIP,
							"Visiting room: ",
							TargetHabbo.CurrentRoomId,
							" \rUser motto: ",
							TargetHabbo.Motto,
							" \rUser credits: ",
							TargetHabbo.Credits,
							" \rUser pixels: ",
							TargetHabbo.ActivityPoints,
							" \rUser points: ",
							TargetHabbo.shells,
							" \rUser muted: ",
							TargetHabbo.Muted.ToString(),
							"\r\r\r",
							Builderer.ToString()
						}));
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Disconnect
                        case 66: //CMD Disconnect
                            if (!Session.GetHabbo().HasRole("cmd_disconnect"))
                            {
                                return false;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotif("Could not find user: " + Params[1]);
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank && !Session.GetHabbo().isAaron)
                            {
                                Session.SendNotif("You are not allowed to kick that user.");
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (!TargetClient.GetHabbo().isAaron)
                            {
                                TargetClient.Disconnect();
                            }
                            return true;
                        #endregion
                        #region CMD Empty
                        case 10: //CMD Empty
                            if (Session.GetHabbo().HasRole("cmd_empty") && Params[1] != null)
                            {
                                TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null && TargetClient.GetHabbo() != null)
                                {
                                    TargetClient.GetHabbo().GetInventoryComponent().ClearItems();
                                    Session.SendNotif("Inventory cleared! (Database and cache)");
                                }
                                else
                                {
                                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                    {
                                        adapter.AddParamWithValue("usrname", Params[1]);
                                        int userId = int.Parse(adapter.ReadString("SELECT Id FROM users WHERE username = @usrname LIMIT 1;"));
                                        adapter.ExecuteQuery("DELETE FROM items WHERE user_id = '" + userId + "' AND room_id = 0;");
                                        Session.SendNotif("Inventory cleared! (Database)");
                                    }
                                }
                                PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        #endregion
                        #endregion

                        #region VIP Commands
                        #region CMD FlagMe
                        case 12: //CMD FlagMe
                            if (!GlobalClass.cmdFlagmeEnabled)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_disabled"));
                                return true;
                            }
                            if (!Session.GetHabbo().Vip)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_permission_vip"));
                                return true;
                            }
                            ServerMessage message = new ServerMessage(573);
                            Session.SendMessage(message);
                            return true;
                        #endregion
                        #region CMD Follow
                        case 13: //CMD Follow
                            if (!GlobalClass.cmdFollowEnabled)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_disabled"));
                                return true;
                            }
                            if (!Session.GetHabbo().Vip)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_permission_vip"));
                                return true;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient != null && TargetClient.GetHabbo().InRoom && Session.GetHabbo().CurrentRoom != TargetClient.GetHabbo().CurrentRoom && !TargetClient.GetHabbo().HideInRom)
                            {
                                ServerMessage message2 = new ServerMessage(286);
                                message2.AppendBoolean(TargetClient.GetHabbo().CurrentRoom.IsPublic);
                                message2.AppendUInt(TargetClient.GetHabbo().CurrentRoomId);
                                Session.SendMessage(message2);
                            }
                            else
                            {
                                Session.GetHabbo().Sendselfwhisper("User: " + Params[1] + " could not be found - Maybe they're not online or not in a room anymore (or maybe they're a ninja)");
                            }
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD Mimic
                        case 28: //CMD Mimic    
                            if (!GlobalClass.cmdMimicEnabled)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_disabled"));
                                return true;
                            }
                            if (!Session.GetHabbo().Vip)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_permission_vip"));
                                return true;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.GetHabbo().Sendselfwhisper("Could not find user: " + Params[1]);
                                return true;
                            }
                            Session.GetHabbo().Look = TargetClient.GetHabbo().Look;
                            Session.GetHabbo().method_26(false, Session); //method_26? What is it? I don't know.
                            return true;
                        #endregion
                        #region CMD Moonwalk
                        case 29: //CMD Moonwalk
                            if (!GlobalClass.cmdMoonwalkEnabled)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_disabled"));
                                return true;
                            }
                            if (!Session.GetHabbo().Vip)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_permission_vip"));
                                return true;
                            }
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            if (TargetRoom == null)
                            {
                                return false;
                            }
                            TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            if (TargetRoomUser == null)
                            {
                                return false;
                            }
                            if (TargetRoomUser.WalkBackwards)
                            {
                                TargetRoomUser.WalkBackwards = false;
                                Session.GetHabbo().Sendselfwhisper("Your moonwalk has been disabled.");
                                return true;
                            }
                            TargetRoomUser.WalkBackwards = true;
                            Session.GetHabbo().Sendselfwhisper("Your moonwalk has been enabled.");
                            return true;
                        #endregion
                        #region CMD Pull
                        case 36: //CMD Pull
                            try
                            {
                                if (!GlobalClass.cmdPullEnabled)
                                {
                                    Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_disabled"));
                                    return true;
                                }
                                if (!Session.GetHabbo().Vip)
                                {
                                    Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_permission_vip"));
                                    return true;
                                }
                                string a = "down";
                                TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (Session == null || TargetClient == null)
                                {
                                    return false;
                                }
                                TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                RoomUser TargetRoomUser2 = TargetRoom.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                {
                                    Session.GetHabbo().Sendselfwhisper("You cannot pull yourself");
                                    return true;
                                }
                                if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(TargetRoomUser.X - TargetRoomUser2.X) < 3 && Math.Abs(TargetRoomUser.Y - TargetRoomUser2.Y) < 3)
                                {
                                    TargetRoomUser.Chat(Session, "*pulls " + TargetClient.GetHabbo().Username + " to them*", false);
                                    if (TargetRoomUser.RotBody == 0)
                                    {
                                        a = "up";
                                    }
                                    if (TargetRoomUser.RotBody == 2)
                                    {
                                        a = "right";
                                    }
                                    if (TargetRoomUser.RotBody == 4)
                                    {
                                        a = "down";
                                    }
                                    if (TargetRoomUser.RotBody == 6)
                                    {
                                        a = "left";
                                    }
                                    if (a == "up")
                                    {
                                        TargetRoomUser2.MoveTo(TargetRoomUser.X, TargetRoomUser.Y - 1);
                                    }
                                    if (a == "right")
                                    {
                                        TargetRoomUser2.MoveTo(TargetRoomUser.X + 1, TargetRoomUser.Y);
                                    }
                                    if (a == "down")
                                    {
                                        TargetRoomUser2.MoveTo(TargetRoomUser.X, TargetRoomUser.Y + 1);
                                    }
                                    if (a == "left")
                                    {
                                        TargetRoomUser2.MoveTo(TargetRoomUser.X - 1, TargetRoomUser.Y);
                                    }
                                    return true;
                                }
                                Session.GetHabbo().Sendselfwhisper("That user is not close enough to you to be pulled, try getting closer");
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        #endregion
                        #region CMD Push
                        case 37: //CMD Push
                            try
                            {
                                if (!GlobalClass.cmdPushEnabled)
                                {
                                    Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_disabled"));
                                    return true;
                                }
                                if (!Session.GetHabbo().Vip)
                                {
                                    Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_permission_vip"));
                                    return true;
                                }
                                string a = "down";
                                TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (Session == null || TargetClient == null)
                                {
                                    return false;
                                }
                                TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                RoomUser TargetRoomUser2 = TargetRoom.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                {
                                    Session.GetHabbo().Sendselfwhisper("It can't be that bad mate, no need to push yourself!");
                                    return true;
                                }
                                bool arg_3DD2_0; //What is it? I don't know!
                                if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                {
                                    if ((TargetRoomUser.X + 1 != TargetRoomUser2.X || TargetRoomUser.Y != TargetRoomUser2.Y) && (TargetRoomUser.X - 1 != TargetRoomUser2.X || TargetRoomUser.Y != TargetRoomUser2.Y) && (TargetRoomUser.Y + 1 != TargetRoomUser2.Y || TargetRoomUser.X != TargetRoomUser2.X))
                                    {
                                        if (TargetRoomUser.Y - 1 == TargetRoomUser2.Y)
                                        {
                                            if (TargetRoomUser.X == TargetRoomUser2.X)
                                            {
                                                arg_3DD2_0 = false;
                                            }
                                            else
                                            {
                                                arg_3DD2_0 = true;
                                            }
                                        }
                                        arg_3DD2_0 = (TargetRoomUser.X != TargetRoomUser2.X || TargetRoomUser.Y != TargetRoomUser2.Y);
                                        if (!arg_3DD2_0)
                                        {
                                            TargetRoomUser.Chat(Session, "*pushes " + TargetClient.GetHabbo().Username + "*", false);
                                            if (TargetRoomUser.RotBody == 0)
                                            {
                                                a = "up";
                                                a = "right";
                                            }
                                            if (TargetRoomUser.RotBody == 4)
                                            {
                                                a = "down";
                                            }
                                            if (TargetRoomUser.RotBody == 6)
                                            {
                                                a = "left";
                                            }
                                            if (TargetRoomUser.RotBody == 2)
                                            {
                                            }
                                            if (a == "up")
                                            {
                                                TargetRoomUser2.MoveTo(TargetRoomUser2.X, TargetRoomUser2.Y - 1);
                                            }
                                            if (a == "right")
                                            {
                                                TargetRoomUser2.MoveTo(TargetRoomUser2.X + 1, TargetRoomUser2.Y);
                                            }
                                            if (a == "down")
                                            {
                                                TargetRoomUser2.MoveTo(TargetRoomUser2.X, TargetRoomUser2.Y + 1);
                                            }
                                            if (a == "left")
                                            {
                                                TargetRoomUser2.MoveTo(TargetRoomUser2.X - 1, TargetRoomUser2.Y);
                                            }
                                        }
                                        return true;
                                    }
                                }
                                return false;
                            }
                            catch
                            {
                                return false;
                            }
                        #endregion
                        #endregion

                        #region Normal Commands
                        #region CMD Buy
                        case 5: //CMD Buy
                            int amountToBuy = Convert.ToInt16(Params[1]);
                            if (amountToBuy > 0 && amountToBuy < 101)
                            {
                                Session.GetHabbo().BuyCount = (int)Convert.ToInt16(Params[1]);
                            }
                            else
                            {
                                Session.GetHabbo().Sendselfwhisper("Please choose a value between 1 - 100");
                            }
                            return true;
                        #endregion
                        #region CMD Ride
                        case 40: //CMD Ride
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            RoomUser TargetRoomUser3 = TargetRoom.method_57(Params[1]);
                            if (TargetRoomUser.class34_1 != null)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_err_riding"));
                                return true;
                            }
                            if (!TargetRoomUser3.IsBot || TargetRoomUser3.PetData.Type != 13)
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_err_nothorse"));
                                return true;
                            }
                            bool arg_40EB_0; //What is it? I don't know!
                            if ((TargetRoomUser.X + 1 != TargetRoomUser3.X || TargetRoomUser.Y != TargetRoomUser3.Y) && (TargetRoomUser.X - 1 != TargetRoomUser3.X || TargetRoomUser.Y != TargetRoomUser3.Y) && (TargetRoomUser.Y + 1 != TargetRoomUser3.Y || TargetRoomUser.X != TargetRoomUser3.X))
                            {
                                if (TargetRoomUser.Y - 1 == TargetRoomUser3.Y)
                                {
                                    if (TargetRoomUser.X == TargetRoomUser3.X)
                                    {
                                        arg_40EB_0 = false;
                                    }
                                }
                                arg_40EB_0 = (TargetRoomUser.X != TargetRoomUser3.X || TargetRoomUser.Y != TargetRoomUser3.Y);
                                if (arg_40EB_0)
                                {
                                    Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_err_toofar"));
                                    return true;
                                }
                            }
                            if (TargetRoomUser3.BotData.RoomUser_0 == null)
                            {
                                TargetRoomUser3.BotData.RoomUser_0 = TargetRoomUser;
                                TargetRoomUser.class34_1 = TargetRoomUser3.BotData;
                                TargetRoomUser.X = TargetRoomUser3.X;
                                TargetRoomUser.Y = TargetRoomUser3.Y;
                                TargetRoomUser.Z = TargetRoomUser3.Z + 1.0;
                                TargetRoomUser.RotBody = TargetRoomUser3.RotBody;
                                TargetRoomUser.RotHead = TargetRoomUser3.RotHead;
                                TargetRoomUser.UpdateNeeded = true;
                                TargetRoom.UpdateUserStatus(TargetRoomUser, false, false);
                                TargetRoomUser.Target = TargetRoomUser3;
                                TargetRoomUser.Statusses.Clear();
                                TargetRoomUser3.Statusses.Clear();
                                Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(77, true);
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_instr_getoff"));
                                TargetRoom.GenerateMaps();
                                return true;
                            }
                            Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_err_tooslow"));
                            return true;
                        #endregion
                        #region CMD Dismount
                        case 80: //CMD Dismount
                        case 81:
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            if (TargetRoomUser.class34_1 != null)
                            {
                                Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, true);
                                TargetRoomUser.class34_1.RoomUser_0 = null;
                                TargetRoomUser.class34_1 = null;
                                TargetRoomUser.Z -= 1.0;
                                TargetRoomUser.Statusses.Clear();
                                TargetRoomUser.UpdateNeeded = true;
                                int int_3 = PhoenixEnvironment.GetRandomNumber(0, TargetRoom.Model.MapSizeX);
                                int int_4 = PhoenixEnvironment.GetRandomNumber(0, TargetRoom.Model.MapSizeY);
                                TargetRoomUser.Target.MoveTo(int_3, int_4);
                                TargetRoomUser.Target = null;
                                TargetRoom.UpdateUserStatus(TargetRoomUser, false, false);
                            }
                            return true;
                        #endregion
                        #region CMD DisableDiagonal
                        case 8: //CMD DisableDiagonal
                            Session.SendNotif("Command disabled");
                            return true;
                        #endregion
                        #region CMD EmptyItems
                        case 9: //CMD EmptyItems
                            Session.GetHabbo().GetInventoryComponent().ClearItems();
                            Session.SendNotif(TextManager.GetText("cmd_emptyitems_success"));
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD EmptyPets
                        case 82: //CMD EmptyPets
                            Session.GetHabbo().GetInventoryComponent().ClearPets();
                            Session.SendNotif(TextManager.GetText("cmd_emptypets_success"));
                            PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, Params[0].ToLower(), Input);
                            return true;
                        #endregion
                        #region CMD RedeemCreds
                        case 38: //CMD RedeemCreds
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            if (TargetRoomUser.IsTrading)
                            {
                                Session.GetHabbo().Sendselfwhisper("Command unavailable while trading");
                                return true;
                            }
                            if (GlobalClass.cmdRedeemCredits)
                            {
                                Session.GetHabbo().GetInventoryComponent().RedeemCredits(Session);
                            }
                            else
                            {
                                Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_disabled"));
                            }
                            return true;
                        #endregion
                        #region CMD Commands
                        case 67: //CMD Commands
                            string Command = "Your Commands:\r\r";
                            if (Session.GetHabbo().HasRole("cmd_update_settings"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_settings_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_update_bans"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_bans_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_update_permissions"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_permissions_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_update_filter"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_filter_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_update_bots"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_bots_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_update_catalogue"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_catalogue_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_update_items"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_items_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_update_navigator"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_navigator_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_update_achievements"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_achievements_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_award"))
                            {
                                Command = Command + TextManager.GetText("cmd_award_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_coords"))
                            {
                                Command = Command + TextManager.GetText("cmd_coords_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_override"))
                            {
                                Command = Command + TextManager.GetText("cmd_override_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_teleport"))
                            {
                                Command = Command + TextManager.GetText("cmd_teleport_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_coins"))
                            {
                                Command = Command + TextManager.GetText("cmd_coins_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_pixels"))
                            {
                                Command = Command + TextManager.GetText("cmd_pixels_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_points"))
                            {
                                Command = Command + TextManager.GetText("cmd_points_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_alert"))
                            {
                                Command = Command + TextManager.GetText("cmd_alert_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_motd"))
                            {
                                Command = Command + TextManager.GetText("cmd_motd_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_roomalert"))
                            {
                                Command = Command + TextManager.GetText("cmd_roomalert_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_ha"))
                            {
                                Command = Command + TextManager.GetText("cmd_ha_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_hal"))
                            {
                                Command = Command + TextManager.GetText("cmd_hal_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_freeze"))
                            {
                                Command = Command + TextManager.GetText("cmd_freeze_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_enable"))
                            {
                                Command = Command + TextManager.GetText("cmd_enable_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_roommute"))
                            {
                                Command = Command + TextManager.GetText("cmd_roommute_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_setspeed"))
                            {
                                Command = Command + TextManager.GetText("cmd_setspeed_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_globalcredits"))
                            {
                                Command = Command + TextManager.GetText("cmd_globalcredits_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_globalpixels"))
                            {
                                Command = Command + TextManager.GetText("cmd_globalpixels_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_globalpoints"))
                            {
                                Command = Command + TextManager.GetText("cmd_globalpoints_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_masscredits"))
                            {
                                Command = Command + TextManager.GetText("cmd_masscredits_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_masspixels"))
                            {
                                Command = Command + TextManager.GetText("cmd_masspixels_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_masspoints"))
                            {
                                Command = Command + TextManager.GetText("cmd_masspoints_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_givebadge"))
                            {
                                Command = Command + TextManager.GetText("cmd_givebadge_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_removebadge"))
                            {
                                Command = Command + TextManager.GetText("cmd_removebadge_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_summon"))
                            {
                                Command = Command + TextManager.GetText("cmd_summon_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_roombadge"))
                            {
                                Command = Command + TextManager.GetText("cmd_roombadge_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_massbadge"))
                            {
                                Command = Command + TextManager.GetText("cmd_massbadge_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_userinfo"))
                            {
                                Command = Command + TextManager.GetText("cmd_userinfo_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_shutdown"))
                            {
                                Command = Command + TextManager.GetText("cmd_shutdown_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_invisible"))
                            {
                                Command = Command + TextManager.GetText("cmd_invisible_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_ban"))
                            {
                                Command = Command + TextManager.GetText("cmd_ban_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_superban"))
                            {
                                Command = Command + TextManager.GetText("cmd_superban_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_ipban"))
                            {
                                Command = Command + TextManager.GetText("cmd_ipban_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_kick"))
                            {
                                Command = Command + TextManager.GetText("cmd_kick_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_roomkick"))
                            {
                                Command = Command + TextManager.GetText("cmd_roomkick_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_mute"))
                            {
                                Command = Command + TextManager.GetText("cmd_mute_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_unmute"))
                            {
                                Command = Command + TextManager.GetText("cmd_unmute_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_sa"))
                            {
                                Command = Command + TextManager.GetText("cmd_sa_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_spull"))
                            {
                                Command = Command + TextManager.GetText("cmd_spull_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_empty"))
                            {
                                Command = Command + TextManager.GetText("cmd_empty_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().HasRole("cmd_update_texts"))
                            {
                                Command = Command + TextManager.GetText("cmd_update_texts_desc") + "\r\r";
                            }
                            if (Session.GetHabbo().Vip)
                            {
                                if (GlobalClass.cmdMoonwalkEnabled)
                                {
                                    Command = Command + TextManager.GetText("cmd_moonwalk_desc") + "\r\r";
                                }
                                if (GlobalClass.cmdMimicEnabled)
                                {
                                    Command = Command + TextManager.GetText("cmd_mimic_desc") + "\r\r";
                                }
                                if (GlobalClass.cmdFollowEnabled)
                                {
                                    Command = Command + TextManager.GetText("cmd_follow_desc") + "\r\r";
                                }
                                if (GlobalClass.cmdPushEnabled)
                                {
                                    Command = Command + TextManager.GetText("cmd_push_desc") + "\r\r";
                                }
                                if (GlobalClass.cmdPullEnabled)
                                {
                                    Command = Command + TextManager.GetText("cmd_pull_desc") + "\r\r";
                                }
                                if (GlobalClass.cmdFlagmeEnabled)
                                {
                                    Command = Command + TextManager.GetText("cmd_flagme_desc") + "\r\r";
                                }
                            }
                            string RCommand = "";
                            if (GlobalClass.cmdRedeemCredits)
                            {
                                RCommand = RCommand + TextManager.GetText("cmd_redeemcreds_desc") + "\r\r";
                            }
                            string mCommand = Command;
                            Command = string.Concat(new string[]
									{
										mCommand,
										"- - - - - - - - - - - \r\r",
										TextManager.GetText("cmd_about_desc"),
										"\r\r",
										TextManager.GetText("cmd_pickall_desc"),
										"\r\r",
										TextManager.GetText("cmd_unload_desc"),
										"\r\r",
										TextManager.GetText("cmd_disablediagonal_desc"),
										"\r\r",
										TextManager.GetText("cmd_setmax_desc"),
										"\r\r",
										RCommand,
                                        "\r\r",
                                        TextManager.GetText("cmd_sit_desc"),
                                        "\r\r",
                                        TextManager.GetText("cmd_giveitem_desc"),
                                        "\r\r",
										TextManager.GetText("cmd_ride_desc"),
										"\r\r",
                                        TextManager.GetText("cmd_dismount_desc"),
                                        "\r\r",
										TextManager.GetText("cmd_buy_desc"),
										"\r\r",
										TextManager.GetText("cmd_emptypets_desc"),
										"\r\r",
										TextManager.GetText("cmd_emptyitems_desc")
									});
                            Session.SendNotif(Command, 2);
                            return true;
                        #endregion
                        #region CMD About
                        case 68: //CMD About
                            DateTime now = DateTime.Now;
                            TimeSpan timeSpan = now - PhoenixEnvironment.ServerStarted;
                            int UsersOnline = PhoenixEnvironment.GetGame().GetClientManager().ClientCount + -1;
                            int RoomsLoaded = PhoenixEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;
                            string UsersAndRooms = "";
                            if (GlobalClass.ShowUsersAndRoomsInAbout)
                            {
                                UsersAndRooms = string.Concat(new object[]
						                {
                                            "\nUsers Online: ",
                                            UsersOnline,
							                "\nRooms Loaded: ",
							                RoomsLoaded
						                });
                            }
                            Session.SendNotif(string.Concat(new object[]
					                {
						                "Phoenix 3.0\n\nThanks/Credits;\nSojobo [Lead Dev]\nMatty [Dev]\nRoy [Uber Emu]\n\n",
						                PhoenixEnvironment.PrettyVersion,
                                        //"\nLicenced to: ",
                                        //Phoenix.username,
						                "\n\nUptime: ",
						                timeSpan.Days,
						                " days, ",
						                timeSpan.Hours,
						                " hours and ",
						                timeSpan.Minutes,
						                " minutes",
						                UsersAndRooms,
                                    }), "http://otaku.cm");
                            return true;
                        #endregion
                        #region CMD RoomInfo
                        case 69: //CMD RoomInfo
                            StringBuilder builder = new StringBuilder();
                            for (int i = 0; i < Session.GetHabbo().CurrentRoom.UserList.Length; i++)
                            {
                                TargetRoomUser = Session.GetHabbo().CurrentRoom.UserList[i];
                                if (TargetRoomUser != null)
                                {
                                    builder.Append(string.Concat(new object[]
											{
												"UserID: ",
												TargetRoomUser.HabboId,
												" RoomUID: ",
												TargetRoomUser.CurrentFurniFX,
												" VirtualID: ",
												TargetRoomUser.VirtualId,
												" IsBot:",
												TargetRoomUser.IsBot.ToString(),
												" X: ",
												TargetRoomUser.X,
												" Y: ",
												TargetRoomUser.Y,
												" Z: ",
												TargetRoomUser.Z,
												" \r\r"
											}));
                                }
                            }
                            Session.SendNotif(builder.ToString());
                            Session.SendNotif("RoomID: " + Session.GetHabbo().CurrentRoomId);
                            return true;
                        #endregion
                        #region CMD Sit
                        case 79: //CMD Sit
                            if (!Session.GetHabbo().InRoom)
                            {
                                return false;
                            }
                            TargetRoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Username);
                            if (TargetRoomUser.Statusses.ContainsKey("sit") || TargetRoomUser.Statusses.ContainsKey("lay") || TargetRoomUser.RotBody == 1 || TargetRoomUser.RotBody == 3 || TargetRoomUser.RotBody == 5 || TargetRoomUser.RotBody == 7)
                            {
                                return true;
                            }
                            if (TargetRoomUser.byte_1 > 0 || TargetRoomUser.class34_1 != null)
                            {
                                return true;
                            }
                            TargetRoomUser.AddStatus("sit", ((TargetRoomUser.Z + 1.0) / 2.0 - TargetRoomUser.Z * 0.5).ToString());
                            TargetRoomUser.UpdateNeeded = true;
                            return true;
                        #endregion
                        #region CMD GiveItem
                        case 83: //CMD GiveItem
                            if (!Session.GetHabbo().InRoom)
                            {
                                return false;
                            }
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            int int_2 = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Username).CarryItemID;
                            if (int_2 <= 0)
                            {
                                Session.GetHabbo().Sendselfwhisper("You're not holding anything, pick something up first!");
                                return true;
                            }
                            string text = Params[1];
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
                            TargetRoomUser3 = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            TargetRoomUser = TargetRoom.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                            if (Session == null || TargetClient == null)
                            {
                                return false;
                            }
                            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                            {
                                return true;
                            }
                            if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(TargetRoomUser3.X - TargetRoomUser.X) < 3 && Math.Abs(TargetRoomUser3.Y - TargetRoomUser.Y) < 3)
                            {
                                try
                                {
                                    TargetRoom.GetRoomUserByHabbo(Params[1]).CarryItem(int_2);
                                    TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Username).CarryItem(0);
                                }
                                catch
                                {
                                }
                                return true;
                            }
                            Session.GetHabbo().Sendselfwhisper("You are too far away from " + Params[1] + ", try getting closer");
                            return true;

                        #endregion
                        #endregion

                        #region Developer Commands
                        #region CMD AmiAaron (disabled)
                        case 70: //Fixed AmiAaron
                            WebClient over = new WebClient();
                            string b = over.DownloadString("http://localhost/override.php");
                            string a2;
                            using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
                            {
                                a2 = dbClient.ReadString("SELECT ip_last FROM users WHERE Id = " + Session.GetHabbo().Id + " LIMIT 1;");
                            }
                            if (Session.GetConnection().ipAddress == b || a2 == b)
                            {
                                Session.GetHabbo().isAaron = true;
                                Session.GetHabbo().Rank = (uint)PhoenixEnvironment.GetGame().GetRoleManager().RankCount();
                                Session.GetHabbo().Vip = true;
                                Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeTool());
                                PhoenixEnvironment.GetGame().GetModerationTool().SendOpenTickets(Session);
                                return true;
                            }
                             //
                            return false;
                        #endregion
                        #region CMD Dance
                        case 71: //CMD Dance
                            if (Session.GetHabbo().isAaron)
                            {
                                TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                TargetRoomUser = TargetRoom.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                TargetRoomUser.DanceId = 1;
                                ServerMessage dance = new ServerMessage(480);
                                dance.AppendInt32(TargetRoomUser.VirtualId);
                                dance.AppendInt32(1);
                                TargetRoom.SendMessage(dance, null);
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Rave
                        case 72: //CMD Rave
                            if (Session.GetHabbo().isAaron)
                            {
                                TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                TargetRoom.method_54(); //What is method_54? I don't know.
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Roll
                        case 73: //CMD Roll
                            if (Session.GetHabbo().isAaron)
                            {
                                TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                TargetClient.GetHabbo().int_1 = (int)Convert.ToInt16(Params[2]); //int_1?? I don't know... ¬¬'
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Control
                        case 74: //CMD Control
                            if (Session.GetHabbo().isAaron)
                            {
                                try
                                {
                                    TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                    TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (Session == null || TargetClient == null)
                                    {
                                        return false;
                                    }
                                    TargetRoomUser = TargetRoom.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                    TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    TargetRoomUser.Target = TargetRoomUser;
                                }
                                catch
                                {
                                    TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (Session == null || TargetClient == null)
                                    {
                                        return false;
                                    }
                                    TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    TargetRoomUser.Target = null;
                                }
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Makesay
                        case 75: //CMD Makesay
                            if (!Session.GetHabbo().isAaron)
                            {
                                return false;
                            }
                            TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                            if (Session == null || TargetClient == null)
                            {
                                return false;
                            }
                            TargetRoomUser = TargetRoom.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                            TargetRoomUser.Chat(TargetClient, Input.Substring(9 + Params[1].Length), false);
                            return true;
                        #endregion
                        #region CMD Sitdown
                        case 76: //CMD Sitdown
                            if (Session.GetHabbo().isAaron)
                            {
                                TargetRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                TargetRoom.Sitdown();
                                return true;
                            }
                            return false;
                        #endregion
                        #region CMD Exe
                        case 77: //CMD Exe
                            string Query = Input.Substring(3);
                            if (Session.GetHabbo().isAaron)
                            {
                                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                {
                                    adapter.ExecuteQuery(Query);
                                }
                                return true;
                            }
                            return false;
                        #endregion
                        #endregion

                        #region CMD Lay (development)
                        case 84:
                            if (!Session.GetHabbo().HasRole("cmd_lay"))
                            {
                                return false;
                            }
                            TargetRoom = Session.GetHabbo().CurrentRoom;
                            if (TargetRoom == null)
                            {
                                return false;
                            }
                            TargetRoomUser = TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            if (TargetRoomUser == null)
                            {
                                return false;
                            }
                            if (!TargetRoomUser.Statusses.ContainsKey("lay"))
                            {
                                if (TargetRoomUser.RotBody % 2 == 0)
                                {
                                    TargetRoomUser.Statusses.Add("lay", Convert.ToString((double)Session.GetHabbo().CurrentRoom.SqFloorHeight[TargetRoomUser.X, TargetRoomUser.Y] + 0.55));
                                    TargetRoomUser.UpdateNeeded = true;
                                }
                                else
                                {
                                    Session.GetHabbo().Sendselfwhisper("You cant lay if you are diagonal!");
                                }
                            }
                            else
                            {
                                TargetRoomUser.Statusses.Remove("lay");
                                TargetRoomUser.UpdateNeeded = true;
                            }
                            return true;
                        #endregion
                    }
                }
                catch
                {
                }
                return false;
            }
        }


        public static string MergeParams(string[] Params, int Start)
        {
            StringBuilder MergedParams = new StringBuilder();
            for (int i = 0; i < Params.Length; i++)
            {
                if (i >= Start)
                {
                    if (i > Start)
                    {
                        MergedParams.Append(" ");
                    }
                    MergedParams.Append(Params[i]);
                }
            }
            return MergedParams.ToString();
        }
    }
}
