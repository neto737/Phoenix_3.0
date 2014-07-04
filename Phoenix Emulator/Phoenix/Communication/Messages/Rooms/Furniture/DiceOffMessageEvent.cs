using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Furniture
{
	internal sealed class DiceOffMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null)
				{
					RoomItem class2 = @class.GetItem(Event.PopWiredUInt());
					if (class2 != null)
					{
						bool bool_ = false;
						if (@class.CheckRights(Session))
						{
							bool_ = true;
						}
						class2.Interactor.OnTrigger(Session, class2, -1, bool_);
					}
				}
			}
			catch
			{
			}
		}
	}
}
