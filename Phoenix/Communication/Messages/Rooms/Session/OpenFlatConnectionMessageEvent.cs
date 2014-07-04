using System;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Rooms.Session
{
	internal sealed class OpenFlatConnectionMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			string string_ = Event.PopFixedString();
			Event.PopWiredInt32();
			if (PhoenixEnvironment.GetConfig().data["emu.messages.roommgr"] == "1")
			{
				Logging.WriteLine("[RoomMgr] Requesting Private Room [ID: " + num + "]");
			}
			Session.GetMessageHandler().PrepareRoomForUser(num, string_);
		}
	}
}
