using System;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
namespace Phoenix.Communication.Messages.Wired
{
	internal sealed class UpdateActionMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				uint uint_ = Event.PopWiredUInt();
				RoomItem item = room.GetItem(uint_);
                switch (item.GetBaseItem().InteractionType.ToLower())
				{
				case "wf_act_give_phx":
				{
					Event.PopWiredBoolean();
					string text2 = Event.PopFixedString();
					text2 = PhoenixEnvironment.FilterInjectionChars(text2, false, true);
					text2 = ChatCommandHandler.ApplyWordFilter(text2);
					if (!(text2 == item.Extra1))
					{
						string string_ = text2.Split(new char[]
						{
							':'
						})[0].ToLower();
						if (PhoenixEnvironment.GetGame().GetRoleManager().HasWiredEffectRole(string_, Session))
						{
							item.Extra1 = text2;
						}
						else
						{
							Session.GetHabbo().Sendselfwhisper(TextManager.GetText("wired_error_permissions"));
						}
					}
					break;
				}
				case "wf_cnd_phx":
				{
					Event.PopWiredBoolean();
					string text2 = Event.PopFixedString();
					text2 = PhoenixEnvironment.FilterInjectionChars(text2, false, true);
					text2 = ChatCommandHandler.ApplyWordFilter(text2);
					if (!(text2 == item.Extra1))
					{
						string string_ = text2.Split(new char[]
						{
							':'
						})[0].ToLower();
						if (PhoenixEnvironment.GetGame().GetRoleManager().HasWiredConditionRole(string_, Session))
						{
							item.Extra1 = text2;
						}
						else
						{
							Session.GetHabbo().Sendselfwhisper(TextManager.GetText("wired_error_permissions"));
						}
					}
					break;
				}
				case "wf_act_saymsg":
				{
					Event.PopWiredBoolean();
					string text2 = Event.PopFixedString();
					text2 = PhoenixEnvironment.FilterInjectionChars(text2, false, true);
					if (text2.Length > 100)
					{
						text2 = text2.Substring(0, 100);
					}
					item.Extra1 = text2;
					break;
				}
				case "wf_trg_furnistate":
				case "wf_trg_onfurni":
				case "wf_trg_offfurni":
				case "wf_act_moveuser":
				case "wf_act_togglefurni":
				{
					Event.PopWiredBoolean();
					Event.PopFixedString();
					item.Extra1 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
					item.Extra1 = item.Extra1.Substring(0, item.Extra1.Length - 2);
					Event.ResetPointer();
					item = room.GetItem(Event.PopWiredUInt());
					Event.PopWiredBoolean();
					Event.PopFixedString();
					int num2 = Event.PopWiredInt32();
					item.Extra2 = "";
					for (int i = 0; i < num2; i++)
					{
						item.Extra2 = item.Extra2 + "," + Convert.ToString(Event.PopWiredUInt());
					}
					if (item.Extra2.Length > 0)
					{
						item.Extra2 = item.Extra2.Substring(1);
					}
					break;
				}
				case "wf_act_givepoints":
					Event.PopWiredInt32();
					item.Extra1 = Convert.ToString(Event.PopWiredInt32());
					item.Extra2 = Convert.ToString(Event.PopWiredInt32());
					break;
				case "wf_act_moverotate":
				{
					Event.PopWiredInt32();
					item.Extra1 = Convert.ToString(Event.PopWiredInt32());
					item.Extra2 = Convert.ToString(Event.PopWiredInt32());
					Event.PopFixedString();
					int num2 = Event.PopWiredInt32();
					item.Extra3 = "";
					item.Extra4 = "";
					if (num2 > 0)
					{
						item.Extra4 = OldEncoding.encodeVL64(num2);
						for (int i = 0; i < num2; i++)
						{
							int num3 = Event.PopWiredInt32();
							item.Extra4 += OldEncoding.encodeVL64(num3);
							item.Extra3 = item.Extra3 + "," + Convert.ToString(num3);
						}
						item.Extra3 = item.Extra3.Substring(1);
					}
					item.Extra5 = Convert.ToString(Event.PopWiredInt32());
					break;
				}
				case "wf_act_matchfurni":
				{
					Event.PopWiredInt32();
					item.Extra2 = "";
					if (Event.PopWiredBoolean())
					{
                        item.Extra2 = item.Extra2 + "I";
					}
					else
					{
                        item.Extra2 = item.Extra2 + "H";
					}
					if (Event.PopWiredBoolean())
					{
                        item.Extra2 = item.Extra2 + "I";
					}
					else
					{
                        item.Extra2 = item.Extra2 + "H";
					}
					if (Event.PopWiredBoolean())
					{
                        item.Extra2 = item.Extra2 + "I";
					}
					else
					{
                        item.Extra2 = item.Extra2 + "H";
					}
					Event.PopFixedString();
					int num2 = Event.PopWiredInt32();
					item.Extra1 = "";
					item.Extra3 = "";
					item.Extra4 = "";
					if (num2 > 0)
					{
						item.Extra4 = OldEncoding.encodeVL64(num2);
						for (int i = 0; i < num2; i++)
						{
							int num3 = Event.PopWiredInt32();
							item.Extra4 += OldEncoding.encodeVL64(num3);
							item.Extra3 = item.Extra3 + "," + Convert.ToString(num3);
							RoomItem class3 = room.GetItem(Convert.ToUInt32(num3));
							RoomItem expr_5E6 = item;
							object string_2 = expr_5E6.Extra1;
							expr_5E6.Extra1 = string.Concat(new object[]
							{
								string_2,
								";",
								class3.GetX,
								",",
								class3.GetY,
								",",
								class3.GetZ,
								",",
								class3.Rot,
								",",
								class3.ExtraData
							});
						}
						item.Extra3 = item.Extra3.Substring(1);
						item.Extra1 = item.Extra1.Substring(1);
					}
					item.Extra5 = Convert.ToString(Event.PopWiredInt32());
					break;
				}
				}
				item.UpdateState(true, false);
			}
			catch
			{
			}
		}
	}
}
