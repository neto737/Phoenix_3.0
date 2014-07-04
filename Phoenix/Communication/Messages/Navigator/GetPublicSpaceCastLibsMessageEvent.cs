using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class GetPublicSpaceCastLibsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Event.PopFixedString();
			Event.PopWiredInt32();
            RoomData @class = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(num);
			if (@class != null)
			{
				if (@class.Type == "private")
				{
					ServerMessage Message = new ServerMessage(286u);
					Message.AppendBoolean(@class.IsPublicRoom);
					Message.AppendUInt(num);
					Session.SendMessage(Message);
				}
				else
				{
					ServerMessage Message2 = new ServerMessage(453u);
					Message2.AppendUInt(@class.Id);
					Message2.AppendStringWithBreak(@class.CCTs);
					Message2.AppendUInt(@class.Id);
					Session.SendMessage(Message2);
				}
			}
		}
	}
}
