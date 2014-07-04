using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Rooms.Pets
{
	internal sealed class RespectPetMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && !@class.IsPublic)
			{
				uint uint_ = Event.PopWiredUInt();
				RoomUser class2 = @class.method_48(uint_);
				if (class2 != null && class2.PetData != null && Session.GetHabbo().DailyPetRespectPoints > 0)
				{
					class2.PetData.OnRespect();
					Session.GetHabbo().DailyPetRespectPoints--;
					using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class3.AddParamWithValue("userid", Session.GetHabbo().Id);
						class3.ExecuteQuery("UPDATE user_stats SET dailypetrespectpoints = dailypetrespectpoints - 1 WHERE Id = @userid LIMIT 1");
					}
				}
			}
		}
	}
}
