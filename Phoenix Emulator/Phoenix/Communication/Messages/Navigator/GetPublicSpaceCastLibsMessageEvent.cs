using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class GetPublicSpaceCastLibsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint Id = Event.PopWiredUInt();
			Event.PopFixedString();
			Event.PopWiredInt32();
            RoomData Data = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(Id);
			if (Data != null)
			{
				if (Data.Type == "private")
				{
					ServerMessage Message = new ServerMessage(286);
					Message.AppendBoolean(Data.IsPublicRoom);
					Message.AppendUInt(Id);
					Session.SendMessage(Message);
				}
				else
				{
					ServerMessage Message2 = new ServerMessage(453);
					Message2.AppendUInt(Data.Id);
					Message2.AppendStringWithBreak(Data.CCTs);
					Message2.AppendUInt(Data.Id);
					Session.SendMessage(Message2);
				}
			}
		}
	}
}
