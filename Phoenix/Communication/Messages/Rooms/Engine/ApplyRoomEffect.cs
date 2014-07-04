using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.Storage;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class ApplyRoomEffect : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				UserItem class2 = Session.GetHabbo().GetInventoryComponent().GetItem(Event.PopWiredUInt());
				if (class2 != null)
				{
					string text = "floor";
					if (class2.GetBaseItem().Name.ToLower().Contains("wallpaper"))
					{
						text = "wallpaper";
					}
					else
					{
						if (class2.GetBaseItem().Name.ToLower().Contains("landscape"))
						{
							text = "landscape";
						}
					}
					string text2 = text;
					if (text2 != null)
					{
						if (!(text2 == "floor"))
						{
							if (!(text2 == "wallpaper"))
							{
								if (text2 == "landscape")
								{
									@class.Landscape = class2.ExtraData;
								}
							}
							else
							{
								@class.Wallpaper = class2.ExtraData;
								if (Session.GetHabbo().CurrentQuestId == 11u)
								{
									PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(11u, Session);
								}
							}
						}
						else
						{
							@class.Floor = class2.ExtraData;
							if (Session.GetHabbo().CurrentQuestId == 13u)
							{
								PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(13u, Session);
							}
						}
					}
					using (DatabaseClient class3 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class3.AddParamWithValue("extradata", class2.ExtraData);
						class3.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE rooms SET ",
							text,
							" = @extradata WHERE Id = '",
							@class.RoomId,
							"' LIMIT 1"
						}));
					}
					Session.GetHabbo().GetInventoryComponent().RemoveItem(class2.Id, 0u, false);
					ServerMessage Message = new ServerMessage(46u);
					Message.AppendStringWithBreak(text);
					Message.AppendStringWithBreak(class2.ExtraData);
					@class.SendMessage(Message, null);
					PhoenixEnvironment.GetGame().GetRoomManager().method_18(@class.RoomId);
				}
			}
		}
	}
}
