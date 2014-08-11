using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Pets;
using Phoenix.Util;
using Phoenix.Messages;
using Phoenix.HabboHotel.RoomBots;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Pets
{
	internal sealed class PlacePetMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && (@class.AllowPet || @class.CheckRights(Session, true)))
			{
				uint uint_ = Event.PopWiredUInt();
				Pet class2 = Session.GetHabbo().GetInventoryComponent().GetPet(uint_);
				if (class2 != null && !class2.PlacedInRoom)
				{
					int num = Event.PopWiredInt32();
					int num2 = Event.PopWiredInt32();
					if (@class.CanWalk(num, num2, 0.0, true, false))
					{
						if (@class.Int32_2 >= GlobalClass.MaxPetsPerRoom)
						{
							Session.SendNotif(TextManager.GetText("error_maxpets") + GlobalClass.MaxPetsPerRoom);
						}
						else
						{
							class2.PlacedInRoom = true;
							class2.RoomId = @class.RoomId;
							List<RandomSpeech> list = new List<RandomSpeech>();
							List<BotResponse> list2 = new List<BotResponse>();
							@class.method_4(new RoomBot(class2.PetId, class2.RoomId, AIType.Pet, "freeroam", class2.Name, "", class2.Look, num, num2, 0, 0, 0, 0, 0, 0, ref list, ref list2, 0), class2);
							if (@class.CheckRights(Session, true))
							{
								Session.GetHabbo().GetInventoryComponent().MovePetToRoom(class2.PetId, @class.RoomId);
							}
						}
					}
				}
			}
		}
	}
}
