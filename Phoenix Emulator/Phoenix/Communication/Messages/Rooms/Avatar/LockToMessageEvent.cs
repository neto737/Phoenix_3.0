using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.HabboHotel.Pathfinding;
namespace Phoenix.Communication.Messages.Rooms.Avatar
{
	internal sealed class LookToMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (room != null)
			{
				RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (roomUserByHabbo != null)
				{
					roomUserByHabbo.Unidle();
					int num = Event.PopWiredInt32();
					int num2 = Event.PopWiredInt32();
					if (num != roomUserByHabbo.X || num2 != roomUserByHabbo.Y)
					{
						int rotation = Rotation.Calculate(roomUserByHabbo.X, roomUserByHabbo.Y, num, num2);
						roomUserByHabbo.SetRot(rotation);
						if (roomUserByHabbo.Riding != null && roomUserByHabbo.Target != null)
						{
							roomUserByHabbo.Target.SetRot(rotation);
						}
					}
				}
			}
		}
	}
}
