using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal sealed class GetModeratorRoomInfoMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
                RoomData class27_ = PhoenixEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(uint_);
				Session.SendMessage(PhoenixEnvironment.GetGame().GetModerationTool().method_14(class27_));
			}
		}
	}
}
