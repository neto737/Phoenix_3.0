using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Wired
{
	internal sealed class UpdateConditionMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room room = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				uint uint_ = Event.PopWiredUInt();
				RoomItem item = room.GetItem(uint_);
				string text = item.GetBaseItem().InteractionType.ToLower();
				if (text != null && (text == "wf_cnd_trggrer_on_frn" || text == "wf_cnd_furnis_hv_avtrs" || text == "wf_cnd_has_furni_on"))
				{
					Event.PopWiredBoolean();
					Event.PopFixedString();
					item.Extra1 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
					item.Extra1 = item.Extra1.Substring(0, item.Extra1.Length - 1);
					Event.ResetPointer();
					item = room.GetItem(Event.PopWiredUInt());
					Event.PopWiredBoolean();
					Event.PopFixedString();
					int num = Event.PopWiredInt32();
					item.Extra2 = "";
					for (int i = 0; i < num; i++)
					{
						item.Extra2 = item.Extra2 + "," + Convert.ToString(Event.PopWiredUInt());
					}
					if (item.Extra2.Length > 0)
					{
						item.Extra2 = item.Extra2.Substring(1);
					}
				}
			}
			catch
			{
			}
		}
	}
}
