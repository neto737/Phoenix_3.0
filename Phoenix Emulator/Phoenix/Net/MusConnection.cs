using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Text;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Users;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
using Phoenix.HabboHotel.Guilds;
using Phoenix.Util;
namespace Phoenix.Net
{
	internal sealed class MusConnection
	{
		private byte[] buffer = new byte[1024];
        private Socket socket;

		public MusConnection(Socket _socket)
		{
			this.socket = _socket;
			try
			{
				this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnEvent_RecieveData), _socket);
			}
			catch
			{
				this.tryClose();
			}
		}

		public void OnEvent_RecieveData(IAsyncResult iAr)
		{
			try
			{
				int count = 0;
				try
				{
					count = this.socket.EndReceive(iAr);
				}
				catch
				{
					this.tryClose();
					return;
				}
				string data = Encoding.Default.GetString(this.buffer, 0, count);
				if (data.Length > 0)
				{
					this.processCommand(data);
				}
			}
			catch
			{
			}
			this.tryClose();
		}

		public void processCommand(string data)
		{
			string str = data.Split(new char[] { Convert.ToChar(1)	})[0];
			string s = data.Split(new char[] { Convert.ToChar(1) })[1];

            GameClient clientByUserID = null;
            DataRow Row = null;
            Habbo habbo;
            Room room;
            GameClient clientByUsername;
            string text3 = str.ToLower();
            uint uint_2;
            uint num2;
            uint num3;
            string text5;

            switch (str.ToLower())
            {
                case "update_items":
                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        PhoenixEnvironment.GetGame().GetItemManager().LoadItems(adapter);
                    }
                    break;

                case "updateusersrooms":
                    habbo = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Convert.ToUInt32(s)).GetHabbo();
                    if (habbo != null)
                    {
                        using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                        {
                            habbo.UpdateRooms(adapter);
                        }
                    }
                    break;

                case "senduser":
                    num2 = uint.Parse(s.Split(new char[] { ' ' })[0]);
					num3 = uint.Parse(s.Split(new char[] { ' ' })[1]);

					clientByUsername = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(num2);
					room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(num3);
					if (clientByUsername != null)
					{
						ServerMessage message1 = new ServerMessage(286);
						message1.AppendBoolean(room.IsPublic);
						message1.AppendUInt(num3);
						clientByUsername.SendMessage(message1);
					}
                    break;

                case "updatevip":
                    habbo = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Convert.ToUInt32(s)).GetHabbo();
                    if (habbo != null)
                    {
                        habbo.UpdateVIP();
                    }
                    break;

                case "giftitem":
                case "giveitem":	
                    num2 = uint.Parse(s.Split(new char[] { ' ' })[0]);
					uint uint_ = uint.Parse(s.Split(new char[] { ' ' })[1]);
					int int_ = int.Parse(s.Split(new char[] { ' ' })[2]);
					string string_ = s.Substring(num2.ToString().Length + uint_.ToString().Length + int_.ToString().Length + 3);

					PhoenixEnvironment.GetGame().GetCatalog().GiveGift(string_, num2, uint_, int_);
                    break;

                case "unloadroom":
                    uint_2 = uint.Parse(s);
					room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(uint_2);
					PhoenixEnvironment.GetGame().GetRoomManager().UnloadRoom(room);
                    break;

                case "roomalert":
                    num3 = uint.Parse(s.Split(new char[] { ' ' })[0]);

                    room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(num3);
                    if (room != null)
                    {
                        string msg = s.Substring(num3.ToString().Length + 1);
                        for (int i = 0; i < room.UserList.Length; i++)
                        {
                            RoomUser user = room.UserList[i];
                            if (user != null)
                            {
                                user.GetClient().SendNotif(msg);
                            }
                        }
                    }
                    break;

                case "updategroup":
                    int int_2 = int.Parse(s.Split(new char[] { ' ' })[0]);

                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        GuildManager.UpdateGroup(adapter, int_2);
                    }
                    break;

                case "updateusersgroups":
					uint_2 = uint.Parse(s);
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_2).GetHabbo().UpdateGroups(adapter);
					}
                    break;

                case "shutdown":
					PhoenixEnvironment.BeginShutDown();
                    break;

                case "update_filter":
                case "refresh_filter":
                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        ChatCommandHandler.UpdateFilters(adapter);
                    }
                    break;

                case "updatecredits":
					clientByUserID = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint.Parse(s));
					if (clientByUserID != null)
					{
						int int_3 = 0;
						using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
						{
							int_3 = (int)adapter.ReadDataRow("SELECT credits FROM users WHERE UserId = '" + clientByUserID.GetHabbo().Id + "' LIMIT 1")[0];
						}
						clientByUserID.GetHabbo().Credits = int_3;
						clientByUserID.GetHabbo().UpdateCreditsBalance(false);
					}
                    break;

                case "updatesettings":
                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        PhoenixEnvironment.GetGame().LoadSettings(adapter);
                    }
                    break;

                case "updatepixels":
					clientByUserID = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint.Parse(s));
					if (clientByUserID != null)
					{
						int int_4 = 0;
						using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
						{
                            int_4 = (int)adapter.ReadDataRow("SELECT activity_points FROM users WHERE UserId = '" + clientByUserID.GetHabbo().Id + "' LIMIT 1")[0];
						}
						clientByUserID.GetHabbo().ActivityPoints = int_4;
						clientByUserID.GetHabbo().UpdateActivityPointsBalance(false);
					}
                    break;

                case "updatepoints":					
                    clientByUserID = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint.Parse(s));
					if (clientByUserID != null)
					{
						clientByUserID.GetHabbo().UpdateShellsBalance(true, false);
					}
                    break;

                case "reloadbans":					
                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						PhoenixEnvironment.GetGame().GetBanManager().LoadBans(adapter);
					}
					PhoenixEnvironment.GetGame().GetClientManager().CheckForAllBanConflicts();                     
                    break;

                case "update_bots":
                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        PhoenixEnvironment.GetGame().GetBotManager().LoadBots(adapter);
                    }
                    break;

                case "signout":
                    PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint.Parse(s)).Disconnect();
                    break;

                case "exe":
                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        adapter.ExecuteQuery(s);
                    }
                    break;

                case "alert":
                    string text6 = s.Split(new char[] { ' ' })[0];
					text5 = s.Substring(text6.Length + 1);
					ServerMessage Message8 = new ServerMessage(808);
					Message8.AppendStringWithBreak(TextManager.GetText("mus_alert_title"));
					Message8.AppendStringWithBreak(text5);
					PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint.Parse(text6)).SendMessage(Message8);
                    break;

                case "sa":						
                    ServerMessage Message = new ServerMessage(134);
					Message.AppendUInt(0);
					Message.AppendString("PHX: " + s);
					PhoenixEnvironment.GetGame().GetClientManager().BroadcastMessageToStaff(Message, Message);                        
                    break;

                case "ha":		
                    ServerMessage Message2 = new ServerMessage(808);
					Message2.AppendStringWithBreak(TextManager.GetText("mus_ha_title"));
					Message2.AppendStringWithBreak(s);
					ServerMessage hotelView = new ServerMessage(161);
					hotelView.AppendStringWithBreak(s);
					PhoenixEnvironment.GetGame().GetClientManager().BroadcastMessage(Message2, hotelView);
                    break;

                case "hal":						
                    string text4 = s.Split(new char[] { ' ' })[0];
				    text5 = s.Substring(text4.Length + 1);
					ServerMessage Message4 = new ServerMessage(161u);
					Message4.AppendStringWithBreak(string.Concat(new string[]
					{
						TextManager.GetText("mus_hal_title"),
						"\r\n",
						text5,
						"\r\n-",
						TextManager.GetText("mus_hal_tail")
					}));
					Message4.AppendStringWithBreak(text4);
					PhoenixEnvironment.GetGame().GetClientManager().BroadcastMessage(Message4);
                    break;

                case "updatemotto":
                case "updatelook":			
                uint_2 = uint.Parse(s);
				clientByUserID = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_2);
				using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					Row = class2.ReadDataRow("SELECT look,gender,motto,mutant_penalty,block_newfriends FROM users WHERE UserId = '" + clientByUserID.GetHabbo().Id + "' LIMIT 1");
				}
                clientByUserID.GetHabbo().Look = (string)Row["look"];
                clientByUserID.GetHabbo().Gender = Row["gender"].ToString().ToLower();
                clientByUserID.GetHabbo().Motto = PhoenixEnvironment.FilterInjectionChars((string)Row["motto"]);
                clientByUserID.GetHabbo().BlockNewFriends = PhoenixEnvironment.EnumToBool(Row["block_newfriends"].ToString());
				ServerMessage Message5 = new ServerMessage(266);
				Message5.AppendInt32(-1);
				Message5.AppendStringWithBreak(clientByUserID.GetHabbo().Look);
				Message5.AppendStringWithBreak(clientByUserID.GetHabbo().Gender.ToLower());
				Message5.AppendStringWithBreak(clientByUserID.GetHabbo().Motto);
				clientByUserID.SendMessage(Message5);
				if (clientByUserID.GetHabbo().InRoom)
				{
					room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(clientByUserID.GetHabbo().CurrentRoomId);
					RoomUser class6 = room.GetRoomUserByHabbo(clientByUserID.GetHabbo().Id);
					ServerMessage Message6 = new ServerMessage(266u);
					Message6.AppendInt32(class6.VirtualId);
					Message6.AppendStringWithBreak(clientByUserID.GetHabbo().Look);
					Message6.AppendStringWithBreak(clientByUserID.GetHabbo().Gender.ToLower());
					Message6.AppendStringWithBreak(clientByUserID.GetHabbo().Motto);
					Message6.AppendInt32(clientByUserID.GetHabbo().AchievementScore);
					Message6.AppendStringWithBreak("");
					room.SendMessage(Message6, null);
				}
				text3 = str.ToLower();
                //if (text3 == null)
                //{
                //    ServerMessage message = new ServerMessage(1);
                //    message.AppendString("Hello Housekeeping, Love from Phoenix Emu");
                //    socket.Send(message.GetBytes());
                //}
				if (text3 == "updatemotto")
				{
					PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(clientByUserID, 5u, 1);
				}
				if (text3 == "updatelook")
				{
					PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(clientByUserID, 1u, 1);
				}
                break;

                default:                    
                    ServerMessage message11 = new ServerMessage(1);
                    message11.AppendString("Hello Housekeeping, Love from Phoenix Emu");
                    socket.Send(message11.GetBytes());
                break;
            }
		}

        public void tryClose()
        {
            try
            {
                this.socket.Shutdown(SocketShutdown.Both);
                this.socket.Close();
                this.socket.Dispose();
            }
            catch
            {
            }
            //this.buffer = null;
            //this.socket = null;
        }
	}
}
