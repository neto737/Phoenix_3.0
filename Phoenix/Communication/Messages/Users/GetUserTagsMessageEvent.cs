using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class GetUserTagsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				RoomUser class2 = @class.GetRoomUserByHabbo(Event.PopWiredUInt());
				if (class2 != null && !class2.IsBot)
				{
					ServerMessage Message = new ServerMessage(350u);
					Message.AppendUInt(class2.GetClient().GetHabbo().Id);
					Message.AppendInt32(class2.GetClient().GetHabbo().list_3.Count);
					using (TimedLock.Lock(class2.GetClient().GetHabbo().list_3))
					{
						foreach (string current in class2.GetClient().GetHabbo().list_3)
						{
							Message.AppendStringWithBreak(current);
						}
					}
					Session.SendMessage(Message);
				}
			}
		}
	}
}
