using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.Storage;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Furniture
{
	internal sealed class PlacePostItMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null)
				{
					if (@class.method_72("stickiepole") > 0 || @class.CheckRights(Session))
					{
						uint uint_ = Event.PopWiredUInt();
						string text = Event.PopFixedString();
						string[] array = text.Split(new char[]
						{
							' '
						});
						if (array[0].Contains("-"))
						{
							array[0] = array[0].Replace("-", "");
						}
						UserItem class2 = Session.GetHabbo().GetInventoryComponent().GetItem(uint_);
						if (class2 != null)
						{
							if (array[0].StartsWith(":"))
							{
								string text2 = @class.method_98(text);
								if (text2 == null)
								{
									ServerMessage Message = new ServerMessage(516u);
									Message.AppendInt32(11);
									Session.SendMessage(Message);
									return;
								}
								RoomItem RoomItem_ = new RoomItem(class2.Id, @class.RoomId, class2.BaseItem, class2.ExtraData, 0, 0, 0.0, 0, text2, @class);
								if (@class.method_82(Session, RoomItem_, true, null))
								{
									Session.GetHabbo().GetInventoryComponent().RemoveItem(uint_, 1u, false);
								}
							}
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
					}
				}
			}
			catch
			{
			}
		}
	}
}
