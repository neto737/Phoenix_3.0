using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Groups;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class LoadUserGroupsEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			DataTable dataTable_ = Session.GetHabbo().GroupMemberships;
			if (dataTable_ != null)
			{
				ServerMessage Message = new ServerMessage(915u);
				Message.AppendInt32(dataTable_.Rows.Count);
				foreach (DataRow dataRow in dataTable_.Rows)
				{
                    Group @class = GroupManager.GetGroup((int)dataRow["groupid"]);
					Message.AppendInt32(@class.Id);
					Message.AppendStringWithBreak(@class.Name);
					Message.AppendStringWithBreak(@class.Badge);
					if (Session.GetHabbo().GroupID == @class.Id) // is favorite group?
					{
						Message.AppendBoolean(true);
					}
					else
					{
						Message.AppendBoolean(false);
					}
				}
				Session.SendMessage(Message);
			}
		}
	}
}
