using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Rooms.Action
{
	internal sealed class RemoveAllRightsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				foreach (uint current in @class.UsersWithRights)
				{
					RoomUser class2 = @class.GetRoomUserByHabbo(current);
					if (class2 != null && !class2.IsBot)
					{
						class2.GetClient().SendMessage(new ServerMessage(43u));
					}
					ServerMessage Message = new ServerMessage(511u);
					Message.AppendUInt(@class.RoomId);
					Message.AppendUInt(current);
					Session.SendMessage(Message);
				}
				using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					class3.ExecuteQuery("DELETE FROM room_rights WHERE room_id = '" + @class.RoomId + "'");
				}
				@class.UsersWithRights.Clear();
			}
		}
	}
}
