using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
using Phoenix.HabboHotel.Groups;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class GetGuildFavorite : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			if (num > 0 && (Session != null && Session.GetHabbo() != null))
			{
				Session.GetHabbo().GroupID = num;
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE user_stats SET groupid = ",
						num,
						" WHERE Id = ",
						Session.GetHabbo().Id,
						" LIMIT 1;"
					}));
				}
				DataTable dataTable_ = Session.GetHabbo().GroupMemberships;
				if (dataTable_ != null)
				{
					ServerMessage Message = new ServerMessage(915u);
					Message.AppendInt32(dataTable_.Rows.Count);
					foreach (DataRow dataRow in dataTable_.Rows)
					{
                        Group class2 = GroupManager.GetGroup((int)dataRow["groupid"]);
						Message.AppendInt32(class2.Id);
						Message.AppendStringWithBreak(class2.Name);
						Message.AppendStringWithBreak(class2.Badge);
						if (Session.GetHabbo().GroupID == class2.Id)
						{
							Message.AppendBoolean(true);
						}
						else
						{
							Message.AppendBoolean(false);
						}
					}
					Session.SendMessage(Message);
					if (Session.GetHabbo().InRoom)
					{
						Room class14_ = Session.GetHabbo().CurrentRoom;
						RoomUser class3 = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
						ServerMessage Message2 = new ServerMessage(28u);
						Message2.AppendInt32(1);
						class3.Serialize(Message2);
						class14_.SendMessage(Message2, null);
                        Group class4 = GroupManager.GetGroup(Session.GetHabbo().GroupID);
						if (!class14_.list_17.Contains(class4))
						{
							class14_.list_17.Add(class4);
							ServerMessage Message3 = new ServerMessage(309u);
							Message3.AppendInt32(class14_.list_17.Count);
                            foreach (Group class2 in class14_.list_17)
							{
								Message3.AppendInt32(class2.Id);
								Message3.AppendStringWithBreak(class2.Badge);
							}
							class14_.SendMessage(Message3, null);
						}
						else
						{
                            foreach (Group current in class14_.list_17)
							{
								if (current == class4 && current.Badge != class4.Badge)
								{
									ServerMessage Message3 = new ServerMessage(309u);
									Message3.AppendInt32(class14_.list_17.Count);
                                    foreach (Group class2 in class14_.list_17)
									{
										Message3.AppendInt32(class2.Id);
										Message3.AppendStringWithBreak(class2.Badge);
									}
									class14_.SendMessage(Message3, null);
								}
							}
						}
					}
				}
			}
		}
	}
}
