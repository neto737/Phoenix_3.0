using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Messenger
{
	internal sealed class FindNewFriendsMessageEvent : MessageEvent
	{
		[CompilerGenerated]
		private static Func<Room, int> func_0;
		public void parse(GameClient Session, ClientMessage Event)
		{
			Dictionary<Room, int> dictionary = PhoenixEnvironment.GetGame().GetRoomManager().method_21();
			Room @class = null;
			IEnumerable<Room> arg_35_0 = dictionary.Keys;
			if (FindNewFriendsMessageEvent.func_0 == null)
			{
				FindNewFriendsMessageEvent.func_0 = new Func<Room, int>(FindNewFriendsMessageEvent.smethod_0);
			}
			IOrderedEnumerable<Room> orderedEnumerable = arg_35_0.OrderByDescending(FindNewFriendsMessageEvent.func_0);
			int num = 0;
			foreach (Room current in orderedEnumerable)
			{
				num++;
				string a = PhoenixEnvironment.GetRandomNumber(1, 5).ToString();
				if (a == "2")
				{
					goto IL_83;
				}
				if (num == orderedEnumerable.Count<Room>())
				{
					goto IL_83;
				}
				bool arg_A2_0 = true;
				IL_A2:
				if (arg_A2_0)
				{
					continue;
				}
				@class = current;
				break;
				IL_83:
				arg_A2_0 = (Session.GetHabbo().CurrentRoom == null || Session.GetHabbo().CurrentRoom == current);
				goto IL_A2;
			}
			if (@class == null)
			{
				ServerMessage Message = new ServerMessage(831u);
				Message.AppendBoolean(false);
				Session.SendMessage(Message);
			}
			else
			{
				ServerMessage Message2 = new ServerMessage(286u);
				Message2.AppendBoolean(@class.IsPublic);
				Message2.AppendUInt(@class.RoomId);
				Session.SendMessage(Message2);
				ServerMessage Message3 = new ServerMessage(831u);
				Message3.AppendBoolean(true);
				Session.SendMessage(Message3);
			}
		}
		[CompilerGenerated]
		private static int smethod_0(Room class14_0)
		{
			return class14_0.UserCount;
		}
	}
}
