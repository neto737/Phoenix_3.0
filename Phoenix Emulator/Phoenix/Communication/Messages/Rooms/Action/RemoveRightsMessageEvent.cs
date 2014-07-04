using System;
using System.Text;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Rooms.Action
{
	internal sealed class RemoveRightsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = Event.PopWiredInt32();
				for (int i = 0; i < num; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(" OR ");
					}
					uint num2 = Event.PopWiredUInt();
					@class.UsersWithRights.Remove(num2);
					stringBuilder.Append(string.Concat(new object[]
					{
						"room_id = '",
						@class.RoomId,
						"' AND user_id = '",
						num2,
						"'"
					}));
					RoomUser class2 = @class.GetRoomUserByHabbo(num2);
					if (class2 != null && !class2.IsBot)
					{
						class2.GetClient().SendMessage(new ServerMessage(43u));
						class2.RemoveStatus("flatctrl");
						class2.UpdateNeeded = true;
					}
					ServerMessage Message = new ServerMessage(511u);
					Message.AppendUInt(@class.RoomId);
					Message.AppendUInt(num2);
					Session.SendMessage(Message);
				}
				using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					class3.ExecuteQuery("DELETE FROM room_rights WHERE " + stringBuilder.ToString());
				}
			}
		}
	}
}
