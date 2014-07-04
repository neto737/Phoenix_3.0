using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Util;
namespace Phoenix.Communication.Messages.Catalog
{
	internal sealed class GetSellablePetBreedsEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(827u);
			string text = Event.ToString().Split(new char[]
			{
				' '
			})[1];
			if (text.ToLower().Contains("pet"))
			{
				int num = Convert.ToInt32(text.Substring(3));
				Message.AppendStringWithBreak("a0 pet" + num);
				switch (num)
				{
				case 0:
					Message.AppendString(TextManager.GetText("pet_breeds_0"));
					break;
				case 1:
					Message.AppendString(TextManager.GetText("pet_breeds_1"));
					break;
				case 2:
					Message.AppendString(TextManager.GetText("pet_breeds_2"));
					break;
				case 3:
					Message.AppendString(TextManager.GetText("pet_breeds_3"));
					break;
				case 4:
					Message.AppendString(TextManager.GetText("pet_breeds_4"));
					break;
				case 5:
					Message.AppendString(TextManager.GetText("pet_breeds_5"));
					break;
				case 6:
					Message.AppendString(TextManager.GetText("pet_breeds_6"));
					break;
				case 7:
					Message.AppendString(TextManager.GetText("pet_breeds_7"));
					break;
				case 8:
					Message.AppendString(TextManager.GetText("pet_breeds_8"));
					break;
				case 9:
					Message.AppendString(TextManager.GetText("pet_breeds_9"));
					break;
				case 10:
					Message.AppendString(TextManager.GetText("pet_breeds_10"));
					break;
				case 11:
					Message.AppendString(TextManager.GetText("pet_breeds_11"));
					break;
				case 12:
					Message.AppendString(TextManager.GetText("pet_breeds_12"));
					break;
				case 13:
					Message.AppendString(TextManager.GetText("pet_breeds_13"));
					break;
				case 14:
					Message.AppendString(TextManager.GetText("pet_breeds_14"));
					break;
				case 15:
					Message.AppendString(TextManager.GetText("pet_breeds_15"));
					break;
				case 16:
					Message.AppendString(TextManager.GetText("pet_breeds_16"));
					break;
				case 17:
					Message.AppendString(TextManager.GetText("pet_breeds_17"));
					break;
				case 18:
					Message.AppendString(TextManager.GetText("pet_breeds_18"));
					break;
				case 19:
					Message.AppendString(TextManager.GetText("pet_breeds_19"));
					break;
				case 20:
					Message.AppendString(TextManager.GetText("pet_breeds_20"));
					break;
				case 21:
					Message.AppendString(TextManager.GetText("pet_breeds_21"));
					break;
				case 22:
					Message.AppendString(TextManager.GetText("pet_breeds_22"));
					break;
				case 23:
					Message.AppendString(TextManager.GetText("pet_breeds_23"));
					break;
				case 24:
					Message.AppendString(TextManager.GetText("pet_breeds_24"));
					break;
				case 25:
					Message.AppendString(TextManager.GetText("pet_breeds_25"));
					break;
				case 26:
					Message.AppendString(TextManager.GetText("pet_breeds_26"));
					break;
				case 27:
					Message.AppendString(TextManager.GetText("pet_breeds_27"));
					break;
				case 28:
					Message.AppendString(TextManager.GetText("pet_breeds_28"));
					break;
				case 29:
					Message.AppendString(TextManager.GetText("pet_breeds_29"));
					break;
				case 30:
					Message.AppendString(TextManager.GetText("pet_breeds_30"));
					break;
				}
				Session.SendMessage(Message);
			}
		}
	}
}
