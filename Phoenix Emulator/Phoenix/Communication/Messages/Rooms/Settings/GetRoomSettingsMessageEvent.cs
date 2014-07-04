using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Settings
{
	internal sealed class GetRoomSettingsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				ServerMessage Message = new ServerMessage(465u);
				Message.AppendUInt(@class.RoomId);
				Message.AppendStringWithBreak(@class.Name);
				Message.AppendStringWithBreak(@class.Description);
				Message.AppendInt32(@class.State);
				Message.AppendInt32(@class.Category);
				Message.AppendInt32(@class.UsersMax);
				Message.AppendInt32(100);
				Message.AppendInt32(@class.Tags.Count);
				foreach (string current in @class.Tags)
				{
					Message.AppendStringWithBreak(current);
				}
				Message.AppendInt32(@class.UsersWithRights.Count);
				foreach (uint current2 in @class.UsersWithRights)
				{
					Message.AppendUInt(current2);
					Message.AppendStringWithBreak(PhoenixEnvironment.GetGame().GetClientManager().GetNameById(current2));
				}
				Message.AppendInt32(@class.UsersWithRights.Count);
				Message.AppendBoolean(@class.AllowPet);
				Message.AppendBoolean(@class.AllowPetsEating);
				Message.AppendBoolean(@class.AllowWalkthrough);
				Message.AppendBoolean(@class.Hidewall);
				Message.AppendInt32(@class.Wallthick);
				Message.AppendInt32(@class.Floorthick);
				Session.SendMessage(Message);
			}
		}
	}
}
