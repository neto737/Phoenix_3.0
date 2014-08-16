using System;
using System.Collections.Generic;
using System.Text;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class UpdateRoomThumbnailMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room Room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (Room != null && Room.CheckRights(Session, true))
			{
				Event.PopWiredInt32();
				Dictionary<int, int> Items = new Dictionary<int, int>();
				int BackgroundImage = Event.PopWiredInt32();
				int ForegroundImage = Event.PopWiredInt32();
				int num3 = Event.PopWiredInt32();

				for (int i = 0; i < num3; i++)
				{
					int num4 = Event.PopWiredInt32();
					int num5 = Event.PopWiredInt32();
					if (num4 < 0 || num4 > 10 || (num5 < 1 || num5 > 27) || Items.ContainsKey(num4))
					{
						return;
					}
					Items.Add(num4, num5);
				}
				if (BackgroundImage >= 1 && BackgroundImage <= 24 && (ForegroundImage >= 0 && ForegroundImage <= 11))
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num6 = 0;
					foreach (KeyValuePair<int, int> Item in Items)
					{
						if (num6 > 0)
						{
							stringBuilder.Append("|");
						}
						stringBuilder.Append(Item.Key + "," + Item.Value);
						num6++;
					}
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.ExecuteQuery("UPDATE rooms SET icon_bg = '" + BackgroundImage + "', icon_fg = '" + ForegroundImage + "', icon_items = '" + stringBuilder.ToString() + "' WHERE Id = '" + Room.RoomId + "' LIMIT 1");
					}
					Room.myIcon = new RoomIcon(BackgroundImage, ForegroundImage, Items);

					ServerMessage Message = new ServerMessage(457);
					Message.AppendUInt(Room.RoomId);
					Message.AppendBoolean(true);
					Session.SendMessage(Message);

					ServerMessage Message2 = new ServerMessage(456);
					Message2.AppendUInt(Room.RoomId);
					Session.SendMessage(Message2);
					RoomData Data = Room.RoomData;

					ServerMessage Message3 = new ServerMessage(454);
					Message3.AppendBoolean(false);
					Data.Serialize(Message3, false, false);
					Session.SendMessage(Message3);
				}
			}
		}
	}
}
