using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class SetObjectDataMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session))
			{
				int num = Event.PopWiredInt32();
				int num2 = Event.PopWiredInt32();
				string text = Event.PopFixedString();
				string text2 = Event.PopFixedString();
				string text3 = Event.PopFixedString();
				string text4 = Event.PopFixedString();
				string text5 = Event.PopFixedString();
				string text6 = Event.PopFixedString();
				string text7 = Event.PopFixedString();
				string text8 = Event.PopFixedString();
				string text9 = Event.PopFixedString();
				string text10 = Event.PopFixedString();
				string text11 = "";
				if (num2 == 10 || num2 == 8)
				{
					text11 = string.Concat(new object[]
					{
						text,
						"=",
						text2,
						Convert.ToChar(9),
						text3,
						"=",
						text4,
						Convert.ToChar(9),
						text5,
						"=",
						text6,
						Convert.ToChar(9),
						text7,
						"=",
						text8
					});
					if (text9 != "")
					{
						text11 = string.Concat(new object[]
						{
							text11,
							Convert.ToChar(9),
							text9,
							"=",
							text10
						});
					}
					using (DatabaseClient class2 = PhoenixEnvironment.GetDatabase().GetClient())
					{
						class2.AddParamWithValue("extradata", text11);
						class2.ExecuteQuery("UPDATE items SET extra_data = @extradata WHERE Id = '" + num + "' LIMIT 1");
					}
					ServerMessage Message = new ServerMessage(88u);
					Message.AppendStringWithBreak(num.ToString());
					Message.AppendStringWithBreak(text11);
					@class.SendMessage(Message, null);
					@class.GetItem((uint)num).ExtraData = text11;
					@class.GetItem((uint)num).UpdateState(true, false);
				}
			}
		}
	}
}
