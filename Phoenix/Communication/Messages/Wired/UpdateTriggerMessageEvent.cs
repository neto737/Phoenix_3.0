using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Wired
{
	internal sealed class UpdateTriggerMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			RoomItem item = room.GetItem(Event.PopWiredUInt());
			if (room != null && item != null)
			{
				string text = item.GetBaseItem().InteractionType.ToLower();
				if (text != null)
				{
					if (!(text == "wf_trg_onsay"))
					{
						if (!(text == "wf_trg_enterroom"))
						{
							if (!(text == "wf_trg_timer"))
							{
								if (!(text == "wf_trg_attime"))
								{
									if (text == "wf_trg_atscore")
									{
										Event.PopWiredBoolean();
										string text2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
										string[] array = text2.Split(new char[]
										{
											'@'
										});
										item.Extra2 = array[0];
										item.Extra1 = Convert.ToString(Event.PopWiredInt32());
									}
								}
								else
								{
									Event.PopWiredBoolean();
									string text2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
									string[] array = text2.Split(new char[]
									{
										'@'
									});
									item.Extra2 = array[0];
									item.Extra1 = Convert.ToString(Convert.ToString((double)Event.PopWiredInt32() * 0.5));
								}
							}
							else
							{
								Event.PopWiredBoolean();
								string text2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
								string[] array = text2.Split(new char[]
								{
									'@'
								});
								item.Extra2 = array[0];
								item.Extra1 = Convert.ToString(Convert.ToString((double)Event.PopWiredInt32() * 0.5));
							}
						}
						else
						{
							Event.PopWiredBoolean();
							string text3 = Event.PopFixedString();
							item.Extra1 = text3;
						}
					}
					else
					{
						Event.PopWiredBoolean();
						bool value = Event.PopWiredBoolean();
						string text3 = Event.PopFixedString();
						text3 = PhoenixEnvironment.FilterInjectionChars(text3, false, true);
						if (text3.Length > 100)
						{
							text3 = text3.Substring(0, 100);
						}
						item.Extra1 = text3;
						item.Extra2 = Convert.ToString(value);
					}
				}
				item.UpdateState(true, false);
			}
		}
	}
}
