using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class IgnoreUserMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room class14_ = Session.GetHabbo().CurrentRoom;
			if (class14_ != null)
			{
				Event.PopWiredUInt();
				string string_ = Event.PopFixedString();
				RoomUser @class = class14_.GetRoomUserByHabbo(string_);
				if (@class != null && @class.GetClient().GetHabbo().Rank <= 2u)
				{
					uint uint_ = @class.GetClient().GetHabbo().Id;
					if (!Session.GetHabbo().MutedUsers.Contains(uint_))
					{
						Session.GetHabbo().MutedUsers.Add(uint_);
						using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
						{
							class2.ExecuteQuery(string.Concat(new object[]
							{
								"INSERT INTO user_ignores(user_id, ignore_id) VALUES (",
								Session.GetHabbo().Id,
								", ",
								uint_,
								");"
							}));
						}
						ServerMessage Message = new ServerMessage(419u);
						Message.AppendInt32(1);
						Session.SendMessage(Message);
					}
				}
			}
		}
	}
}
