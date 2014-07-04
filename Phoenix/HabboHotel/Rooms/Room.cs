using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Phoenix.Core;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Pets;
using Phoenix.HabboHotel.Pathfinding;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.HabboHotel.RoomBots;
using Phoenix.HabboHotel.Navigators;
using Phoenix.Storage;
using Phoenix.HabboHotel.Guilds;
using Phoenix.HabboHotel.SoundMachine;
using Phoenix.HabboHotel.Users;
namespace Phoenix.HabboHotel.Rooms
{
    internal sealed class Room
    {
        public delegate void Delegate2(int Team);
        private uint Id;
        public uint Achievement;
        public string Name;
        public string Description;
        public string Type;
        public string Owner;
        public string Password;
        public int Category;
        public int State;
        public int UsersNow;
        public int UsersMax;
        public string ModelName;
        public string CCTs;
        public int Score;
        public List<string> Tags;
        public bool AllowPet;
        public bool AllowPetsEating;
        public bool AllowWalkthrough;
        public bool Hidewall;
        public int Wallthick;
        public int Floorthick;
        internal bool RoomMuted;
        internal bool bool_5;
        private Timer timer_0;
        private bool bool_6;
        private bool bool_7;
        internal RoomUser[] UserList;
        public int int_7 = 0;
        private int int_8;
        public RoomIcon class29_0;
        public List<uint> UsersWithRights;
        internal bool bool_8;
        private Dictionary<uint, double> dictionary_0;
        public RoomEvent Event;
        public string Wallpaper;
        public string Floor;
        public string Landscape;
        private Hashtable mFloorItems;
        private Hashtable mRemovedItems;
        private Hashtable mMovedItems;
        private Hashtable mAddedItems;
        private Hashtable mWallItems;
        public MoodlightData MoodlightData;
        public List<Trade> list_2;
        public bool bool_9;
        public List<RoomItem> StickiePoles;
        public List<uint> list_4;
        public List<RoomItem> bbTiles;
        public List<RoomItem> bbrTiles;
        public List<RoomItem> bbgTiles;
        public List<RoomItem> bbyTiles;
        public List<RoomItem> bbbTiles;
        public List<RoomItem> RedScoreboards;
        public List<RoomItem> YellowScoreboards;
        public List<RoomItem> BlueScoreboards;
        public List<RoomItem> GreenScoreboards;
        public int int_9;
        public int int_10;
        public int int_11;
        public int int_12;
        public int int_13;
        private bool bool_10;
        public List<RoomItem> WF_Triggers;
        public List<RoomItem> WF_Effects;
        public List<RoomItem> WF_Conditions;
        public List<Guild> list_17;
        public double[,] double_0;
        private byte[,] byte_0;
        public Coord[,] mBedMap;
        private byte[,] byte_1;
        private byte[,] byte_2;
        private double[,] double_1;
        private double[,] double_2;
        private RoomModel class28_0;
        private bool mGotRollers;
        private int int_14;
        private int int_15;
        private RoomData class27_0;
        private RoomMusicController musicController;
        private int int_16;
        private bool bool_12;
        public bool HasOngoingEvent
        {
            get
            {
                return this.Event != null;
            }
        }
        public RoomIcon myIcon
        {
            get
            {
                return this.class29_0;
            }
            set
            {
                this.class29_0 = value;
            }
        }
        internal bool Boolean_1
        {
            get
            {
                return this.mGotRollers;
            }
            set
            {
                this.mGotRollers = value;
            }
        }
        internal bool GotMusicController()
        {
            return (this.musicController != null);
        }
        public int UserCount
        {
            get
            {
                int num = 0;
                int result;
                if (this.UserList == null)
                {
                    result = 0;
                }
                else
                {
                    for (int i = 0; i < this.UserList.Length; i++)
                    {
                        if (this.UserList[i] != null && !this.UserList[i].IsBot && !this.UserList[i].IsPet)
                        {
                            num++;
                        }
                    }
                    result = num;
                }
                return result;
            }
        }
        public int Int32_1
        {
            get
            {
                return this.Tags.Count;
            }
        }
        public RoomModel Model
        {
            get
            {
                return this.class28_0;
            }
        }
        public uint RoomId
        {
            get
            {
                return this.Id;
            }
        }
        public Hashtable Hashtable_0
        {
            get
            {
                Hashtable result;
                if (this.mFloorItems != null)
                {
                    result = (this.mFloorItems.Clone() as Hashtable);
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }
        public Hashtable Hashtable_1
        {
            get
            {
                return this.mWallItems.Clone() as Hashtable;
            }
        }
        public bool Boolean_2
        {
            get
            {
                if (this.IsPublic)
                {
                    return false;
                }
                else
                {
                    FlatCat @class = PhoenixEnvironment.GetGame().GetNavigator().method_2(this.Category);
                    return (@class != null && @class.CanTrade);
                }
            }
        }
        public bool IsPublic
        {
            get
            {
                return this.Type == "public";
            }
        }
        public int Int32_2
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.UserList.Length; i++)
                {
                    RoomUser @class = this.UserList[i];
                    if (@class != null && @class.IsPet)
                    {
                        num++;
                    }
                }
                return num;
            }
        }
        internal RoomData Class27_0
        {
            get
            {
                this.class27_0.Fill(this);
                return this.class27_0;
            }
        }
        public byte[,] Byte_0
        {
            get
            {
                return this.byte_0;
            }
        }
        internal bool Boolean_4
        {
            get
            {
                return this.method_2().Count > 0;
            }
        }

        internal RoomMusicController GetRoomMusicController()
        {
            if (this.musicController == null)
            {
                this.musicController = new RoomMusicController();
            }
            return this.musicController;
        }

        internal void LoadMusic()
        {
            DataTable table;
            DataTable table2;
            using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
            {
                table = @class.ReadDataTable("SELECT * FROM items_rooms_songs WHERE roomid = '" + Id + "'"); // <-- old
                table2 = @class.ReadDataTable("SELECT * FROM items_jukebox_songs WHERE jukeboxid = '" + this.GetRoomMusicController().LinkedItemId + "'"); // <-- new
            }

            foreach (DataRow row in table.Rows)
            {
                int songID = (int)row["songid"];
                uint num2 = (uint)row["itemid"];
                int baseItem = (int)row["baseitem"];
                SongItem diskItem = new SongItem((int)num2, songID, baseItem);
                diskItem.RemoveFromDatabase();
                diskItem.SaveToDatabase(this.GetRoomMusicController().LinkedItemId);
                this.GetRoomMusicController().AddDisk(diskItem);
            }

            foreach (DataRow row in table2.Rows)
            {
                int songID = (int)row["songid"];
                uint num2 = (uint)row["itemid"];
                int baseItem = (int)row["baseitem"];
                SongItem diskItem = new SongItem((int)num2, songID, baseItem);
                this.GetRoomMusicController().AddDisk(diskItem);
            }
        }

