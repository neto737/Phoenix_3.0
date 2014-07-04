using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Furniture
{
	internal sealed class UseFurnitureMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (room != null)
				{
					RoomItem item = room.GetItem(Event.PopWiredUInt());
					if (item != null)
					{
						bool userHasRights = false;
						if (room.CheckRights(Session))
						{
							userHasRights = true;
						}
						item.Interactor.OnTrigger(Session, item, Event.PopWiredInt32(), userHasRights);
						if (Session.GetHabbo().CurrentQuestId == 12)
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(12, Session);
						}
						else if (Session.GetHabbo().CurrentQuestId == 18 && item.GetBaseItem().Name == "bw_lgchair")
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(18, Session);
						}
						else if (Session.GetHabbo().CurrentQuestId == 20 && item.GetBaseItem().Name.Contains("bw_sboard"))
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(20, Session);
						}
						else if (Session.GetHabbo().CurrentQuestId == 21 && item.GetBaseItem().Name.Contains("bw_van"))
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(21, Session);
						}
						else if (Session.GetHabbo().CurrentQuestId == 22 && item.GetBaseItem().Name.Contains("party_floor"))
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(22, Session);
						}
						else if (Session.GetHabbo().CurrentQuestId == 23 && item.GetBaseItem().Name.Contains("party_ball"))
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(23, Session);
						}
						else if (Session.GetHabbo().CurrentQuestId == 24 && item.GetBaseItem().Name.Contains("jukebox"))
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(24, Session);
						}
					}
				}
			}
            catch
            {
            }
		}
	}
}
