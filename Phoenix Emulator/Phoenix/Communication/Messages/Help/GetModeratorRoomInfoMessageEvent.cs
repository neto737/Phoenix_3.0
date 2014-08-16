using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class GetModeratorRoomInfoMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint Id = Event.PopWiredUInt();
                RoomData Data = PhoenixEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(Id);
				Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().SerializeRoomTool(Data));
			}
		}
	}
}
