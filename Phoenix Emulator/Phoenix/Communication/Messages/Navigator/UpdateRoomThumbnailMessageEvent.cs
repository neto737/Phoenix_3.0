using System;
using System.Collections.Generic;
using System.Text;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class UpdateRoomThumbnailMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				Event.PopWiredInt32();
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				int num = Event.PopWiredInt32();
				int num2 = Event.PopWiredInt32();
				int num3 = Event.PopWiredInt32();
				for (int i = 0; i < num3; i++)
				{
					int num4 = Event.PopWiredInt32();
					int num5 = Event.PopWiredInt32();
					if (num4 < 0 || num4 > 10 || (num5 < 1 || num5 > 27) || dictionary.ContainsKey(num4))
					{
						return;
					}
					dictionary.Add(num4, num5);
				}
				if (num >= 1 && num <= 24 && (num2 >= 0 && num2 <= 11))
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num6 = 0;
					foreach (KeyValuePair<int, int> current in dictionary)
					{
						if (num6 > 0)
						{
							stringBuilder.Append("|");
						}
						stringBuilder.Append(current.Key + "," + current.Value);
						num6++;
					}
					using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class2.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE rooms SET icon_bg = '",
							num,
							"', icon_fg = '",
							num2,
							"', icon_items = '",
							stringBuilder.ToString(),
							"' WHERE Id = '",
							@class.RoomId,
							"' LIMIT 1"
						}));
					}
					@class.myIcon = new RoomIcon(num, num2, dictionary);
					ServerMessage Message = new ServerMessage(457u);
					Message.AppendUInt(@class.RoomId);
					Message.AppendBoolean(true);
					Session.SendMessage(Message);
					ServerMessage Message2 = new ServerMessage(456u);
					Message2.AppendUInt(@class.RoomId);
					Session.SendMessage(Message2);
					RoomData class27_ = @class.Class27_0;
					ServerMessage Message3 = new ServerMessage(454u);
					Message3.AppendBoolean(false);
					class27_.Serialize(Message3, false, false);
					Session.SendMessage(Message3);
				}
			}
		}
	}
}
