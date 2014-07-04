using System;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Rooms.Session
{
	internal sealed class OpenConnectionMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Event.PopWiredInt32();
			uint num = Event.PopWiredUInt();
			Event.PopWiredInt32();
			if (PhoenixEnvironment.GetConfig().data["emu.messages.roommgr"] == "1")
			{
				Logging.WriteLine("[RoomMgr] Requesting Public Room [ID: " + num + "]");
			}
            RoomData @class = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(num);
			if (@class != null && !(@class.Type != "public"))
			{
				Session.GetMessageHandler().PrepareRoomForUser(num, "");
			}
		}
	}
}
