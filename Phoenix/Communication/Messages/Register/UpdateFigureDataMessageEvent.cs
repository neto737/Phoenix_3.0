using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Register
{
	internal sealed class UpdateFigureDataMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			string Gender = Event.PopFixedString().ToUpper();
			string Look = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());

			Room room = Session.GetHabbo().CurrentRoom;
			if (room != null)
			{
				RoomUser User = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (User != null)
				{
					User.ChangedClothes = "";
					if (Session.GetHabbo().MaxFloodTime() > 0)
					{
						TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().FloodTime;
						if (timeSpan.Seconds > 4)
						{
							Session.GetHabbo().FloodCount = 0;
						}
						if (timeSpan.Seconds < 4 && Session.GetHabbo().FloodCount > 5)
						{
							ServerMessage Message = new ServerMessage(27);
							Message.AppendInt32(Session.GetHabbo().MaxFloodTime());
							Session.SendMessage(Message);
							return;
						}
						Session.GetHabbo().FloodTime = DateTime.Now;
						Session.GetHabbo().FloodCount++;
					}
					if (Session.GetHabbo().CurrentQuestId == 2)
					{
						PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(2, Session);
					}
					Session.GetHabbo().Look = Look;
					Session.GetHabbo().Gender = Gender.ToLower();
					using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
					{
						adapter.AddParamWithValue("look", Look);
						adapter.AddParamWithValue("gender", Gender);
						adapter.ExecuteQuery("UPDATE users SET look = @look, gender = @gender WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1;");
					}
					ServerMessage Message2 = new ServerMessage(266);
					Message2.AppendInt32(-1);
					Message2.AppendStringWithBreak(Session.GetHabbo().Look);
					Message2.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
					Message2.AppendStringWithBreak(Session.GetHabbo().Motto);
					Message2.AppendInt32(Session.GetHabbo().AchievementScore);
					Message2.AppendStringWithBreak("");
					Session.SendMessage(Message2);
					ServerMessage Message3 = new ServerMessage(266);
					Message3.AppendInt32(User.VirtualId);
					Message3.AppendStringWithBreak(Session.GetHabbo().Look);
					Message3.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
					Message3.AppendStringWithBreak(Session.GetHabbo().Motto);
					Message3.AppendInt32(Session.GetHabbo().AchievementScore);
					Message3.AppendStringWithBreak("");
					room.SendMessage(Message3, null);
					PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 1, 1);
				}
			}
		}
	}
}
