using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Help
{
	internal class ModerateRoomMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasRole("acc_supporttool"))
			{
				uint RoomId = Event.PopWiredUInt();
				bool LockRoom = Event.PopWiredBoolean();
				bool InappropriateRoom = Event.PopWiredBoolean();
				bool KickUsers = Event.PopWiredBoolean();
				string Act = "";
				if (LockRoom)
				{
					Act += "Apply Doorbell";
				}
				if (InappropriateRoom)
				{
					Act += " Change Name";
				}
				if (KickUsers)
				{
					Act += " Kick Users";
				}
				PhoenixEnvironment.GetGame().GetClientManager().RecordCmdLogs(Session, "ModTool - Room Action", Act);
				PhoenixEnvironment.GetGame().GetModerationTool().PerformRoomAction(Session, RoomId, KickUsers, LockRoom, InappropriateRoom);
			}
		}
	}
}
