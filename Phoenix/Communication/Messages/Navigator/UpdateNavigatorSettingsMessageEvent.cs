using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class UpdateNavigatorSettingsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
            RoomData @class = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(num);
			if (num == 0u || (@class != null && !(@class.Owner.ToLower() != Session.GetHabbo().Username.ToLower())))
			{
				Session.GetHabbo().uint_4 = num;
				using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET home_room = '",
						num,
						"' WHERE Id = '",
						Session.GetHabbo().Id,
						"' LIMIT 1;"
					}));
				}
				ServerMessage Message = new ServerMessage(455u);
				Message.AppendUInt(num);
				Session.SendMessage(Message);
			}
		}
	}
}
