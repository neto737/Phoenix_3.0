using System;
using System.Collections.Generic;
using System.Text;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
using Phoenix.HabboHotel.Navigators;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Rooms.Settings
{
	internal sealed class SaveRoomSettingsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				Event.PopWiredInt32();
				string text = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				string text2 = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				if (text2.Length > 255)
				{
					text2 = text2.Substring(0, 255);
				}
				int num = Event.PopWiredInt32();
				string text3 = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				int num2 = Event.PopWiredInt32();
				int num3 = Event.PopWiredInt32();
				int num4 = Event.PopWiredInt32();
				List<string> list = new List<string>();
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < num4; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(",");
					}
					string text4 = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString().ToLower());
					if (text4 == ChatCommandHandler.ApplyWordFilter(text4))
					{
						list.Add(text4);
						stringBuilder.Append(text4);
					}
				}
				if (stringBuilder.Length > 100)
				{
					stringBuilder.Clear();
					stringBuilder.Append("");
				}
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				string a = Event.PlainReadBytes(1)[0].ToString();
				Event.AdvancePointer(1);
				string a2 = Event.PlainReadBytes(1)[0].ToString();
				Event.AdvancePointer(1);
				string a3 = Event.PlainReadBytes(1)[0].ToString();
				Event.AdvancePointer(1);
				string a4 = Event.PlainReadBytes(1)[0].ToString();
				Event.AdvancePointer(1);
				int num9 = Event.PopWiredInt32();
				int num10 = Event.PopWiredInt32();
				if (!(text != ChatCommandHandler.ApplyWordFilter(text)) && !(text2 != ChatCommandHandler.ApplyWordFilter(text2)) && text.Length >= 1 && (num9 >= -2 && num9 <= 1 && num10 >= -2 && num10 <= 1))
				{
					@class.Wallthick = num9;
					@class.Floorthick = num10;
					if (num >= 0 && num <= 2 && (num2 == 10 || num2 == 15 || num2 == 20 || num2 == 25 || num2 == 30 || num2 == 35 || num2 == 40 || num2 == 45 || num2 == 50 || num2 == 55 || num2 == 60 || num2 == 65 || num2 == 70 || num2 == 75 || num2 == 80 || num2 == 85 || num2 == 90 || num2 == 95 || num2 == 100))
					{
						FlatCat class2 = PhoenixEnvironment.GetGame().GetNavigator().method_2(num3);
						if (class2 != null)
						{
							if ((long)class2.MinRank > (long)((ulong)Session.GetHabbo().Rank))
							{
								Session.SendNotif("You are not allowed to use this category. Your room has been moved to no category instead.");
								num3 = 0;
							}
							if (num4 <= 2)
							{
								if (a == "65")
								{
									num5 = 1;
									@class.AllowPet = true;
								}
								else
								{
									@class.AllowPet = false;
								}
								if (a2 == "65")
								{
									num6 = 1;
									@class.AllowPetsEating = true;
								}
								else
								{
									@class.AllowPetsEating = false;
								}
								if (a3 == "65")
								{
									num7 = 1;
									@class.AllowWalkthrough = true;
								}
								else
								{
									@class.AllowWalkthrough = false;
								}
								@class.GenerateMaps();
								if (a4 == "65")
								{
									num8 = 1;
									@class.Hidewall = true;
								}
								else
								{
									@class.Hidewall = false;
								}
								@class.Name = text;
								@class.State = num;
								@class.Description = text2;
								@class.Category = num3;
								if (text3 != "")
								{
									@class.Password = text3;
								}
								@class.Tags = list;
								@class.UsersMax = num2;
								string text5 = "open";
								if (@class.State == 1)
								{
									text5 = "locked";
								}
								else
								{
									if (@class.State > 1)
									{
										text5 = "password";
									}
								}
								using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
								{
									class3.AddParamWithValue("caption", @class.Name);
									class3.AddParamWithValue("description", @class.Description);
									class3.AddParamWithValue("password", @class.Password);
									class3.AddParamWithValue("tags", stringBuilder.ToString());
									class3.ExecuteQuery(string.Concat(new object[]
									{
										"UPDATE rooms SET caption = @caption, description = @description, password = @password, category = '",
										num3,
										"', state = '",
										text5,
										"', tags = @tags, users_max = '",
										num2,
										"', allow_pets = '",
										num5,
										"', allow_pets_eat = '",
										num6,
										"', allow_walkthrough = '",
										num7,
										"', allow_hidewall = '",
										num8,
										"', wallthick = '",
										num9,
										"', floorthick = '",
										num10,
										"'  WHERE Id = '",
										@class.RoomId,
										"' LIMIT 1;"
									}));
								}
								ServerMessage Message = new ServerMessage(467u);
								Message.AppendUInt(@class.RoomId);
								Session.SendMessage(Message);
								ServerMessage Message2 = new ServerMessage(456u);
								Message2.AppendUInt(@class.RoomId);
								Session.SendMessage(Message2);
								ServerMessage Message3 = new ServerMessage(472u);
								Message3.AppendBoolean(@class.Hidewall);
								Message3.AppendInt32(@class.Wallthick);
								Message3.AppendInt32(@class.Floorthick);
								@class.SendMessage(Message3, null);
								ServerMessage Message4 = new ServerMessage(473u);
								Message4.AppendBoolean(true);
								Message4.AppendBoolean(true);
								@class.SendMessage(Message4, null);
                                RoomData class27_ = @class.Class27_0;
								ServerMessage Message5 = new ServerMessage(454u);
								Message5.AppendBoolean(false);
								class27_.method_3(Message5, false, false);
								Session.SendMessage(Message5);
							}
						}
					}
				}
			}
		}
	}
}
