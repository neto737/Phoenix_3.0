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
	internal sealed class ModerationTool
	{
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
			ServerMessage Message = new ServerMessage(531u);
			Message.AppendInt32(-1);
			Message.AppendInt32(this.UserMessagePresets.Count);
			using (TimedLock.Lock(this.UserMessagePresets))
			{
				foreach (string current in this.UserMessagePresets)
				{
					Message.AppendStringWithBreak(current);
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
		public void LoadMessagePresets(DatabaseClient class6_0)
		{
			Logging.Write("Loading Pre-set Help Messages..");
			this.UserMessagePresets.Clear();
			this.RoomMessagePresets.Clear();
			DataTable dataTable = class6_0.ReadDataTable("SELECT type,message FROM moderation_presets WHERE enabled = '1'");
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
		public void method_3(GameClient Session, int int_0, uint uint_0, string string_0)
		{
			if (Session.GetHabbo().CurrentRoomId > 0u)
			{
				RoomData @class = PhoenixEnvironment.GetGame().GetRoomManager().method_11(Session.GetHabbo().CurrentRoomId);
				uint uint_ = 0u;
				using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					class2.AddParamWithValue("message", string_0);
					class2.AddParamWithValue("name", @class.Name);
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO moderation_tickets (score,type,status,sender_id,reported_id,moderator_id,message,room_id,room_name,timestamp) VALUES (1,'",
						int_0,
						"','open','",
						Session.GetHabbo().Id,
						"','",
						uint_0,
						"','0',@message,'",
						@class.Id,
						"',@name,UNIX_TIMESTAMP())"
					}));
					class2.ExecuteQuery("UPDATE user_info SET cfhs = cfhs + 1 WHERE user_id = '" + Session.GetHabbo().Id + "' LIMIT 1");
					uint_ = (uint)class2.ReadDataRow("SELECT Id FROM moderation_tickets WHERE sender_id = '" + Session.GetHabbo().Id + "' ORDER BY Id DESC LIMIT 1")[0];
				}
				SupportTicket class3 = new SupportTicket(uint_, 1, int_0, Session.GetHabbo().Id, uint_0, string_0, @class.Id, @class.Name, PhoenixEnvironment.GetUnixTimestamp(), 0u);
				this.Tickets.Add(class3);
				this.method_11(class3);
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
		public SupportTicket method_5(uint uint_0)
		{
			SupportTicket result;
			using (TimedLock.Lock(this.Tickets))
			{
				foreach (SupportTicket current in this.Tickets)
				{
					if (current.TicketId == uint_0)
					{
						result = current;
						return result;
					}
				}
			}
			result = null;
			return result;
		}
		public void method_6(GameClient Session, uint uint_0)
		{
			SupportTicket @class = this.method_5(uint_0);
			if (@class != null && @class.Status == TicketStatus.OPEN)
			{
				@class.Pick(Session.GetHabbo().Id, true);
				this.method_11(@class);
			}
		}
		public void method_7(GameClient Session, uint uint_0)
		{
			SupportTicket @class = this.method_5(uint_0);
			if (@class != null && @class.Status == TicketStatus.PICKED && @class.ModeratorId == Session.GetHabbo().Id)
			{
				@class.Release(true);
				this.method_11(@class);
			}
		}
		public void method_8(GameClient Session, uint uint_0, int int_0)
		{
			SupportTicket @class = this.method_5(uint_0);
			if (@class != null && @class.Status == TicketStatus.PICKED && @class.ModeratorId == Session.GetHabbo().Id)
			{
				GameClient class2 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(@class.SenderId);
				int int_;
				TicketStatus enum6_;
				switch (int_0)
				{
				case 1:
					int_ = 1;
					enum6_ = TicketStatus.INVALID;
					goto IL_AC;
				case 2:
					int_ = 2;
					enum6_ = TicketStatus.ABUSIVE;
					using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class3.ExecuteQuery("UPDATE user_info SET cfhs_abusive = cfhs_abusive + 1 WHERE user_id = '" + @class.SenderId + "' LIMIT 1");
						goto IL_AC;
					}
				}
				int_ = 0;
				enum6_ = TicketStatus.RESOLVED;
				IL_AC:
				if (class2 != null)
				{
					ServerMessage Message = new ServerMessage(540u);
					Message.AppendInt32(int_);
					class2.SendMessage(Message);
				}
				@class.Close(enum6_, true);
				this.method_11(@class);
			}
		}
		public bool method_9(uint uint_0)
		{
			bool result;
			using (TimedLock.Lock(this.Tickets))
			{
				foreach (SupportTicket current in this.Tickets)
				{
					if (current.SenderId == uint_0 && current.Status == TicketStatus.OPEN)
					{
						result = true;
						return result;
					}
				}
			}
			result = false;
			return result;
		}
		public void method_10(uint uint_0)
		{
			using (TimedLock.Lock(this.Tickets))
			{
				foreach (SupportTicket current in this.Tickets)
				{
					if (current.SenderId == uint_0)
					{
						current.Delete(true);
						this.method_11(current);
						break;
					}
				}
			}
		}
		public void method_11(SupportTicket class111_0)
		{
			PhoenixEnvironment.GetGame().GetClientManager().method_22(class111_0.Serialize(), "acc_supporttool");
		}
		public void method_12(GameClient Session, uint uint_0, bool bool_0, bool bool_1, bool bool_2)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(uint_0);
			if (@class != null)
			{
				if (bool_1)
				{
					@class.State = 1;
					using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class2.ExecuteQuery("UPDATE rooms SET state = 'locked' WHERE Id = '" + @class.RoomId + "' LIMIT 1");
					}
				}
				if (bool_2)
				{
					@class.Name = TextManager.GetText("mod_inappropriate_roomname");
					@class.Description = TextManager.GetText("mod_inappropriate_roomdesc");
					@class.Tags.Clear();
					using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class2.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE rooms SET caption = '",
							@class.Name,
							"', description = '",
							@class.Description,
							"', tags = '' WHERE Id = '",
							@class.RoomId,
							"' LIMIT 1"
						}));
					}
				}
				if (bool_0)
				{
					List<RoomUser> list = new List<RoomUser>();
					for (int i = 0; i < @class.UserList.Length; i++)
					{
						RoomUser class3 = @class.UserList[i];
						if (class3 != null && (class3 != null && !class3.IsBot && class3.GetClient().GetHabbo().Rank < Session.GetHabbo().Rank))
						{
							list.Add(class3);
						}
					}
					for (int i = 0; i < list.Count; i++)
					{
						@class.RemoveUserFromRoom(list[i].GetClient(), true, false);
					}
				}
			}
		}
		public void method_13(uint uint_0, bool bool_0, string string_0)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(uint_0);
			if (@class != null && string_0.Length > 1)
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				for (int i = 0; i < @class.UserList.Length; i++)
				{
					RoomUser class2 = @class.UserList[i];
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
		public ServerMessage method_14(RoomData class27_0)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(class27_0.Id);
			uint uint_ = 0u;
			using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
			{
				try
				{
					class2.AddParamWithValue("owner", class27_0.Owner);
					uint_ = (uint)class2.ReadDataRow("SELECT Id FROM users WHERE username = @owner LIMIT 1")[0];
				}
				catch (Exception)
				{
				}
			}
			ServerMessage Message = new ServerMessage(538u);
			Message.AppendUInt(class27_0.Id);
			Message.AppendInt32(class27_0.UsersNow);
			if (@class != null)
			{
				Message.AppendBoolean(@class.GetRoomUserByHabbo(class27_0.Owner) != null);
			}
			else
			{
				Message.AppendBoolean(false);
			}
			Message.AppendUInt(uint_);
			Message.AppendStringWithBreak(class27_0.Owner);
			Message.AppendUInt(class27_0.Id);
			Message.AppendStringWithBreak(class27_0.Name);
			Message.AppendStringWithBreak(class27_0.Description);
			Message.AppendInt32(class27_0.TagCount);
			foreach (string current in class27_0.Tags)
			{
				Message.AppendStringWithBreak(current);
			}
			if (@class != null)
			{
				Message.AppendBoolean(@class.HasOngoingEvent);
				if (@class.Event == null)
				{
					return Message;
				}
				Message.AppendStringWithBreak(@class.Event.Name);
				Message.AppendStringWithBreak(@class.Event.Description);
				Message.AppendInt32(@class.Event.Tags.Count);
				using (TimedLock.Lock(@class.Event.Tags))
				{
					foreach (string current in @class.Event.Tags)
					{
						Message.AppendStringWithBreak(current);
					}
					return Message;
				}
			}
			Message.AppendBoolean(false);
			return Message;
		}
		public void method_15(GameClient Session, uint uint_0, string string_0, bool bool_0)
		{
			GameClient @class = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_0);
			if (@class != null && @class.GetHabbo().CurrentRoomId >= 1u && @class.GetHabbo().Id != Session.GetHabbo().Id)
			{
				if (@class.GetHabbo().Rank >= Session.GetHabbo().Rank)
				{
					Session.SendNotif(TextManager.GetText("mod_error_permission_kick"));
				}
				else
				{
					Room class2 = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(@class.GetHabbo().CurrentRoomId);
					if (class2 != null)
					{
						class2.RemoveUserFromRoom(@class, true, false);
						if (!bool_0)
						{
							@class.SendNotif(string_0);
							using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								class3.ExecuteQuery("UPDATE user_info SET cautions = cautions + 1 WHERE user_id = '" + uint_0 + "' LIMIT 1");
							}
						}
					}
				}
			}
		}
		public void method_16(GameClient Session, uint uint_0, string string_0, bool bool_0)
		{
			GameClient @class = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_0);
			if (@class != null && @class.GetHabbo().Id != Session.GetHabbo().Id)
			{
				if (bool_0 && @class.GetHabbo().Rank >= Session.GetHabbo().Rank)
				{
					Session.SendNotif(TextManager.GetText("mod_error_permission_caution"));
					bool_0 = false;
				}
				@class.SendNotif(string_0, 0);
				if (bool_0)
				{
					using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class2.ExecuteQuery("UPDATE user_info SET cautions = cautions + 1 WHERE user_id = '" + uint_0 + "' LIMIT 1");
					}
				}
			}
		}
		public void method_17(GameClient Session, uint uint_0, int int_0, string string_0)
		{
			GameClient @class = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(uint_0);
			if (@class != null && @class.GetHabbo().Id != Session.GetHabbo().Id)
			{
				if (@class.GetHabbo().Rank >= Session.GetHabbo().Rank)
				{
					Session.SendNotif(TextManager.GetText("mod_error_permission_ban"));
				}
				else
				{
					double double_ = (double)int_0;
					PhoenixEnvironment.GetGame().GetBanManager().BanUser(@class, Session.GetHabbo().Username, double_, string_0, false);
				}
			}
		}
		public ServerMessage method_18(uint uint_0)
		{
			ServerMessage result;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataRow dataRow = @class.ReadDataRow("SELECT Id, username, online FROM users WHERE Id = '" + uint_0 + "' LIMIT 1");
				DataRow dataRow2 = @class.ReadDataRow("SELECT reg_timestamp, login_timestamp, cfhs, cfhs_abusive, cautions, bans FROM user_info WHERE user_id = '" + uint_0 + "' LIMIT 1");
				if (dataRow == null)
				{
					throw new ArgumentException();
				}
				ServerMessage Message = new ServerMessage(533u);
				Message.AppendUInt((uint)dataRow["Id"]);
				Message.AppendStringWithBreak((string)dataRow["username"]);
				if (dataRow2 != null)
				{
					Message.AppendInt32((int)Math.Ceiling((PhoenixEnvironment.GetUnixTimestamp() - (double)dataRow2["reg_timestamp"]) / 60.0));
					Message.AppendInt32((int)Math.Ceiling((PhoenixEnvironment.GetUnixTimestamp() - (double)dataRow2["login_timestamp"]) / 60.0));
				}
				else
				{
					Message.AppendInt32(0);
					Message.AppendInt32(0);
				}
				if (dataRow["online"].ToString() == "1")
				{
					Message.AppendBoolean(true);
				}
				else
				{
					Message.AppendBoolean(false);
				}
				if (dataRow2 != null)
				{
					Message.AppendInt32((int)dataRow2["cfhs"]);
					Message.AppendInt32((int)dataRow2["cfhs_abusive"]);
					Message.AppendInt32((int)dataRow2["cautions"]);
					Message.AppendInt32((int)dataRow2["bans"]);
				}
				else
				{
					Message.AppendInt32(0);
					Message.AppendInt32(0);
					Message.AppendInt32(0);
					Message.AppendInt32(0);
				}
				result = Message;
			}
			return result;
		}
		public ServerMessage method_19(uint uint_0)
		{
			ServerMessage result;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable dataTable = @class.ReadDataTable("SELECT room_id,hour,minute FROM user_roomvisits WHERE user_id = '" + uint_0 + "' ORDER BY entry_timestamp DESC LIMIT 50");
				ServerMessage Message = new ServerMessage(537u);
				Message.AppendUInt(uint_0);
				Message.AppendStringWithBreak(PhoenixEnvironment.GetGame().GetClientManager().GetNameById(uint_0));
				if (dataTable != null)
				{
					Message.AppendInt32(dataTable.Rows.Count);
					IEnumerator enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							RoomData class2 = PhoenixEnvironment.GetGame().GetRoomManager().method_11((uint)dataRow["room_id"]);
							Message.AppendBoolean(class2.IsPublicRoom);
							Message.AppendUInt(class2.Id);
							Message.AppendStringWithBreak(class2.Name);
							Message.AppendInt32((int)dataRow["hour"]);
							Message.AppendInt32((int)dataRow["minute"]);
						}
						goto IL_12A;
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
				IL_12A:
				result = Message;
			}
			return result;
		}
		public ServerMessage method_20(uint uint_0)
		{
			ServerMessage result;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable dataTable = @class.ReadDataTable("SELECT room_id,entry_timestamp,exit_timestamp FROM user_roomvisits WHERE user_id = '" + uint_0 + "' ORDER BY entry_timestamp DESC LIMIT 5");
				ServerMessage Message = new ServerMessage(536u);
				Message.AppendUInt(uint_0);
				Message.AppendStringWithBreak(PhoenixEnvironment.GetGame().GetClientManager().GetNameById(uint_0));
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
								dataTable2 = @class.ReadDataTable(string.Concat(new object[]
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
								dataTable2 = @class.ReadDataTable(string.Concat(new object[]
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
							RoomData class2 = PhoenixEnvironment.GetGame().GetRoomManager().method_11((uint)dataRow["room_id"]);
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
						goto IL_2EF;
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
				IL_2EF:
				result = Message;
			}
			return result;
		}
		public ServerMessage method_21(SupportTicket class111_0, RoomData class27_0, double double_0)
		{
			ServerMessage result;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable dataTable = @class.ReadDataTable(string.Concat(new object[]
				{
					"SELECT user_id,user_name,hour,minute,message FROM chatlogs WHERE room_id = '",
					class27_0.Id,
					"' AND timestamp >= '",
					double_0 - 300.0,
					"' AND timestamp < UNIX_TIMESTAMP() ORDER BY timestamp DESC LIMIT 100"
				}));
				ServerMessage Message = new ServerMessage(534u);
				Message.AppendUInt(class111_0.TicketId);
				Message.AppendUInt(class111_0.SenderId);
				Message.AppendUInt(class111_0.ReportedId);
				Message.AppendBoolean(class27_0.IsPublicRoom);
				Message.AppendUInt(class27_0.Id);
				Message.AppendStringWithBreak(class27_0.Name);
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
						goto IL_186;
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
				IL_186:
				result = Message;
			}
			return result;
		}
		public ServerMessage method_22(uint uint_0)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(uint_0);
			if (@class == null)
			{
				throw new ArgumentException();
			}
			bool bool_ = false;
			if (@class.Type.ToLower() == "public")
			{
				bool_ = true;
			}
			ServerMessage result;
			using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataTable dataTable = class2.ReadDataTable("SELECT user_id,user_name,hour,minute,message FROM chatlogs WHERE room_id = '" + @class.RoomId + "' ORDER BY timestamp DESC LIMIT 150");
				ServerMessage Message = new ServerMessage(535u);
				Message.AppendBoolean(bool_);
				Message.AppendUInt(@class.RoomId);
				Message.AppendStringWithBreak(@class.Name);
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
						goto IL_17A;
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
				IL_17A:
				result = Message;
			}
			return result;
		}
	}
}
