using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.Storage;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class PlaceObjectMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session) && (GlobalClass.AllowFriendlyFurni || !(@class.Owner != Session.GetHabbo().Username)))
			{
				string text = Event.PopFixedString();
				string[] array = text.Split(new char[]
				{
					' '
				});
				if (array[0].Contains("-"))
				{
					array[0] = array[0].Replace("-", "");
				}
				uint uint_ = 0u;
				try
				{
					uint_ = uint.Parse(array[0]);
				}
				catch
				{
					return;
				}
				UserItem class2 = Session.GetHabbo().GetInventoryComponent().GetItem(uint_);
				if (class2 != null)
				{
					string text2 = class2.GetBaseItem().InteractionType.ToLower();
					if (text2 != null && text2 == "dimmer" && @class.method_72("dimmer") >= 1)
					{
						Session.SendNotif("You can only have one moodlight in a room.");
					}
                    else if (text2 != null && text2 == "jukebox" && @class.method_72("jukebox") >= 1)
                    {
                        Session.SendNotif("You can only have one jukebox in a room.");
                    }
					else
					{
						RoomItem RoomItem_;
						if (array[1].StartsWith(":"))
						{
							string text3 = @class.method_98(":" + text.Split(new char[]
							{
								':'
							})[1]);
							if (text3 == null)
							{
								ServerMessage Message = new ServerMessage(516u);
								Message.AppendInt32(11);
								Session.SendMessage(Message);
								return;
							}
							RoomItem_ = new RoomItem(class2.Id, @class.RoomId, class2.BaseItem, class2.ExtraData, 0, 0, 0.0, 0, text3, @class);
							if (!@class.method_82(Session, RoomItem_, true, null))
							{
								goto IL_32C;
							}
							Session.GetHabbo().GetInventoryComponent().RemoveItem(uint_, 1u, false);
							using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								class3.ExecuteQuery(string.Concat(new object[]
								{
									"UPDATE items SET room_id = '",
									@class.RoomId,
									"' WHERE Id = '",
									class2.Id,
									"' LIMIT 1"
								}));
								goto IL_32C;
							}
						}
						int int_ = int.Parse(array[1]);
						int int_2 = int.Parse(array[2]);
						int int_3 = int.Parse(array[3]);
						RoomItem_ = new RoomItem(class2.Id, @class.RoomId, class2.BaseItem, class2.ExtraData, 0, 0, 0.0, 0, "", @class);
						if (@class.method_79(Session, RoomItem_, int_, int_2, int_3, true, false, false))
						{
							Session.GetHabbo().GetInventoryComponent().RemoveItem(uint_, 1u, false);
							using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
							{
								class3.ExecuteQuery(string.Concat(new object[]
								{
									"UPDATE items SET room_id = '",
									@class.RoomId,
									"' WHERE Id = '",
									class2.Id,
									"' LIMIT 1"
								}));
							}
						}
						IL_32C:
						if (Session.GetHabbo().CurrentQuestId == 14u)
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(14u, Session);
						}
					}
				}
			}
		}
	}
}
