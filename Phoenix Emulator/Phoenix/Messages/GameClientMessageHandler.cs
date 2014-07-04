using System;
using System.Data;
using System.Text.RegularExpressions;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Advertisements;
using Phoenix.HabboHotel.Items;
using Phoenix.Storage;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Messages
{
	internal sealed class GameClientMessageHandler
	{
		private delegate void Delegate();
		private const int HIGHEST_MESSAGE_ID = 4004;
		private GameClient Session;
		private ClientMessage Request;
		private ServerMessage Response;
		private RequestHandler[] RequestHandlers;
		public GameClientMessageHandler(GameClient Session)
		{
			this.Session = Session;
            this.RequestHandlers = new RequestHandler[HIGHEST_MESSAGE_ID];
			this.Response = new ServerMessage(0);
		}
		public ServerMessage GetResponse()
		{
			return this.Response;
		}
		public void Destroy()
		{
			this.Session = null;
			this.RequestHandlers = null;
			this.Request = null;
			this.Response = null;
		}
		public void HandleRequest(ClientMessage Request)
		{
            if (Request.Id > HIGHEST_MESSAGE_ID)
			{
				Logging.WriteLine("Warning - out of protocol request: " + Request.Header);
			}
			else
			{
				if (this.RequestHandlers[(int)((UIntPtr)Request.Id)] != null && Request != null)
				{
					this.Request = Request;
					this.RequestHandlers[(int)((UIntPtr)Request.Id)]();
					this.Request = null;
				}
			}
		}
		public void SendResponse()
		{
			if (this.Response != null && this.Response.Id > 0u && this.Session.GetConnection() != null)
			{
				this.Session.GetConnection().SendMessage(this.Response);
			}
		}
		public void GetAdvertisement()
		{
			RoomAdvertisement randomRoomAdvertisement = PhoenixEnvironment.GetGame().GetAdvertisementManager().GetRandomRoomAdvertisement();
			this.Response.Init(258);
			if (randomRoomAdvertisement == null)
			{
				this.Response.AppendStringWithBreak("");
				this.Response.AppendStringWithBreak("");
			}
			else
			{
				this.Response.AppendStringWithBreak(randomRoomAdvertisement.AdImage);
				this.Response.AppendStringWithBreak(randomRoomAdvertisement.AdLink);
				randomRoomAdvertisement.OnView();
			}
			this.SendResponse();
		}

		public void PrepareRoomForUser(uint Id, string Password)
		{
			this.ClearRoomLoading();
			if (PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(Id) != null)
			{
				if (this.Session.GetHabbo().InRoom)
				{
					Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
					if (room != null)
					{
						room.RemoveUserFromRoom(this.Session, false, false);
					}
				}
				Room room2 = PhoenixEnvironment.GetGame().GetRoomManager().LoadRoom(Id);
                if (room2 != null)
                {
                    this.Session.GetHabbo().LoadingRoom = Id;
                    if (room2.UserIsBanned(this.Session.GetHabbo().Id))
                    {
                        if (!room2.HasBanExpired(this.Session.GetHabbo().Id))
                        {
                            ServerMessage Message = new ServerMessage(224);
                            Message.AppendInt32(4);
                            this.Session.SendMessage(Message);
                            ServerMessage Message2 = new ServerMessage(18);
                            this.Session.SendMessage(Message2);
                            return;
                        }
                        room2.RemoveBan(this.Session.GetHabbo().Id);
                    }
                    if (room2.UsersNow >= room2.UsersMax && !PhoenixEnvironment.GetGame().GetRoleManager().RankHasRight(this.Session.GetHabbo().Rank, "acc_enter_fullrooms") && !this.Session.GetHabbo().Vip)
                    {
                        ServerMessage Message = new ServerMessage(224);
                        Message.AppendInt32(1);
                        this.Session.SendMessage(Message);
                        ServerMessage Message2 = new ServerMessage(18u);
                        this.Session.SendMessage(Message2);
                    }
                    else
                    {
                        if (room2.Type == "public")
                        {
                            if ((room2.State > 0) && !this.Session.GetHabbo().HasRole("acc_restrictedrooms"))
                            {
                                this.Session.SendNotif("This public room is accessible to Phoenix staff only.");
                                ServerMessage Message2 = new ServerMessage(18);
                                this.Session.SendMessage(Message2);
                                return;
                            }
                            ServerMessage Message3 = new ServerMessage(166);
                            Message3.AppendStringWithBreak("/client/public/" + room2.ModelName + "/0");
                            this.Session.SendMessage(Message3);
                        }
                        else if (room2.Type == "private")
                        {
                            ServerMessage Logging = new ServerMessage(19);
                            this.Session.SendMessage(Logging);
                            if (this.Session.GetHabbo().IsTeleporting && (room2.GetItem(this.Session.GetHabbo().TeleporterId) == null))
                            {
                                this.Session.GetHabbo().IsTeleporting = false;
                                this.Session.GetHabbo().TeleporterId = 0;
                                ServerMessage Message5 = new ServerMessage(131);
                                this.Session.SendMessage(Message5);
                                return;
                            }
                            if (!this.Session.GetHabbo().HasRole("acc_enter_anyroom") && !room2.CheckRights(this.Session, true) && !this.Session.GetHabbo().IsTeleporting)
                            {
                                if (room2.State == 1)
                                {
                                    if (room2.UserCount == 0)
                                    {
                                        ServerMessage Message5 = new ServerMessage(131);
                                        this.Session.SendMessage(Message5);
                                        return;
                                    }
                                    ServerMessage Message6 = new ServerMessage(91);
                                    Message6.AppendStringWithBreak("");
                                    this.Session.SendMessage(Message6);
                                    this.Session.GetHabbo().Waitingfordoorbell = true;
                                    ServerMessage Message7 = new ServerMessage(91);
                                    Message7.AppendStringWithBreak(this.Session.GetHabbo().Username);
                                    room2.method_61(Message7);
                                    return;
                                }
                                if (room2.State == 2 && Password.ToLower() != room2.Password.ToLower())
                                {
                                    ServerMessage Message8 = new ServerMessage(33);
                                    Message8.AppendInt32(-100002);
                                    this.Session.SendMessage(Message8);
                                    ServerMessage Message2 = new ServerMessage(18);
                                    this.Session.SendMessage(Message2);
                                    return;
                                }
                            }
                            ServerMessage Message3 = new ServerMessage(166);
                            Message3.AppendStringWithBreak("/client/private/" + room2.RoomId + "/Id");
                            this.Session.SendMessage(Message3);
                        }
                        this.Session.GetHabbo().LoadingChecksPassed = true;
                        this.LoadRoomForUser();
                    }
                }
		    }    
        }

		public void LoadRoomForUser()
		{
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().LoadingRoom);
			if (room != null && this.Session.GetHabbo().LoadingChecksPassed)
			{
				ServerMessage Message = new ServerMessage(69);
				Message.AppendStringWithBreak(room.ModelName);
				Message.AppendUInt(room.RoomId);
				this.Session.SendMessage(Message);
				if (this.Session.GetHabbo().SpectatorMode)
				{
					ServerMessage Message2 = new ServerMessage(254);
					this.Session.SendMessage(Message2);
				}
				if (room.Type == "private")
				{
					if (room.Wallpaper != "0.0")
					{
						ServerMessage Message3 = new ServerMessage(46);
						Message3.AppendStringWithBreak("wallpaper");
						Message3.AppendStringWithBreak(room.Wallpaper);
						this.Session.SendMessage(Message3);
					}
					if (room.Floor != "0.0")
					{
						ServerMessage Logging = new ServerMessage(46);
						Logging.AppendStringWithBreak("floor");
						Logging.AppendStringWithBreak(room.Floor);
						this.Session.SendMessage(Logging);
					}
					ServerMessage Message5 = new ServerMessage(46);
					Message5.AppendStringWithBreak("landscape");
					Message5.AppendStringWithBreak(room.Landscape);
					this.Session.SendMessage(Message5);
					if (room.CheckRights(this.Session, true))
					{
						ServerMessage Message6 = new ServerMessage(42);
						this.Session.SendMessage(Message6);
						ServerMessage Message7 = new ServerMessage(47);
						this.Session.SendMessage(Message7);
					}
					else
					{
						if (room.CheckRights(this.Session))
						{
							ServerMessage Message6 = new ServerMessage(42);
							this.Session.SendMessage(Message6);
						}
					}
					ServerMessage Message8 = new ServerMessage(345);
					if (this.Session.GetHabbo().RatedRooms.Contains(room.RoomId) || room.CheckRights(this.Session, true))
					{
						Message8.AppendInt32(room.Score);
					}
					else
					{
						Message8.AppendInt32(-1);
					}
					this.Session.SendMessage(Message8);
					if (room.HasOngoingEvent)
					{
						this.Session.SendMessage(room.Event.Serialize(this.Session));
					}
					else
					{
						ServerMessage Message9 = new ServerMessage(370);
						Message9.AppendStringWithBreak("-1");
						this.Session.SendMessage(Message9);
					}
				}
				this.GetAdvertisement();
			}
		}
		public void ClearRoomLoading()
		{
			this.Session.GetHabbo().LoadingRoom = 0u;
			this.Session.GetHabbo().LoadingChecksPassed = false;
			this.Session.GetHabbo().Waitingfordoorbell = false;
		}
		public bool NameFree(string string_0)
		{
			if (!Regex.IsMatch(string_0, "^[-a-zA-Z0-9._:,]+$"))
			{
				return false;
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					dataRow = @class.ReadDataRow("SELECT * FROM users WHERE username = '" + string_0 + "'");
				}
				return (dataRow == null);
			}
		}
        private delegate void RequestHandler();
	}
}
