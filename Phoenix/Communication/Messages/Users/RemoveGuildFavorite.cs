using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
using Phoenix.HabboHotel.Guilds;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class RemoveGuildFavorite : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			if (num > 0 && (Session != null && Session.GetHabbo() != null))
			{
				Session.GetHabbo().GroupID = 0;
				if (Session.GetHabbo().InRoom)
				{
					Room class14_ = Session.GetHabbo().CurrentRoom;
					RoomUser @class = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
					ServerMessage Message = new ServerMessage(28u);
					Message.AppendInt32(1);
					@class.Serialize(Message);
					class14_.SendMessage(Message, null);
				}
				using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
				{
					class2.ExecuteQuery("UPDATE user_stats SET groupid = 0 WHERE Id = " + Session.GetHabbo().Id + " LIMIT 1;");
				}
				DataTable dataTable_ = Session.GetHabbo().GroupMemberships;
				if (dataTable_ != null)
				{
					ServerMessage Message2 = new ServerMessage(915u);
					Message2.AppendInt32(dataTable_.Rows.Count);
					foreach (DataRow dataRow in dataTable_.Rows)
					{
                        Guild class3 = GuildManager.GetGuild((int)dataRow["groupid"]);
						Message2.AppendInt32(class3.Id);
						Message2.AppendStringWithBreak(class3.Name);
						Message2.AppendStringWithBreak(class3.Badge);
						if (Session.GetHabbo().GroupID == class3.Id)
						{
							Message2.AppendBoolean(true);
						}
						else
						{
							Message2.AppendBoolean(false);
						}
					}
					Session.SendMessage(Message2);
				}
			}
		}
	}
}
