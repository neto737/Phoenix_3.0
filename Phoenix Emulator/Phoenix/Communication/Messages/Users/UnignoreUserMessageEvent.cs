using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class UnignoreUserMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room class14_ = Session.GetHabbo().CurrentRoom;
			if (class14_ != null)
			{
				Event.PopWiredUInt();
				string string_ = Event.PopFixedString();
				RoomUser @class = class14_.GetRoomUserByHabbo(string_);
				if (@class != null)
				{
					uint uint_ = @class.GetClient().GetHabbo().Id;
					if (Session.GetHabbo().MutedUsers.Contains(uint_))
					{
						Session.GetHabbo().MutedUsers.Remove(uint_);
						using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
						{
							class2.ExecuteQuery(string.Concat(new object[]
							{
								"DELETE FROM user_ignores WHERE user_id = ",
								Session.GetHabbo().Id,
								" AND ignore_id = ",
								uint_,
								" LIMIT 1;"
							}));
						}
						ServerMessage Message = new ServerMessage(419u);
						Message.AppendInt32(3);
						Session.SendMessage(Message);
					}
				}
			}
		}
	}
}
