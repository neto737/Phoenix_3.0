using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
using Phoenix.Storage;
using Phoenix.Util;
namespace Phoenix.HabboHotel.Support
{
	class ModerationTool
    {
        #region General
        public List<SupportTicket> Tickets;
		public List<string> UserMessagePresets;
		public List<string> RoomMessagePresets;

		public ModerationTool()
		{
			this.Tickets = new List<SupportTicket>();
			this.UserMessagePresets = new List<string>();
			this.RoomMessagePresets = new List<string>();
		}

		public ServerMessage SerializeTool()
		{
			ServerMessage Message = new ServerMessage(531);
			Message.AppendInt32(-1);
			Message.AppendInt32(this.UserMessagePresets.Count);
			using (TimedLock.Lock(this.UserMessagePresets))
			{
				foreach (string Preset in this.UserMessagePresets)
				{
					Message.AppendStringWithBreak(Preset);
				}
			}
			Message.AppendUInt(6);
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1"));
			Message.AppendUInt(8);
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_problem1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_solution1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_problem2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_solution2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_problem3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_solution3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_problem4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_solution4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_problem5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_solution5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_problem6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_solution6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_problem7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_solution7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_problem8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category1_solution8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2"));
			Message.AppendUInt(8);
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_problem1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_solution1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_problem2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_solution2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_problem3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_solution3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_problem4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_solution4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_problem5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_solution5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_problem6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_solution6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_problem7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_solution7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_problem8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category2_solution8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3"));
			Message.AppendUInt(8);
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_problem1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_solution1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_problem2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_solution2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_problem3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_solution3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_problem4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_solution4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_problem5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_solution5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_problem6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_solution6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_problem7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_solution7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_problem8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category3_solution8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4"));
			Message.AppendUInt(8);
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_problem1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_solution1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_problem2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_solution2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_problem3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_solution3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_problem4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_solution4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_problem5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_solution5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_problem6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_solution6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_problem7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_solution7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_problem8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category4_solution8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5"));
			Message.AppendUInt(8);
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_problem1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_solution1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_problem2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_solution2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_problem3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_solution3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_problem4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_solution4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_problem5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_solution5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_problem6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_solution6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_problem7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_solution7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_problem8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category5_solution8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6"));
			Message.AppendUInt(8);
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_problem1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_solution1"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_problem2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_solution2"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_problem3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_solution3"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_problem4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_solution4"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_problem5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_solution5"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_problem6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_solution6"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_problem7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_solution7"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_problem8"));
			Message.AppendStringWithBreak(TextManager.GetText("mod_tool_category6_solution8"));
			Message.AppendStringWithBreak("");
			Message.AppendStringWithBreak("");
			Message.AppendStringWithBreak("");
			Message.AppendStringWithBreak("");
			Message.AppendStringWithBreak("");
			Message.AppendStringWithBreak("");
			Message.AppendStringWithBreak("");
			Message.AppendInt32(this.RoomMessagePresets.Count);
			using (TimedLock.Lock(this.RoomMessagePresets))
			{
				foreach (string current in this.RoomMessagePresets)
				{
					Message.AppendStringWithBreak(current);
				}
			}
			Message.AppendStringWithBreak("");
			return Message;
		}

        public void LoadMessagePresets(DatabaseClient adapter)
		{
			Logging.Write("Loading Pre-set Help Messages..");
			this.UserMessagePresets.Clear();
			this.RoomMessagePresets.Clear();
			DataTable dataTable = adapter.ReadDataTable("SELECT type,message FROM moderation_presets WHERE enabled = '1'");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					string item = (string)dataRow["message"];
					string text = dataRow["type"].ToString().ToLower();
					if (text != null)
					{
						if (!(text == "message"))
						{
							if (text == "roommessage")
							{
								this.RoomMessagePresets.Add(item);
							}
						}
						else
						{
							this.UserMessagePresets.Add(item);
						}
					}
				}
				Logging.WriteLine("completed!");
			}
		}
        #endregion

        #region Support Tickets
        public void LoadPendingTickets(DatabaseClient class6_0)
		{
			Logging.Write("Loading Current Help Tickets..");
			DataTable dataTable = class6_0.ReadDataTable("SELECT Id,score,type,status,sender_id,reported_id,moderator_id,message,room_id,room_name,timestamp FROM moderation_tickets WHERE status = 'open' OR status = 'picked'");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					SupportTicket @class = new SupportTicket((uint)dataRow["Id"], (int)dataRow["score"], (int)dataRow["type"], (uint)dataRow["sender_id"], (uint)dataRow["reported_id"], (string)dataRow["message"], (uint)dataRow["room_id"], (string)dataRow["room_name"], (double)dataRow["timestamp"], (uint)dataRow["moderator_id"]);
					if (dataRow["status"].ToString().ToLower() == "picked")
					{
						@class.Pick((uint)dataRow["moderator_id"], false);
					}
					this.Tickets.Add(@class);
				}
				Logging.WriteLine("completed!");
			}
		}

        public void SendNewTicket(GameClient Session, int Type, uint ReportedUser, string Message)
		{
			if (Session.GetHabbo().CurrentRoomId > 0)
			{
				RoomData Data = PhoenixEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(Session.GetHabbo().CurrentRoomId);
				uint TicketId = 0;
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.AddParamWithValue("message", Message);
					adapter.AddParamWithValue("name", Data.Name);
					adapter.ExecuteQuery("INSERT INTO moderation_tickets (score,type,status,sender_id,reported_id,moderator_id,message,room_id,room_name,timestamp) VALUES (1,'" + Type + "','open','" + Session.GetHabbo().Id + "','" + ReportedUser + "','0',@message,'" + Data.Id + "',@name,UNIX_TIMESTAMP())");
					adapter.ExecuteQuery("UPDATE user_info SET cfhs = cfhs + 1 WHERE user_id = '" + Session.GetHabbo().Id + "' LIMIT 1");
					TicketId = (uint)adapter.ReadDataRow("SELECT Id FROM moderation_tickets WHERE sender_id = '" + Session.GetHabbo().Id + "' ORDER BY Id DESC LIMIT 1")[0];
				}
				SupportTicket Ticket = new SupportTicket(TicketId, 1, Type, Session.GetHabbo().Id, ReportedUser, Message, Data.Id, Data.Name, PhoenixEnvironment.GetUnixTimestamp(), 0u);
				this.Tickets.Add(Ticket);
				this.SendTicketToModerators(Ticket);
			}
		}

		public void SendOpenTickets(GameClient Session)
		{
            foreach (SupportTicket ticket in this.Tickets.ToArray())
            {
                if ((ticket.Status == TicketStatus.OPEN) || (ticket.Status == TicketStatus.PICKED))
                {
                    Session.SendMessage(ticket.Serialize());
                }
            }
		}

		public SupportTicket GetTicket(uint TicketId)
		{
			using (TimedLock.Lock(this.Tickets))
			{
				foreach (SupportTicket current in this.Tickets)
				{
					if (current.TicketId == TicketId)
					{
						return current;
					}
				}
			}
			return null;
		}

		public void PickTicket(GameClient Session, uint TicketId)
		{
			SupportTicket Ticket = this.GetTicket(TicketId);
			if (Ticket != null && Ticket.Status == TicketStatus.OPEN)
			{
				Ticket.Pick(Session.GetHabbo().Id, true);
				this.SendTicketToModerators(Ticket);
			}
		}

		public void ReleaseTicket(GameClient Session, uint TicketId)
		{
			SupportTicket Ticket = this.GetTicket(TicketId);
			if (Ticket != null && Ticket.Status == TicketStatus.PICKED && Ticket.ModeratorId == Session.GetHabbo().Id)
			{
				Ticket.Release(true);
				this.SendTicketToModerators(Ticket);
			}
		}

		public void CloseTicket(GameClient Session, uint TicketId, int Result)
		{
			SupportTicket Ticket = this.GetTicket(TicketId);
			if (Ticket != null && Ticket.Status == TicketStatus.PICKED && Ticket.ModeratorId == Session.GetHabbo().Id)
			{
				GameClient Client = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Ticket.SenderId);
				int ResultCode;
				TicketStatus NewStatus;

				switch (Result)
				{
				case 1:
					ResultCode = 1;
					NewStatus = TicketStatus.INVALID;
                    break;

				case 2:
					ResultCode = 2;
					NewStatus = TicketStatus.ABUSIVE;
					using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
					{
						dbClient.ExecuteQuery("UPDATE user_info SET cfhs_abusive = cfhs_abusive + 1 WHERE user_id = '" + Ticket.SenderId + "' LIMIT 1");
					}
                    break;

                case 3:
                default:
                    ResultCode = 0;
				    NewStatus = TicketStatus.RESOLVED;
                    break;
				}

				if (Client != null)
				{
					ServerMessage Message = new ServerMessage(540);
					Message.AppendInt32(ResultCode);
					Client.SendMessage(Message);
				}

				Ticket.Close(NewStatus, true);
				this.SendTicketToModerators(Ticket);
			}
		}

		public bool UsersHasPendingTicket(uint Id)
		{
			using (TimedLock.Lock(this.Tickets))
			{
				foreach (SupportTicket current in this.Tickets)
				{
					if (current.SenderId == Id && current.Status == TicketStatus.OPEN)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void DeletePendingTicketForUser(uint Id)
		{
			using (TimedLock.Lock(this.Tickets))
			{
				foreach (SupportTicket Ticket in this.Tickets)
				{
					if (Ticket.SenderId == Id)
					{
						Ticket.Delete(true);
						this.SendTicketToModerators(Ticket);
						break;
					}
				}
			}
		}

		public void SendTicketToModerators(SupportTicket Ticket)
		{
			PhoenixEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(Ticket.Serialize(), "acc_supporttool");
		}

		public void PerformRoomAction(GameClient Session, uint RoomId, bool KickUsers, bool LockRoom, bool InappropriateRoom)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
			if (Room != null)
			{
				if (LockRoom)
				{
					Room.State = 1;
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.ExecuteQuery("UPDATE rooms SET state = 'locked' WHERE Id = '" + Room.RoomId + "' LIMIT 1");
					}
				}
				if (InappropriateRoom)
				{
					Room.Name = TextManager.GetText("mod_inappropriate_roomname");
					Room.Description = TextManager.GetText("mod_inappropriate_roomdesc");
					Room.Tags.Clear();

					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.ExecuteQuery("UPDATE rooms SET caption = '" + Room.Name + "', description = '" + Room.Description + "', tags = '' WHERE Id = '" + Room.RoomId + "' LIMIT 1");
					}
				}
				if (KickUsers)
				{
					List<RoomUser> Kick = new List<RoomUser>();
					for (int i = 0; i < Room.UserList.Length; i++)
					{
						RoomUser User = Room.UserList[i];
						if (User != null && (User != null && !User.IsBot && User.GetClient().GetHabbo().Rank < Session.GetHabbo().Rank))
						{
							Kick.Add(User);
						}
					}
					for (int i = 0; i < Kick.Count; i++)
					{
						Room.RemoveUserFromRoom(Kick[i].GetClient(), true, false);
					}
				}
			}
		}

		public void method_13(uint uint_0, bool bool_0, string string_0)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(uint_0);
			if (Room != null && string_0.Length > 1)
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				for (int i = 0; i < Room.UserList.Length; i++)
				{
					RoomUser class2 = Room.UserList[i];
					if (class2 != null && !class2.IsBot)
					{
						class2.GetClient().SendNotif(string_0, 0);
						if (class2.GetClient().GetHabbo().Rank <= 2u)
						{
							if (num > 0)
							{
								stringBuilder.Append(" OR ");
							}
							stringBuilder.Append("user_id = '" + class2.GetClient().GetHabbo().Id + "'");
							num++;
						}
					}
				}
				if (bool_0 && num > 0)
				{
					using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class3.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE user_info SET cautions = cautions + 1 WHERE ",
							stringBuilder.ToString(),
							" LIMIT ",
							num
						}));
					}
				}
			}
		}

		public ServerMessage SerializeRoomTool(RoomData Data)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Data.Id);
			uint OwnerId = 0;
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				try
				{
					adapter.AddParamWithValue("owner", Data.Owner);
					OwnerId = (uint)adapter.ReadDataRow("SELECT Id FROM users WHERE username = @owner LIMIT 1")[0];
				}
				catch (Exception)
				{
				}
			}
			ServerMessage Message = new ServerMessage(538);
			Message.AppendUInt(Data.Id);
			Message.AppendInt32(Data.UsersNow);
			if (Room != null)
			{
				Message.AppendBoolean(Room.GetRoomUserByHabbo(Data.Owner) != null);
			}
			else
			{
				Message.AppendBoolean(false);
			}
			Message.AppendUInt(OwnerId);
			Message.AppendStringWithBreak(Data.Owner);
			Message.AppendUInt(Data.Id);
			Message.AppendStringWithBreak(Data.Name);
			Message.AppendStringWithBreak(Data.Description);
			Message.AppendInt32(Data.TagCount);
			foreach (string current in Data.Tags)
			{
				Message.AppendStringWithBreak(current);
			}
			if (Room != null)
			{
				Message.AppendBoolean(Room.HasOngoingEvent);
				if (Room.Event == null)
				{
					return Message;
				}
				Message.AppendStringWithBreak(Room.Event.Name);
				Message.AppendStringWithBreak(Room.Event.Description);
				Message.AppendInt32(Room.Event.Tags.Count);
				using (TimedLock.Lock(Room.Event.Tags))
				{
					foreach (string current in Room.Event.Tags)
					{
						Message.AppendStringWithBreak(current);
					}
					return Message;
				}
			}
			Message.AppendBoolean(false);
			return Message;
		}
        #endregion

        #region User Moderation
        public void KickUser(GameClient ModSession, uint UserId, string Message, bool Soft)
        {
            GameClient Client = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(UserId);
            if (Client == null || Client.GetHabbo().CurrentRoomId < 1 || Client.GetHabbo().Id == ModSession.GetHabbo().Id)
            {
                return;
            }
            if (Client.GetHabbo().Rank >= ModSession.GetHabbo().Rank)
            {
                ModSession.SendNotif(TextManager.GetText("mod_error_permission_kick"));
            }

            Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Client.GetHabbo().CurrentRoomId);

            if (Room != null)
            {
                Room.RemoveUserFromRoom(Client, true, false);
                if (!Soft)
                {
                    Client.SendNotif(Message);
                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        adapter.ExecuteQuery("UPDATE user_info SET cautions = cautions + 1 WHERE user_id = '" + UserId + "' LIMIT 1");
                    }
                }
            }
        }

		public void AlertUser(GameClient ModSession, uint UserId, string Message, bool Caution)
		{
			GameClient Client = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(UserId);

			if (Client != null && Client.GetHabbo().Id != ModSession.GetHabbo().Id)
			{
				if (Caution && Client.GetHabbo().Rank >= ModSession.GetHabbo().Rank)
				{
					ModSession.SendNotif(TextManager.GetText("mod_error_permission_caution"));
					Caution = false;
				}
				Client.SendNotif(Message, 0);
				if (Caution)
				{
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.ExecuteQuery("UPDATE user_info SET cautions = cautions + 1 WHERE user_id = '" + UserId + "' LIMIT 1");
					}
				}
			}
		}

		public void BanUser(GameClient ModSession, uint UserId, int Length, string Message)
		{
			GameClient Client = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(UserId);

			if (Client != null && Client.GetHabbo().Id != ModSession.GetHabbo().Id)
			{
				if (Client.GetHabbo().Rank >= ModSession.GetHabbo().Rank)
				{
					ModSession.SendNotif(TextManager.GetText("mod_error_permission_ban"));
				}
				else
				{
					Double dLength = Length;
					PhoenixEnvironment.GetGame().GetBanManager().BanUser(Client, ModSession.GetHabbo().Username, dLength, Message, false);
				}
			}
		}
        #endregion

        #region User Info

        public ServerMessage SerializeUserInfo(uint UserId)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataRow User = adapter.ReadDataRow("SELECT Id, username, online FROM users WHERE Id = '" + UserId + "' LIMIT 1");
				DataRow Info = adapter.ReadDataRow("SELECT reg_timestamp, login_timestamp, cfhs, cfhs_abusive, cautions, bans FROM user_info WHERE user_id = '" + UserId + "' LIMIT 1");
				if (User == null)
				{
					throw new ArgumentException();
				}
				ServerMessage Message = new ServerMessage(533);
				Message.AppendUInt((uint)User["Id"]);
				Message.AppendStringWithBreak((string)User["username"]);
				if (Info != null)
				{
					Message.AppendInt32((int)Math.Ceiling((PhoenixEnvironment.GetUnixTimestamp() - (double)Info["reg_timestamp"]) / 60.0));
					Message.AppendInt32((int)Math.Ceiling((PhoenixEnvironment.GetUnixTimestamp() - (double)Info["login_timestamp"]) / 60.0));
				}
				else
				{
					Message.AppendInt32(0);
					Message.AppendInt32(0);
				}
				if (User["online"].ToString() == "1")
				{
					Message.AppendBoolean(true);
				}
				else
				{
					Message.AppendBoolean(false);
				}
				if (Info != null)
				{
					Message.AppendInt32((int)Info["cfhs"]);
					Message.AppendInt32((int)Info["cfhs_abusive"]);
					Message.AppendInt32((int)Info["cautions"]);
					Message.AppendInt32((int)Info["bans"]);
				}
				else
				{
					Message.AppendInt32(0);
					Message.AppendInt32(0);
					Message.AppendInt32(0);
					Message.AppendInt32(0);
				}
				return Message;
			}
		}

		public ServerMessage SerializeRoomVisits(uint UserId)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable dataTable = adapter.ReadDataTable("SELECT room_id,hour,minute FROM user_roomvisits WHERE user_id = '" + UserId + "' ORDER BY entry_timestamp DESC LIMIT 50");
				ServerMessage Message = new ServerMessage(537);
				Message.AppendUInt(UserId);
				Message.AppendStringWithBreak(PhoenixEnvironment.GetGame().GetClientManager().GetNameById(UserId));
				if (dataTable != null)
				{
					Message.AppendInt32(dataTable.Rows.Count);
					IEnumerator enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow Row = (DataRow)enumerator.Current;
							RoomData RoomData = PhoenixEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData((uint)Row["room_id"]);
							Message.AppendBoolean(RoomData.IsPublicRoom);
							Message.AppendUInt(RoomData.Id);
							Message.AppendStringWithBreak(RoomData.Name);
							Message.AppendInt32((int)Row["hour"]);
							Message.AppendInt32((int)Row["minute"]);
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				Message.AppendInt32(0);

				return Message;
			}
		}
        #endregion

        #region Chatlog
        public ServerMessage SerializeUserChatlog(uint UserId)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable dataTable = adapter.ReadDataTable("SELECT room_id,entry_timestamp,exit_timestamp FROM user_roomvisits WHERE user_id = '" + UserId + "' ORDER BY entry_timestamp DESC LIMIT 5");
				ServerMessage Message = new ServerMessage(536u);
				Message.AppendUInt(UserId);
				Message.AppendStringWithBreak(PhoenixEnvironment.GetGame().GetClientManager().GetNameById(UserId));
				if (dataTable != null)
				{
					Message.AppendInt32(dataTable.Rows.Count);
					IEnumerator enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							DataTable dataTable2;
							if ((double)dataRow["exit_timestamp"] <= 0.0)
							{
								dataTable2 = adapter.ReadDataTable(string.Concat(new object[]
								{
									"SELECT user_id,user_name,hour,minute,message FROM chatlogs WHERE room_id = '",
									(uint)dataRow["room_id"],
									"' AND timestamp > ",
									(double)dataRow["entry_timestamp"],
									" AND timestamp < UNIX_TIMESTAMP() ORDER BY timestamp DESC LIMIT 100"
								}));
							}
							else
							{
								dataTable2 = adapter.ReadDataTable(string.Concat(new object[]
								{
									"SELECT user_id,user_name,hour,minute,message FROM chatlogs WHERE room_id = '",
									(uint)dataRow["room_id"],
									"' AND timestamp > ",
									(double)dataRow["entry_timestamp"],
									" AND timestamp < ",
									(double)dataRow["exit_timestamp"],
									" ORDER BY timestamp DESC LIMIT 100"
								}));
							}
							RoomData class2 = PhoenixEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData((uint)dataRow["room_id"]);
							Message.AppendBoolean(class2.IsPublicRoom);
							Message.AppendUInt(class2.Id);
							Message.AppendStringWithBreak(class2.Name);
							if (dataTable2 != null)
							{
								Message.AppendInt32(dataTable2.Rows.Count);
								IEnumerator enumerator2 = dataTable2.Rows.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										DataRow dataRow2 = (DataRow)enumerator2.Current;
										Message.AppendInt32((int)dataRow2["hour"]);
										Message.AppendInt32((int)dataRow2["minute"]);
										Message.AppendUInt((uint)dataRow2["user_id"]);
										Message.AppendStringWithBreak((string)dataRow2["user_name"]);
										Message.AppendStringWithBreak((string)dataRow2["message"]);
									}
									continue;
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
							Message.AppendInt32(0);
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				Message.AppendInt32(0);
				return Message;
			}
		}

		public ServerMessage SerializeTicketChatlog(SupportTicket Ticket, RoomData RoomData, double Timestamp)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable dataTable = adapter.ReadDataTable("SELECT user_id,user_name,hour,minute,message FROM chatlogs WHERE room_id = '" + RoomData.Id + "' AND timestamp >= '" + (Timestamp - 300.0) + "' AND timestamp < UNIX_TIMESTAMP() ORDER BY timestamp DESC LIMIT 100");
				ServerMessage Message = new ServerMessage(534);
				Message.AppendUInt(Ticket.TicketId);
				Message.AppendUInt(Ticket.SenderId);
				Message.AppendUInt(Ticket.ReportedId);
				Message.AppendBoolean(RoomData.IsPublicRoom);
				Message.AppendUInt(RoomData.Id);
				Message.AppendStringWithBreak(RoomData.Name);
				if (dataTable != null)
				{
					Message.AppendInt32(dataTable.Rows.Count);
					IEnumerator enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							Message.AppendInt32((int)dataRow["hour"]);
							Message.AppendInt32((int)dataRow["minute"]);
							Message.AppendUInt((uint)dataRow["user_id"]);
							Message.AppendStringWithBreak((string)dataRow["user_name"]);
							Message.AppendStringWithBreak((string)dataRow["message"]);
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				Message.AppendInt32(0);

				return Message;
			}
		}

		public ServerMessage SerializeRoomChatlog(uint roomID)
		{
			Room currentRoom = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(roomID);
			if (currentRoom == null)
			{
				throw new ArgumentException();
			}
			bool IsPublic = false;
			if (currentRoom.Type.ToLower() == "public")
			{
				IsPublic = true;
			}

			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable Data = adapter.ReadDataTable("SELECT user_id,user_name,hour,minute,message FROM chatlogs WHERE room_id = '" + currentRoom.RoomId + "' ORDER BY timestamp DESC LIMIT 150");
				ServerMessage Message = new ServerMessage(535);
				Message.AppendBoolean(IsPublic);
				Message.AppendUInt(currentRoom.RoomId);
				Message.AppendStringWithBreak(currentRoom.Name);
				if (Data != null)
				{
					Message.AppendInt32(Data.Rows.Count);
					IEnumerator enumerator = Data.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							Message.AppendInt32((int)dataRow["hour"]);
							Message.AppendInt32((int)dataRow["minute"]);
							Message.AppendUInt((uint)dataRow["user_id"]);
							Message.AppendStringWithBreak((string)dataRow["user_name"]);
							Message.AppendStringWithBreak((string)dataRow["message"]);
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				Message.AppendInt32(0);

				return Message;
			}
        }
        #endregion
    }
}
