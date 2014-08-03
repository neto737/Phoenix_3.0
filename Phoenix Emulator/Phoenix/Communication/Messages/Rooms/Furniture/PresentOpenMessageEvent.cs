using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.Storage;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Furniture
{
	internal sealed class PresentOpenMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null && @class.CheckRights(Session, true))
				{
					RoomItem class2 = @class.GetItem(Event.PopWiredUInt());
					if (class2 != null)
					{
						DataRow dataRow = null;
						using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
						{
							dataRow = class3.ReadDataRow("SELECT base_id,amount,extra_data FROM user_presents WHERE item_id = '" + class2.Id + "' LIMIT 1");
						}
						if (dataRow != null)
						{
							Item class4 = PhoenixEnvironment.GetGame().GetItemManager().GetItem((uint)dataRow["base_id"]);
							if (class4 != null)
							{
								@class.RemoveFurniture(Session, class2.Id, true, true);
								ServerMessage Message = new ServerMessage(219u);
								Message.AppendUInt(class2.Id);
								Session.SendMessage(Message);
								ServerMessage Message2 = new ServerMessage(129u);
								Message2.AppendStringWithBreak(class4.Type.ToString());
								Message2.AppendInt32(class4.SpriteId);
								Message2.AppendStringWithBreak(class4.Name);
								Session.SendMessage(Message2);
								using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
								{
									class3.ExecuteQuery("DELETE FROM user_presents WHERE item_id = '" + class2.Id + "' LIMIT 1");
								}
								PhoenixEnvironment.GetGame().GetCatalog().method_9(Session, class4, (int)dataRow["amount"], (string)dataRow["extra_data"], true, 0u);
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
