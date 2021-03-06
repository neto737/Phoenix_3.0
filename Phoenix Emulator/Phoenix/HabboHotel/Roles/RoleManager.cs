using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Roles
{
    internal class RoleManager
    {
        public Dictionary<string, int> CommandsList = new Dictionary<string, int>();
        public Dictionary<string, int> PetCommandsList = new Dictionary<string, int>();
        public Dictionary<uint, string> RankBadge = new Dictionary<uint, string>();
        public Dictionary<uint, int> RankFlood = new Dictionary<uint, int>();
        public Dictionary<uint, List<string>> RankPermissions = new Dictionary<uint, List<string>>();
        public Dictionary<uint, List<string>> UserPermissions = new Dictionary<uint, List<string>>();

        public void ClearPermissions()
        {
            RankBadge.Clear();
            UserPermissions.Clear();
            RankPermissions.Clear();
            RankFlood.Clear();
        }

        public bool ContainsRank(uint Rank)
        {
            return RankPermissions.ContainsKey(Rank);
        }

        public bool ContainsUser(uint UserID)
        {
            return UserPermissions.ContainsKey(UserID);
        }

        public int FloodTime(uint RankId)
        {
            return RankFlood[RankId];
        }

        public List<string> GetRightsForHabbo(uint UserID, uint Rank)
        {
            List<string> list = new List<string>();
            if (this.ContainsUser(UserID))
            {
                return UserPermissions[UserID];
            }
            return RankPermissions[Rank];
        }

        public bool HasWiredConditionRole(string command, GameClient Session)
        {
            switch (command)
            {
                case "roomuserseq":
                case "roomuserslt":
                case "roomusersmt":
                case "roomusersmte":
                case "roomuserslte":
                    if (!Session.GetHabbo().HasRole("wired_cnd_roomusers"))
                    {
                        break;
                    }
                    return true;

                case "userhasachievement":
                case "userhasntachievement":
                    if (!Session.GetHabbo().HasRole("wired_cnd_userhasachievement"))
                    {
                        break;
                    }
                    return true;

                case "userhasbadge":
                case "userhasntbadge":
                    if (!Session.GetHabbo().HasRole("wired_cnd_userhasbadge"))
                    {
                        break;
                    }
                    return true;

                case "userhasvip":
                case "userhasntvip":
                    if (!Session.GetHabbo().HasRole("wired_cnd_userhasvip"))
                    {
                        break;
                    }
                    return true;

                case "userhaseffect":
                case "userhasnteffect":
                    if (!Session.GetHabbo().HasRole("wired_cnd_userhaseffect"))
                    {
                        break;
                    }
                    return true;

                case "userrankeq":
                case "userrankmt":
                case "userrankmte":
                case "userranklt":
                case "userranklte":
                    if (!Session.GetHabbo().HasRole("wired_cnd_userrank"))
                    {
                        break;
                    }
                    return true;

                case "usercreditseq":
                case "usercreditsmt":
                case "usercreditsmte":
                case "usercreditslt":
                case "usercreditslte":
                    if (!Session.GetHabbo().HasRole("wired_cnd_usercredits"))
                    {
                        break;
                    }
                    return true;

                case "userpixelseq":
                case "userpixelsmt":
                case "userpixelsmte":
                case "userpixelslt":
                case "userpixelslte":
                    if (!Session.GetHabbo().HasRole("wired_cnd_userpixels"))
                    {
                        break;
                    }
                    return true;

                case "userpointseq":
                case "userpointsmt":
                case "userpointsmte":
                case "userpointslt":
                case "userpointslte":
                    if (!Session.GetHabbo().HasRole("wired_cnd_userpoints"))
                    {
                        break;
                    }
                    return true;

                case "usergroupeq":
                case "userisingroup":
                    if (!Session.GetHabbo().HasRole("wired_cnd_usergroups"))
                    {
                        break;
                    }
                    return true;

                case "wearing":
                case "notwearing":
                    if (!Session.GetHabbo().HasRole("wired_cnd_wearing"))
                    {
                        break;
                    }
                    return true;

                case "carrying":
                case "notcarrying":
                    if (!Session.GetHabbo().HasRole("wired_cnd_carrying"))
                    {
                        break;
                    }
                    return true;
            }
            return false;
        }

        public bool HasWiredEffectRole(string command, GameClient Session)
        {
            switch (command)
            {
                case "sql":
                    if (!Session.GetHabbo().HasRole("wired_give_sql"))
                    {
                        break;
                    }
                    return true;

                case "badge":
                    if (!Session.GetHabbo().HasRole("wired_give_badge"))
                    {
                        break;
                    }
                    return true;

                case "effect":
                    if (!Session.GetHabbo().HasRole("wired_give_effect"))
                    {
                        break;
                    }
                    return true;

                case "award":
                    if (!Session.GetHabbo().HasRole("wired_give_award"))
                    {
                        break;
                    }
                    return true;

                case "dance":
                    if (!Session.GetHabbo().HasRole("wired_give_dance"))
                    {
                        break;
                    }
                    return true;

                case "send":
                    if (!Session.GetHabbo().HasRole("wired_give_send"))
                    {
                        break;
                    }
                    return true;

                case "credits":
                    if (!Session.GetHabbo().HasRole("wired_give_credits"))
                    {
                        break;
                    }
                    return true;

                case "pixels":
                    if (!Session.GetHabbo().HasRole("wired_give_pixels"))
                    {
                        break;
                    }
                    return true;

                case "points":
                    if (!Session.GetHabbo().HasRole("wired_give_points"))
                    {
                        break;
                    }
                    return true;

                case "rank":
                    if (!Session.GetHabbo().HasRole("wired_give_rank"))
                    {
                        break;
                    }
                    return true;

                case "respect":
                    if (!Session.GetHabbo().HasRole("wired_give_respect"))
                    {
                        break;
                    }
                    return true;

                case "handitem":
                    if (!Session.GetHabbo().HasRole("wired_give_handitem"))
                    {
                        break;
                    }
                    return true;

                case "alert":
                    if (!Session.GetHabbo().HasRole("wired_give_alert"))
                    {
                        break;
                    }
                    return true;
            }
            return false;
        }

        public void LoadRoles(DatabaseClient adapter)
        {
            Logging.Write(TextManager.GetText("emu_loadroles"));
            this.ClearPermissions();
            DataTable table = adapter.ReadDataTable("SELECT * FROM ranks ORDER BY id ASC;");
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    RankBadge.Add((uint)row["id"], row["badgeid"].ToString());
                }
            }
            table = adapter.ReadDataTable("SELECT * FROM permissions_users ORDER BY userid ASC;");
            if (table != null)
            {
                foreach (DataRow Row in table.Rows)
                {
                    List<string> Command = new List<string>();
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_settings"].ToString()))
                    {
                        Command.Add("cmd_update_settings");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_bans"].ToString()))
                    {
                        Command.Add("cmd_update_bans");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_bots"].ToString()))
                    {
                        Command.Add("cmd_update_bots");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_catalogue"].ToString()))
                    {
                        Command.Add("cmd_update_catalogue");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_navigator"].ToString()))
                    {
                        Command.Add("cmd_update_navigator");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_items"].ToString()))
                    {
                        Command.Add("cmd_update_items");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_award"].ToString()))
                    {
                        Command.Add("cmd_award");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_coords"].ToString()))
                    {
                        Command.Add("cmd_coords");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_override"].ToString()))
                    {
                        Command.Add("cmd_override");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_coins"].ToString()))
                    {
                        Command.Add("cmd_coins");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_pixels"].ToString()))
                    {
                        Command.Add("cmd_pixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_ha"].ToString()))
                    {
                        Command.Add("cmd_ha");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_hal"].ToString()))
                    {
                        Command.Add("cmd_hal");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_freeze"].ToString()))
                    {
                        Command.Add("cmd_freeze");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_roommute"].ToString()))
                    {
                        Command.Add("cmd_roommute");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_setspeed"].ToString()))
                    {
                        Command.Add("cmd_setspeed");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_masscredits"].ToString()))
                    {
                        Command.Add("cmd_masscredits");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_globalcredits"].ToString()))
                    {
                        Command.Add("cmd_globalcredits");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_masspixels"].ToString()))
                    {
                        Command.Add("cmd_masspixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_globalpixels"].ToString()))
                    {
                        Command.Add("cmd_globalpixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_roombadge"].ToString()))
                    {
                        Command.Add("cmd_roombadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_massbadge"].ToString()))
                    {
                        Command.Add("cmd_massbadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_userinfo"].ToString()))
                    {
                        Command.Add("cmd_userinfo");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_userinfo_viewip"].ToString()))
                    {
                        Command.Add("cmd_userinfo_viewip");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_shutdown"].ToString()))
                    {
                        Command.Add("cmd_shutdown");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_givebadge"].ToString()))
                    {
                        Command.Add("cmd_givebadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_removebadge"].ToString()))
                    {
                        Command.Add("cmd_removebadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_summon"].ToString()))
                    {
                        Command.Add("cmd_summon");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_invisible"].ToString()))
                    {
                        Command.Add("cmd_invisible");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_ban"].ToString()))
                    {
                        Command.Add("cmd_ban");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_superban"].ToString()))
                    {
                        Command.Add("cmd_superban");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_roomkick"].ToString()))
                    {
                        Command.Add("cmd_roomkick");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_roomalert"].ToString()))
                    {
                        Command.Add("cmd_roomalert");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_mute"].ToString()))
                    {
                        Command.Add("cmd_mute");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_unmute"].ToString()))
                    {
                        Command.Add("cmd_unmute");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_alert"].ToString()))
                    {
                        Command.Add("cmd_alert");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_motd"].ToString()))
                    {
                        Command.Add("cmd_motd");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_kick"].ToString()))
                    {
                        Command.Add("cmd_kick");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_filter"].ToString()))
                    {
                        Command.Add("cmd_update_filter");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_permissions"].ToString()))
                    {
                        Command.Add("cmd_update_permissions");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_sa"].ToString()))
                    {
                        Command.Add("cmd_sa");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["receive_sa"].ToString()))
                    {
                        Command.Add("receive_sa");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_ipban"].ToString()))
                    {
                        Command.Add("cmd_ipban");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_spull"].ToString()))
                    {
                        Command.Add("cmd_spull");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_disconnect"].ToString()))
                    {
                        Command.Add("cmd_disconnect");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_achievements"].ToString()))
                    {
                        Command.Add("cmd_update_achievements");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_texts"].ToString()))
                    {
                        Command.Add("cmd_update_texts");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_teleport"].ToString()))
                    {
                        Command.Add("cmd_teleport");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_points"].ToString()))
                    {
                        Command.Add("cmd_points");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_masspoints"].ToString()))
                    {
                        Command.Add("cmd_masspoints");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_globalpoints"].ToString()))
                    {
                        Command.Add("cmd_globalpoints");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_empty"].ToString()))
                    {
                        Command.Add("cmd_empty");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["ignore_roommute"].ToString()))
                    {
                        Command.Add("ignore_roommute");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_anyroomrights"].ToString()))
                    {
                        Command.Add("acc_anyroomrights");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_anyroomowner"].ToString()))
                    {
                        Command.Add("acc_anyroomowner");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_supporttool"].ToString()))
                    {
                        Command.Add("acc_supporttool");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_chatlogs"].ToString()))
                    {
                        Command.Add("acc_chatlogs");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_enter_fullrooms"].ToString()))
                    {
                        Command.Add("acc_enter_fullrooms");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_enter_anyroom"].ToString()))
                    {
                        Command.Add("acc_enter_anyroom");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_restrictedrooms"].ToString()))
                    {
                        Command.Add("acc_restrictedrooms");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_unkickable"].ToString()))
                    {
                        Command.Add("acc_unkickable");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_unbannable"].ToString()))
                    {
                        Command.Add("acc_unbannable");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["ignore_friendsettings"].ToString()))
                    {
                        Command.Add("ignore_friendsettings");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_sql"].ToString()))
                    {
                        Command.Add("wired_give_sql");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_badge"].ToString()))
                    {
                        Command.Add("wired_give_badge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_effect"].ToString()))
                    {
                        Command.Add("wired_give_effect");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_award"].ToString()))
                    {
                        Command.Add("wired_give_award");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_dance"].ToString()))
                    {
                        Command.Add("wired_give_dance");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_send"].ToString()))
                    {
                        Command.Add("wired_give_send");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_credits"].ToString()))
                    {
                        Command.Add("wired_give_credits");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_pixels"].ToString()))
                    {
                        Command.Add("wired_give_pixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_points"].ToString()))
                    {
                        Command.Add("wired_give_points");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_rank"].ToString()))
                    {
                        Command.Add("wired_give_rank");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_respect"].ToString()))
                    {
                        Command.Add("wired_give_respect");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_handitem"].ToString()))
                    {
                        Command.Add("wired_give_handitem");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_alert"].ToString()))
                    {
                        Command.Add("wired_give_alert");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_roomusers"].ToString()))
                    {
                        Command.Add("wired_cnd_roomusers");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userhasachievement"].ToString()))
                    {
                        Command.Add("wired_cnd_userhasachievement");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userhasbadge"].ToString()))
                    {
                        Command.Add("wired_cnd_userhasbadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userhasvip"].ToString()))
                    {
                        Command.Add("wired_cnd_userhasvip");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userhaseffect"].ToString()))
                    {
                        Command.Add("wired_cnd_userhaseffect");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userrank"].ToString()))
                    {
                        Command.Add("wired_cnd_userrank");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_usercredits"].ToString()))
                    {
                        Command.Add("wired_cnd_usercredits");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userpixels"].ToString()))
                    {
                        Command.Add("wired_cnd_userpixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userpoints"].ToString()))
                    {
                        Command.Add("wired_cnd_userpoints");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_usergroups"].ToString()))
                    {
                        Command.Add("wired_cnd_usergroups");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_wearing"].ToString()))
                    {
                        Command.Add("wired_cnd_wearing");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_carrying"].ToString()))
                    {
                        Command.Add("wired_cnd_carrying");
                    }
                    this.UserPermissions.Add((uint)Row["userid"], Command);
                }
            }
            table = adapter.ReadDataTable("SELECT * FROM permissions_ranks ORDER BY rank ASC;");
            if (table != null)
            {
                foreach (DataRow Row in table.Rows)
                {
                    this.RankFlood.Add((uint)Row["rank"], (int)Row["floodtime"]);
                }
                foreach (DataRow Row in table.Rows)
                {
                    List<string> list2 = new List<string>();
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_settings"].ToString()))
                    {
                        list2.Add("cmd_update_settings");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_bans"].ToString()))
                    {
                        list2.Add("cmd_update_bans");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_bots"].ToString()))
                    {
                        list2.Add("cmd_update_bots");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_catalogue"].ToString()))
                    {
                        list2.Add("cmd_update_catalogue");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_navigator"].ToString()))
                    {
                        list2.Add("cmd_update_navigator");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_items"].ToString()))
                    {
                        list2.Add("cmd_update_items");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_award"].ToString()))
                    {
                        list2.Add("cmd_award");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_coords"].ToString()))
                    {
                        list2.Add("cmd_coords");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_override"].ToString()))
                    {
                        list2.Add("cmd_override");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_coins"].ToString()))
                    {
                        list2.Add("cmd_coins");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_pixels"].ToString()))
                    {
                        list2.Add("cmd_pixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_ha"].ToString()))
                    {
                        list2.Add("cmd_ha");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_hal"].ToString()))
                    {
                        list2.Add("cmd_hal");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_freeze"].ToString()))
                    {
                        list2.Add("cmd_freeze");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_roommute"].ToString()))
                    {
                        list2.Add("cmd_roommute");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_setspeed"].ToString()))
                    {
                        list2.Add("cmd_setspeed");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_masscredits"].ToString()))
                    {
                        list2.Add("cmd_masscredits");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_globalcredits"].ToString()))
                    {
                        list2.Add("cmd_globalcredits");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_masspixels"].ToString()))
                    {
                        list2.Add("cmd_masspixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_globalpixels"].ToString()))
                    {
                        list2.Add("cmd_globalpixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_roombadge"].ToString()))
                    {
                        list2.Add("cmd_roombadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_massbadge"].ToString()))
                    {
                        list2.Add("cmd_massbadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_userinfo"].ToString()))
                    {
                        list2.Add("cmd_userinfo");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_userinfo_viewip"].ToString()))
                    {
                        list2.Add("cmd_userinfo_viewip");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_shutdown"].ToString()))
                    {
                        list2.Add("cmd_shutdown");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_givebadge"].ToString()))
                    {
                        list2.Add("cmd_givebadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_removebadge"].ToString()))
                    {
                        list2.Add("cmd_removebadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_summon"].ToString()))
                    {
                        list2.Add("cmd_summon");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_invisible"].ToString()))
                    {
                        list2.Add("cmd_invisible");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_ban"].ToString()))
                    {
                        list2.Add("cmd_ban");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_superban"].ToString()))
                    {
                        list2.Add("cmd_superban");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_roomkick"].ToString()))
                    {
                        list2.Add("cmd_roomkick");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_roomalert"].ToString()))
                    {
                        list2.Add("cmd_roomalert");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_mute"].ToString()))
                    {
                        list2.Add("cmd_mute");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_unmute"].ToString()))
                    {
                        list2.Add("cmd_unmute");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_alert"].ToString()))
                    {
                        list2.Add("cmd_alert");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_motd"].ToString()))
                    {
                        list2.Add("cmd_motd");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_kick"].ToString()))
                    {
                        list2.Add("cmd_kick");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_filter"].ToString()))
                    {
                        list2.Add("cmd_update_filter");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_permissions"].ToString()))
                    {
                        list2.Add("cmd_update_permissions");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_sa"].ToString()))
                    {
                        list2.Add("cmd_sa");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["receive_sa"].ToString()))
                    {
                        list2.Add("receive_sa");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_ipban"].ToString()))
                    {
                        list2.Add("cmd_ipban");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_spull"].ToString()))
                    {
                        list2.Add("cmd_spull");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_disconnect"].ToString()))
                    {
                        list2.Add("cmd_disconnect");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_achievements"].ToString()))
                    {
                        list2.Add("cmd_update_achievements");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_update_texts"].ToString()))
                    {
                        list2.Add("cmd_update_texts");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_teleport"].ToString()))
                    {
                        list2.Add("cmd_teleport");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_points"].ToString()))
                    {
                        list2.Add("cmd_points");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_masspoints"].ToString()))
                    {
                        list2.Add("cmd_masspoints");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_globalpoints"].ToString()))
                    {
                        list2.Add("cmd_globalpoints");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmd_empty"].ToString()))
                    {
                        list2.Add("cmd_empty");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["ignore_roommute"].ToString()))
                    {
                        list2.Add("ignore_roommute");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_anyroomrights"].ToString()))
                    {
                        list2.Add("acc_anyroomrights");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_anyroomowner"].ToString()))
                    {
                        list2.Add("acc_anyroomowner");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_supporttool"].ToString()))
                    {
                        list2.Add("acc_supporttool");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_chatlogs"].ToString()))
                    {
                        list2.Add("acc_chatlogs");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_enter_fullrooms"].ToString()))
                    {
                        list2.Add("acc_enter_fullrooms");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_enter_anyroom"].ToString()))
                    {
                        list2.Add("acc_enter_anyroom");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_restrictedrooms"].ToString()))
                    {
                        list2.Add("acc_restrictedrooms");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_unkickable"].ToString()))
                    {
                        list2.Add("acc_unkickable");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["acc_unbannable"].ToString()))
                    {
                        list2.Add("acc_unbannable");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["ignore_friendsettings"].ToString()))
                    {
                        list2.Add("ignore_friendsettings");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_sql"].ToString()))
                    {
                        list2.Add("wired_give_sql");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_badge"].ToString()))
                    {
                        list2.Add("wired_give_badge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_effect"].ToString()))
                    {
                        list2.Add("wired_give_effect");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_award"].ToString()))
                    {
                        list2.Add("wired_give_award");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_dance"].ToString()))
                    {
                        list2.Add("wired_give_dance");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_send"].ToString()))
                    {
                        list2.Add("wired_give_send");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_credits"].ToString()))
                    {
                        list2.Add("wired_give_credits");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_pixels"].ToString()))
                    {
                        list2.Add("wired_give_pixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_points"].ToString()))
                    {
                        list2.Add("wired_give_points");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_rank"].ToString()))
                    {
                        list2.Add("wired_give_rank");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_respect"].ToString()))
                    {
                        list2.Add("wired_give_respect");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_handitem"].ToString()))
                    {
                        list2.Add("wired_give_handitem");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_give_alert"].ToString()))
                    {
                        list2.Add("wired_give_alert");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_roomusers"].ToString()))
                    {
                        list2.Add("wired_cnd_roomusers");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userhasachievement"].ToString()))
                    {
                        list2.Add("wired_cnd_userhasachievement");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userhasbadge"].ToString()))
                    {
                        list2.Add("wired_cnd_userhasbadge");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userhasvip"].ToString()))
                    {
                        list2.Add("wired_cnd_userhasvip");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userhaseffect"].ToString()))
                    {
                        list2.Add("wired_cnd_userhaseffect");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userrank"].ToString()))
                    {
                        list2.Add("wired_cnd_userrank");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_usercredits"].ToString()))
                    {
                        list2.Add("wired_cnd_usercredits");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userpixels"].ToString()))
                    {
                        list2.Add("wired_cnd_userpixels");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_userpoints"].ToString()))
                    {
                        list2.Add("wired_cnd_userpoints");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_usergroups"].ToString()))
                    {
                        list2.Add("wired_cnd_usergroups");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_wearing"].ToString()))
                    {
                        list2.Add("wired_cnd_wearing");
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["wired_cnd_carrying"].ToString()))
                    {
                        list2.Add("wired_cnd_carrying");
                    }
                    this.RankPermissions.Add((uint)Row["rank"], list2);
                }
            }
            table = adapter.ReadDataTable("SELECT * FROM permissions_vip;");
            if (table != null)
            {
                GlobalClass.cmdPushEnabled = false;
                GlobalClass.cmdPullEnabled = false;
                GlobalClass.cmdFlagmeEnabled = false;
                GlobalClass.cmdMimicEnabled = false;
                GlobalClass.cmdMoonwalkEnabled = false;
                GlobalClass.cmdFollowEnabled = false;
                GlobalClass.cmdFacelessEnabled = false;
                GlobalClass.cmdEnableEnabled = false;

                foreach (DataRow Row in table.Rows)
                {
                    if (PhoenixEnvironment.EnumToBool(Row["cmdPush"].ToString()))
                    {
                        GlobalClass.cmdPushEnabled = true;
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmdPull"].ToString()))
                    {
                        GlobalClass.cmdPullEnabled = true;
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmdFlagme"].ToString()))
                    {
                        GlobalClass.cmdFlagmeEnabled = true;
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmdMimic"].ToString()))
                    {
                        GlobalClass.cmdMimicEnabled = true;
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmdMoonwalk"].ToString()))
                    {
                        GlobalClass.cmdMoonwalkEnabled = true;
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmdFaceless"].ToString()))
                    {
                        GlobalClass.cmdFacelessEnabled = true;
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmdEnable"].ToString()))
                    {
                        GlobalClass.cmdEnableEnabled = true;
                    }
                    if (PhoenixEnvironment.EnumToBool(Row["cmdFollow"].ToString()))
                    {
                        GlobalClass.cmdFollowEnabled = true;
                    }
                }
            }
            this.PetCommandsList.Clear();
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_sleep"), 1);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_free"), 2);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_sit"), 3);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_lay"), 4);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_stay"), 5);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_here"), 6);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_dead"), 7);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_beg"), 8);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_jump"), 9);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_stfu"), 10);
            this.PetCommandsList.Add(TextManager.GetText("pet_cmd_talk"), 11);
            this.CommandsList.Clear();
            this.CommandsList.Add(TextManager.GetText("cmd_about_name"), 1);
            this.CommandsList.Add(TextManager.GetText("cmd_alert_name"), 2); //Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_award_name"), 3);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_ban_name"), 4);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_buy_name"), 5);//------ Normal User
            this.CommandsList.Add(TextManager.GetText("cmd_coins_name"), 6);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_coords_name"), 7);//------ Normal/VIP User
            this.CommandsList.Add(TextManager.GetText("cmd_disablediagonal_name"), 8);//--------- Normal/VIP user
            this.CommandsList.Add(TextManager.GetText("cmd_emptyitems_name"), 9);//---------- Normal user
            this.CommandsList.Add(TextManager.GetText("cmd_empty_name"), 10);//---------- normal  user
            this.CommandsList.Add(TextManager.GetText("cmd_enable_name"), 11);//------- VIP user
            this.CommandsList.Add(TextManager.GetText("cmd_flagme_name"), 12);//------- VIP user
            this.CommandsList.Add(TextManager.GetText("cmd_follow_name"), 13);//------- VIP user
            this.CommandsList.Add(TextManager.GetText("cmd_freeze_name"), 14);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_givebadge_name"), 15);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_globalcredits_name"), 16);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_globalpixels_name"), 17);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_globalpoints_name"), 18);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_hal_name"), 19);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_ha_name"), 20);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_invisible_name"), 21);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_ipban_name"), 22);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_kick_name"), 23);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_massbadge_name"), 24);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_masscredits_name"), 25);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_masspixels_name"), 26);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_masspoints_name"), 27);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_mimic_name"), 28);//VIP
            this.CommandsList.Add(TextManager.GetText("cmd_moonwalk_name"), 29);//VIP
            this.CommandsList.Add(TextManager.GetText("cmd_motd_name"), 30);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_mute_name"), 31);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_override_name"), 32);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_pickall_name"), 33);//normal user
            this.CommandsList.Add(TextManager.GetText("cmd_pixels_name"), 34);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_points_name"), 35);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_pull_name"), 36);//VIP
            this.CommandsList.Add(TextManager.GetText("cmd_push_name"), 37);//VIP
            this.CommandsList.Add(TextManager.GetText("cmd_redeemcreds_name"), 38);
            this.CommandsList.Add(TextManager.GetText("cmd_removebadge_name"), 39);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_ride_name"), 40);
            this.CommandsList.Add(TextManager.GetText("cmd_roomalert_name"), 41);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_roombadge_name"), 42);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_roomkick_name"), 43);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_roommute_name"), 44);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_sa_name"), 45);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_setmax_name"), 46);
            this.CommandsList.Add(TextManager.GetText("cmd_setspeed_name"), 47);
            this.CommandsList.Add(TextManager.GetText("cmd_shutdown_name"), 48);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_spull_name"), 49);
            this.CommandsList.Add(TextManager.GetText("cmd_summon_name"), 50);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_superban_name"), 51);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_teleport_name"), 52);
            this.CommandsList.Add(TextManager.GetText("cmd_unload_name"), 53);
            this.CommandsList.Add(TextManager.GetText("cmd_unmute_name"), 54);
            this.CommandsList.Add(TextManager.GetText("cmd_update_achievements_name"), 55);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_update_bans_name"), 56);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_update_bots_name"), 57);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_update_catalogue_name"), 58);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_update_filter_name"), 59);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_update_items_name"), 60);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_update_navigator_name"), 61);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_update_permissions_name"), 62);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_update_settings_name"), 63);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_userinfo_name"), 64);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_update_texts_name"), 65);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_disconnect_name"), 66);//Maked Command - Moderation ok
            this.CommandsList.Add(TextManager.GetText("cmd_commands_name"), 67);
            this.CommandsList.Add("about", 68);
            this.CommandsList.Add(TextManager.GetText("cmd_roominfo_name"), 69);
            this.CommandsList.Add("amiaaron", 70); //Developer command
            this.CommandsList.Add("dance", 71); //Developer command
            this.CommandsList.Add("rave", 72); //Developer command
            this.CommandsList.Add("roll", 73); //Developer command
            this.CommandsList.Add("control", 74); //Developer command
            this.CommandsList.Add("makesay", 75); //Developer command
            this.CommandsList.Add("sitdown", 76); //Developer command
            this.CommandsList.Add("exe", 77); //Developer command
            this.CommandsList.Add(TextManager.GetText("cmd_sit_name"), 79); //normal user
            this.CommandsList.Add(TextManager.GetText("cmd_dismount_name"), 80); //normal user
            this.CommandsList.Add(TextManager.GetText("cmd_emptypets_name"), 82); //normal user
            this.CommandsList.Add("getoff", 81); //Developer command
            this.CommandsList.Add(TextManager.GetText("cmd_giveitem_name"), 83); //Developer command
            this.CommandsList.Add("lay", 84); //normal user
            this.CommandsList.Add(TextManager.GetText("cmd_faceless_name"), 85);
            Logging.WriteLine("completed!");
            try
            {
                if (int.Parse(PhoenixEnvironment.GetConfig().data["debug"]) == 1)
                {
                    Logging.WriteLine("Commands loaded:" + this.CommandsList.Count.ToString());
                }
            }
            catch { }
        }

        public int RankCount()
        {
            return RankBadge.Count;
        }

        public bool RankHasRight(uint RankId, string Role)
        {
            if (!ContainsRank(RankId))
            {
                return false;
            }
            List<string> list = this.RankPermissions[RankId];
            return list.Contains(Role);
        }

        public string RanksBadge(uint Rank)
        {
            return RankBadge[Rank];
        }

        public bool UserHasPermission(uint UserID, string Role)
        {
            if (!ContainsUser(UserID))
            {
                return false;
            }
            List<string> Permission = this.UserPermissions[UserID];
            return Permission.Contains(Role);
        }

        public bool UserHasPersonalPermissions(uint UserID)
        {
            return ContainsUser(UserID);
        }
    }
}
