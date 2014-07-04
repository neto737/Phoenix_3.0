using System;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Rooms.Chat
{
	internal sealed class WhisperMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (room != null && Session != null)
			{
				if (Session.GetHabbo().Muted)
				{
					Session.SendNotif(TextManager.GetText("error_muted"));
				}
				else
				{
					if (Session.GetHabbo().HasRole("ignore_roommute") || !room.RoomMuted)
					{
						string str = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
						string name = str.Split(new char[]
						{
							' '
						})[0];
						string s = str.Substring(name.Length + 1);
						s = ChatCommandHandler.ApplyWordFilter(s);
						RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
						RoomUser user2 = room.GetRoomUserByHabbo(name);
						if (Session.GetHabbo().MaxFloodTime() > 0)
						{
							TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().FloodTime;
							if (timeSpan.Seconds > 4)
							{
								Session.GetHabbo().FloodCount = 0;
							}
							if (timeSpan.Seconds < 4 && Session.GetHabbo().FloodCount > 5 && !roomUserByHabbo.IsBot)
							{
								ServerMessage Message = new ServerMessage(27);
								Message.AppendInt32(Session.GetHabbo().MaxFloodTime());
								Session.SendMessage(Message);
								Session.GetHabbo().Muted = true;
								Session.GetHabbo().MuteLength = Session.GetHabbo().MaxFloodTime();
								return;
							}
							Session.GetHabbo().FloodTime = DateTime.Now;
							Session.GetHabbo().FloodCount++;
						}
						ServerMessage Message2 = new ServerMessage(25);
						Message2.AppendInt32(roomUserByHabbo.VirtualId);
						Message2.AppendStringWithBreak(s);
						Message2.AppendBoolean(false);
						if (roomUserByHabbo != null && !roomUserByHabbo.IsBot)
						{
							roomUserByHabbo.GetClient().SendMessage(Message2);
						}
						roomUserByHabbo.Unidle();
						if (user2 != null && !user2.IsBot && (user2.GetClient().GetHabbo().MutedUsers.Count <= 0 || !user2.GetClient().GetHabbo().MutedUsers.Contains(Session.GetHabbo().Id)))
						{
							user2.GetClient().SendMessage(Message2);
							if (GlobalClass.RecordChatlogs && !Session.GetHabbo().isAaron)
							{
								using (DatabaseClient client = PhoenixEnvironment.GetDatabase().GetClient())
								{
									client.AddParamWithValue("message", "<Whisper to " + user2.GetClient().GetHabbo().Username + ">: " + s);
									client.ExecuteQuery(string.Concat(new object[]
									{
										"INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
										Session.GetHabbo().Id,
										"','",
										room.RoomId,
										"','",
										DateTime.Now.Hour,
										"','",
										DateTime.Now.Minute,
										"',UNIX_TIMESTAMP(),@message,'",
										Session.GetHabbo().Username,
										"','",
										DateTime.Now.ToLongDateString(),
										"')"
									}));
								}
							}
						}
					}
				}
			}
		}
	}
}
