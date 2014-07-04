using System;
using System.Linq;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Engine
{
	internal sealed class SetClothingChangeDataMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session))
			{
				uint num = Event.PopWiredUInt();
				string a = Event.PopFixedString().ToUpper();
				string text = PhoenixEnvironment.FilterInjectionChars(Event.PopFixedString());
				text = text.Replace("hd-99999-99999", "");
				text += ".";
				RoomItem class2 = @class.Hashtable_0[num] as RoomItem;
				if (class2.ExtraData.Contains(','))
				{
					class2.Extra1 = class2.ExtraData.Split(new char[]
					{
						','
					})[0];
					class2.Extra2 = class2.ExtraData.Split(new char[]
					{
						','
					})[1];
				}
				if (a == "M")
				{
					class2.Extra1 = text;
				}
				else
				{
					class2.Extra2 = text;
				}
				class2.ExtraData = class2.Extra1 + "," + class2.Extra2;
				class2.UpdateState(true, true);
			}
		}
	}
}
