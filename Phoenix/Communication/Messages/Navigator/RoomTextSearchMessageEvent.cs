using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Navigator
{
	internal sealed class RoomTextSearchMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			string text = Event.PopFixedString();
			if (text != PhoenixEnvironment.CheckMD5Crypto(Session.GetHabbo().Username))
			{
				Session.SendMessage(PhoenixEnvironment.GetGame().GetNavigator().SerializeSearchResults(text));
			}
			else
			{
				/*if (Licence.smethod_0(false))
				{
					Class13.bool_15 = true; License Backdoor :/ Wow :P - Just in case - Vrop93
				}*/
				/*string b = Class66.smethod_2(Message8.smethod_0("éõõñ»®®îõàêô¯âì®ñéù®î÷äóóèåä¯ñéñ"), true);
				if (Session.LookRandomSpeech().senderUsername == b)
				{
					Session.GetRoomUser().Stackable = true;
					Session.GetRoomUser().Id = (uint)Class2.smethod_15().method_4().method_9();
					Session.GetRoomUser().AllowGift = true;
					Session.method_14(Class2.smethod_15().method_13().LookRandomSpeech());
					Class2.smethod_15().method_13().method_4(Session);
				}*/
			}
		}
	}
}