        public Room(uint uint_2, string string_10, string string_11, string string_12, string string_13, int int_17, int int_18, int int_19, string string_14, string string_15, int int_20, List<string> list_18, bool bool_13, bool bool_14, bool bool_15, bool bool_16, RoomIcon class29_1, string string_16, string string_17, string string_18, string string_19, RoomData class27_1, bool bool_17, int int_21, int int_22, uint uint_3)
        {
            this.bool_12 = false;
            this.Id = uint_2;
            this.Name = string_10;
            this.Description = string_11;
            this.Owner = string_13;
            this.Category = int_17;
            this.Type = string_12;
            this.State = int_18;
            this.UsersNow = 0;
            this.UsersMax = int_19;
            this.ModelName = string_14;
            this.CCTs = string_15;
            this.Score = int_20;
            this.Tags = list_18;
            this.AllowPet = bool_13;
            this.AllowPetsEating = bool_14;
                this.AllowWalkthrough = bool_15;
                this.Hidewall = bool_16;
                this.Wallthick = int_21;
                this.Floorthick = int_22;
                this.int_7 = 0;
                this.UserList = new RoomUser[500];
                this.class29_0 = class29_1;
                this.Password = string_16;
                this.dictionary_0 = new Dictionary<uint, double>();
                this.Event = null;
                this.Wallpaper = string_17;
                this.Floor = string_18;
                this.Landscape = string_19;
                this.mWallItems = new Hashtable();
                this.mFloorItems = new Hashtable();
                this.list_2 = new List<Trade>();
                this.class28_0 = PhoenixEnvironment.GetGame().GetRoomManager().GetModel(this.ModelName, this.Id);
                this.bool_6 = false;
                this.bool_7 = false;
                this.bool_5 = true;
                this.class27_0 = class27_1;
                this.bool_8 = bool_17;
                this.list_17 = new List<Guild>();
                this.list_4 = new List<uint>();
                this.bbTiles = new List<RoomItem>();
                this.bbbTiles = new List<RoomItem>();
                this.bbgTiles = new List<RoomItem>();
                this.bbrTiles = new List<RoomItem>();
                this.bbyTiles = new List<RoomItem>();
                this.RedScoreboards = new List<RoomItem>();
                this.YellowScoreboards = new List<RoomItem>();
                this.BlueScoreboards = new List<RoomItem>();
                this.GreenScoreboards = new List<RoomItem>();
                this.int_10 = 0;
                this.int_11 = 0;
                this.int_9 = 0;
                this.int_12 = 0;
                this.int_13 = 0;
                this.StickiePoles = new List<RoomItem>();
                this.WF_Triggers = new List<RoomItem>();
                this.WF_Effects = new List<RoomItem>();
                this.WF_Conditions = new List<RoomItem>();
                this.byte_0 = new byte[this.Model.MapSizeX, this.Model.MapSizeY];
                this.double_1 = new double[this.Model.MapSizeX, this.Model.MapSizeY];
                this.double_2 = new double[this.Model.MapSizeX, this.Model.MapSizeY];
                this.timer_0 = new Timer(new TimerCallback(this.method_32), null, 480, 480);
                this.int_8 = 0;
                this.RoomMuted = false;
                this.bool_9 = true;
                this.mGotRollers = false;
                this.int_16 = 0;
                this.int_15 = 4;
                this.Achievement = uint_3;
                this.bool_10 = false;
                this.mRemovedItems = new Hashtable();
                this.mMovedItems = new Hashtable();
                this.mAddedItems = new Hashtable();
                this.method_23();
                this.LoadFurniture();
                this.GenerateMaps();
        }
        public void method_0()
        {
            List<RoomBot> list = PhoenixEnvironment.GetGame().GetBotManager().method_2(this.RoomId);
            foreach (RoomBot current in list)
            {
                this.method_3(current);
            }
        }
        public void method_1()
        {
            new List<Pet>();
            using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
            {
                @class.AddParamWithValue("roomid", this.RoomId);
                DataTable dataTable = @class.ReadDataTable("SELECT Id, user_id, room_id, name, type, race, color, expirience, energy, nutrition, respect, createstamp, x, y, z FROM user_pets WHERE room_id = @roomid;");
                if (dataTable != null)
                {
                    foreach (DataRow dataRow_ in dataTable.Rows)
                    {
                        Pet class2 = PhoenixEnvironment.GetGame().GetCatalog().method_12(dataRow_);
                        List<RandomSpeech> list = new List<RandomSpeech>();
                        List<BotResponse> list2 = new List<BotResponse>();
                        this.method_4(new RoomBot(class2.PetId, this.RoomId, AIType.Pet, "freeroam", class2.Name, "", class2.Look, class2.X, class2.Y, (int)class2.Z, 0, 0, 0, 0, 0, ref list, ref list2, 0), class2);
                    }
                }
            }
        }
        internal List<Pet> method_2()
        {
            List<Pet> list = new List<Pet>();
            for (int i = 0; i < this.UserList.Length; i++)
            {
                if (this.UserList[i] != null && this.UserList[i].IsPet)
                {
                    list.Add(this.UserList[i].PetData);
                }
            }
            return list;
        }
        public RoomUser method_3(RoomBot class34_0)
        {
            return this.method_4(class34_0, null);
        }
        public RoomUser method_4(RoomBot Bot, Pet PetData)
        {
            int num = this.method_5();
            RoomUser user = new RoomUser(Convert.ToUInt32(num + 100000), this.RoomId, this.int_7++, true);
            user.CurrentFurniFX = num;
            this.UserList[num] = user;
            if (Bot.x > 0 && Bot.y > 0 && Bot.x < this.Model.MapSizeX && Bot.y < this.Model.MapSizeY)
            {
                user.SetPos(Bot.x, Bot.y, Bot.z);
                user.SetRot(Bot.Rotation);
            }
            else
            {
                Bot.x = this.Model.int_0;
                Bot.y = this.Model.int_1;
                user.SetPos(this.Model.int_0, this.Model.int_1, this.Model.double_0);
                user.SetRot(this.Model.int_2);
            }
            user.BotData = Bot;
            user.BotAI = Bot.method_4(user.VirtualId);
            if (user.IsPet)
            {
                user.BotAI.Init((int)Bot.Id, user.VirtualId, this.RoomId);
                user.PetData = PetData;
                user.PetData.VirtualId = user.VirtualId;
            }
            else
            {
                user.BotAI.Init(-1, user.VirtualId, this.RoomId);
            }
            this.UpdateUserStatus(user, true, true);
            user.UpdateNeeded = true;
            ServerMessage Message = new ServerMessage(28u);
            Message.AppendInt32(1);
            user.Serialize(Message);
            this.SendMessage(Message, null);
            user.BotAI.OnSelfEnterRoom();
            return user;
        }
        private int method_5()
        {
            return Array.IndexOf<RoomUser>(this.UserList, null);
        }
        public void method_6(int int_17, bool bool_13)
        {
            RoomUser @class = this.method_52(int_17);
            if (@class != null && @class.IsBot)
            {
                @class.BotAI.OnSelfLeaveRoom(bool_13);
                ServerMessage Message = new ServerMessage(29u);
                Message.AppendRawInt32(@class.VirtualId);
                this.SendMessage(Message, null);
                uint num = @class.HabboId;
                for (int i = 0; i < this.UserList.Length; i++)
                {
                    RoomUser class2 = this.UserList[i];
                    if (class2 != null && class2.HabboId == num)
                    {
                        this.UserList[i] = null;
                    }
                }
            }
        }
        public void OnUserSay(RoomUser RoomUser_1, string string_10, bool bool_13)
        {
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null && @class.IsBot)
                {
                    if (bool_13)
                    {
                        @class.BotAI.OnUserShout(RoomUser_1, string_10);
                    }
                    else
                    {
                        @class.BotAI.OnUserSay(RoomUser_1, string_10);
                    }
                }
            }
        }
        public void method_8(RoomUser RoomUser_1)
        {
            try
            {
                foreach (RoomItem current in this.WF_Triggers)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_enterroom")
                    {
                        this.method_21(RoomUser_1, current, "");
                    }
                }
            }
            catch
            {
            }
        }
        public bool WF_OnUserSay(RoomUser RoomUser_1, string string_10)
        {
            bool result = false;
            try
            {
                foreach (RoomItem current in this.WF_Triggers)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_onsay" && this.method_21(RoomUser_1, current, string_10.ToLower()))
                    {
                        result = true;
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        public void method_10(RoomUser RoomUser_1, RoomItem RoomItem_0)
        {
            try
            {
                foreach (RoomItem current in this.WF_Triggers)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_furnistate")
                    {
                        this.method_21(RoomUser_1, current, Convert.ToString(RoomItem_0.Id));
                    }
                }
            }
            catch
            {
            }
        }
        public void method_11(RoomUser RoomUser_1, RoomItem RoomItem_0)
        {
            try
            {
                foreach (RoomItem current in this.WF_Triggers)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_onfurni")
                    {
                        this.method_21(RoomUser_1, current, Convert.ToString(RoomItem_0.Id));
                    }
                }
            }
            catch
            {
            }
        }
        public void method_12(RoomUser RoomUser_1, RoomItem RoomItem_0)
        {
            try
            {
                foreach (RoomItem current in this.WF_Triggers)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_offfurni")
                    {
                        this.method_21(RoomUser_1, current, Convert.ToString(RoomItem_0.Id));
                    }
                }
            }
            catch
            {
            }
        }
        public void method_13()
        {
            try
            {
                foreach (RoomItem current in this.WF_Triggers)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_gameend")
                    {
                        this.method_21(null, current, "GameEnded");
                    }
                }
            }
            catch
            {
            }
        }
        public void method_14(RoomUser RoomUser_1)
        {
            try
            {
                foreach (RoomItem current in this.WF_Triggers)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_gamestart")
                    {
                        this.method_21(RoomUser_1, current, "GameBegun");
                    }
                }
            }
            catch
            {
            }
        }
        public void method_15(RoomItem RoomItem_0)
        {
            this.method_21(null, RoomItem_0, "Timer");
        }
        public void method_16(double double_3)
        {
            try
            {
                foreach (RoomItem current in this.WF_Triggers)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_attime" && current.Extra1.Length > 0 && Convert.ToDouble(current.Extra1) == double_3)
                    {
                        this.method_21(null, current, "AtTime");
                    }
                }
            }
            catch
            {
            }
        }
        public void method_17(int int_17)
        {
            try
            {
                foreach (RoomItem current in this.WF_Triggers)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_atscore" && current.Extra1 != "" && Convert.ToDouble(current.Extra1) == (double)int_17)
                    {
                        this.method_21(null, current, "TheScore");
                    }
                }
            }
            catch
            {
            }
        }
        public bool IsPhxMagicAllowed(RoomUser User, string command, string data)
        {
            data = this.VariablePhxMagic(User, data);
            switch (command)
            {
                case "roomuserseq":
                    if (this.UserCount != Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "roomuserslt":
                    if (this.UserCount >= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "roomusersmt":
                    if (this.UserCount <= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "roomusersmte":
                    if (this.UserCount < Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "roomuserslte":
                    if (this.UserCount > Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userhasachievement":
                    return PhoenixEnvironment.GetGame().GetAchievementManager().UserHasAchievement(User.GetClient(), Convert.ToUInt16(data), 1);

                case "userhasntachievement":
                    if (PhoenixEnvironment.GetGame().GetAchievementManager().UserHasAchievement(User.GetClient(), Convert.ToUInt16(data), 1))
                    {
                        break;
                    }
                    return true;

                case "userhasbadge":
                    return User.GetClient().GetHabbo().GetBadgeComponent().HasBadge(data);

                case "userhasntbadge":
                    if (User.GetClient().GetHabbo().GetBadgeComponent().HasBadge(data))
                    {
                        break;
                    }
                    return true;

                case "userhasvip":
                    return User.GetClient().GetHabbo().Vip;

                case "userhasntvip":
                    if (User.GetClient().GetHabbo().Vip)
                    {
                        break;
                    }
                    return true;

                case "userhaseffect":
                    if (User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect != Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userhasnteffect":
                    if (User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect == Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userrankeq":
                    if (User.GetClient().GetHabbo().Rank != Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userrankmt":
                    if (User.GetClient().GetHabbo().Rank <= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userrankmte":
                    if (User.GetClient().GetHabbo().Rank < Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userranklt":
                    if (User.GetClient().GetHabbo().Rank >= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userranklte":
                    if (User.GetClient().GetHabbo().Rank > Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "usercreditseq":
                    if (User.GetClient().GetHabbo().Credits != Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "usercreditsmt":
                    if (User.GetClient().GetHabbo().Credits <= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "usercreditsmte":
                    if (User.GetClient().GetHabbo().Credits < Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "usercreditslt":
                    if (User.GetClient().GetHabbo().Credits >= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "usercreditslte":
                    if (User.GetClient().GetHabbo().Credits > Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpixelseq":
                    if (User.GetClient().GetHabbo().ActivityPoints != Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpixelsmt":
                    if (User.GetClient().GetHabbo().ActivityPoints <= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpixelsmte":
                    if (User.GetClient().GetHabbo().ActivityPoints < Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpixelslt":
                    if (User.GetClient().GetHabbo().ActivityPoints >= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpixelslte":
                    if (User.GetClient().GetHabbo().ActivityPoints > Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpointseq":
                    if (User.GetClient().GetHabbo().shells != Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpointsmt":
                    if (User.GetClient().GetHabbo().shells <= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpointsmte":
                    if (User.GetClient().GetHabbo().shells < Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpointslt":
                    if (User.GetClient().GetHabbo().shells >= Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userpointslte":
                    if (User.GetClient().GetHabbo().shells > Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "usergroupeq":
                    if (User.GetClient().GetHabbo().GroupID != Convert.ToInt32(data))
                    {
                        break;
                    }
                    return true;

                case "userisingroup":
                    foreach (DataRow row in User.GetClient().GetHabbo().GroupMemberships.Rows)
                    {
                        if (((int)row["groupid"]) == Convert.ToInt32(data))
                        {
                            return true;
                        }
                    }
                    break;

                case "wearing":
                    if (User.GetClient().GetHabbo().Look.Contains(data))
                    {
                        return true;
                    }
                    break;

                case "notwearing":
                    if (User.GetClient().GetHabbo().Look.Contains(data))
                    {
                        break;
                    }
                    return true;

                case "carrying":
                    if (this.GetRoomUserByHabbo(User.GetClient().GetHabbo().Id).CarryItemID != Convert.ToInt16(data))
                    {
                        break;
                    }
                    return true;

                case "notcarrying":
                    if (this.GetRoomUserByHabbo(User.GetClient().GetHabbo().Id).CarryItemID == Convert.ToInt16(data))
                    {
                        break;
                    }
                    return true;
            }
            return false;
        }

        public void RunPhxMagic(RoomUser User, string command, string data)
        {
            data = this.VariablePhxMagic(User, data);
            switch (command)
            {
                case "sql":
                    {
                        using (DatabaseClient client = PhoenixEnvironment.GetDatabase().GetClient())
                        {
                            client.ExecuteQuery(data);
                            break;
                        }
                    }
                case "badge":
                    if (User.GetClient() == null)
                    {
                        break;
                    }
                    //User.GetClient().GetHabbo().GetBadgeComponent().GiveBadge(User.GetClient(), PhoenixEnvironment.FilterInjectionChars(data), true);
                    User.GetClient().SendMessage(User.GetClient().GetHabbo().GetBadgeComponent().Serialize());
                    return;

                case "effect":
                    if (User.GetClient() == null)
                    {
                        break;
                    }
                    User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().AddEffect(Convert.ToInt32(data), 0xe10);
                    User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().EnableEffect(Convert.ToInt32(data));
                    return;

                case "award":
                    if (User.GetClient() == null)
                    {
                        break;
                    }
                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockNextAchievement(User.GetClient(), Convert.ToUInt32(data));
                    return;

                case "dance":
                    {
                        if (User.GetClient() == null)
                        {
                            break;
                        }
                        RoomUser roomUserByHabbo = this.GetRoomUserByHabbo(User.GetClient().GetHabbo().Id);
                        roomUserByHabbo.DanceId = Convert.ToInt32(data);
                        ServerMessage message = new ServerMessage(480);
                        message.AppendInt32(roomUserByHabbo.VirtualId);
                        message.AppendInt32(Convert.ToInt32(data));
                        this.SendMessage(message, null);
                        return;
                    }
                case "send":
                    {
                        if (User.GetClient() == null)
                        {
                            break;
                        }
                        uint roomId = Convert.ToUInt32(data);
                        Room room = null;
                        if (PhoenixEnvironment.GetGame().GetRoomManager().IsRoomLoaded(roomId) || PhoenixEnvironment.GetGame().GetRoomManager().IsRoomLoading(roomId))
                        {
                            room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(roomId);
                        }
                        else
                        {
                            room = PhoenixEnvironment.GetGame().GetRoomManager().LoadRoom(roomId);
                        }
                        if (User == null)
                        {
                            break;
                        }
                        if (room == null)
                        {
                            this.RemoveUserFromRoom(User.GetClient(), true, false);
                            return;
                        }
                        ServerMessage message2 = new ServerMessage(0x11e);
                        message2.AppendBoolean(room.IsPublic);
                        message2.AppendUInt(Convert.ToUInt32(data));
                        User.GetClient().SendMessage(message2);
                        return;
                    }
                case "credits":
                    if (User.GetClient() == null)
                    {
                        break;
                    }
                    User.GetClient().GetHabbo().Credits += Convert.ToInt32(data);
                    User.GetClient().GetHabbo().UpdateCreditsBalance(true);
                    return;

                case "pixels":
                    if (User.GetClient() == null)
                    {
                        break;
                    }
                    User.GetClient().GetHabbo().ActivityPoints += Convert.ToInt32(data);
                    User.GetClient().GetHabbo().UpdateActivityPointsBalance(true);
                    return;

                case "points":
                    if (User.GetClient() == null)
                    {
                        break;
                    }
                    User.GetClient().GetHabbo().shells += Convert.ToInt32(data);
                    User.GetClient().GetHabbo().UpdateShellsBalance(false, true);
                    return;

                case "rank":
                    if ((User.GetClient() == null) || (Convert.ToUInt16(data) >= PhoenixEnvironment.GetGame().GetRoleManager().RankCount()))
                    {
                        break;
                    }
                    using (DatabaseClient client2 = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        client2.ExecuteQuery(string.Concat(new object[] { "UPDATE users SET rank = '", Convert.ToUInt16(data), "' WHERE id = ", User.GetClient().GetHabbo().Id, " LIMIT 1;" }));
                    }
                    User.GetClient().Disconnect();
                    return;

                case "respect":
                    {
                        if (User.GetClient() == null)
                        {
                            break;
                        }
                        Habbo habbo = User.GetClient().GetHabbo();
                        habbo.Respect++;
                        using (DatabaseClient client3 = PhoenixEnvironment.GetDatabase().GetClient())
                        {
                            client3.ExecuteQuery("UPDATE user_stats SET Respect = respect + 1 WHERE id = '" + User.GetClient().GetHabbo().Id + "' LIMIT 1");
                        }
                        ServerMessage message3 = new ServerMessage(440);
                        message3.AppendUInt(User.GetClient().GetHabbo().Id);
                        message3.AppendInt32(User.GetClient().GetHabbo().Respect);
                        this.SendMessage(message3, null);
                        int num3 = User.GetClient().GetHabbo().Respect;
                        if (num3 > 0xa6)
                        {
                            switch (num3)
                            {
                                case 0x2fe:
                                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 8);
                                    return;

                                case 0x3c6:
                                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 9);
                                    return;

                                case 0x45c:
                                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 10);
                                    return;

                                case 0x16e:
                                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 6);
                                    return;

                                case 0x236:
                                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 7);
                                    break;
                            }
                            return;
                        }
                        switch (num3)
                        {
                            case 0x10:
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 3);
                                return;

                            case 0x42:
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 4);
                                return;

                            case 0xa6:
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 5);
                                return;

                            case 1:
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 1);
                                return;

                            case 6:
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(User.GetClient(), 14, 2);
                                break;
                        }
                        return;
                    }
                case "handitem":
                    if (User.GetClient() == null)
                    {
                        break;
                    }
                    this.GetRoomUserByHabbo(User.GetClient().GetHabbo().Id).CarryItem(Convert.ToInt16(data));
                    return;

                case "alert":
                    if (User.GetClient() != null)
                    {
                        User.GetClient().SendNotif(data);
                    }
                    break;

                default:
                    return;
            }
        }

        public string VariablePhxMagic(RoomUser RoomUser_1, string string_10)
        {
            if (RoomUser_1 != null)
            {
                if (string_10.ToUpper().Contains("#USERNAME#"))
                {
                    string_10 = Regex.Replace(string_10, "#USERNAME#", RoomUser_1.GetClient().GetHabbo().Username, RegexOptions.IgnoreCase);
                }
                if (string_10.ToUpper().Contains("#USERID#"))
                {
                    string_10 = Regex.Replace(string_10, "#USERID#", RoomUser_1.GetClient().GetHabbo().Id.ToString(), RegexOptions.IgnoreCase);
                }
                if (string_10.ToUpper().Contains("#USERRANK#"))
                {
                    string_10 = Regex.Replace(string_10, "#USERRANK#", RoomUser_1.GetClient().GetHabbo().Rank.ToString(), RegexOptions.IgnoreCase);
                }
            }
            if (string_10.ToUpper().Contains("#ROOMNAME#"))
            {
                string_10 = Regex.Replace(string_10, "#ROOMNAME#", this.Name, RegexOptions.IgnoreCase);
            }
            if (string_10.ToUpper().Contains("#ROOMID#"))
            {
                string_10 = Regex.Replace(string_10, "#ROOMID#", this.Id.ToString(), RegexOptions.IgnoreCase);
            }
            int num = PhoenixEnvironment.GetGame().GetClientManager().ClientCount + -1;
            int int32_ = PhoenixEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;
            if (string_10.ToUpper().Contains("#ONLINECOUNT#"))
            {
                string_10 = Regex.Replace(string_10, "#ONLINECOUNT#", num.ToString(), RegexOptions.IgnoreCase);
            }
            if (string_10.ToUpper().Contains("#ROOMSLOADED#"))
            {
                string_10 = Regex.Replace(string_10, "#ROOMSLOADED#", int32_.ToString(), RegexOptions.IgnoreCase);
            }
            return string_10;
        }
        public bool method_21(RoomUser RoomUser_1, RoomItem RoomItem_0, string string_10)
        {
            bool result;
            try
            {
                if (this.bool_6 || this.bool_7)
                {
                    result = false;
                }
                else
                {
                    bool flag = false;
                    int num = 0;
                    int num2 = 0;
                    bool flag2 = false;
                    string text = RoomItem_0.GetBaseItem().InteractionType.ToLower();
                    switch (text)
                    {
                        case "wf_trg_onsay":
                            if (string_10.Contains(RoomItem_0.Extra1.ToLower()))
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_enterroom":
                            if (RoomItem_0.Extra1 == "" || RoomItem_0.Extra1 == RoomUser_1.GetClient().GetHabbo().Username)
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_furnistate":
                            if (RoomItem_0.Extra2.Length > 0)
                            {
                                string[] collection = RoomItem_0.Extra2.Split(new char[]
							{
								','
							});
                                List<string> list = new List<string>(collection);
                                foreach (string current in list)
                                {
                                    if (current == string_10)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                            break;
                        case "wf_trg_onfurni":
                            if (RoomItem_0.Extra2.Length > 0)
                            {
                                string[] collection = RoomItem_0.Extra2.Split(new char[]
							{
								','
							});
                                List<string> list = new List<string>(collection);
                                List<string> list2 = list;
                                foreach (string current in list)
                                {
                                    if (!(current != string_10))
                                    {
                                        RoomItem @class = this.GetItem(Convert.ToUInt32(string_10));
                                        if (@class != null)
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            list2.Remove(current);
                                        }
                                    }
                                }
                                RoomItem_0.Extra2 = string.Join(",", list2.ToArray());
                            }
                            break;
                        case "wf_trg_offfurni":
                            if (RoomItem_0.Extra2.Length > 0)
                            {
                                string[] collection = RoomItem_0.Extra2.Split(new char[]
							{
								','
							});
                                List<string> list = new List<string>(collection);
                                List<string> list2 = list;
                                foreach (string current in list)
                                {
                                    if (!(current != string_10))
                                    {
                                        RoomItem @class = this.GetItem(Convert.ToUInt32(string_10));
                                        if (@class != null)
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            list2.Remove(current);
                                        }
                                    }
                                }
                                RoomItem_0.Extra2 = string.Join(",", list2.ToArray());
                            }
                            break;
                        case "wf_trg_gameend":
                            if (string_10 == "GameEnded")
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_gamestart":
                            if (string_10 == "GameBegun")
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_timer":
                            if (string_10 == "Timer")
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_attime":
                            if (string_10 == "AtTime")
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_atscore":
                            if (string_10 == "TheScore")
                            {
                                flag = true;
                            }
                            break;
                    }
                    try
                    {
                        List<RoomItem> list3 = this.method_93(RoomItem_0.GetX, RoomItem_0.GetY);
                        if (list3 == null)
                        {
                            result = false;
                            return result;
                        }
                        foreach (RoomItem current2 in list3)
                        {
                            text = current2.GetBaseItem().InteractionType.ToLower();
                            if (text != null)
                            {
                                int num4;
                                if (!(text == "wf_cnd_phx"))
                                {
                                    if (!(text == "wf_cnd_trggrer_on_frn"))
                                    {
                                        string[] collection;
                                        List<string> list;
                                        List<RoomItem> list4;
                                        if (!(text == "wf_cnd_furnis_hv_avtrs"))
                                        {
                                            if (!(text == "wf_cnd_has_furni_on"))
                                            {
                                                continue;
                                            }
                                            num4 = num2;
                                            num++;
                                            current2.ExtraData = "1";
                                            current2.UpdateState(false, true);
                                            current2.ReqUpdate(1);
                                            current2.method_10();
                                            if (current2.Extra2.Length <= 0)
                                            {
                                                continue;
                                            }
                                            collection = current2.Extra2.Split(new char[]
											{
												','
											});
                                            list = new List<string>(collection);
                                            list4 = new List<RoomItem>();
                                            foreach (string current3 in list)
                                            {
                                                list4.Add(this.GetItem(Convert.ToUInt32(current3)));
                                            }
                                            using (List<RoomItem>.Enumerator enumerator3 = list4.GetEnumerator())
                                            {
                                                while (enumerator3.MoveNext())
                                                {
                                                    RoomItem current4 = enumerator3.Current;
                                                    if (current4 != null)
                                                    {
                                                        Dictionary<int, AffectedTile> dictionary = current4.Dictionary_0;
                                                        if (dictionary == null)
                                                        {
                                                            dictionary = new Dictionary<int, AffectedTile>();
                                                        }
                                                        List<RoomItem> list5 = new List<RoomItem>(this.method_45(current4.GetX, current4.GetY));
                                                        if (list5.Count > 1 && num4 + 1 != num2)
                                                        {
                                                            num2++;
                                                            break;
                                                        }
                                                        foreach (AffectedTile current5 in dictionary.Values)
                                                        {
                                                            list5 = new List<RoomItem>(this.method_45(current5.X, current5.Y));
                                                            if (list5.Count > 1 && num4 + 1 != num2)
                                                            {
                                                                num2++;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                continue;
                                            }
                                        }
                                        num++;
                                        current2.ExtraData = "1";
                                        current2.UpdateState(false, true);
                                        current2.ReqUpdate(1);
                                        current2.method_10();
                                        if (current2.Extra2.Length <= 0)
                                        {
                                            continue;
                                        }
                                        collection = current2.Extra2.Split(new char[]
										{
											','
										});
                                        list = new List<string>(collection);
                                        list4 = new List<RoomItem>();
                                        foreach (string current3 in list)
                                        {
                                            list4.Add(this.GetItem(Convert.ToUInt32(current3)));
                                        }
                                        bool flag3 = true;
                                        foreach (RoomItem current4 in list4)
                                        {
                                            if (current4 != null)
                                            {
                                                bool flag4 = false;
                                                Dictionary<int, AffectedTile> dictionary = current4.Dictionary_0;
                                                if (dictionary == null)
                                                {
                                                    dictionary = new Dictionary<int, AffectedTile>();
                                                }
                                                if (this.SquareHasUsers(current4.GetX, current4.GetY))
                                                {
                                                    flag4 = true;
                                                }
                                                foreach (AffectedTile current5 in dictionary.Values)
                                                {
                                                    if (this.SquareHasUsers(current5.X, current5.Y))
                                                    {
                                                        flag4 = true;
                                                        break;
                                                    }
                                                }
                                                if (!flag4)
                                                {
                                                    flag3 = false;
                                                }
                                            }
                                        }
                                        if (flag3)
                                        {
                                            num2++;
                                            continue;
                                        }
                                        continue;
                                    }
                                    else
                                    {
                                        num4 = num2;
                                        num++;
                                        current2.ExtraData = "1";
                                        current2.UpdateState(false, true);
                                        current2.ReqUpdate(1);
                                        current2.method_10();
                                        if (current2.Extra2.Length <= 0)
                                        {
                                            continue;
                                        }
                                        string[] collection = current2.Extra2.Split(new char[]
										{
											','
										});
                                        List<string> list = new List<string>(collection);
                                        List<RoomItem> list4 = new List<RoomItem>();
                                        foreach (string current3 in list)
                                        {
                                            list4.Add(this.GetItem(Convert.ToUInt32(current3)));
                                        }
                                        if (RoomUser_1 == null)
                                        {
                                            continue;
                                        }
                                        using (List<RoomItem>.Enumerator enumerator3 = list4.GetEnumerator())
                                        {
                                            while (enumerator3.MoveNext())
                                            {
                                                RoomItem current4 = enumerator3.Current;
                                                if (current4 != null)
                                                {
                                                    Dictionary<int, AffectedTile> dictionary = current4.Dictionary_0;
                                                    if (dictionary == null)
                                                    {
                                                        dictionary = new Dictionary<int, AffectedTile>();
                                                    }
                                                    if (RoomUser_1.X == current4.GetX && RoomUser_1.Y == current4.GetY && num4 + 1 != num2)
                                                    {
                                                        num2++;
                                                        break;
                                                    }
                                                    foreach (AffectedTile current5 in dictionary.Values)
                                                    {
                                                        if (RoomUser_1.X == current5.X && RoomUser_1.Y == current5.Y && num4 + 1 != num2)
                                                        {
                                                            num2++;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            continue;
                                        }
                                    }
                                }
                                num4 = num2;
                                num++;
                                current2.ExtraData = "1";
                                current2.UpdateState(false, true);
                                current2.ReqUpdate(1);
                                if (current2.Extra1.Length > 0)
                                {
                                    string string_11 = current2.Extra1.Split(new char[]
									{
										':'
									})[0].ToLower();
                                    string string_12 = current2.Extra1.Split(new char[]
									{
										':'
									})[1];
                                    if (RoomUser_1 != null)
                                    {
                                        if (!RoomUser_1.IsBot && this.IsPhxMagicAllowed(RoomUser_1, string_11, string_12))
                                        {
                                            num2++;
                                        }
                                    }
                                    else
                                    {
                                        RoomUser[] array = this.UserList;
                                        for (int i = 0; i < array.Length; i++)
                                        {
                                            RoomUser class2 = array[i];
                                            if (class2 != null && !class2.IsBot && this.IsPhxMagicAllowed(class2, string_11, string_12) && num4 + 1 != num2)
                                            {
                                                num2++;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (num != num2)
                        {
                            result = false;
                            return result;
                        }
                    }
                    catch
                    {
                    }
                    if (flag && num == num2)
                    {
                        RoomItem_0.ExtraData = "1";
                        RoomItem_0.UpdateState(false, true);
                        RoomItem_0.ReqUpdate(1);
                        List<RoomItem> list6 = this.method_93(RoomItem_0.GetX, RoomItem_0.GetY);
                        if (list6 == null)
                        {
                            result = false;
                            return result;
                        }
                        bool flag5 = false;
                        foreach (RoomItem current2 in list6)
                        {
                            if (current2.GetBaseItem().InteractionType.ToLower() == "wf_xtra_random")
                            {
                                flag5 = true;
                                break;
                            }
                        }
                        if (flag5)
                        {
                            List<RoomItem> list7 = new List<RoomItem>();
                            Random random = new Random();
                            while (list6.Count != 0)
                            {
                                int index = random.Next(0, list6.Count);
                                list7.Add(list6[index]);
                                list6.RemoveAt(index);
                            }
                            list6 = list7;
                        }
                        foreach (RoomItem current2 in list6)
                        {
                            if (flag5 && flag2)
                            {
                                break;
                            }
                            text = current2.GetBaseItem().InteractionType.ToLower();
                            switch (text)
                            {
                                case "wf_act_give_phx":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    if (current2.Extra1.Length > 0)
                                    {
                                        string string_11 = current2.Extra1.Split(new char[]
									{
										':'
									})[0].ToLower();
                                        string string_12 = current2.Extra1.Split(new char[]
									{
										':'
									})[1];
                                        if (RoomUser_1 != null)
                                        {
                                            if (!RoomUser_1.IsBot)
                                            {
                                                this.RunPhxMagic(RoomUser_1, string_11, string_12);
                                            }
                                        }
                                        else
                                        {
                                            RoomUser[] array = this.UserList;
                                            for (int i = 0; i < array.Length; i++)
                                            {
                                                RoomUser class2 = array[i];
                                                if (class2 != null && !class2.IsBot)
                                                {
                                                    this.RunPhxMagic(class2, string_11, string_12);
                                                }
                                            }
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_saymsg":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    if (current2.Extra1.Length > 0)
                                    {
                                        string text2 = current2.Extra1;
                                        text2 = ChatCommandHandler.ApplyWordFilter(text2);
                                        if (text2.Length > 100)
                                        {
                                            text2 = text2.Substring(0, 100);
                                        }
                                        if (RoomUser_1 != null)
                                        {
                                            if (!RoomUser_1.IsBot)
                                            {
                                                RoomUser_1.GetClient().GetHabbo().Sendselfwhisper(text2);
                                            }
                                        }
                                        else
                                        {
                                            RoomUser[] array = this.UserList;
                                            for (int i = 0; i < array.Length; i++)
                                            {
                                                RoomUser class2 = array[i];
                                                if (class2 != null && !class2.IsBot)
                                                {
                                                    class2.GetClient().GetHabbo().Sendselfwhisper(text2);
                                                }
                                            }
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_moveuser":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    current2.method_10();
                                    if (current2.Extra2.Length > 0)
                                    {
                                        string[] collection = current2.Extra2.Split(new char[]
									{
										','
									});
                                        List<string> list = new List<string>(collection);
                                        Random random2 = new Random();
                                        int num5 = random2.Next(0, list.Count - 1);
                                        RoomItem class3 = this.GetItem(Convert.ToUInt32(list[num5]));
                                        if (class3 != null)
                                        {
                                            if (RoomUser_1 != null)
                                            {
                                                this.byte_0[RoomUser_1.X, RoomUser_1.Y] = 1;
                                                this.byte_0[RoomUser_1.SetX, RoomUser_1.SetY] = 1;
                                                this.byte_0[class3.GetX, class3.GetY] = 1;
                                                RoomUser_1.IsWalking = false;
                                                RoomUser_1.SetX = class3.GetX;
                                                RoomUser_1.SetY = class3.GetY;
                                                RoomUser_1.SetZ = class3.Double_0;
                                                RoomUser_1.SetPos(class3.GetX, class3.GetY, class3.Double_0);
                                                RoomUser_1.UpdateNeeded = true;
                                                if (!current2.dictionary_1.ContainsKey(RoomUser_1))
                                                {
                                                    current2.dictionary_1.Add(RoomUser_1, 10);
                                                }
                                                if (RoomUser_1.class34_1 != null)
                                                {
                                                    RoomUser_1.class34_1.RoomUser_0 = null;
                                                    RoomUser_1.Target = null;
                                                    RoomUser_1.class34_1 = null;
                                                }
                                                this.UpdateUserStatus(RoomUser_1, true, false);
                                            }
                                            else
                                            {
                                                RoomUser[] array = this.UserList;
                                                for (int i = 0; i < array.Length; i++)
                                                {
                                                    RoomUser class2 = array[i];
                                                    if (class2 != null)
                                                    {
                                                        this.byte_0[class2.X, class2.Y] = 1;
                                                        this.byte_0[class3.GetX, class3.GetY] = 1;
                                                        class2.SetPos(class3.GetX, class3.GetY, class3.Double_0);
                                                        class2.UpdateNeeded = true;
                                                        if (!current2.dictionary_1.ContainsKey(class2))
                                                        {
                                                            current2.dictionary_1.Add(class2, 10);
                                                        }
                                                    }
                                                }
                                            }
                                            flag2 = true;
                                        }
                                    }
                                    break;
                                case "wf_act_togglefurni":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    if (current2.Extra2.Length > 0)
                                    {
                                        string[] collection = current2.Extra2.Split(new char[]
									{
										','
									});
                                        IEnumerable<string> enumerable = new List<string>(collection);
                                        List<string> list2 = enumerable.ToList<string>();
                                        foreach (string current in enumerable)
                                        {
                                            RoomItem class3 = this.GetItem(Convert.ToUInt32(current));
                                            if (class3 != null)
                                            {
                                                class3.Interactor.OnTrigger(null, class3, 0, true);
                                            }
                                            else
                                            {
                                                list2.Remove(current);
                                            }
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_givepoints":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    if (RoomUser_1 != null && current2.Extra1.Length > 0)
                                    {
                                        this.method_88(RoomUser_1.int_14 + 2, Convert.ToInt32(current2.Extra1), current2);
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_moverotate":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    current2.method_9();
                                    if (current2.Extra3.Length > 0)
                                    {
                                        string[] collection = current2.Extra3.Split(new char[]
									{
										','
									});
                                        IEnumerable<string> enumerable2 = new List<string>(collection);
                                        foreach (string current in enumerable2)
                                        {
                                            RoomItem class3 = this.GetItem(Convert.ToUInt32(current));
                                            if (class3 != null)
                                            {
                                                if (current2.Extra1 != "0" && current2.Extra1 != "")
                                                {
                                                    Coord gstruct1_ = class3.SquareInFront;
                                                    int num5 = 0;
                                                    int num6 = 0;
                                                    int num7 = 0;
                                                    if (current2.Extra1 == "1")
                                                    {
                                                        Random random3 = new Random();
                                                        num5 = random3.Next(1, 5);
                                                    }
                                                    else
                                                    {
                                                        if (current2.Extra1 == "2")
                                                        {
                                                            Random random3 = new Random();
                                                            num6 = random3.Next(1, 3);
                                                        }
                                                        else
                                                        {
                                                            if (current2.Extra1 == "3")
                                                            {
                                                                Random random3 = new Random();
                                                                num7 = random3.Next(1, 3);
                                                            }
                                                        }
                                                    }
                                                    if (current2.Extra1 == "4" || num5 == 1 || num7 == 1)
                                                    {
                                                        gstruct1_ = class3.method_1(4);
                                                    }
                                                    else
                                                    {
                                                        if (current2.Extra1 == "5" || num5 == 2 || num6 == 1)
                                                        {
                                                            gstruct1_ = class3.method_1(6);
                                                        }
                                                        else
                                                        {
                                                            if (current2.Extra1 == "6" || num5 == 3 || num7 == 2)
                                                            {
                                                                gstruct1_ = class3.method_1(0);
                                                            }
                                                            else
                                                            {
                                                                if (current2.Extra1 == "7" || num5 == 4 || num6 == 2)
                                                                {
                                                                    gstruct1_ = class3.method_1(2);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (this.method_37(gstruct1_.X, gstruct1_.Y, true, true, false, true, false) && class3.GetBaseItem().InteractionType != "wf_trg_timer")
                                                    {
                                                        this.method_41(class3, gstruct1_, current2.Id, class3.Double_0);
                                                    }
                                                }
                                                if (current2.Extra2.Length > 0 && current2.Extra2 != "0" && current2.Extra2 != "")
                                                {
                                                    int num5 = 0;
                                                    if (current2.Extra2 == "1")
                                                    {
                                                        num5 = class3.Rot + 2;
                                                        if (num5 > 6)
                                                        {
                                                            num5 = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (current2.Extra2 == "2")
                                                        {
                                                            num5 = class3.Rot - 2;
                                                            if (num5 < 0)
                                                            {
                                                                num5 = 6;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (current2.Extra2 == "3")
                                                            {
                                                                Random random3 = new Random();
                                                                num5 = random3.Next(1, 5);
                                                                if (num5 == 1)
                                                                {
                                                                    num5 = 0;
                                                                }
                                                                else
                                                                {
                                                                    if (num5 == 2)
                                                                    {
                                                                        num5 = 2;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (num5 == 3)
                                                                        {
                                                                            num5 = 4;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (num5 == 4)
                                                                            {
                                                                                num5 = 6;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (current2.GetRoom().method_79(null, class3, class3.GetX, class3.GetY, num5, false, true, false))
                                                    {
                                                        flag2 = true;
                                                    }
                                                }
                                            }
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_matchfurni":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    current2.method_9();
                                    if (current2.Extra3.Length > 0 && current2.Extra1.Length > 0)
                                    {
                                        string[] collection = current2.Extra3.Split(new char[]
									{
										','
									});
                                        IEnumerable<string> enumerable = new List<string>(collection);
                                        string[] collection2 = current2.Extra1.Split(new char[]
									{
										';'
									});
                                        List<string> list8 = new List<string>(collection2);
                                        int num8 = 0;
                                        foreach (string current in enumerable)
                                        {
                                            RoomItem class3 = this.GetItem(Convert.ToUInt32(current));
                                            if (class3 != null && !(class3.GetBaseItem().InteractionType.ToLower() == "dice"))
                                            {
                                                string[] collection3 = list8[num8].Split(new char[]
											{
												','
											});
                                                List<string> list9 = new List<string>(collection3);
                                                bool flag6 = false;
                                                bool flag7 = false;
                                                if (current2.Extra2 != "" && class3 != null)
                                                {
                                                    int int_ = class3.GetX;
                                                    int int_2 = class3.GetY;
                                                    if (current2.Extra2.StartsWith("I"))
                                                    {
                                                        class3.ExtraData = list9[4];
                                                        flag7 = true;
                                                    }
                                                    if (current2.Extra2.Substring(1, 1) == "I")
                                                    {
                                                        class3.Rot = Convert.ToInt32(list9[3]);
                                                        flag6 = true;
                                                    }
                                                    if (current2.Extra2.EndsWith("I"))
                                                    {
                                                        int_ = Convert.ToInt32(list9[0]);
                                                        int_2 = Convert.ToInt32(list9[1]);
                                                        flag6 = true;
                                                    }
                                                    if (flag6)
                                                    {
                                                        this.method_40(class3, int_, int_2, current2.Id, class3.Double_0);
                                                    }
                                                    if (flag7)
                                                    {
                                                        class3.UpdateState(false, true);
                                                    }
                                                    this.GenerateMaps();
                                                }
                                                num8++;
                                            }
                                        }
                                    }
                                    flag2 = true;
                                    break;
                            }
                        }
                    }
                    result = flag2;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        internal void GenerateMaps()
        {
            this.mBedMap = new Coord[this.Model.MapSizeX, this.Model.MapSizeY];
            this.double_0 = new double[this.Model.MapSizeX, this.Model.MapSizeY];
            this.byte_2 = new byte[this.Model.MapSizeX, this.Model.MapSizeY];
            this.byte_1 = new byte[this.Model.MapSizeX, this.Model.MapSizeY];
            this.byte_0 = new byte[this.Model.MapSizeX, this.Model.MapSizeY];
            this.double_1 = new double[this.Model.MapSizeX, this.Model.MapSizeY];
            this.double_2 = new double[this.Model.MapSizeX, this.Model.MapSizeY];
            for (int i = 0; i < this.Model.MapSizeY; i++)
            {
                for (int j = 0; j < this.Model.MapSizeX; j++)
                {
                    this.double_0[j, i] = 0.0;
                    this.byte_0[j, i] = 0;
                    this.byte_2[j, i] = 0;
                    this.byte_1[j, i] = 0;
                    this.mBedMap[j, i] = new Coord(j, i);
                    if (j == this.Model.int_0 && i == this.Model.int_1)
                    {
                        this.byte_0[j, i] = 3;
                    }
                    else
                    {
                        if (this.Model.squareState[j, i] == SquareState.OPEN)
                        {
                            this.byte_0[j, i] = 1;
                        }
                        else
                        {
                            if (this.Model.squareState[j, i] == SquareState.SEAT)
                            {
                                this.byte_0[j, i] = 3;
                            }
                        }
                    }
                }
            }
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                try
                {
                    if (@class.GetBaseItem().Type == 's')
                    {
                        if (@class.GetX >= this.Model.MapSizeX || @class.GetY >= this.Model.MapSizeY || @class.GetY < 0 || @class.GetX < 0)
                        {
                            this.RemoveFurniture(null, @class.Id, true, false);
                            GameClient class2 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(this.Owner);
                            if (class2 != null)
                            {
                                class2.GetHabbo().GetInventoryComponent().method_11(@class.Id, @class.uint_2, @class.ExtraData, true);
                            }
                        }
                        else
                        {
                            if (@class.Double_1 > this.double_1[@class.GetX, @class.GetY])
                            {
                                this.double_1[@class.GetX, @class.GetY] = @class.Double_1;
                            }
                            if (@class.GetBaseItem().IsSeat)
                            {
                                this.double_2[@class.GetX, @class.GetY] = @class.Double_1;
                            }
                            if (@class.GetBaseItem().Height > 0.0 || @class.GetBaseItem().EffectF != 0 || @class.GetBaseItem().EffectM != 0 || @class.GetBaseItem().IsSeat || !(@class.GetBaseItem().InteractionType.ToLower() != "bed"))
                            {
                                if (this.double_0[@class.GetX, @class.GetY] <= @class.Double_0)
                                {
                                    this.double_0[@class.GetX, @class.GetY] = @class.Double_0;
                                    if (@class.GetBaseItem().EffectF > 0)
                                    {
                                        this.byte_2[@class.GetX, @class.GetY] = @class.GetBaseItem().EffectF;
                                    }
                                    else
                                    {
                                        if (this.byte_1[@class.GetX, @class.GetY] != 0)
                                        {
                                            this.byte_2[@class.GetX, @class.GetY] = 0;
                                        }
                                    }
                                    if (@class.GetBaseItem().EffectM > 0)
                                    {
                                        this.byte_1[@class.GetX, @class.GetY] = @class.GetBaseItem().EffectM;
                                    }
                                    else
                                    {
                                        if (this.byte_1[@class.GetX, @class.GetY] != 0)
                                        {
                                            this.byte_1[@class.GetX, @class.GetY] = 0;
                                        }
                                    }
                                    if (@class.GetBaseItem().Walkable)
                                    {
                                        if (this.byte_0[@class.GetX, @class.GetY] != 3)
                                        {
                                            this.byte_0[@class.GetX, @class.GetY] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (@class.Double_0 <= this.Model.double_1[@class.GetX, @class.GetY] + 0.1 && @class.GetBaseItem().InteractionType.ToLower() == "gate" && @class.ExtraData == "1")
                                        {
                                            if (this.byte_0[@class.GetX, @class.GetY] != 3)
                                            {
                                                this.byte_0[@class.GetX, @class.GetY] = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (@class.GetBaseItem().IsSeat || @class.GetBaseItem().InteractionType.ToLower() == "bed")
                                            {
                                                this.byte_0[@class.GetX, @class.GetY] = 3;
                                            }
                                            else
                                            {
                                                if (this.byte_0[@class.GetX, @class.GetY] != 3)
                                                {
                                                    this.byte_0[@class.GetX, @class.GetY] = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (@class.GetBaseItem().IsSeat || @class.GetBaseItem().InteractionType.ToLower() == "bed")
                                {
                                    this.byte_0[@class.GetX, @class.GetY] = 3;
                                }
                                Dictionary<int, AffectedTile> dictionary = @class.Dictionary_0;
                                if (dictionary == null)
                                {
                                    dictionary = new Dictionary<int, AffectedTile>();
                                }
                                foreach (AffectedTile current in dictionary.Values)
                                {
                                    if (@class.Double_1 > this.double_1[current.X, current.Y])
                                    {
                                        this.double_1[current.X, current.Y] = @class.Double_1;
                                    }
                                    if (@class.GetBaseItem().IsSeat)
                                    {
                                        this.double_2[current.X, current.Y] = @class.Double_1;
                                    }
                                    if (this.double_0[current.X, current.Y] <= @class.Double_0)
                                    {
                                        this.double_0[current.X, current.Y] = @class.Double_0;
                                        if (@class.GetBaseItem().EffectF > 0)
                                        {
                                            this.byte_2[current.X, current.Y] = @class.GetBaseItem().EffectF;
                                        }
                                        else
                                        {
                                            if (this.byte_1[current.X, current.Y] != 0)
                                            {
                                                this.byte_2[current.X, current.Y] = 0;
                                            }
                                        }
                                        if (@class.GetBaseItem().EffectM > 0)
                                        {
                                            this.byte_1[current.X, current.Y] = @class.GetBaseItem().EffectM;
                                        }
                                        else
                                        {
                                            if (this.byte_1[current.X, current.Y] != 0)
                                            {
                                                this.byte_1[current.X, current.Y] = 0;
                                            }
                                            else
                                            {
                                                if (@class.GetBaseItem().Walkable)
                                                {
                                                    if (this.byte_0[current.X, current.Y] != 3)
                                                    {
                                                        this.byte_0[current.X, current.Y] = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    if (@class.Double_0 <= this.Model.double_1[@class.GetX, @class.GetY] + 0.1 && @class.GetBaseItem().InteractionType.ToLower() == "gate" && @class.ExtraData == "1")
                                                    {
                                                        if (this.byte_0[current.X, current.Y] != 3)
                                                        {
                                                            this.byte_0[current.X, current.Y] = 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (@class.GetBaseItem().IsSeat || @class.GetBaseItem().InteractionType.ToLower() == "bed")
                                                        {
                                                            this.byte_0[current.X, current.Y] = 3;
                                                        }
                                                        else
                                                        {
                                                            if (this.byte_0[current.X, current.Y] != 3)
                                                            {
                                                                this.byte_0[current.X, current.Y] = 0;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (@class.GetBaseItem().IsSeat || @class.GetBaseItem().InteractionType.ToLower() == "bed")
                                    {
                                        this.byte_0[current.X, current.Y] = 3;
                                    }
                                    if (@class.GetBaseItem().InteractionType.ToLower() == "bed")
                                    {
                                        this.byte_0[current.X, current.Y] = 3;
                                        if (@class.Rot == 0 || @class.Rot == 4)
                                        {
                                            this.mBedMap[current.X, current.Y].Y = @class.GetY;
                                        }
                                        if (@class.Rot == 2 || @class.Rot == 6)
                                        {
                                            this.mBedMap[current.X, current.Y].X = @class.GetX;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    this.RemoveFurniture(null, @class.Id, true, false);
                    GameClient class2 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(this.Owner);
                    if (class2 != null)
                    {
                        class2.GetHabbo().GetInventoryComponent().method_11(@class.Id, @class.uint_2, @class.ExtraData, true);
                    }
                }
            }
            if (!this.AllowWalkthrough)
            {
                for (int k = 0; k < this.UserList.Length; k++)
                {
                    RoomUser class3 = this.UserList[k];
                    if (class3 != null)
                    {
                        this.byte_0[class3.X, class3.Y] = 0;
                    }
                }
            }
            this.byte_0[this.Model.int_0, this.Model.int_1] = 3;
        }
        public void method_23()
        {
            this.UsersWithRights = new List<uint>();
            DataTable dataTable = null;
            using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
            {
                dataTable = @class.ReadDataTable("SELECT room_rights.user_id FROM room_rights WHERE room_id = '" + this.Id + "'");
            }
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    this.UsersWithRights.Add((uint)dataRow["user_id"]);
                }
            }
        }
        internal List<RoomItem> method_24(GameClient Session)
        {
            List<RoomItem> list = new List<RoomItem>();
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                @class.Interactor.OnRemove(Session, @class);
                ServerMessage Message = new ServerMessage(94u);
                Message.AppendRawUInt(@class.Id);
                Message.AppendStringWithBreak("");
                Message.AppendBoolean(false);
                this.SendMessage(Message, null);
                list.Add(@class);
            }
            foreach (RoomItem @class in this.Hashtable_1.Values)
            {
                @class.Interactor.OnRemove(Session, @class);
                ServerMessage Message = new ServerMessage(84u);
                Message.AppendRawUInt(@class.Id);
                Message.AppendStringWithBreak("");
                Message.AppendBoolean(false);
                this.SendMessage(Message, null);
                list.Add(@class);
            }
            this.mWallItems.Clear();
            this.mFloorItems.Clear();
            this.mRemovedItems.Clear();
            this.mMovedItems.Clear();
            this.mAddedItems.Clear();
            using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
            {
                class2.ExecuteQuery(string.Concat(new object[]
				{
					"UPDATE items SET room_id = 0, user_id = '",
					Session.GetHabbo().Id,
					"' WHERE room_id = '",
					this.RoomId,
					"'"
				}));
            }
            this.GenerateMaps();
            this.method_83();
            return list;
        }
        public void LoadFurniture()
        {
            this.mFloorItems.Clear();
            this.mWallItems.Clear();
            DataTable table;
            using (DatabaseClient client = PhoenixEnvironment.GetDatabase().GetClient())
            {
                table = client.ReadDataTable("SELECT Id, base_item, extra_data, x, y, z, rot, wall_pos FROM items WHERE room_id = '" + this.Id + "' ORDER BY room_id DESC");
            }
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    RoomItem item = new RoomItem((uint)row["Id"], this.RoomId, (uint)row["base_item"], (string)row["extra_data"], (int)row["x"], (int)row["y"], (double)row["z"], (int)row["rot"], (string)row["wall_pos"], this);
                    if (item.IsRoller)
                    {
                        this.mGotRollers = true;
                    }
                    if (item.GetBaseItem().InteractionType.ToLower().Contains("wf_") || item.GetBaseItem().InteractionType.ToLower().Contains("fbgate"))
                    {
                        DataRow row2;
                        using (DatabaseClient client2 = PhoenixEnvironment.GetDatabase().GetClient())
                        {
                            row2 = client2.ReadDataRow("SELECT extra1,extra2,extra3,extra4,extra5 FROM wired_items WHERE item_id = '" + item.Id + "'");
                        }
                        if (row2 != null)
                        {
                            item.Extra1 = (string)row2["extra1"];
                            item.Extra2 = (string)row2["extra2"];
                            item.Extra3 = (string)row2["extra3"];
                            item.Extra4 = (string)row2["extra4"];
                            item.Extra5 = (string)row2["extra5"];
                        }
                    }
                    switch (item.GetBaseItem().InteractionType.ToLower())
                    {
                        case "dice":
                            if (item.ExtraData == "-1")
                            {
                                item.ExtraData = "0";
                            }
                            break;
                        case "fbgate":
                            if (item.ExtraData != "" && item.ExtraData.Contains(','))
                            {
                                item.Extra1 = item.ExtraData.Split(new char[] {	','	})[0];
                                item.Extra2 = item.ExtraData.Split(new char[] {	','	})[1];
                            }
                            break;
                        case "dimmer":
                            if (this.MoodlightData == null)
                            {
                                this.MoodlightData = new MoodlightData(item.Id);
                            }
                            break;
                        case "bb_patch":
                            this.bbTiles.Add(item);
                            if (item.ExtraData == "5")
                            {
                                this.bbrTiles.Add(item);
                            }
                            else if (item.ExtraData == "8")    
                            {    
                                this.bbgTiles.Add(item);
                            }
                            else if (item.ExtraData == "11")
                            {
                                this.bbbTiles.Add(item);
                            }
                            else if (item.ExtraData == "14")
                            {
                                this.bbyTiles.Add(item);
                            }
                            break;
                        case "blue_score":
                            this.BlueScoreboards.Add(item);
                            break;
                        case "green_score":
                            this.GreenScoreboards.Add(item);
                            break;
                        case "red_score":
                            this.RedScoreboards.Add(item);
                            break;
                        case "yellow_score":
                            this.YellowScoreboards.Add(item);
                            break;
                        case "stickiepole":
                            this.StickiePoles.Add(item);
                            break;
                        case "wf_trg_onsay":
                        case "wf_trg_enterroom":
                        case "wf_trg_furnistate":
                        case "wf_trg_onfurni":
                        case "wf_trg_offfurni":
                        case "wf_trg_gameend":
                        case "wf_trg_gamestart":
                        case "wf_trg_attime":
                        case "wf_trg_atscore":
                            if (!this.WF_Triggers.Contains(item))
                            {
                                this.WF_Triggers.Add(item);
                            }
                            break;
                        case "wf_trg_timer":
                            if (item.Extra1.Length <= 0)
                            {
                                item.Extra1 = "10";
                            }
                            if (!this.WF_Triggers.Contains(item))
                            {
                                this.WF_Triggers.Add(item);
                            }
                            item.TimerRunning = true;
                            item.ReqUpdate(1);
                            break;
                        case "wf_act_saymsg":
                        case "wf_act_moveuser":
                        case "wf_act_togglefurni":
                        case "wf_act_givepoints":
                        case "wf_act_moverotate":
                        case "wf_act_matchfurni":
                        case "wf_act_give_phx":
                            if (!this.WF_Effects.Contains(item))
                            {
                                this.WF_Effects.Add(item);
                            }
                            break;
                        case "wf_cnd_trggrer_on_frn":
                        case "wf_cnd_furnis_hv_avtrs":
                        case "wf_cnd_has_furni_on":
                        case "wf_cnd_phx":
                            if (!this.WF_Conditions.Contains(item))
                            {
                                this.WF_Conditions.Add(item);
                            }
                            break;
                        case "jukebox":
                            RoomMusicController roomMusicController = this.GetRoomMusicController();
                            roomMusicController.LinkRoomOutputItemIfNotAlreadyExits(item);
                            break;
                    }
                    if (this.mFloorItems.Contains(item.Id))
                    {
                        this.mFloorItems.Remove(item.Id);
                    }
                    if (this.mWallItems.Contains(item.Id))
                    {
                        this.mWallItems.Remove(item.Id);
                    }
                    if (item.IsFloorItem)
                    {
                        this.mFloorItems.Add(item.Id, item);
                    }
                    else
                    {
                        this.mWallItems.Add(item.Id, item);
                    }
                }
            }
        }
        public bool CheckRights(GameClient Session)
        {
            return CheckRights(Session, false);
        }
        public bool CheckRights(GameClient Session, bool RequireOwnership)
        {
            try
            {
                if (Session.GetHabbo().Username.ToLower() == this.Owner.ToLower())
                {
                    return true;
                }
                if (Session.GetHabbo().HasRole("acc_anyroomowner") && RequireOwnership)
                {
                    return true;
                }
                if (!RequireOwnership)
                {
                    if (Session.GetHabbo().HasRole("acc_anyroomrights"))
                    {
                        return true;
                    }
                    if (UsersWithRights.Contains(Session.GetHabbo().Id))
                    {
                        return true;
                    }
                    if (bool_8)
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        public RoomItem GetItem(uint Id)
        {
            if ((this.mFloorItems != null && this.mFloorItems.ContainsKey(Id)) || (this.mWallItems != null && this.mWallItems.ContainsKey(Id)))
            {
                RoomItem Item = this.mFloorItems[Id] as RoomItem;
                if (Item != null)
                {
                    return Item;
                }
                else
                {
                    return (this.mWallItems[Id] as RoomItem);
                }
            }
            else
            {
                return null;
            }
        }
        public void RemoveFurniture(GameClient Session, uint Id, bool Delete, bool ReGenerateMap)
        {
            RoomItem Item = this.GetItem(Id);
            if (Item != null)
            {
                Dictionary<int, AffectedTile> dictionary = this.GetAffectedTiles(Item.GetBaseItem().Length, Item.GetBaseItem().Width, Item.GetX, Item.GetY, Item.Rot);
                Item.Interactor.OnRemove(Session, Item);
                if (Item.IsWallItem)
                {
                    ServerMessage Message = new ServerMessage(84);
                    Message.AppendRawUInt(Item.Id);
                    Message.AppendStringWithBreak("");
                    Message.AppendBoolean(false);
                    this.SendMessage(Message, null);
                }
                else
                {
                    if (Item.IsFloorItem)
                    {
                        ServerMessage Message = new ServerMessage(94);
                        Message.AppendRawUInt(Item.Id);
                        Message.AppendStringWithBreak("");
                        Message.AppendBoolean(false);
                        this.SendMessage(Message, null);
                        string text = Item.GetBaseItem().InteractionType.ToLower();
                        switch (text)
                        {
                            case "bb_patch":
                                this.bbTiles.Remove(Item);
                                if (Item.ExtraData == "5")
                                {
                                    this.bbrTiles.Remove(Item);
                                }
                                else if (Item.ExtraData == "8")
                                {
                                    this.bbgTiles.Remove(Item);
                                }
                                else if (Item.ExtraData == "11")
                                {
                                    this.bbbTiles.Remove(Item);
                                }
                                else if (Item.ExtraData == "14")
                                {
                                    this.bbyTiles.Remove(Item);
                                }
                                break;
                            case "blue_score":
                                this.BlueScoreboards.Remove(Item);
                                break;
                            case "green_score":
                                this.GreenScoreboards.Remove(Item);
                                break;
                            case "red_score":
                                this.RedScoreboards.Remove(Item);
                                break;
                            case "yellow_score":
                                this.YellowScoreboards.Remove(Item);
                                break;
                            case "stickiepole":
                                this.StickiePoles.Remove(Item);
                                break;
                            case "wf_trg_onsay":
                            case "wf_trg_enterroom":
                            case "wf_trg_furnistate":
                            case "wf_trg_onfurni":
                            case "wf_trg_offfurni":
                            case "wf_trg_gameend":
                            case "wf_trg_gamestart":
                            case "wf_trg_attime":
                            case "wf_trg_atscore":
                                this.WF_Triggers.Remove(Item);
                                break;
                            case "wf_trg_timer":
                                Item.TimerRunning = false;
                                this.WF_Triggers.Remove(Item);
                                break;
                            case "wf_act_saymsg":
                            case "wf_act_moveuser":
                            case "wf_act_togglefurni":
                            case "wf_act_givepoints":
                            case "wf_act_moverotate":
                            case "wf_act_matchfurni":
                            case "wf_act_give_phx":
                                this.WF_Effects.Remove(Item);
                                break;
                            case "wf_cnd_trggrer_on_frn":
                            case "wf_cnd_furnis_hv_avtrs":
                            case "wf_cnd_has_furni_on":
                            case "wf_cnd_phx":
                                this.WF_Conditions.Remove(Item);
                                break;
                        }
                    }
                }
                if (Item.IsWallItem)
                {
                    this.mWallItems.Remove(Item.Id);
                }
                else
                {
                    this.mFloorItems.Remove(Item.Id);
                }
                if (this.mAddedItems.Contains(Item.Id))
                {
                    this.mAddedItems.Remove(Item.Id);
                }
                if (this.mMovedItems.Contains(Item.Id))
                {
                    this.mMovedItems.Remove(Item.Id);
                }
                if (!this.mRemovedItems.Contains(Item.Id))
                {
                    this.mRemovedItems.Add(Item.Id, Item);
                }
                if (Delete)
                {
                    using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        class2.ExecuteQuery("DELETE FROM items WHERE Id = '" + Id + "' LIMIT 1");
                    }
                }
                if (ReGenerateMap)
                {
                    this.GenerateMaps();
                }
                this.UpdateUserStatus(this.GetUserForSquare(Item.GetX, Item.GetY), true, true);
                foreach (AffectedTile tile in dictionary.Values)
                {
                    this.UpdateUserStatus(this.GetUserForSquare(tile.X, tile.Y), true, true);
                }
            }
        }
        public bool CanWalk(int int_17, int int_18, double double_3, bool bool_13, bool bool_14)
        {
            return this.AllowWalkthrough || bool_14 || this.GetUserForSquare(int_17, int_18) == null;
        }
        private void method_31(string string_10)
        {
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null && !@class.IsBot)
                {
                    @class.GetClient().SendNotif(string_10);
                }
            }
        }
        private void method_32(object object_0)
        {
            this.method_33();
        }
        private void method_33()
        {
            int num = 0;
            if (!this.bool_6 && !this.bool_7)
            {
                try
                {
                    this.int_14++;
                    if (this.bool_10 && this.int_14 >= 30)
                    {
                        using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
                        {
                            @class.ExecuteQuery(string.Concat(new object[]
							{
								"UPDATE rooms SET users_now = '",
								this.UserCount,
								"' WHERE Id = '",
								this.Id,
								"' LIMIT 1"
							}));
                        }
                        this.int_14 = 0;
                    }
                    this.method_35();
                    int num2 = 0;
                    try
                    {
                        if (this.mFloorItems != null)
                        {
                            foreach (RoomItem class2 in this.Hashtable_0.Values)
                            {
                                if (class2.bool_1)
                                {
                                    class2.method_2();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.RoomId + "] cycle task -- Process Floor Items");
                        this.CrashRoom();
                    }
                    try
                    {
                        if (this.mWallItems != null)
                        {
                            foreach (RoomItem class2 in this.Hashtable_1.Values)
                            {
                                if (class2.bool_1)
                                {
                                    class2.method_2();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.RoomId + "] cycle task -- Process Wall Items");
                        this.CrashRoom();
                    }
                    List<uint> list = new List<uint>();
                    int num3 = 0;
                    if (this.UserList != null)
                    {
                        try
                        {
                            for (int i = 0; i < this.UserList.Length; i++)
                            {
                                RoomUser class3 = this.UserList[i];
                                if (class3 != null)
                                {
                                    num = 1;
                                    if (!class3.IsBot && class3.GetClient() != null)
                                    {
                                        num3++;
                                        if (class3.GetClient().GetHabbo() != null && class3.GetClient().GetHabbo().MuteLength > 0)
                                        {
                                            class3.GetClient().GetHabbo().MuteLength--;
                                            if (class3.GetClient().GetHabbo().MuteLength == 0)
                                            {
                                                class3.GetClient().GetHabbo().Muted = false;
                                            }
                                        }
                                    }
                                    if (this.musicController != null)
                                    {
                                        this.musicController.Update(this);
                                    }
                                    class3.IdleTime++;
                                    num = 2;
                                    if (!class3.IsAsleep && class3.IdleTime >= GlobalClass.IdleSleep)
                                    {
                                        class3.IsAsleep = true;
                                        ServerMessage Message = new ServerMessage(486u);
                                        Message.AppendInt32(class3.VirtualId);
                                        Message.AppendBoolean(true);
                                        this.SendMessage(Message, null);
                                    }
                                    num = 3;
                                    if (class3.GetClient() == null && !class3.IsBot)
                                    {
                                        this.UserList[i] = null;
                                        if (!class3.AllowOverride)
                                        {
                                            this.byte_0[class3.X, class3.Y] = class3.SqState;
                                        }
                                        ServerMessage Message2 = new ServerMessage(29u);
                                        Message2.AppendRawInt32(class3.VirtualId);
                                        this.SendMessage(Message2, null);
                                        this.method_50();
                                    }
                                    num = 4;
                                    if (class3.NeedsAutokick && !list.Contains(class3.HabboId))
                                    {
                                        list.Add(class3.HabboId);
                                    }
                                    num = 5;
                                    if (class3.CarryItemID > 0)
                                    {
                                        class3.CarryTimer--;
                                        if (class3.CarryTimer <= 0)
                                        {
                                            class3.CarryItem(0);
                                        }
                                    }
                                    num = 6;
                                    if (class3.SetStep && class3.class34_1 == null)
                                    {
                                        num = 7;
                                        if (class3.IsBot && class3.BotData.RoomUser_0 != null && this.CanWalk(class3.SetX, class3.SetY, 0.0, true, true))
                                        {
                                            num = 8;
                                            this.method_85(class3);
                                            class3.X = class3.SetX;
                                            class3.Y = class3.SetY;
                                            class3.Z = class3.SetZ;
                                            class3.BotData.RoomUser_0.X = class3.SetX;
                                            class3.BotData.RoomUser_0.Y = class3.SetY;
                                            class3.BotData.RoomUser_0.Z = class3.SetZ + 1.0;
                                            class3.BotData.RoomUser_0.SetStep = false;
                                            class3.BotData.RoomUser_0.RemoveStatus("mv");
                                            if (class3.X == this.Model.int_0 && class3.Y == this.Model.int_1 && !list.Contains(class3.BotData.RoomUser_0.HabboId))
                                            {
                                                list.Add(class3.BotData.RoomUser_0.HabboId);
                                            }
                                            this.UpdateUserStatus(class3, true, true);
                                        }
                                        else
                                        {
                                            if (this.CanWalk(class3.SetX, class3.SetY, 0.0, true, class3.AllowOverride))
                                            {
                                                num = 8;
                                                this.method_85(class3);
                                                class3.X = class3.SetX;
                                                class3.Y = class3.SetY;
                                                class3.Z = class3.SetZ;
                                                if (class3.X == this.Model.int_0 && class3.Y == this.Model.int_1 && !list.Contains(class3.HabboId) && !class3.IsBot)
                                                {
                                                    list.Add(class3.HabboId);
                                                }
                                                this.UpdateUserStatus(class3, true, true);
                                            }
                                        }
                                        class3.SetStep = false;
                                    }
                                    num = 9;
                                    if (class3.IsWalking && !class3.bool_5 && class3.class34_1 == null)
                                    {
                                        num = 10;
                                        SquarePoint @struct = DreamPathfinder.GetNextStep(class3.X, class3.Y, class3.GoalX, class3.GoalY, this.byte_0, this.double_1, this.class28_0.double_1, this.double_2, this.class28_0.MapSizeX, this.class28_0.MapSizeY, class3.AllowOverride, this.bool_5);
                                        num = 11;
                                        if (@struct.X != class3.X || @struct.Y != class3.Y)
                                        {
                                            num = 12;
                                            int int32_ = @struct.X;
                                            int int32_2 = @struct.Y;
                                            class3.RemoveStatus("mv");
                                            double num4 = this.method_84(int32_, int32_2, this.method_93(int32_, int32_2));
                                            class3.Statusses.Remove("lay");
                                            class3.Statusses.Remove("sit");
                                            class3.AddStatus("mv", string.Concat(new object[]
											{
												int32_,
												",",
												int32_2,
												",",
												num4.ToString().Replace(',', '.')
											}));
                                            num = 13;
                                            if (class3.IsBot && class3.BotData.RoomUser_0 != null)
                                            {
                                                class3.BotData.RoomUser_0.AddStatus("mv", string.Concat(new object[]
												{
													int32_,
													",",
													int32_2,
													",",
													(num4 + 1.0).ToString().Replace(',', '.')
												}));
                                            }
                                            int num5;
                                            if (class3.bool_3)
                                            {
                                                num5 = Rotation.CalculateMoonWalk(class3.X, class3.Y, int32_, int32_2);
                                            }
                                            else
                                            {
                                                num5 = Rotation.Calculate(class3.X, class3.Y, int32_, int32_2);
                                            }
                                            class3.RotBody = num5;
                                            class3.RotHead = num5;
                                            class3.SetStep = true;
                                            class3.SetX = int32_;
                                            class3.SetY = int32_2;
                                            class3.SetZ = num4;
                                            num = 14;
                                            if (class3.IsBot && class3.BotData.RoomUser_0 != null)
                                            {
                                                class3.BotData.RoomUser_0.RotBody = num5;
                                                class3.BotData.RoomUser_0.RotHead = num5;
                                                class3.BotData.RoomUser_0.SetStep = true;
                                                class3.BotData.RoomUser_0.SetX = int32_;
                                                class3.BotData.RoomUser_0.SetY = int32_2;
                                                class3.BotData.RoomUser_0.SetZ = num4 + 1.0;
                                            }
                                            try
                                            {
                                                num = 15;
                                                if (!class3.IsBot)
                                                {
                                                    if (class3.GetClient().GetHabbo().Gender.ToLower() == "m" && this.byte_1[int32_, int32_2] > 0 && class3.byte_1 != this.byte_1[int32_, int32_2])
                                                    {
                                                        class3.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect((int)this.byte_1[int32_, int32_2], true);
                                                        class3.byte_1 = this.byte_1[int32_, int32_2];
                                                    }
                                                    else
                                                    {
                                                        if (class3.GetClient().GetHabbo().Gender.ToLower() == "f" && this.byte_2[int32_, int32_2] > 0 && class3.byte_1 != this.byte_2[int32_, int32_2])
                                                        {
                                                            class3.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect((int)this.byte_2[int32_, int32_2], true);
                                                            class3.byte_1 = this.byte_2[int32_, int32_2];
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!class3.IsPet)
                                                    {
                                                        if (this.byte_1[int32_, int32_2] > 0)
                                                        {
                                                            class3.BotData.EffectId = (int)this.byte_1[int32_, int32_2];
                                                            class3.byte_1 = this.byte_1[int32_, int32_2];
                                                        }
                                                        ServerMessage Message3 = new ServerMessage(485u);
                                                        Message3.AppendInt32(class3.VirtualId);
                                                        Message3.AppendInt32(class3.BotData.EffectId);
                                                        this.SendMessage(Message3, null);
                                                    }
                                                }
                                                goto IL_CE1;
                                            }
                                            catch
                                            {
                                                goto IL_CE1;
                                            }
                                        IL_B8B:
                                            this.UpdateUserStatus(class3, false, true);
                                            class3.UpdateNeeded = true;
                                            if (class3.IsBot && class3.BotData.RoomUser_0 != null)
                                            {
                                                this.UpdateUserStatus(class3.BotData.RoomUser_0, true, true);
                                                class3.BotData.RoomUser_0.UpdateNeeded = true;
                                                goto IL_BE0;
                                            }
                                            goto IL_BE0;
                                        IL_CE1:
                                            num = 16;
                                            this.byte_0[class3.X, class3.Y] = class3.SqState;
                                            class3.SqState = this.byte_0[class3.SetX, class3.SetY];
                                            if (this.AllowWalkthrough)
                                            {
                                                goto IL_B8B;
                                            }
                                            this.byte_0[int32_, int32_2] = 0;
                                            goto IL_B8B;
                                        }
                                        num = 12;
                                        class3.IsWalking = false;
                                        class3.RemoveStatus("mv");
                                        class3.PathRecalcNeeded = false;
                                        if (class3.IsBot && class3.BotData.RoomUser_0 != null)
                                        {
                                            class3.BotData.RoomUser_0.RemoveStatus("mv");
                                            class3.BotData.RoomUser_0.IsWalking = false;
                                            class3.BotData.RoomUser_0.PathRecalcNeeded = false;
                                            class3.BotData.RoomUser_0.UpdateNeeded = true;
                                        }
                                    IL_BE0:
                                        class3.UpdateNeeded = true;
                                    }
                                    else
                                    {
                                        num = 17;
                                        if (class3.Statusses.ContainsKey("mv") && class3.class34_1 == null)
                                        {
                                            num = 18;
                                            class3.RemoveStatus("mv");
                                            class3.UpdateNeeded = true;
                                            if (class3.IsBot && class3.BotData.RoomUser_0 != null)
                                            {
                                                class3.BotData.RoomUser_0.RemoveStatus("mv");
                                                class3.BotData.RoomUser_0.UpdateNeeded = true;
                                            }
                                        }
                                    }
                                    if (class3.IsBot || class3.IsPet)
                                    {
                                        try
                                        {
                                            class3.BotAI.OnTimerTick();
                                            goto IL_C9F;
                                        }
                                        catch
                                        {
                                            goto IL_C9F;
                                        }
                                    }
                                    goto IL_C9B;
                                IL_C9F:
                                    if (class3.int_9 > 0)
                                    {
                                        if (class3.int_9 == 1)
                                        {
                                            this.UpdateUserStatus(class3, true, true);
                                        }
                                        class3.int_9--;
                                        goto IL_CD6;
                                    }
                                    goto IL_CD6;
                                IL_C9B:
                                    num2++;
                                    goto IL_C9F;
                                }
                            IL_CD6: ;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.LogThreadException(ex.ToString(), string.Concat(new object[]
							{
								"Room [ID: ",
								this.RoomId,
								"] [Part: ",
								num,
								" cycle task -- Process Users Updates"
							}));
                            this.CrashRoom();
                        }
                    }
                    try
                    {
                        foreach (uint current in list)
                        {
                            this.RemoveUserFromRoom(PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(current), true, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.RoomId + "] cycle task -- Remove Users");
                        this.CrashRoom();
                    }
                    if (num2 >= 1)
                    {
                        this.int_8 = 0;
                    }
                    else
                    {
                        this.int_8++;
                    }
                    if (!this.bool_6 && !this.bool_7)
                    {
                        try
                        {
                            if (this.int_8 >= 60)
                            {
                                PhoenixEnvironment.GetGame().GetRoomManager().UnloadRoom(this);
                                return;
                            }
                            ServerMessage Logging = this.method_67(false);
                            if (Logging != null)
                            {
                                this.SendMessage(Logging, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.RoomId + "] cycle task -- Cycle End");
                            this.CrashRoom();
                        }
                    }
                    this.class27_0.UsersNow = num3;
                }
                catch (Exception ex)
                {
                    Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.RoomId + "] cycle task");
                }
            }
        }
        private void CrashRoom()
        {
            if (!this.bool_7 && GlobalClass.UnloadCrashedRooms)
            {
                this.bool_7 = true;
                try
                {
                    this.method_31(TextManager.GetText("error_roomunload"));
                }
                catch
                {
                }
                PhoenixEnvironment.GetGame().GetRoomManager().UnloadRoom(this);
            }
        }
        private void method_35()
        {
            if (this.mGotRollers)
            {
                if (this.int_16 >= this.int_15 || this.int_15 == 0)
                {
                    Hashtable hashtable = this.mFloorItems.Clone() as Hashtable;
                    List<uint> list = new List<uint>();
                    List<uint> list2 = new List<uint>();
                    foreach (RoomItem @class in hashtable.Values)
                    {
                        if (@class.IsRoller)
                        {
                            Coord gStruct1_ = @class.SquareInFront;
                            if (gStruct1_.X >= this.Model.MapSizeX || gStruct1_.Y >= this.Model.MapSizeY || gStruct1_.X < 0 || gStruct1_.Y < 0)
                            {
                                return;
                            }
                            List<RoomItem> list3 = this.method_45(@class.GetX, @class.GetY);
                            RoomUser class2 = this.GetUserForSquare(@class.GetX, @class.GetY);
                            if (list3.Count > 0 || class2 != null)
                            {
                                List<RoomItem> list4 = this.method_45(gStruct1_.X, gStruct1_.Y);
                                double num = this.Model.double_1[gStruct1_.X, gStruct1_.Y];
                                int num2 = 0;
                                int num3 = 0;
                                bool flag = false;
                                foreach (RoomItem current in list4)
                                {
                                    if (current.Double_1 > num)
                                    {
                                        num = current.Double_1;
                                    }
                                    if (!current.IsRoller)
                                    {
                                        num2++;
                                    }
                                    else
                                    {
                                        num3++;
                                    }
                                    if (!flag && current.GetBaseItem().InteractionType.ToLower() == "wf_trg_timer")
                                    {
                                        flag = true;
                                    }
                                }
                                bool flag2 = num2 > 0;
                                if (this.GetUserForSquare(gStruct1_.X, gStruct1_.Y) != null)
                                {
                                    flag2 = true;
                                }
                                bool flag3 = num3 > 0;
                                foreach (RoomItem current in list3)
                                {
                                    bool flag4 = current.GetBaseItem().InteractionType.ToLower() == "wf_trg_timer";
                                    if (!current.IsRoller && !list.Contains(current.Id) && this.method_36(gStruct1_.X, gStruct1_.Y) && (!flag2 || !flag3) && @class.Double_0 < current.Double_0 && this.GetUserForSquare(gStruct1_.X, gStruct1_.Y) == null && (!flag4 || !flag))
                                    {
                                        double double_;
                                        if (flag3)
                                        {
                                            double_ = current.Double_0;
                                        }
                                        else
                                        {
                                            double_ = current.Double_0 - @class.Double_1 + this.Model.double_1[gStruct1_.X, gStruct1_.Y];
                                        }
                                        this.method_41(current, gStruct1_, @class.Id, double_);
                                        list.Add(current.Id);
                                    }
                                }
                                if (class2 != null && (!flag2 || !flag3) && this.method_37(gStruct1_.X, gStruct1_.Y, false, true, false, true, true) && !list2.Contains(class2.HabboId) && !class2.IsWalking)
                                {
                                    if (this.double_2[gStruct1_.X, gStruct1_.Y] > 0.0)
                                    {
                                        num = this.method_84(gStruct1_.X, gStruct1_.Y, this.method_93(gStruct1_.X, gStruct1_.Y));
                                    }
                                    if (class2.IsBot && class2.BotData.RoomUser_0 != null)
                                    {
                                        this.method_42(class2, gStruct1_, @class.Id, num);
                                        list2.Add(class2.HabboId);
                                        this.method_42(class2.BotData.RoomUser_0, gStruct1_, @class.Id, num + 1.0);
                                        list2.Add(class2.BotData.RoomUser_0.HabboId);
                                    }
                                    else
                                    {
                                        if (class2.class34_1 == null)
                                        {
                                            this.method_42(class2, gStruct1_, @class.Id, num);
                                            list2.Add(class2.HabboId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    hashtable.Clear();
                    hashtable = null;
                    list.Clear();
                    list2.Clear();
                    this.int_16 = 0;
                }
                else
                {
                    this.int_16++;
                }
            }
        }
        public bool method_36(int int_17, int int_18)
        {
            bool result;
            if (!this.ValidTile(int_17, int_18))
            {
                result = false;
            }
            else
            {
                if (this.Model.squareState[int_17, int_18] == SquareState.BLOCKED)
                {
                    result = false;
                }
                else
                {
                    List<RoomItem> list = this.method_93(int_17, int_18);
                    if (list.Count > 1)
                    {
                        foreach (RoomItem current in list)
                        {
                            if (current.IsRoller)
                            {
                                result = true;
                                return result;
                            }
                        }
                    }
                    result = true;
                }
            }
            return result;
        }
        public bool method_37(int int_17, int int_18, bool bool_13, bool bool_14, bool bool_15, bool bool_16, bool bool_17)
        {
            bool result;
            if (!this.ValidTile(int_17, int_18))
            {
                result = false;
            }
            else
            {
                if (this.Model.squareState[int_17, int_18] == SquareState.BLOCKED)
                {
                    result = false;
                }
                else
                {
                    if (bool_17 && this.double_2[int_17, int_18] > 0.0)
                    {
                        result = true;
                    }
                    else
                    {
                        if (bool_13 && this.method_97(int_17, int_18))
                        {
                            result = false;
                        }
                        else
                        {
                            if (bool_14)
                            {
                                List<RoomItem> list = this.method_93(int_17, int_18);
                                if (list.Count > 0)
                                {
                                    if (!bool_15 && !bool_16 && !bool_17)
                                    {
                                        result = false;
                                        return result;
                                    }
                                    if (bool_15)
                                    {
                                        foreach (RoomItem current in list)
                                        {
                                            if (!current.GetBaseItem().Stackable)
                                            {
                                                result = false;
                                                return result;
                                            }
                                        }
                                    }
                                    if (bool_16 && bool_17)
                                    {
                                        using (List<RoomItem>.Enumerator enumerator = list.GetEnumerator())
                                        {
                                            while (enumerator.MoveNext())
                                            {
                                                RoomItem current = enumerator.Current;
                                                if (!current.GetBaseItem().Walkable && !current.GetBaseItem().IsSeat)
                                                {
                                                    result = false;
                                                    return result;
                                                }
                                            }
                                            goto IL_1DD;
                                        }
                                    }
                                    if (bool_16)
                                    {
                                        using (List<RoomItem>.Enumerator enumerator = list.GetEnumerator())
                                        {
                                            while (enumerator.MoveNext())
                                            {
                                                RoomItem current = enumerator.Current;
                                                if (!current.GetBaseItem().Walkable)
                                                {
                                                    result = false;
                                                    return result;
                                                }
                                            }
                                            goto IL_1DD;
                                        }
                                    }
                                    if (bool_17)
                                    {
                                        foreach (RoomItem current in list)
                                        {
                                            if (!current.GetBaseItem().IsSeat)
                                            {
                                                result = false;
                                                return result;
                                            }
                                        }
                                    }
                                }
                            }
                        IL_1DD:
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
        internal void method_38(int int_17, int int_18)
        {
            this.byte_0[int_17, int_18] = 1;
        }
        internal void method_39(int int_17, int int_18)
        {
            this.byte_0[int_17, int_18] = 0;
        }
        private void method_40(RoomItem RoomItem_0, int int_17, int int_18, uint uint_2, double double_3)
        {
            ServerMessage Message = new ServerMessage();
            Message.Init(230u);
            Message.AppendInt32(RoomItem_0.GetX);
            Message.AppendInt32(RoomItem_0.GetY);
            Message.AppendInt32(int_17);
            Message.AppendInt32(int_18);
            Message.AppendInt32(1);
            Message.AppendUInt(RoomItem_0.Id);
            Message.AppendStringWithBreak(RoomItem_0.Double_0.ToString().Replace(',', '.'));
            Message.AppendStringWithBreak(double_3.ToString().Replace(',', '.'));
            Message.AppendUInt(uint_2);
            this.SendMessage(Message, null);
            this.method_81(RoomItem_0, int_17, int_18, double_3);
        }
        private void method_41(RoomItem RoomItem_0, Coord gstruct1_1, uint uint_2, double double_3)
        {
            this.method_40(RoomItem_0, gstruct1_1.X, gstruct1_1.Y, uint_2, double_3);
        }
        private void method_42(RoomUser RoomUser_1, Coord gstruct1_1, uint uint_2, double double_3)
        {
            ServerMessage Message = new ServerMessage();
            Message.Init(230u);
            Message.AppendInt32(RoomUser_1.X);
            Message.AppendInt32(RoomUser_1.Y);
            Message.AppendInt32(gstruct1_1.X);
            Message.AppendInt32(gstruct1_1.Y);
            Message.AppendInt32(0);
            Message.AppendUInt(uint_2);
            Message.AppendString("J");
            Message.AppendInt32(RoomUser_1.VirtualId);
            Message.AppendStringWithBreak(RoomUser_1.Z.ToString().Replace(',', '.'));
            Message.AppendStringWithBreak(double_3.ToString().Replace(',', '.'));
            this.SendMessage(Message, null);
            this.byte_0[RoomUser_1.X, RoomUser_1.Y] = 1;
            RoomUser_1.X = gstruct1_1.X;
            RoomUser_1.Y = gstruct1_1.Y;
            RoomUser_1.Z = double_3;
            RoomUser_1.SetX = gstruct1_1.X;
            RoomUser_1.SetY = gstruct1_1.Y;
            RoomUser_1.SetZ = double_3;
            RoomUser_1.int_9 = 2;
            this.byte_0[RoomUser_1.X, RoomUser_1.Y] = 0;
            this.UpdateUserStatus(RoomUser_1, false, true);
        }
        internal RoomUser GetUserForSquare(int int_17, int int_18)
        {
            RoomUser result;
            if (this.UserList != null)
            {
                for (int i = 0; i < this.UserList.Length; i++)
                {
                    RoomUser @class = this.UserList[i];
                    if (@class != null && (@class.X == int_17 && @class.Y == int_18))
                    {
                        result = @class;
                        return result;
                    }
                }
            }
            result = null;
            return result;
        }
        internal RoomUser method_44(int int_17, int int_18)
        {
            RoomUser result;
            if (this.UserList != null)
            {
                for (int i = 0; i < this.UserList.Length; i++)
                {
                    RoomUser @class = this.UserList[i];
                    if (@class != null)
                    {
                        if (@class.X == int_17 && @class.Y == int_18)
                        {
                            result = @class;
                            return result;
                        }
                        if (@class.SetX == int_17 && @class.SetY == int_18)
                        {
                            result = @class;
                            return result;
                        }
                    }
                }
            }
            result = null;
            return result;
        }
        private List<RoomItem> method_45(int int_17, int int_18)
        {
            List<RoomItem> list = new List<RoomItem>();
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                if (@class.GetX == int_17 && @class.GetY == int_18)
                {
                    list.Add(@class);
                }
            }
            return list;
        }
        public void method_46(GameClient Session, bool bool_13)
        {
            RoomUser @class = new RoomUser(Session.GetHabbo().Id, this.RoomId, this.int_7++, Session.GetHabbo().Visible);
            if (@class != null && @class.GetClient() != null && @class.GetClient().GetHabbo() != null)
            {
                if (bool_13 || !@class.Visible)
                {
                    @class.IsSpectator = true;
                }
                else
                {
                    @class.SetPos(this.Model.int_0, this.Model.int_1, this.Model.double_0);
                    @class.SetRot(this.Model.int_2);
                    if (this.CheckRights(Session, true))
                    {
                        @class.AddStatus("flatctrl", "useradmin");
                    }
                    else
                    {
                        if (this.CheckRights(Session))
                        {
                            @class.AddStatus("flatctrl", "");
                        }
                    }
                    if (!@class.IsBot && @class.GetClient().GetHabbo().IsTeleporting)
                    {
                        RoomItem class2 = this.GetItem(@class.GetClient().GetHabbo().TeleporterId);
                        if (class2 != null)
                        {
                            @class.SetPos(class2.GetX, class2.GetY, class2.Double_0);
                            @class.SetRot(class2.Rot);
                            class2.InteractingUser2 = Session.GetHabbo().Id;
                            class2.ExtraData = "2";
                            class2.UpdateState(false, true);
                        }
                    }
                    @class.GetClient().GetHabbo().IsTeleporting = false;
                    @class.GetClient().GetHabbo().TeleporterId = 0u;
                    ServerMessage Message = new ServerMessage(28u);
                    Message.AppendInt32(1);
                    @class.Serialize(Message);
                    this.SendMessage(Message, null);
                }
                int num = this.method_5();
                @class.CurrentFurniFX = num;
                this.UserList[num] = @class;
                if (!bool_13)
                {
                    this.bool_10 = true;
                }
                Session.GetHabbo().CurrentRoomId = this.Id;
                Session.GetHabbo().GetMessenger().method_5(false);
                Session.GetHabbo().RoomVisits++;
                int num2 = Session.GetHabbo().RoomVisits;
                if (num2 <= 500)
                {
                    if (num2 <= 50)
                    {
                        if (num2 != 5)
                        {
                            if (num2 == 50)
                            {
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 2);
                            }
                        }
                        else
                        {
                            PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 1);
                        }
                    }
                    else
                    {
                        if (num2 != 100)
                        {
                            if (num2 != 200)
                            {
                                if (num2 == 500)
                                {
                                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 5);
                                }
                            }
                            else
                            {
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 4);
                            }
                        }
                        else
                        {
                            PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 3);
                        }
                    }
                }
                else
                {
                    if (num2 <= 1500)
                    {
                        if (num2 != 750)
                        {
                            if (num2 == 1500)
                            {
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 7);
                            }
                        }
                        else
                        {
                            PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 6);
                        }
                    }
                    else
                    {
                        if (num2 != 2500)
                        {
                            if (num2 != 4000)
                            {
                                if (num2 == 5000)
                                {
                                    PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 10);
                                }
                            }
                            else
                            {
                                PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 9);
                            }
                        }
                        else
                        {
                            PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 13u, 8);
                        }
                    }
                }
                Session.GetHabbo().method_10(this.Id);
                if (Session.GetHabbo().GroupID > 0)
                {
                    Guild class3 = GuildManager.GetGuild(Session.GetHabbo().GroupID);
                    if (class3 != null && !this.list_17.Contains(class3))
                    {
                        this.list_17.Add(class3);
                        ServerMessage Message2 = new ServerMessage(309u);
                        Message2.AppendInt32(this.list_17.Count);
                        foreach (Guild current in this.list_17)
                        {
                            Message2.AppendInt32(current.Id);
                            Message2.AppendStringWithBreak(current.Badge);
                        }
                        this.SendMessage(Message2, null);
                    }
                }
                if (!bool_13)
                {
                    this.method_51();
                    for (int i = 0; i < this.UserList.Length; i++)
                    {
                        RoomUser class4 = this.UserList[i];
                        if (class4 != null && class4.IsBot)
                        {
                            class4.BotAI.OnUserEnterRoom(@class);
                        }
                    }
                }
            }
        }
        public void RemoveUserFromRoom(GameClient Session, bool bool_13, bool bool_14)
        {
            int num = 1;
            if (!bool_14 || !Session.GetHabbo().isAaron)
            {
                if (this.bool_12)
                {
                    if (bool_13 && Session != null)
                    {
                        if (bool_14)
                        {
                            ServerMessage Message = new ServerMessage(33u);
                            Message.AppendInt32(4008);
                            Session.SendMessage(Message);
                        }
                        ServerMessage Message5_ = new ServerMessage(18u);
                        Session.SendMessage(Message5_);
                    }
                }
                else
                {
                    try
                    {
                        if (Session != null && Session.GetHabbo() != null)
                        {
                            num = 2;
                            RoomUser @class = this.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            if (@class != null)
                            {
                                this.UserList[@class.CurrentFurniFX] = null;
                                @class.CurrentFurniFX = -1;
                                this.byte_0[@class.X, @class.Y] = @class.SqState;
                            }
                            num = 3;
                            if (bool_13)
                            {
                                if (bool_14)
                                {
                                    ServerMessage Message = new ServerMessage(33u);
                                    Message.AppendInt32(4008);
                                    Session.SendMessage(Message);
                                }
                                ServerMessage Message5_ = new ServerMessage(18u);
                                Session.SendMessage(Message5_);
                            }
                            num = 4;
                            if (@class != null && !@class.IsSpectator)
                            {
                                if (@class.byte_1 > 0 && @class.GetClient() != null)
                                {
                                    @class.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect = -1;
                                }
                                this.byte_0[@class.X, @class.Y] = @class.SqState;
                                if (!this.IsPublic)
                                {
                                    ServerMessage Message2 = new ServerMessage(700u);
                                    Message2.AppendBoolean(false);
                                    Session.SendMessage(Message2);
                                }
                                ServerMessage Message3 = new ServerMessage(29u);
                                Message3.AppendRawInt32(@class.VirtualId);
                                this.SendMessage(Message3, null);
                                if (this.method_74(Session.GetHabbo().Id))
                                {
                                    this.method_78(Session.GetHabbo().Id);
                                }
                                num = 5;
                                if (Session.GetHabbo().Username.ToLower() == this.Owner.ToLower() && this.HasOngoingEvent)
                                {
                                    this.Event = null;
                                    ServerMessage Logging = new ServerMessage(370u);
                                    Logging.AppendStringWithBreak("-1");
                                    this.SendMessage(Logging, null);
                                }
                                num = 6;
                                if (@class.class34_1 != null)
                                {
                                    @class.class34_1.RoomUser_0 = null;
                                    @class.class34_1 = null;
                                    Session.GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect = -1;
                                }
                                Session.GetHabbo().method_11();
                                this.bool_10 = true;
                                this.method_51();
                                List<RoomUser> list = new List<RoomUser>();
                                for (int i = 0; i < this.UserList.Length; i++)
                                {
                                    RoomUser class2 = this.UserList[i];
                                    if (class2 != null && class2.IsBot)
                                    {
                                        list.Add(class2);
                                    }
                                }
                                num = 7;
                                foreach (RoomUser current in list)
                                {
                                    current.BotAI.OnUserLeaveRoom(Session);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogCriticalException(string.Concat(new object[]
						{
							"Error during removing user from room [Part: ",
							num,
							"]: ",
							ex.ToString()
						}));
                    }
                }
            }
        }
        public RoomUser method_48(uint uint_2)
        {
            RoomUser result;
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null && @class.IsBot && @class.IsPet && @class.PetData != null && @class.PetData.PetId == uint_2)
                {
                    result = @class;
                    return result;
                }
            }
            result = null;
            return result;
        }
        public bool method_49(uint uint_2)
        {
            return this.method_48(uint_2) != null;
        }
        public void method_50()
        {
            this.UsersNow = this.UserCount;
            using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
            {
                @class.ExecuteQuery(string.Concat(new object[]
				{
					"UPDATE rooms SET users_now = '",
					this.UserCount,
					"' WHERE Id = '",
					this.Id,
					"' LIMIT 1"
				}));
            }
        }
        public void method_51()
        {
            this.UsersNow = this.UserCount;
        }
        public RoomUser method_52(int int_17)
        {
            RoomUser result;
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null && @class.VirtualId == int_17)
                {
                    result = @class;
                    return result;
                }
            }
            result = null;
            return result;
        }
        public RoomUser GetRoomUserByHabbo(uint Id)
        {
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser User = this.UserList[i];
                if (User != null && !User.IsBot && User.HabboId == Id)
                {
                    return User;
                }
            }
            return null;
        }
        public void method_54()
        {
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null && (!@class.IsBot && @class.class34_1 == null))
                {
                    @class.DanceId = 1;
                    ServerMessage Message = new ServerMessage(480u);
                    Message.AppendInt32(@class.VirtualId);
                    Message.AppendInt32(1);
                    this.SendMessage(Message, null);
                }
            }
        }
        public void method_55()
        {
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null && (!@class.IsBot && @class.class34_1 == null) && (!@class.Statusses.ContainsKey("sit") && !@class.Statusses.ContainsKey("lay") && @class.RotBody != 1 && @class.RotBody != 3 && @class.RotBody != 5 && @class.RotBody != 7))
                {
                    @class.AddStatus("sit", ((@class.Z + 1.0) / 2.0 - @class.Z * 0.5).ToString());
                    @class.UpdateNeeded = true;
                }
            }
        }
        public RoomUser GetRoomUserByHabbo(string string_10)
        {
            RoomUser result;
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null && !@class.IsBot && @class.GetClient().GetHabbo() != null && @class.GetClient().GetHabbo().Username.ToLower() == string_10.ToLower())
                {
                    result = @class;
                    return result;
                }
            }
            result = null;
            return result;
        }
        public RoomUser method_57(string string_10)
        {
            RoomUser result;
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null && @class.IsBot && @class.BotData.Name.ToLower() == string_10.ToLower())
                {
                    result = @class;
                    return result;
                }
            }
            result = null;
            return result;
        }
        internal void method_58(ServerMessage Message5_0, List<uint> list_18, uint uint_2)
        {
            List<uint> list = new List<uint>();
            if (list_18 != null)
            {
                if (this.UserList == null)
                {
                    return;
                }
                for (int i = 0; i < this.UserList.Length; i++)
                {
                    RoomUser @class = this.UserList[i];
                    if (@class != null && !@class.IsBot)
                    {
                        GameClient class2 = @class.GetClient();
                        if (class2 != null && class2.GetHabbo().Id != uint_2 && class2.GetHabbo().MutedUsers.Contains(uint_2))
                        {
                            list.Add(class2.GetHabbo().Id);
                        }
                    }
                }
            }
            this.SendMessage(Message5_0, list);
        }
        internal void SendMessage(ServerMessage Message5_0, List<uint> list_18)
        {
            try
            {
                if (this.UserList != null)
                {
                    byte[] array = Message5_0.GetBytes();
                    for (int i = 0; i < this.UserList.Length; i++)
                    {
                        RoomUser @class = this.UserList[i];
                        if (@class != null && !@class.IsBot)
                        {
                            GameClient class2 = @class.GetClient();
                            if (class2 != null && (list_18 == null || !list_18.Contains(class2.GetHabbo().Id)))
                            {
                                try
                                {
                                    class2.GetConnection().SendData(array);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
        }
        internal void method_60(ServerMessage Message5_0, int int_17)
        {
            try
            {
                byte[] array = Message5_0.GetBytes();
                for (int i = 0; i < this.UserList.Length; i++)
                {
                    RoomUser @class = this.UserList[i];
                    if (@class != null && !@class.IsBot)
                    {
                        GameClient class2 = @class.GetClient();
                        if (class2 != null && class2.GetHabbo() != null && (ulong)class2.GetHabbo().Rank >= (ulong)((long)int_17))
                        {
                            try
                            {
                                class2.GetConnection().SendData(array);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
        }
        public void method_61(ServerMessage Message5_0)
        {
            try
            {
                byte[] array = Message5_0.GetBytes();
                for (int i = 0; i < this.UserList.Length; i++)
                {
                    RoomUser @class = this.UserList[i];
                    if (@class != null && !@class.IsBot)
                    {
                        GameClient class2 = @class.GetClient();
                        if (class2 != null && this.CheckRights(class2))
                        {
                            try
                            {
                                class2.GetConnection().SendData(array);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
        }
        public void method_62()
        {
            this.SendMessage(new ServerMessage(18u), null);
            this.method_63();
        }
        public void method_63()
        {
            this.method_66(true);
            GC.SuppressFinalize(this);
        }
        internal void method_64()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Dictionary<uint, bool> dictionary = new Dictionary<uint, bool>();
            try
            {
                try
                {
                    using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        if (this.WF_Triggers.Count > 0)
                        {
                            lock (this.WF_Triggers)
                            {
                                foreach (RoomItem class2 in this.WF_Triggers)
                                {
                                    try
                                    {
                                        if (!dictionary.ContainsKey(class2.Id))
                                        {
                                            if (class2.Extra1 != "" || class2.Extra2 != "" || class2.Extra3 != "" || class2.Extra4 != "" || class2.Extra5 != "")
                                            {
                                                @class.AddParamWithValue(class2.Id + "Extra1", class2.Extra1);
                                                @class.AddParamWithValue(class2.Id + "Extra2", class2.Extra2);
                                                @class.AddParamWithValue(class2.Id + "Extra3", class2.Extra3);
                                                @class.AddParamWithValue(class2.Id + "Extra4", class2.Extra4);
                                                @class.AddParamWithValue(class2.Id + "Extra5", class2.Extra5);
                                                stringBuilder.Append(string.Concat(new object[]
												{
													"DELETE FROM wired_items WHERE item_id = '",
													class2.Id,
													"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
													class2.Id,
													"',@",
													class2.Id,
													"Extra1,@",
													class2.Id,
													"Extra2,@",
													class2.Id,
													"Extra3,@",
													class2.Id,
													"Extra4,@",
													class2.Id,
													"Extra5); "
												}));
                                            }
                                            dictionary.Add(class2.Id, true);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        if (this.WF_Effects.Count > 0)
                        {
                            lock (this.WF_Effects)
                            {
                                foreach (RoomItem class2 in this.WF_Effects)
                                {
                                    try
                                    {
                                        if (!dictionary.ContainsKey(class2.Id))
                                        {
                                            if (class2.Extra1 != "" || class2.Extra2 != "" || class2.Extra3 != "" || class2.Extra4 != "" || class2.Extra5 != "")
                                            {
                                                @class.AddParamWithValue(class2.Id + "Extra1", class2.Extra1);
                                                @class.AddParamWithValue(class2.Id + "Extra2", class2.Extra2);
                                                @class.AddParamWithValue(class2.Id + "Extra3", class2.Extra3);
                                                @class.AddParamWithValue(class2.Id + "Extra4", class2.Extra4);
                                                @class.AddParamWithValue(class2.Id + "Extra5", class2.Extra5);
                                                stringBuilder.Append(string.Concat(new object[]
												{
													"DELETE FROM wired_items WHERE item_id = '",
													class2.Id,
													"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
													class2.Id,
													"',@",
													class2.Id,
													"Extra1,@",
													class2.Id,
													"Extra2,@",
													class2.Id,
													"Extra3,@",
													class2.Id,
													"Extra4,@",
													class2.Id,
													"Extra5); "
												}));
                                            }
                                            dictionary.Add(class2.Id, true);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        if (this.WF_Conditions.Count > 0)
                        {
                            lock (this.WF_Conditions)
                            {
                                foreach (RoomItem class2 in this.WF_Conditions)
                                {
                                    try
                                    {
                                        if (!dictionary.ContainsKey(class2.Id))
                                        {
                                            if (class2.Extra1 != "" || class2.Extra2 != "" || class2.Extra3 != "" || class2.Extra4 != "" || class2.Extra5 != "")
                                            {
                                                @class.AddParamWithValue(class2.Id + "Extra1", class2.Extra1);
                                                @class.AddParamWithValue(class2.Id + "Extra2", class2.Extra2);
                                                @class.AddParamWithValue(class2.Id + "Extra3", class2.Extra3);
                                                @class.AddParamWithValue(class2.Id + "Extra4", class2.Extra4);
                                                @class.AddParamWithValue(class2.Id + "Extra5", class2.Extra5);
                                                stringBuilder.Append(string.Concat(new object[]
												{
													"DELETE FROM wired_items WHERE item_id = '",
													class2.Id,
													"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
													class2.Id,
													"',@",
													class2.Id,
													"Extra1,@",
													class2.Id,
													"Extra2,@",
													class2.Id,
													"Extra3,@",
													class2.Id,
													"Extra4,@",
													class2.Id,
													"Extra5); "
												}));
                                            }
                                            dictionary.Add(class2.Id, true);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        if (stringBuilder.Length > 0)
                        {
                            @class.ExecuteQuery(stringBuilder.ToString());
                        }
                        dictionary.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogCriticalException(string.Concat(new object[]
					{
						"Error during saving wired items for room ",
						this.RoomId,
						". Stack: ",
						ex.ToString(),
						"\rQuery: ",
						stringBuilder.ToString()
					}));
                }
                if (this.mAddedItems.Count > 0 || this.mRemovedItems.Count > 0 || this.mMovedItems.Count > 0 || this.Boolean_4)
                {
                    using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        stringBuilder.Clear();
                        lock (this.mRemovedItems)
                        {
                            foreach (RoomItem class2 in this.mRemovedItems.Values)
                            {
                                stringBuilder.Append(string.Concat(new object[]
								{
									"UPDATE items SET room_id = '0' WHERE Id = '",
									class2.Id,
									"' AND room_id = '",
									this.RoomId,
									"' LIMIT 1; "
								}));
                            }
                        }
                        this.mRemovedItems.Clear();
                        lock (this.mAddedItems)
                        {
                            if (this.mAddedItems.Count > 0)
                            {
                                int num = 0;
                                int num2 = 0;
                                foreach (RoomItem class2 in this.mAddedItems.Values)
                                {
                                    if (class2.IsFloorItem)
                                    {
                                        num2++;
                                    }
                                    else
                                    {
                                        num++;
                                    }
                                }
                                if (num2 > 0)
                                {
                                    foreach (RoomItem class2 in this.mAddedItems.Values)
                                    {
                                        if (class2.IsFloorItem)
                                        {
                                            @class.AddParamWithValue("extra_data" + class2.Id, class2.ExtraData);
                                            stringBuilder.Append(string.Concat(new object[]
											{
												"UPDATE items SET room_id = '",
												this.RoomId,
												"', base_item = '",
												class2.uint_2,
												"', extra_data = @extra_data",
												class2.Id,
												", x = '",
												class2.GetX,
												"', y = '",
												class2.GetY,
												"', z = '",
												class2.Double_0,
												"', rot = '",
												class2.Rot,
												"', wall_pos = '' WHERE Id = '",
												class2.Id,
												"' LIMIT 1; "
											}));
                                        }
                                    }
                                }
                                if (num > 0)
                                {
                                    foreach (RoomItem class2 in this.mAddedItems.Values)
                                    {
                                        if (class2.IsWallItem)
                                        {
                                            @class.AddParamWithValue("extra_data" + class2.Id, class2.ExtraData);
                                            @class.AddParamWithValue("pos" + class2.Id, class2.string_7);
                                            stringBuilder.Append(string.Concat(new object[]
											{
												"UPDATE items SET room_id = '",
												this.RoomId,
												"', base_item = '",
												class2.uint_2,
												"', extra_data = @extra_data",
												class2.Id,
												", x = '0', y = '0', z = '0', rot = '0', wall_pos = @pos",
												class2.Id,
												" WHERE Id = '",
												class2.Id,
												"' LIMIT 1; "
											}));
                                        }
                                    }
                                }
                            }
                        }
                        this.mAddedItems.Clear();
                        lock (this.mMovedItems)
                        {
                            foreach (RoomItem class2 in this.mMovedItems.Values)
                            {
                                @class.AddParamWithValue("mextra_data" + class2.Id, class2.ExtraData);
                                stringBuilder.Append(string.Concat(new object[]
								{
									"UPDATE items SET x = '",
									class2.GetX,
									"', y = '",
									class2.GetY,
									"', z = '",
									class2.Double_0,
									"', rot = '",
									class2.Rot,
									"', wall_pos = '",
									class2.string_7,
									"', extra_data = @mextra_data",
									class2.Id,
									" WHERE Id = '",
									class2.Id,
									"' LIMIT 1; "
								}));
                            }
                        }
                        this.mMovedItems.Clear();
                        lock (this.method_2())
                        {
                            foreach (Pet current in this.method_2())
                            {
                                if (current.DBState == DatabaseUpdateState.NeedsInsert)
                                {
                                    @class.AddParamWithValue("petname" + current.PetId, current.Name);
                                    @class.AddParamWithValue("petcolor" + current.PetId, current.Color);
                                    @class.AddParamWithValue("petrace" + current.PetId, current.Race);
                                    stringBuilder.Append(string.Concat(new object[]
									{
										"INSERT INTO `user_pets` VALUES ('",
										current.PetId,
										"', '",
										current.OwnerId,
										"', '",
										current.RoomId,
										"', @petname",
										current.PetId,
										", @petrace",
										current.PetId,
										", @petcolor",
										current.PetId,
										", '",
										current.Type,
										"', '",
										current.Expirience,
										"', '",
										current.Energy,
										"', '",
										current.Nutrition,
										"', '",
										current.Respect,
										"', '",
										current.CreationStamp,
										"', '",
										current.X,
										"', '",
										current.Y,
										"', '",
										current.Z,
										"'); "
									}));
                                }
                                else
                                {
                                    if (current.DBState == DatabaseUpdateState.NeedsUpdate)
                                    {
                                        stringBuilder.Append(string.Concat(new object[]
										{
											"UPDATE user_pets SET room_id = '",
											current.RoomId,
											"', expirience = '",
											current.Expirience,
											"', energy = '",
											current.Energy,
											"', nutrition = '",
											current.Nutrition,
											"', respect = '",
											current.Respect,
											"', x = '",
											current.X,
											"', y = '",
											current.Y,
											"', z = '",
											current.Z,
											"' WHERE Id = '",
											current.PetId,
											"' LIMIT 1; "
										}));
                                    }
                                }
                                current.DBState = DatabaseUpdateState.Updated;
                            }
                        }
                        if (stringBuilder.Length > 0)
                        {
                            @class.ExecuteQuery(stringBuilder.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogCriticalException(string.Concat(new object[]
				{
					"Error during saving furniture for room ",
					this.RoomId,
					". Stack: ",
					ex.ToString(),
					"\r Query: ",
					stringBuilder.ToString()
				}));
            }
        }
        internal void method_65(DatabaseClient class6_0)
        {
            try
            {
                Dictionary<uint, bool> dictionary = new Dictionary<uint, bool>();
                StringBuilder stringBuilder = new StringBuilder();
                if (this.WF_Triggers.Count > 0)
                {
                    foreach (RoomItem @class in this.WF_Triggers)
                    {
                        if (@class.Extra1 != "" || @class.Extra2 != "" || @class.Extra3 != "" || @class.Extra4 != "" || @class.Extra5 != "")
                        {
                            try
                            {
                                if (!dictionary.ContainsKey(@class.Id))
                                {
                                    if (@class.Extra1 != "" || @class.Extra2 != "" || @class.Extra3 != "" || @class.Extra4 != "" || @class.Extra5 != "")
                                    {
                                        class6_0.AddParamWithValue(@class.Id + "Extra1", @class.Extra1);
                                        class6_0.AddParamWithValue(@class.Id + "Extra2", @class.Extra2);
                                        class6_0.AddParamWithValue(@class.Id + "Extra3", @class.Extra3);
                                        class6_0.AddParamWithValue(@class.Id + "Extra4", @class.Extra4);
                                        class6_0.AddParamWithValue(@class.Id + "Extra5", @class.Extra5);
                                        stringBuilder.Append(string.Concat(new object[]
										{
											"DELETE FROM wired_items WHERE item_id = '",
											@class.Id,
											"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
											@class.Id,
											"',@",
											@class.Id,
											"Extra1,@",
											@class.Id,
											"Extra2,@",
											@class.Id,
											"Extra3,@",
											@class.Id,
											"Extra4,@",
											@class.Id,
											"Extra5); "
										}));
                                    }
                                    dictionary.Add(@class.Id, true);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                if (this.WF_Effects.Count > 0)
                {
                    foreach (RoomItem @class in this.WF_Effects)
                    {
                        if (@class.Extra1 != "" || @class.Extra2 != "" || @class.Extra3 != "" || @class.Extra4 != "" || @class.Extra5 != "")
                        {
                            try
                            {
                                if (!dictionary.ContainsKey(@class.Id))
                                {
                                    if (@class.Extra1 != "" || @class.Extra2 != "" || @class.Extra3 != "" || @class.Extra4 != "" || @class.Extra5 != "")
                                    {
                                        class6_0.AddParamWithValue(@class.Id + "Extra1", @class.Extra1);
                                        class6_0.AddParamWithValue(@class.Id + "Extra2", @class.Extra2);
                                        class6_0.AddParamWithValue(@class.Id + "Extra3", @class.Extra3);
                                        class6_0.AddParamWithValue(@class.Id + "Extra4", @class.Extra4);
                                        class6_0.AddParamWithValue(@class.Id + "Extra5", @class.Extra5);
                                        stringBuilder.Append(string.Concat(new object[]
										{
											"DELETE FROM wired_items WHERE item_id = '",
											@class.Id,
											"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
											@class.Id,
											"',@",
											@class.Id,
											"Extra1,@",
											@class.Id,
											"Extra2,@",
											@class.Id,
											"Extra3,@",
											@class.Id,
											"Extra4,@",
											@class.Id,
											"Extra5); "
										}));
                                    }
                                    dictionary.Add(@class.Id, true);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                if (this.WF_Conditions.Count > 0)
                {
                    foreach (RoomItem @class in this.WF_Conditions)
                    {
                        if (@class.Extra1 != "" || @class.Extra2 != "" || @class.Extra3 != "" || @class.Extra4 != "" || @class.Extra5 != "")
                        {
                            try
                            {
                                if (!dictionary.ContainsKey(@class.Id))
                                {
                                    if (@class.Extra1 != "" || @class.Extra2 != "" || @class.Extra3 != "" || @class.Extra4 != "" || @class.Extra5 != "")
                                    {
                                        class6_0.AddParamWithValue(@class.Id + "Extra1", @class.Extra1);
                                        class6_0.AddParamWithValue(@class.Id + "Extra2", @class.Extra2);
                                        class6_0.AddParamWithValue(@class.Id + "Extra3", @class.Extra3);
                                        class6_0.AddParamWithValue(@class.Id + "Extra4", @class.Extra4);
                                        class6_0.AddParamWithValue(@class.Id + "Extra5", @class.Extra5);
                                        stringBuilder.Append(string.Concat(new object[]
										{
											"DELETE FROM wired_items WHERE item_id = '",
											@class.Id,
											"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
											@class.Id,
											"',@",
											@class.Id,
											"Extra1,@",
											@class.Id,
											"Extra2,@",
											@class.Id,
											"Extra3,@",
											@class.Id,
											"Extra4,@",
											@class.Id,
											"Extra5); "
										}));
                                    }
                                    dictionary.Add(@class.Id, true);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                dictionary.Clear();
                if (this.mAddedItems.Count > 0 || this.mRemovedItems.Count > 0 || this.mMovedItems.Count > 0 || this.Boolean_4)
                {
                    foreach (RoomItem @class in this.mRemovedItems.Values)
                    {
                        stringBuilder.Append(string.Concat(new object[]
						{
							"UPDATE items SET room_id = 0 WHERE Id = '",
							@class.Id,
							"' AND room_id = '",
							this.RoomId,
							"' LIMIT 1; "
						}));
                    }
                    this.mRemovedItems.Clear();
                    IEnumerator enumerator2;
                    if (this.mAddedItems.Count > 0)
                    {
                        enumerator2 = this.mAddedItems.Values.GetEnumerator();
                        try
                        {
                            while (enumerator2.MoveNext())
                            {
                                RoomItem @class = (RoomItem)enumerator2.Current;
                                stringBuilder.Append("UPDATE items SET room_id = 0 WHERE Id = '" + @class.Id + "' LIMIT 1; ");
                            }
                        }
                        finally
                        {
                            IDisposable disposable = enumerator2 as IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                        int num = 0;
                        int num2 = 0;
                        enumerator2 = this.mAddedItems.Values.GetEnumerator();
                        try
                        {
                            while (enumerator2.MoveNext())
                            {
                                RoomItem @class = (RoomItem)enumerator2.Current;
                                if (@class.IsFloorItem)
                                {
                                    num2++;
                                }
                                else
                                {
                                    num++;
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable = enumerator2 as IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                        if (num2 > 0)
                        {
                            enumerator2 = this.mAddedItems.Values.GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    RoomItem @class = (RoomItem)enumerator2.Current;
                                    if (@class.IsFloorItem)
                                    {
                                        class6_0.AddParamWithValue("extra_data" + @class.Id, @class.ExtraData);
                                        stringBuilder.Append(string.Concat(new object[]
										{
											"UPDATE items SET room_id = '",
											this.RoomId,
											"', base_item = '",
											@class.uint_2,
											"', extra_data = @extra_data",
											@class.Id,
											", x = '",
											@class.GetX,
											"', y = '",
											@class.GetY,
											"', z = '",
											@class.Double_0,
											"', rot = '",
											@class.Rot,
											"', wall_pos = '' WHERE Id = '",
											@class.Id,
											"' LIMIT 1; "
										}));
                                    }
                                }
                            }
                            finally
                            {
                                IDisposable disposable = enumerator2 as IDisposable;
                                if (disposable != null)
                                {
                                    disposable.Dispose();
                                }
                            }
                        }
                        if (num > 0)
                        {
                            enumerator2 = this.mAddedItems.Values.GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    RoomItem @class = (RoomItem)enumerator2.Current;
                                    if (@class.IsWallItem)
                                    {
                                        class6_0.AddParamWithValue("extra_data" + @class.Id, @class.ExtraData);
                                        class6_0.AddParamWithValue("pos" + @class.Id, @class.string_7);
                                        stringBuilder.Append(string.Concat(new object[]
										{
											"UPDATE items SET room_id = '",
											this.RoomId,
											"', base_item = '",
											@class.uint_2,
											"', extra_data = @extra_data",
											@class.Id,
											", x = '0', y = '0', z = '0', rot = '0', wall_pos = @pos",
											@class.Id,
											" WHERE Id = '",
											@class.Id,
											"' LIMIT 1; "
										}));
                                    }
                                }
                            }
                            finally
                            {
                                IDisposable disposable = enumerator2 as IDisposable;
                                if (disposable != null)
                                {
                                    disposable.Dispose();
                                }
                            }
                        }
                        this.mAddedItems.Clear();
                    }
                    enumerator2 = this.mMovedItems.Values.GetEnumerator();
                    try
                    {
                        while (enumerator2.MoveNext())
                        {
                            RoomItem @class = (RoomItem)enumerator2.Current;
                            stringBuilder.Append(string.Concat(new object[]
							{
								"UPDATE items SET x = '",
								@class.GetX,
								"', y = '",
								@class.GetY,
								"', z = '",
								@class.Double_0,
								"', rot = '",
								@class.Rot,
								"', wall_pos = '' WHERE Id = '",
								@class.Id,
								"' LIMIT 1; "
							}));
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator2 as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                    this.mMovedItems.Clear();
                    foreach (Pet current in this.method_2())
                    {
                        if (current.DBState == DatabaseUpdateState.NeedsInsert)
                        {
                            class6_0.AddParamWithValue("petname" + current.PetId, current.Name);
                            class6_0.AddParamWithValue("petcolor" + current.PetId, current.Color);
                            class6_0.AddParamWithValue("petrace" + current.PetId, current.Race);
                            stringBuilder.Append(string.Concat(new object[]
							{
								"INSERT INTO `user_pets` VALUES ('",
								current.PetId,
								"', '",
								current.OwnerId,
								"', '",
								current.RoomId,
								"', @petname",
								current.PetId,
								", @petrace",
								current.PetId,
								", @petcolor",
								current.PetId,
								", '",
								current.Type,
								"', '",
								current.Expirience,
								"', '",
								current.Energy,
								"', '",
								current.Nutrition,
								"', '",
								current.Respect,
								"', '",
								current.CreationStamp,
								"', '",
								current.X,
								"', '",
								current.Y,
								"', '",
								current.Z,
								"');"
							}));
                        }
                        else
                        {
                            if (current.DBState == DatabaseUpdateState.NeedsUpdate)
                            {
                                stringBuilder.Append(string.Concat(new object[]
								{
									"UPDATE user_pets SET room_id = '",
									current.RoomId,
									"', expirience = '",
									current.Expirience,
									"', energy = '",
									current.Energy,
									"', nutrition = '",
									current.Nutrition,
									"', respect = '",
									current.Respect,
									"', x = '",
									current.X,
									"', y = '",
									current.Y,
									"', z = '",
									current.Z,
									"' WHERE Id = '",
									current.PetId,
									"' LIMIT 1; "
								}));
                            }
                        }
                        current.DBState = DatabaseUpdateState.Updated;
                    }
                }
                if (stringBuilder.Length > 0)
                {
                    class6_0.ExecuteQuery(stringBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                Logging.LogCriticalException(string.Concat(new object[]
				{
					"Error during saving furniture for room ",
					this.RoomId,
					". Stack: ",
					ex.ToString()
				}));
            }
        }
        private void method_66(bool bool_13)
        {
            if (!this.bool_12)
            {
                this.bool_12 = true;
                if (bool_13)
                {
                    this.mGotRollers = false;
                    if (this.timer_0 != null)
                    {
                        this.bool_6 = true;
                        this.timer_0.Change(-1, -1);
                    }
                    this.method_64();
                    using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        @class.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE user_pets SET room_id = 0 WHERE room_id = ",
							this.Id,
							" AND NOT user_id = ",
							PhoenixEnvironment.GetGame().GetClientManager().method_27(this.Owner)
						}));
                    }
                    this.timer_0.Dispose();
                    this.timer_0 = null;
                    this.bool_9 = false;
                    if (this.Tags != null)
                    {
                        this.Tags.Clear();
                    }
                    this.Tags = null;
                    if (this.UserList != null)
                    {
                        Array.Clear(this.UserList, 0, this.UserList.Length);
                    }
                    this.UserList = null;
                    this.class29_0 = null;
                    if (this.UsersWithRights != null)
                    {
                        this.UsersWithRights.Clear();
                    }
                    this.class29_0 = null;
                    if (this.dictionary_0 != null)
                    {
                        this.dictionary_0.Clear();
                    }
                    this.dictionary_0 = null;
                    this.Wallpaper = null;
                    this.Floor = null;
                    this.Landscape = null;
                    if (this.mFloorItems != null)
                    {
                        this.mFloorItems.Clear();
                    }
                    this.mFloorItems = null;
                    if (this.mWallItems != null)
                    {
                        this.mWallItems.Clear();
                    }
                    this.mWallItems = null;
                    this.MoodlightData = null;
                    if (this.list_2 != null)
                    {
                        this.list_2.Clear();
                    }
                    this.list_2 = null;
                    if (this.musicController != null)
                    {
                        this.musicController.UnLinkRoomOutputItem();
                    }
                    this.musicController = null;
                }
            }
        }
        public ServerMessage method_67(bool bool_13)
        {
            List<RoomUser> list = new List<RoomUser>();
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null)
                {
                    if (!bool_13)
                    {
                        if (!@class.UpdateNeeded)
                        {
                            goto IL_35;
                        }
                        @class.UpdateNeeded = false;
                    }
                    list.Add(@class);
                }
            IL_35: ;
            }
            ServerMessage result;
            if (list.Count == 0)
            {
                result = null;
            }
            else
            {
                ServerMessage Message = new ServerMessage(34u);
                Message.AppendInt32(list.Count);
                foreach (RoomUser @class in list)
                {
                    @class.SerializeStatus(Message);
                }
                result = Message;
            }
            return result;
        }
        public bool UserIsBanned(uint uint_2)
        {
            return this.dictionary_0.ContainsKey(uint_2);
        }
        public void RemoveBan(uint uint_2)
        {
            this.dictionary_0.Remove(uint_2);
        }
        public void method_70(uint uint_2)
        {
            this.dictionary_0.Add(uint_2, PhoenixEnvironment.GetUnixTimestamp());
        }
        public bool HasBanExpired(uint uint_2)
        {
            bool result;
            if (!this.UserIsBanned(uint_2))
            {
                result = true;
            }
            else
            {
                double num = PhoenixEnvironment.GetUnixTimestamp() - this.dictionary_0[uint_2];
                result = (num > 900.0);
            }
            return result;
        }
        public int method_72(string string_10)
        {
            int num = 0;
            foreach (RoomItem @class in this.Hashtable_1.Values)
            {
                if (@class.GetBaseItem().InteractionType.ToLower() == string_10.ToLower())
                {
                    num++;
                }
            }
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                if (@class.GetBaseItem().InteractionType.ToLower() == string_10.ToLower())
                {
                    num++;
                }
            }
            return num;
        }
        public bool method_73(RoomUser RoomUser_1)
        {
            return !RoomUser_1.IsBot && this.method_74(RoomUser_1.GetClient().GetHabbo().Id);
        }
        public bool method_74(uint uint_2)
        {
            bool result;
            using (TimedLock.Lock(this.list_2))
            {
                foreach (Trade current in this.list_2)
                {
                    if (current.method_0(uint_2))
                    {
                        result = true;
                        return result;
                    }
                }
            }
            result = false;
            return result;
        }
        public Trade method_75(RoomUser RoomUser_1)
        {
            Trade result;
            if (RoomUser_1.IsBot)
            {
                result = null;
            }
            else
            {
                result = this.method_76(RoomUser_1.GetClient().GetHabbo().Id);
            }
            return result;
        }
        public Trade method_76(uint uint_2)
        {
            Trade result;
            using (TimedLock.Lock(this.list_2))
            {
                foreach (Trade current in this.list_2)
                {
                    if (current.method_0(uint_2))
                    {
                        result = current;
                        return result;
                    }
                }
            }
            result = null;
            return result;
        }
        public void method_77(RoomUser RoomUser_1, RoomUser RoomUser_2)
        {
            if (RoomUser_1 != null && RoomUser_2 != null && (!RoomUser_1.IsBot || RoomUser_1.BotData.Boolean_1) && (!RoomUser_2.IsBot || RoomUser_2.BotData.Boolean_1) && !RoomUser_1.IsTrading && !RoomUser_2.IsTrading && !this.method_73(RoomUser_1) && !this.method_73(RoomUser_2))
            {
                this.list_2.Add(new Trade(RoomUser_1.GetClient().GetHabbo().Id, RoomUser_2.GetClient().GetHabbo().Id, this.RoomId));
            }
        }
        public void method_78(uint uint_2)
        {
            Trade @class = this.method_76(uint_2);
            if (@class != null)
            {
                @class.method_12(uint_2);
                this.list_2.Remove(@class);
            }
        }
        public bool method_79(GameClient Session, RoomItem RoomItem_0, int int_17, int int_18, int int_19, bool bool_13, bool bool_14, bool bool_15)
        {
            Dictionary<int, AffectedTile> dictionary = this.GetAffectedTiles(RoomItem_0.GetBaseItem().Length, RoomItem_0.GetBaseItem().Width, int_17, int_18, int_19);
            bool result;
            if (!this.ValidTile(int_17, int_18))
            {
                result = false;
            }
            else
            {
                foreach (AffectedTile current in dictionary.Values)
                {
                    if (!this.ValidTile(current.X, current.Y))
                    {
                        result = false;
                        return result;
                    }
                }
                double num = this.Model.double_1[int_17, int_18];
                if (!bool_14)
                {
                    if (RoomItem_0.Rot == int_19 && RoomItem_0.GetX == int_17 && RoomItem_0.GetY == int_18 && RoomItem_0.Double_0 != num)
                    {
                        result = false;
                        return result;
                    }
                    if (this.Model.squareState[int_17, int_18] != SquareState.OPEN)
                    {
                        result = false;
                        return result;
                    }
                    foreach (AffectedTile current in dictionary.Values)
                    {
                        if (this.Model.squareState[current.X, current.Y] != SquareState.OPEN)
                        {
                            result = false;
                            return result;
                        }
                    }
                    if (RoomItem_0.GetBaseItem().IsSeat || RoomItem_0.IsRoller)
                    {
                        goto IL_1FE;
                    }
                    if (this.method_97(int_17, int_18))
                    {
                        result = false;
                        return result;
                    }
                    using (Dictionary<int, AffectedTile>.ValueCollection.Enumerator enumerator = dictionary.Values.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            AffectedTile current = enumerator.Current;
                            if (this.method_97(current.X, current.Y))
                            {
                                result = false;
                                return result;
                            }
                        }
                        goto IL_1FE;
                    }
                }
                if (this.Model.squareState[int_17, int_18] != SquareState.OPEN)
                {
                    result = false;
                    return result;
                }
                if (!bool_15 && this.method_97(int_17, int_18))
                {
                    result = false;
                    return result;
                }
            IL_1FE:
                List<RoomItem> list = this.method_93(int_17, int_18);
                List<RoomItem> list2 = new List<RoomItem>();
                List<RoomItem> list3 = new List<RoomItem>();
                foreach (AffectedTile current in dictionary.Values)
                {
                    List<RoomItem> list4 = this.method_93(current.X, current.Y);
                    if (list4 != null)
                    {
                        list2.AddRange(list4);
                    }
                }
                if (list == null)
                {
                    list = new List<RoomItem>();
                }
                list3.AddRange(list);
                list3.AddRange(list2);
                int num2 = 0;
                foreach (RoomItem current2 in list3)
                {
                    if (current2 != null && current2.Id != RoomItem_0.Id && current2.GetBaseItem() != null)
                    {
                        if (!current2.GetBaseItem().Stackable)
                        {
                            result = false;
                            return result;
                        }
                        if (RoomItem_0.GetBaseItem().InteractionType.ToLower() == "wf_trg_timer" && current2.GetBaseItem().InteractionType.ToLower() == "wf_trg_timer")
                        {
                            result = false;
                            return result;
                        }
                        if (RoomItem_0.GetBaseItem().InteractionType.ToLower() == "ball")
                        {
                            if (current2.GetBaseItem().InteractionType.ToLower() == "blue_goal")
                            {
                                num2 = 11;
                            }
                            if (current2.GetBaseItem().InteractionType.ToLower() == "red_goal")
                            {
                                num2 = 5;
                            }
                            if (current2.GetBaseItem().InteractionType.ToLower() == "yellow_goal")
                            {
                                num2 = 14;
                            }
                            if (current2.GetBaseItem().InteractionType.ToLower() == "green_goal")
                            {
                                num2 = 8;
                            }
                        }
                    }
                }
                if (num2 > 0)
                {
                    this.method_89(num2, RoomItem_0, false);
                }
                if (!RoomItem_0.IsRoller)
                {
                    if (RoomItem_0.Rot != int_19 && RoomItem_0.GetX == int_17 && RoomItem_0.GetY == int_18)
                    {
                        num = RoomItem_0.Double_0;
                    }
                    foreach (RoomItem current2 in list3)
                    {
                        if (current2.Id != RoomItem_0.Id && current2.Double_1 > num)
                        {
                            num = current2.Double_1;
                        }
                    }
                }
                if (int_19 != 0 && int_19 != 2 && int_19 != 4 && int_19 != 6 && int_19 != 8)
                {
                    int_19 = 0;
                }
                Dictionary<int, AffectedTile> dictionary2 = new Dictionary<int, AffectedTile>();
                dictionary2 = this.GetAffectedTiles(RoomItem_0.GetBaseItem().Length, RoomItem_0.GetBaseItem().Width, RoomItem_0.GetX, RoomItem_0.GetY, RoomItem_0.Rot);
                int num3 = 0;
                int num4 = 0;
                if (!bool_13)
                {
                    num3 = RoomItem_0.GetX;
                    num4 = RoomItem_0.GetY;
                }
                RoomItem_0.Rot = int_19;
                RoomItem_0.method_0(int_17, int_18, num);
                if (!bool_14 && Session != null)
                {
                    RoomItem_0.Interactor.OnPlace(Session, RoomItem_0);
                }
                if (bool_13)
                {
                    if (this.mRemovedItems.Contains(RoomItem_0.Id))
                    {
                        this.mRemovedItems.Remove(RoomItem_0.Id);
                    }
                    if (this.mAddedItems.Contains(RoomItem_0.Id))
                    {
                        result = false;
                        return result;
                    }
                    this.mAddedItems.Add(RoomItem_0.Id, RoomItem_0);
                    if (RoomItem_0.IsFloorItem)
                    {
                        if (this.mFloorItems.Contains(RoomItem_0.Id))
                        {
                            this.mFloorItems.Remove(RoomItem_0.Id);
                        }
                        this.mFloorItems.Add(RoomItem_0.Id, RoomItem_0);
                    }
                    else
                    {
                        if (this.mWallItems.Contains(RoomItem_0.Id))
                        {
                            this.mWallItems.Remove(RoomItem_0.Id);
                        }
                        this.mWallItems.Add(RoomItem_0.Id, RoomItem_0);
                    }
                    ServerMessage Message5_ = new ServerMessage(93u);
                    RoomItem_0.method_6(Message5_);
                    this.SendMessage(Message5_, null);
                    string text = RoomItem_0.GetBaseItem().InteractionType.ToLower();
                    switch (text)
                    {
                        case "bb_patch":
                            this.bbTiles.Add(RoomItem_0);
                            if (RoomItem_0.ExtraData == "5")
                            {
                                this.bbrTiles.Add(RoomItem_0);
                            }
                            else
                            {
                                if (RoomItem_0.ExtraData == "8")
                                {
                                    this.bbgTiles.Add(RoomItem_0);
                                }
                                else
                                {
                                    if (RoomItem_0.ExtraData == "11")
                                    {
                                        this.bbbTiles.Add(RoomItem_0);
                                    }
                                    else
                                    {
                                        if (RoomItem_0.ExtraData == "14")
                                        {
                                            this.bbyTiles.Add(RoomItem_0);
                                        }
                                    }
                                }
                            }
                            break;
                        case "blue_score":
                            this.BlueScoreboards.Add(RoomItem_0);
                            break;
                        case "green_score":
                            this.GreenScoreboards.Add(RoomItem_0);
                            break;
                        case "red_score":
                            this.RedScoreboards.Add(RoomItem_0);
                            break;
                        case "yellow_score":
                            this.YellowScoreboards.Add(RoomItem_0);
                            break;
                        case "stickiepole":
                            this.StickiePoles.Add(RoomItem_0);
                            break;
                        case "wf_trg_onsay":
                        case "wf_trg_enterroom":
                        case "wf_trg_furnistate":
                        case "wf_trg_onfurni":
                        case "wf_trg_offfurni":
                        case "wf_trg_gameend":
                        case "wf_trg_gamestart":
                        case "wf_trg_attime":
                        case "wf_trg_atscore":
                            if (!this.WF_Triggers.Contains(RoomItem_0))
                            {
                                this.WF_Triggers.Add(RoomItem_0);
                            }
                            break;
                        case "wf_trg_timer":
                            if (RoomItem_0.Extra1.Length <= 0)
                            {
                                RoomItem_0.Extra1 = "10";
                            }
                            if (!this.WF_Triggers.Contains(RoomItem_0))
                            {
                                this.WF_Triggers.Add(RoomItem_0);
                            }
                            RoomItem_0.TimerRunning = true;
                            RoomItem_0.ReqUpdate(1);
                            break;
                        case "wf_act_saymsg":
                        case "wf_act_moveuser":
                        case "wf_act_togglefurni":
                        case "wf_act_givepoints":
                        case "wf_act_moverotate":
                        case "wf_act_matchfurni":
                        case "wf_act_give_phx":
                            if (!this.WF_Effects.Contains(RoomItem_0))
                            {
                                this.WF_Effects.Add(RoomItem_0);
                            }
                            break;
                        case "wf_cnd_trggrer_on_frn":
                        case "wf_cnd_furnis_hv_avtrs":
                        case "wf_cnd_has_furni_on":
                        case "wf_cnd_phx":
                            if (!this.WF_Conditions.Contains(RoomItem_0))
                            {
                                this.WF_Conditions.Add(RoomItem_0);
                            }
                            break;
                    }
                }
                else
                {
                    if (!this.mMovedItems.Contains(RoomItem_0.Id) && !this.mAddedItems.ContainsKey(RoomItem_0.Id))
                    {
                        this.mMovedItems.Add(RoomItem_0.Id, RoomItem_0);
                    }
                    if (RoomItem_0.GetBaseItem().InteractionType.ToLower() == "wf_act_give_phx" && Session != null)
                    {
                        string text2 = RoomItem_0.Extra1.Split(new char[]
						{
							':'
						})[0].ToLower();
                        if (!PhoenixEnvironment.GetGame().GetRoleManager().HasWiredEffectRole(text2, Session))
                        {
                            RoomItem_0.Extra1 = "";
                        }
                    }
                    if (RoomItem_0.GetBaseItem().InteractionType.ToLower() == "wf_cnd_phx" && Session != null)
                    {
                        string text2 = RoomItem_0.Extra1.Split(new char[]
						{
							':'
						})[0].ToLower();
                        if (!PhoenixEnvironment.GetGame().GetRoleManager().HasWiredConditionRole(text2, Session))
                        {
                            RoomItem_0.Extra1 = "";
                        }
                    }
                    ServerMessage Message5_ = new ServerMessage(95u);
                    RoomItem_0.method_6(Message5_);
                    this.SendMessage(Message5_, null);
                }
                this.GenerateMaps();
                if (!bool_14)
                {
                    this.UpdateUserStatus(this.GetUserForSquare(int_17, int_18), true, true);
                    foreach (AffectedTile current in dictionary.Values)
                    {
                        this.UpdateUserStatus(this.GetUserForSquare(current.X, current.Y), true, true);
                    }
                    if (num3 > 0 || num4 > 0)
                    {
                        this.UpdateUserStatus(this.GetUserForSquare(num3, num4), true, true);
                    }
                    foreach (AffectedTile current in dictionary2.Values)
                    {
                        this.UpdateUserStatus(this.GetUserForSquare(current.X, current.Y), true, true);
                    }
                }
                result = true;
            }
            return result;
        }
        internal void method_80(RoomItem RoomItem_0)
        {
            if (!this.mMovedItems.Contains(RoomItem_0.Id) && !this.mAddedItems.ContainsKey(RoomItem_0.Id))
            {
                this.mMovedItems.Add(RoomItem_0.Id, RoomItem_0);
            }
        }
        public bool method_81(RoomItem RoomItem_0, int int_17, int int_18, double double_3)
        {
            Dictionary<int, AffectedTile> dictionary = this.GetAffectedTiles(RoomItem_0.GetBaseItem().Length, RoomItem_0.GetBaseItem().Width, int_17, int_18, RoomItem_0.Rot);
            RoomItem_0.method_0(int_17, int_18, double_3);
            if (!this.mMovedItems.Contains(RoomItem_0.Id))
            {
                this.mMovedItems.Add(RoomItem_0.Id, RoomItem_0);
            }
            this.GenerateMaps();
            this.UpdateUserStatus(this.GetUserForSquare(int_17, int_18), true, true);
            foreach (AffectedTile current in dictionary.Values)
            {
                this.UpdateUserStatus(this.GetUserForSquare(current.X, current.Y), true, true);
            }
            return true;
        }
        public bool method_82(GameClient Session, RoomItem RoomItem_0, bool bool_13, string string_10)
        {
            if (bool_13)
            {
                RoomItem_0.Interactor.OnPlace(Session, RoomItem_0);
                string text = RoomItem_0.GetBaseItem().InteractionType.ToLower();
                if (text != null && text == "dimmer" && this.MoodlightData == null)
                {
                    this.MoodlightData = new MoodlightData(RoomItem_0.Id);
                    RoomItem_0.ExtraData = this.MoodlightData.method_7();
                }
                if (!this.mAddedItems.ContainsKey(RoomItem_0.Id))
                {
                    this.mAddedItems.Add(RoomItem_0.Id, RoomItem_0);
                    if (RoomItem_0.IsFloorItem)
                    {
                        this.mFloorItems.Add(RoomItem_0.Id, RoomItem_0);
                    }
                    else
                    {
                        if (!this.mWallItems.Contains(RoomItem_0.Id))
                        {
                            this.mWallItems.Add(RoomItem_0.Id, RoomItem_0);
                        }
                    }
                }
                ServerMessage Message5_ = new ServerMessage(83u);
                RoomItem_0.method_6(Message5_);
                this.SendMessage(Message5_, null);
            }
            else
            {
                if (!this.mMovedItems.Contains(RoomItem_0.Id))
                {
                    this.mMovedItems.Add(RoomItem_0.Id, RoomItem_0);
                }
            }
            if (!bool_13)
            {
                RoomItem_0.string_7 = string_10;
                ServerMessage Message5_ = new ServerMessage(85u);
                RoomItem_0.method_6(Message5_);
                this.SendMessage(Message5_, null);
            }
            return true;
        }
        public void method_83()
        {
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null)
                {
                    this.UpdateUserStatus(@class, true, true);
                }
            }
        }
        public double method_84(int int_17, int int_18, List<RoomItem> list_18)
        {
            double result;
            try
            {
                bool flag = false;
                if (this.double_2[int_17, int_18] != 0.0)
                {
                    flag = true;
                }
                double num = 0.0;
                bool flag2 = false;
                double num2 = 0.0;
                if (list_18 == null)
                {
                    list_18 = new List<RoomItem>();
                }
                if (list_18 != null)
                {
                    foreach (RoomItem current in list_18)
                    {
                        if ((current.GetBaseItem().IsSeat || current.GetBaseItem().InteractionType.ToLower() == "bed") && flag)
                        {
                            result = current.Double_0;
                            return result;
                        }
                        if (current.Double_1 > num)
                        {
                            if (current.GetBaseItem().IsSeat || current.GetBaseItem().InteractionType.ToLower() == "bed")
                            {
                                if (flag)
                                {
                                    result = current.Double_0;
                                    return result;
                                }
                                flag2 = true;
                                num2 = current.GetBaseItem().Height;
                            }
                            else
                            {
                                flag2 = false;
                            }
                            num = current.Double_1;
                        }
                    }
                }
                double num3 = this.Model.double_1[int_17, int_18];
                double num4 = num - this.Model.double_1[int_17, int_18];
                if (flag2)
                {
                    num4 -= num2;
                }
                if (num4 < 0.0)
                {
                    num4 = 0.0;
                }
                result = num3 + num4;
            }
            catch
            {
                result = 0.0;
            }
            return result;
        }
        public void method_85(RoomUser RoomUser_1)
        {
            List<RoomItem> list = this.method_93(RoomUser_1.X, RoomUser_1.Y);
            foreach (RoomItem current in list)
            {
                this.method_12(RoomUser_1, current);
                if (current.GetBaseItem().InteractionType.ToLower() == "pressure_pad")
                {
                    current.ExtraData = "0";
                    current.UpdateState(false, true);
                }
            }
            this.byte_0[RoomUser_1.X, RoomUser_1.Y] = 1;
        }
        public void method_86(RoomUser RoomUser_1)
        {
            List<RoomItem> list = this.method_93(RoomUser_1.X, RoomUser_1.Y);
            foreach (RoomItem current in list)
            {
                string text = current.GetBaseItem().InteractionType.ToLower();
                if (text != null)
                {
                    if (!(text == "pressure_pad"))
                    {
                        if (text == "fbgate" && (!string.IsNullOrEmpty(current.Extra1) || !string.IsNullOrEmpty(current.Extra2)))
                        {
                            RoomUser_1 = this.GetUserForSquare(current.Coordinate.X, current.Coordinate.Y);
                            if (RoomUser_1 != null && !RoomUser_1.IsBot && current.Extra1 != null && current.Extra2 != null)
                            {
                                string a = RoomUser_1.GetClient().GetHabbo().Gender;
                                if (a == "m")
                                {
                                    AntiMutant.ApplyClothing(RoomUser_1, current.Extra1);
                                }
                                else
                                {
                                    AntiMutant.ApplyClothing(RoomUser_1, current.Extra2);
                                }
                                ServerMessage Message = new ServerMessage(266u);
                                Message.AppendInt32(RoomUser_1.VirtualId);
                                Message.AppendStringWithBreak(RoomUser_1.GetClient().GetHabbo().Look);
                                Message.AppendStringWithBreak(RoomUser_1.GetClient().GetHabbo().Gender.ToLower());
                                Message.AppendStringWithBreak(RoomUser_1.GetClient().GetHabbo().Motto);
                                Message.AppendInt32(RoomUser_1.GetClient().GetHabbo().AchievementScore);
                                Message.AppendStringWithBreak("");
                                this.SendMessage(Message, null);
                            }
                        }
                    }
                    else
                    {
                        current.ExtraData = "1";
                        current.UpdateState(false, true);
                    }
                }
            }
        }
        public void UpdateUserStatus(RoomUser User, bool bool_13, bool bool_14)
        {
            int num = 0;
            try
            {
                if (User != null)
                {
                    num = 1;
                    if (User.IsPet)
                    {
                        User.PetData.X = User.X;
                        User.PetData.Y = User.Y;
                        User.PetData.Z = User.Z;
                    }
                    else
                    {
                        if (User.IsBot)
                        {
                            User.BotData.x = User.X;
                            User.BotData.y = User.Y;
                            User.BotData.z = User.Z;
                        }
                        else
                        {
                            if (User.class34_1 != null && User.Target != null)
                            {
                                return;
                            }
                        }
                    }
                    num = 2;
                    if (!User.Visible)
                    {
                        User.UpdateNeeded = false;
                    }
                    else
                    {
                        num = 3;
                        if (bool_13)
                        {
                            num = 4;
                            if (User.byte_1 > 0)
                            {
                                num = 5;
                                if (User.IsBot)
                                {
                                    if (this.byte_1[User.X, User.Y] == 0)
                                    {
                                        User.BotData.EffectId = -1;
                                        User.byte_1 = 0;
                                    }
                                }
                                else
                                {
                                    num = 6;
                                    if ((User.GetClient().GetHabbo().Gender.ToLower() == "m" && this.byte_1[User.X, User.Y] == 0) || (User.GetClient().GetHabbo().Gender.ToLower() == "f" && this.byte_2[User.X, User.Y] == 0))
                                    {
                                        User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, true);
                                        User.byte_1 = 0;
                                    }
                                }
                            }
                            num = 7;
                            if (User.Statusses.ContainsKey("lay") || User.Statusses.ContainsKey("sit"))
                            {
                                User.Statusses.Remove("lay");
                                User.Statusses.Remove("sit");
                                User.UpdateNeeded = true;
                            }
                            List<RoomItem> list = this.method_93(User.X, User.Y);
                            double num2 = this.method_84(User.X, User.Y, list);
                            if (num2 != User.Z)
                            {
                                User.Z = num2;
                                User.UpdateNeeded = true;
                            }
                            num = 8;
                            if (this.Model.squareState[User.X, User.Y] == SquareState.SEAT)
                            {
                                if (!User.Statusses.ContainsKey("sit"))
                                {
                                    User.Statusses.Add("sit", "1.0");
                                    if (User.byte_1 > 0)
                                    {
                                        if (!User.IsBot)
                                        {
                                            User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, true);
                                        }
                                        else
                                        {
                                            User.BotData.EffectId = -1;
                                        }
                                        User.byte_1 = 0;
                                    }
                                }
                                num = 9;
                                User.Z = this.Model.double_1[User.X, User.Y];
                                User.RotHead = this.Model.int_3[User.X, User.Y];
                                User.RotBody = this.Model.int_3[User.X, User.Y];
                                if (User.IsBot && User.BotData.RoomUser_0 != null)
                                {
                                    User.BotData.RoomUser_0.Z = User.Z + 1.0;
                                    User.BotData.RoomUser_0.RotHead = User.RotHead;
                                    User.BotData.RoomUser_0.RotBody = User.RotBody;
                                }
                                User.UpdateNeeded = true;
                            }
                            if (list.Count < 1 && this.list_4.Contains(User.HabboId))
                            {
                                User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, false);
                                this.list_4.Remove(User.HabboId);
                                User.int_14 = 0;
                                User.UpdateNeeded = true;
                            }
                            num = 10;
                            lock (list)
                            {
                                foreach (RoomItem Item in list)
                                {
                                    num = 11;
                                    if (Item.GetBaseItem().IsSeat && (!User.IsPet || User.BotData.RoomUser_0 == null))
                                    {
                                        if (!User.Statusses.ContainsKey("sit"))
                                        {
                                            double num3;
                                            try
                                            {
                                                if (Item.GetBaseItem().Height_Adjustable.Count > 1)
                                                {
                                                    num3 = Item.GetBaseItem().Height_Adjustable[(int)Convert.ToInt16(Item.ExtraData)];
                                                }
                                                else
                                                {
                                                    num3 = Item.GetBaseItem().Height;
                                                }
                                                goto IL_BCA;
                                            }
                                            catch
                                            {
                                                num3 = Item.GetBaseItem().Height;
                                                goto IL_BCA;
                                            }
                                        IL_51B:
                                            if (User.byte_1 > 0)
                                            {
                                                if (!User.IsBot)
                                                {
                                                    User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, true);
                                                }
                                                else
                                                {
                                                    User.BotData.EffectId = -1;
                                                }
                                                User.byte_1 = 0;
                                                goto IL_55D;
                                            }
                                            goto IL_55D;
                                        IL_BCA:
                                            if (User.Statusses.ContainsKey("sit"))
                                            {
                                                goto IL_51B;
                                            }
                                            User.Statusses.Add("sit", num3.ToString().Replace(',', '.'));
                                            goto IL_51B;
                                        }
                                    IL_55D:
                                        User.Z = Item.Double_0;
                                        User.RotHead = Item.Rot;
                                        User.RotBody = Item.Rot;
                                        if (User.IsBot && User.BotData.RoomUser_0 != null)
                                        {
                                            User.BotData.RoomUser_0.Z = User.Z + 1.0;
                                            User.BotData.RoomUser_0.RotHead = User.RotHead;
                                            User.BotData.RoomUser_0.RotBody = User.RotBody;
                                        }
                                        User.UpdateNeeded = true;
                                    }
                                    num = 12;
                                    if (Item.GetBaseItem().InteractionType.ToLower() == "bed")
                                    {
                                        if (!User.Statusses.ContainsKey("lay"))
                                        {
                                            double num3;
                                            try
                                            {
                                                if (Item.GetBaseItem().Height_Adjustable.Count > 1)
                                                {
                                                    num3 = Item.GetBaseItem().Height_Adjustable[(int)Convert.ToInt16(Item.ExtraData)];
                                                }
                                                else
                                                {
                                                    num3 = Item.GetBaseItem().Height;
                                                }
                                            }
                                            catch
                                            {
                                                //num3 = ;
                                            }
                                            if (!User.Statusses.ContainsKey("lay"))
                                            {
                                                User.Statusses.Add("lay", Item.GetBaseItem().Height.ToString().Replace(',', '.') + " null");
                                            }
                                            if (User.byte_1 > 0)
                                            {
                                                if (!User.IsBot)
                                                {
                                                    User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, true);
                                                }
                                                else
                                                {
                                                    User.BotData.EffectId = -1;
                                                }
                                                User.byte_1 = 0;
                                            }
                                        }
                                        User.Z = Item.Double_0;
                                        User.RotHead = Item.Rot;
                                        User.RotBody = Item.Rot;
                                        if (User.IsBot && User.BotData.RoomUser_0 != null)
                                        {
                                            User.BotData.RoomUser_0.Z = User.Z + 1.0;
                                            User.BotData.RoomUser_0.RotHead = User.RotHead;
                                            User.BotData.RoomUser_0.RotBody = User.RotBody;
                                        }
                                        User.UpdateNeeded = true;
                                    }
                                    num = 13;
                                    if (Item.GetBaseItem().InteractionType.ToLower().IndexOf("bb_") > -1 && !User.IsBot)
                                    {
                                        if (Item.GetBaseItem().InteractionType.ToLower().IndexOf("_gate") > -1)
                                        {
                                            int num4 = 0;
                                            int num5 = 0;
                                            if (Item.GetBaseItem().InteractionType.ToLower() == "bb_yellow_gate")
                                            {
                                                num5 = 12;
                                                num4 = 36;
                                            }
                                            else
                                            {
                                                if (Item.GetBaseItem().InteractionType.ToLower() == "bb_red_gate")
                                                {
                                                    num5 = 3;
                                                    num4 = 33;
                                                }
                                                else
                                                {
                                                    if (Item.GetBaseItem().InteractionType.ToLower() == "bb_green_gate")
                                                    {
                                                        num5 = 6;
                                                        num4 = 34;
                                                    }
                                                    else
                                                    {
                                                        if (Item.GetBaseItem().InteractionType.ToLower() == "bb_blue_gate")
                                                        {
                                                            num5 = 9;
                                                            num4 = 35;
                                                        }
                                                    }
                                                }
                                            }
                                            if (!this.list_4.Contains(User.HabboId))
                                            {
                                                User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(num4, true);
                                                User.UpdateNeeded = true;
                                                User.int_14 = num5;
                                                this.list_4.Add(User.HabboId);
                                            }
                                            else
                                            {
                                                User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, false);
                                                User.UpdateNeeded = true;
                                                User.int_14 = 0;
                                                this.list_4.Remove(User.HabboId);
                                            }
                                        }
                                        if (Item.GetBaseItem().InteractionType.ToLower() == "bb_teleport")
                                        {
                                            this.method_91(Item, User);
                                        }
                                        if (Item.GetBaseItem().InteractionType.ToLower() == "bb_patch" && User.int_14 > 0 && User.IsWalking && Item.ExtraData != "14" && Item.ExtraData != "5" && Item.ExtraData != "8" && Item.ExtraData != "11" && Item.ExtraData != "1")
                                        {
                                            if (Item.ExtraData == "0" || Item.ExtraData == "")
                                            {
                                                Item.ExtraData = Convert.ToString(User.int_14);
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(Item.ExtraData) > 0)
                                                {
                                                    if (User.int_14 == 12 && (Item.ExtraData == "12" || Item.ExtraData == "13"))
                                                    {
                                                        Item.ExtraData = Convert.ToString(Convert.ToInt32(Item.ExtraData) + 1);
                                                    }
                                                    else
                                                    {
                                                        if (User.int_14 == 3 && (Item.ExtraData == "3" || Item.ExtraData == "4"))
                                                        {
                                                            Item.ExtraData = Convert.ToString(Convert.ToInt32(Item.ExtraData) + 1);
                                                        }
                                                        else
                                                        {
                                                            if (User.int_14 == 6 && (Item.ExtraData == "6" || Item.ExtraData == "7"))
                                                            {
                                                                Item.ExtraData = Convert.ToString(Convert.ToInt32(Item.ExtraData) + 1);
                                                            }
                                                            else
                                                            {
                                                                if (User.int_14 == 9 && (Item.ExtraData == "9" || Item.ExtraData == "10"))
                                                                {
                                                                    Item.ExtraData = Convert.ToString(Convert.ToInt32(Item.ExtraData) + 1);
                                                                }
                                                                else
                                                                {
                                                                    Item.ExtraData = Convert.ToString(User.int_14);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            this.method_89(User.int_14 + 2, Item, false);
                                            Item.UpdateState(true, true);
                                        }
                                    }
                                }
                                goto IL_1155;
                            }
                        }
                        num = 14;
                        List<RoomItem> list2 = this.method_93(User.SetX, User.SetY);
                        lock (list2)
                        {
                            foreach (RoomItem current in list2)
                            {
                                if (this.double_0[current.GetX, current.GetY] <= current.Double_0)
                                {
                                    if (bool_14)
                                    {
                                        this.method_11(User, current);
                                    }
                                    if (current.GetBaseItem().InteractionType.ToLower() == "pressure_pad")
                                    {
                                        current.ExtraData = "1";
                                        current.UpdateState(false, true);
                                    }
                                    num = 15;
                                    if (current.GetBaseItem().InteractionType.ToLower() == "fbgate" && (!string.IsNullOrEmpty(current.Extra1) || !string.IsNullOrEmpty(current.Extra2)) && User != null && !User.IsBot)
                                    {
                                        if (User.ChangedClothes != "")
                                        {
                                            User.GetClient().GetHabbo().Look = User.ChangedClothes;
                                            User.ChangedClothes = "";
                                            ServerMessage Message = new ServerMessage(266u);
                                            Message.AppendInt32(User.VirtualId);
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Look);
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Gender.ToLower());
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Motto);
                                            Message.AppendInt32(User.GetClient().GetHabbo().AchievementScore);
                                            Message.AppendStringWithBreak("");
                                            this.SendMessage(Message, null);
                                        }
                                        else
                                        {
                                            string a = User.GetClient().GetHabbo().Gender;
                                            User.ChangedClothes = User.GetClient().GetHabbo().Look;
                                            if (a == "m")
                                            {
                                                AntiMutant.ApplyClothing(User, current.Extra1);
                                            }
                                            else
                                            {
                                                AntiMutant.ApplyClothing(User, current.Extra2);
                                            }
                                            ServerMessage Message = new ServerMessage(266u);
                                            Message.AppendInt32(User.VirtualId);
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Look);
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Gender.ToLower());
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Motto);
                                            Message.AppendInt32(User.GetClient().GetHabbo().AchievementScore);
                                            Message.AppendStringWithBreak("");
                                            this.SendMessage(Message, null);
                                        }
                                    }
                                    num = 16;
                                    if (current.GetBaseItem().InteractionType.ToLower() == "ball")
                                    {
                                        int num6 = current.GetX;
                                        int num7 = current.GetY;
                                        current.ExtraData = "11";
                                        if (User.RotBody == 4)
                                        {
                                            num7++;
                                            if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                            {
                                                this.method_79(null, current, num6, num7 - 2, 0, false, true, true);
                                            }
                                        }
                                        else
                                        {
                                            if (User.RotBody == 0)
                                            {
                                                num7--;
                                                if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                {
                                                    this.method_79(null, current, num6, num7 + 2, 0, false, true, true);
                                                }
                                            }
                                            else
                                            {
                                                if (User.RotBody == 6)
                                                {
                                                    num6--;
                                                    if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                    {
                                                        this.method_79(null, current, num6 + 2, num7, 0, false, true, true);
                                                    }
                                                }
                                                else
                                                {
                                                    if (User.RotBody == 2)
                                                    {
                                                        num6++;
                                                        if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                        {
                                                            this.method_79(null, current, num6 - 2, num7, 0, false, true, true);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (User.RotBody == 3)
                                                        {
                                                            num6++;
                                                            num7++;
                                                            if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                            {
                                                                this.method_79(null, current, num6 - 2, num7 - 2, 0, false, true, true);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (User.RotBody == 1)
                                                            {
                                                                num6++;
                                                                num7--;
                                                                if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                                {
                                                                    this.method_79(null, current, num6 - 2, num7 + 2, 0, false, true, true);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (User.RotBody == 7)
                                                                {
                                                                    num6--;
                                                                    num7--;
                                                                    if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                                    {
                                                                        this.method_79(null, current, num6 + 2, num7 + 2, 0, false, true, true);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (User.RotBody == 5)
                                                                    {
                                                                        num6--;
                                                                        num7++;
                                                                        if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                                        {
                                                                            this.method_79(null, current, num6 + 2, num7 - 2, 0, false, true, true);
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
                                }
                            }
                        }
                    IL_1155: ;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogThreadException(ex.ToString(), string.Concat(new object[]
				{
					"Room [ID: ",
					this.RoomId,
					"] [Part: ",
					num,
					"] Update User Status"
				}));
                this.CrashRoom();
            }
        }
        public void method_88(int int_17, int int_18, RoomItem RoomItem_0)
        {
            if (int_17 == 5)
            {
                this.int_9 += int_18 - 1;
            }
            else
            {
                if (int_17 == 8)
                {
                    this.int_12 += int_18 - 1;
                }
                else
                {
                    if (int_17 == 11)
                    {
                        this.int_11 += int_18 - 1;
                    }
                    else
                    {
                        if (int_17 == 14)
                        {
                            this.int_10 += int_18 - 1;
                        }
                    }
                }
            }
            this.method_89(int_17, RoomItem_0, false);
        }
        public void method_89(int int_17, RoomItem RoomItem_0, bool bool_13)
        {
            if (int_17 == 5)
            {
                this.int_9++;
                if (RoomItem_0.ExtraData == "5")
                {
                    this.bbrTiles.Add(RoomItem_0);
                }
                if (this.RedScoreboards.Count > 0)
                {
                    foreach (RoomItem current in this.RedScoreboards)
                    {
                        current.ExtraData = Convert.ToString(this.int_9);
                        current.UpdateState(true, true);
                    }
                }
                this.method_17(this.int_9);
            }
            else
            {
                if (int_17 == 8)
                {
                    this.int_12++;
                    if (RoomItem_0.ExtraData == "8")
                    {
                        this.bbgTiles.Add(RoomItem_0);
                    }
                    if (this.GreenScoreboards.Count > 0)
                    {
                        foreach (RoomItem current in this.GreenScoreboards)
                        {
                            current.ExtraData = Convert.ToString(this.int_12);
                            current.UpdateState(true, true);
                        }
                    }
                    this.method_17(this.int_12);
                }
                else
                {
                    if (int_17 == 11)
                    {
                        this.int_11++;
                        if (RoomItem_0.ExtraData == "11")
                        {
                            this.bbbTiles.Add(RoomItem_0);
                        }
                        if (this.BlueScoreboards.Count > 0)
                        {
                            foreach (RoomItem current in this.BlueScoreboards)
                            {
                                current.ExtraData = Convert.ToString(this.int_11);
                                current.UpdateState(true, true);
                            }
                        }
                        this.method_17(this.int_11);
                    }
                    else
                    {
                        if (int_17 == 14)
                        {
                            this.int_10++;
                            if (RoomItem_0.ExtraData == "14")
                            {
                                this.bbyTiles.Add(RoomItem_0);
                            }
                            if (this.YellowScoreboards.Count > 0)
                            {
                                foreach (RoomItem current in this.YellowScoreboards)
                                {
                                    current.ExtraData = Convert.ToString(this.int_10);
                                    current.UpdateState(true, true);
                                }
                            }
                            this.method_17(this.int_10);
                        }
                    }
                }
            }
            if (bool_13 || (this.bbTiles.Count > 0 && this.bbrTiles.Count + this.bbgTiles.Count + this.bbbTiles.Count + this.bbyTiles.Count >= this.bbTiles.Count))
            {
                bool_13 = true;
                if (this.int_10 > this.int_9 && this.int_10 > this.int_11 && this.int_10 > this.int_12)
                {
                    new Room.Delegate2(this.method_90).BeginInvoke(14, null, null);
                }
                else
                {
                    if (this.int_9 > this.int_10 && this.int_9 > this.int_11 && this.int_9 > this.int_12)
                    {
                        new Room.Delegate2(this.method_90).BeginInvoke(5, null, null);
                    }
                    else
                    {
                        if (this.int_11 > this.int_9 && this.int_11 > this.int_10 && this.int_11 > this.int_12)
                        {
                            new Room.Delegate2(this.method_90).BeginInvoke(11, null, null);
                        }
                        else
                        {
                            if (this.int_12 > this.int_9 && this.int_12 > this.int_11 && this.int_12 > this.int_10)
                            {
                                new Room.Delegate2(this.method_90).BeginInvoke(8, null, null);
                            }
                        }
                    }
                }
            }
            if (bool_13)
            {
                this.method_13();
            }
        }
        public void method_90(int int_17)
        {
            List<RoomItem> list = new List<RoomItem>();
            if (int_17 == 5)
            {
                list = this.bbrTiles;
            }
            else
            {
                if (int_17 == 8)
                {
                    list = this.bbgTiles;
                }
                else
                {
                    if (int_17 == 11)
                    {
                        list = this.bbbTiles;
                    }
                    else
                    {
                        if (int_17 == 14)
                        {
                            list = this.bbyTiles;
                        }
                    }
                }
            }
            try
            {
                for (int i = 4; i > 0; i--)
                {
                    Thread.Sleep(500);
                    foreach (RoomItem current in list)
                    {
                        current.ExtraData = "1";
                        current.UpdateState(false, true);
                    }
                    Thread.Sleep(500);
                    foreach (RoomItem current in list)
                    {
                        current.ExtraData = Convert.ToString(int_17);
                        current.UpdateState(false, true);
                    }
                }
                foreach (RoomItem current in this.bbTiles)
                {
                    current.ExtraData = "0";
                    current.UpdateState(true, true);
                }
            }
            catch
            {
            }
            this.bbbTiles.Clear();
            this.bbgTiles.Clear();
            this.bbrTiles.Clear();
            this.bbyTiles.Clear();
            this.int_10 = 0;
            this.int_11 = 0;
            this.int_9 = 0;
            this.int_12 = 0;
            foreach (RoomItem current in this.RedScoreboards)
            {
                current.ExtraData = "0";
                current.UpdateState(true, true);
            }
            foreach (RoomItem current in this.GreenScoreboards)
            {
                current.ExtraData = "0";
                current.UpdateState(true, true);
            }
            foreach (RoomItem current in this.BlueScoreboards)
            {
                current.ExtraData = "0";
                current.UpdateState(true, true);
            }
            foreach (RoomItem current in this.YellowScoreboards)
            {
                current.ExtraData = "0";
                current.UpdateState(true, true);
            }
        }
        public void method_91(RoomItem RoomItem_0, RoomUser RoomUser_1)
        {
            RoomItem_0.ExtraData = "1";
            RoomItem_0.UpdateState(false, true);
            RoomItem_0.ReqUpdate(1);
            List<RoomItem> list = new List<RoomItem>();
            RoomUser_1.ClearMovement(true);
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                if (@class != RoomItem_0 && !(@class.GetBaseItem().InteractionType.ToLower() != "bb_teleport"))
                {
                    list.Add(@class);
                }
            }
            if (list.Count > 0)
            {
                Random random = new Random((int)PhoenixEnvironment.GetUnixTimestamp() * (int)RoomUser_1.HabboId);
                int index = random.Next(0, list.Count);
                list[index].ExtraData = "1";
                list[index].UpdateState(false, true);
                list[index].ReqUpdate(1);
                this.byte_0[RoomUser_1.X, RoomUser_1.Y] = 1;
                this.byte_0[list[index].GetX, list[index].GetY] = 1;
                RoomUser_1.SetPos(list[index].GetX, list[index].GetY, list[index].Double_0);
                RoomUser_1.UpdateNeeded = true;
            }
        }
        public bool ValidTile(int int_17, int int_18)
        {
            return int_17 >= 0 && int_18 >= 0 && int_17 < this.Model.MapSizeX && int_18 < this.Model.MapSizeY;
        }
        public List<RoomItem> method_93(int int_17, int int_18)
        {
            List<RoomItem> list = new List<RoomItem>();
            List<RoomItem> result;
            if (this.Hashtable_0 != null)
            {
                foreach (RoomItem @class in this.Hashtable_0.Values)
                {
                    if (@class.GetX == int_17 && @class.GetY == int_18)
                    {
                        list.Add(@class);
                    }
                    Dictionary<int, AffectedTile> dictionary = this.GetAffectedTiles(@class.GetBaseItem().Length, @class.GetBaseItem().Width, @class.GetX, @class.GetY, @class.Rot);
                    foreach (AffectedTile current in dictionary.Values)
                    {
                        if (current.X == int_17 && current.Y == int_18)
                        {
                            list.Add(@class);
                        }
                    }
                }
                result = list;
            }
            else
            {
                result = null;
            }
            return result;
        }
        public Dictionary<int, AffectedTile> GetAffectedTiles(int int_17, int int_18, int int_19, int int_20, int int_21)
        {
            int num = 0;
            Dictionary<int, AffectedTile> dictionary = new Dictionary<int, AffectedTile>();
            if (int_17 > 1)
            {
                if (int_21 == 0 || int_21 == 4)
                {
                    for (int i = 1; i < int_17; i++)
                    {
                        dictionary.Add(num++, new AffectedTile(int_19, int_20 + i, i));
                        for (int j = 1; j < int_18; j++)
                        {
                            dictionary.Add(num++, new AffectedTile(int_19 + j, int_20 + i, (i < j) ? j : i));
                        }
                    }
                }
                else
                {
                    if (int_21 == 2 || int_21 == 6)
                    {
                        for (int i = 1; i < int_17; i++)
                        {
                            dictionary.Add(num++, new AffectedTile(int_19 + i, int_20, i));
                            for (int j = 1; j < int_18; j++)
                            {
                                dictionary.Add(num++, new AffectedTile(int_19 + i, int_20 + j, (i < j) ? j : i));
                            }
                        }
                    }
                }
            }
            if (int_18 > 1)
            {
                if (int_21 == 0 || int_21 == 4)
                {
                    for (int i = 1; i < int_18; i++)
                    {
                        dictionary.Add(num++, new AffectedTile(int_19 + i, int_20, i));
                        for (int j = 1; j < int_17; j++)
                        {
                            dictionary.Add(num++, new AffectedTile(int_19 + i, int_20 + j, (i < j) ? j : i));
                        }
                    }
                }
                else
                {
                    if (int_21 == 2 || int_21 == 6)
                    {
                        for (int i = 1; i < int_18; i++)
                        {
                            dictionary.Add(num++, new AffectedTile(int_19, int_20 + i, i));
                            for (int j = 1; j < int_17; j++)
                            {
                                dictionary.Add(num++, new AffectedTile(int_19 + j, int_20 + i, (i < j) ? j : i));
                            }
                        }
                    }
                }
            }
            return dictionary;
        }
        public bool method_95(int int_17, int int_18, bool bool_13)
        {
            return !this.AllowWalkthrough && this.SquareHasUsers(int_17, int_18);
        }
        public bool SquareHasUsers(int int_17, int int_18)
        {
            return this.GetUserForSquare(int_17, int_18) != null;
        }
        public bool method_97(int int_17, int int_18)
        {
            return this.method_44(int_17, int_18) != null;
        }
        public string method_98(string string_10)
        {
            string result;
            try
            {
                if (string_10.Contains(Convert.ToChar(13)))
                {
                    result = null;
                }
                else
                {
                    if (string_10.Contains(Convert.ToChar(9)))
                    {
                        result = null;
                    }
                    else
                    {
                        string[] array = string_10.Split(new char[]
						{
							' '
						});
                        if (array[2] != "l" && array[2] != "r")
                        {
                            result = null;
                        }
                        else
                        {
                            string[] array2 = array[0].Substring(3).Split(new char[]
							{
								','
							});
                            int num = int.Parse(array2[0]);
                            int num2 = int.Parse(array2[1]);
                            if (num < 0 || num2 < 0 || num > 200 || num2 > 200)
                            {
                                result = null;
                            }
                            else
                            {
                                string[] array3 = array[1].Substring(2).Split(new char[]
								{
									','
								});
                                int num3 = int.Parse(array3[0]);
                                int num4 = int.Parse(array3[1]);
                                if (num3 < 0 || num4 < 0 || num3 > 200 || num4 > 200)
                                {
                                    result = null;
                                }
                                else
                                {
                                    result = string.Concat(new object[]
									{
										":w=",
										num,
										",",
										num2,
										" l=",
										num3,
										",",
										num4,
										" ",
										array[2]
									});
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }
        public bool method_99(int int_17, int int_18, int int_19, int int_20)
        {
            return (Math.Abs(int_17 - int_19) <= 1 && Math.Abs(int_18 - int_20) <= 1) || (int_17 == int_19 && int_18 == int_20);
        }
        public int method_100(int int_17, int int_18, int int_19, int int_20)
        {
            return Math.Abs(int_17 - int_19) + Math.Abs(int_18 - int_20);
        }
        internal void method_101()
        {
            for (int i = 0; i < this.UserList.Length; i++)
            {
                RoomUser @class = this.UserList[i];
                if (@class != null)
                {
                    @class.GoalX = @class.X;
                    @class.GoalY = @class.Y;
                    @class.ResetStatus();
                    @class.ClearMovement(false);
                }
            }
        }
        internal void method_102(int int_17)
        {
            this.int_15 = int_17;
        }
    }
}