using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.HabboHotel.Groups;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class GetHabboGroupBadgesMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().LoadingRoom > 0u)
			{
				Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().LoadingRoom);
				if (@class != null && Session.GetHabbo().GroupID > 0)
				{
                    Group class2 = GroupManager.GetGroup(Session.GetHabbo().GroupID);
					if (class2 != null && !@class.list_17.Contains(class2))
					{
						@class.list_17.Add(class2);
						ServerMessage Message = new ServerMessage(309u);
						Message.AppendInt32(@class.list_17.Count);
                        foreach (Group current in @class.list_17)
						{
							Message.AppendInt32(current.Id);
							Message.AppendStringWithBreak(current.Badge);
						}
						@class.SendMessage(Message, null);
					}
					else
					{
                        foreach (Group current2 in @class.list_17)
						{
							if (current2 == class2 && current2.Badge != class2.Badge)
							{
								ServerMessage Message = new ServerMessage(309u);
								Message.AppendInt32(@class.list_17.Count);
                                foreach (Group current in @class.list_17)
								{
									Message.AppendInt32(current.Id);
									Message.AppendStringWithBreak(current.Badge);
								}
								@class.SendMessage(Message, null);
							}
						}
					}
				}
				if (@class != null && @class.list_17.Count > 0)
				{
					ServerMessage Message = new ServerMessage(309u);
					Message.AppendInt32(@class.list_17.Count);
                    foreach (Group current in @class.list_17)
					{
						Message.AppendInt32(current.Id);
						Message.AppendStringWithBreak(current.Badge);
					}
					Session.SendMessage(Message);
				}
			}
		}
	}
}
