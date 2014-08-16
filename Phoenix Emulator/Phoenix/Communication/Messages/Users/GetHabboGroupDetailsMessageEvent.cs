using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
using Phoenix.HabboHotel.Guilds;
namespace Phoenix.Communication.Messages.Users
{
	internal sealed class GetHabboGroupDetailsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			if (num > 0 && (Session != null && Session.GetHabbo() != null))
			{
                Guild @class = GuildManager.GetGuild(num);
				if (@class != null)
				{
					ServerMessage Message = new ServerMessage(311u);
					Message.AppendInt32(@class.Id);
					Message.AppendStringWithBreak(@class.Name);
					Message.AppendStringWithBreak(@class.Desc);
					Message.AppendStringWithBreak(@class.Badge);
					if (@class.RoomId > 0u)
					{
						Message.AppendUInt(@class.RoomId);
						if (PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(@class.RoomId) != null)
						{
							Message.AppendStringWithBreak(PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(@class.RoomId).Name);
							goto IL_15A;
						}
						using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
						{
							try
							{
								DataRow dataRow_ = class2.ReadDataRow("SELECT * FROM rooms WHERE Id = " + @class.RoomId + " LIMIT 1;");
								string string_ = PhoenixEnvironment.GetGame().GetRoomManager().FetchRoomData(@class.RoomId, dataRow_).Name;
								Message.AppendStringWithBreak(string_);
							}
							catch
							{
								Message.AppendInt32(-1);
								Message.AppendStringWithBreak("");
							}
							goto IL_15A;
						}
					}
					Message.AppendInt32(-1);
					Message.AppendStringWithBreak("");
					IL_15A:
					bool flag = false;
					foreach (DataRow dataRow in Session.GetHabbo().GroupMemberships.Rows)
					{
						if ((int)dataRow["groupid"] == @class.Id)
						{
							flag = true;
						}
					}
					if (Session.GetHabbo().list_0.Contains(@class.Id))
					{
						Message.AppendInt32(2);
					}
					else
					{
						if (flag)
						{
							Message.AppendInt32(1);
						}
						else
						{
							if (@class.Locked == "closed")
							{
								Message.AppendInt32(1);
							}
							else
							{
								if (@class.List.Contains((int)Session.GetHabbo().Id))
								{
									Message.AppendInt32(1);
								}
								else
								{
									Message.AppendInt32(0);
								}
							}
						}
					}
					Message.AppendInt32(@class.List.Count);
					if (Session.GetHabbo().GroupID == @class.Id)
					{
						Message.AppendBoolean(true);
					}
					else
					{
						Message.AppendBoolean(false);
					}
					Session.SendMessage(Message);
				}
			}
		}
	}
}
