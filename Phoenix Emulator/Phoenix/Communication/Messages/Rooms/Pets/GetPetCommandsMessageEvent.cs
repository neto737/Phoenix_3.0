using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Pets
{
	internal sealed class GetPetCommandsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			RoomUser class2 = @class.method_48(num);
			if (class2 != null && class2.PetData != null)
			{
				ServerMessage Message = new ServerMessage(605u);
				Message.AppendUInt(num);
				int i = class2.PetData.Level;
				Message.AppendInt32(18);
				Message.AppendInt32(0);
				Message.AppendInt32(1);
				Message.AppendInt32(2);
				Message.AppendInt32(3);
				Message.AppendInt32(4);
				Message.AppendInt32(17);
				Message.AppendInt32(5);
				Message.AppendInt32(6);
				Message.AppendInt32(7);
				Message.AppendInt32(8);
				Message.AppendInt32(9);
				Message.AppendInt32(10);
				Message.AppendInt32(11);
				Message.AppendInt32(12);
				Message.AppendInt32(13);
				Message.AppendInt32(14);
				Message.AppendInt32(15);
				Message.AppendInt32(16);
				int num2 = 0;
				while (i > num2)
				{
					num2++;
					Message.AppendInt32(num2);
				}
				Message.AppendInt32(0);
				Message.AppendInt32(1);
				Message.AppendInt32(2);
				Session.SendMessage(Message);
			}
		}
	}
}
