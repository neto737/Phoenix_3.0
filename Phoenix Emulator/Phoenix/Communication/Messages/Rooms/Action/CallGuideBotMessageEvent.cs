using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.RoomBots;
using Phoenix.HabboHotel.Rooms;
using Phoenix.HabboHotel.Pathfinding;
namespace Phoenix.Communication.Messages.Rooms.Action
{
	internal sealed class CallGuideBotMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CheckRights(Session, true))
			{
				for (int i = 0; i < @class.UserList.Length; i++)
				{
					RoomUser class2 = @class.UserList[i];
					if (class2 != null && (class2.IsBot && class2.BotData.AiType == AIType.Guide))
					{
						ServerMessage Message = new ServerMessage(33u);
						Message.AppendInt32(4009);
						Session.SendMessage(Message);
						return;
					}
				}
				if (Session.GetHabbo().CalledGuideBot)
				{
					ServerMessage Message = new ServerMessage(33u);
					Message.AppendInt32(4010);
					Session.SendMessage(Message);
				}
				else
				{
					RoomUser class3 = @class.method_3(PhoenixEnvironment.GetGame().GetBotManager().GetBot(2u));
					class3.SetPos(@class.Model.DoorX, @class.Model.DoorY, @class.Model.DoorZ);
					class3.UpdateNeeded = true;
					RoomUser class4 = @class.GetRoomUserByHabbo(@class.Owner);
					if (class4 != null)
					{
						class3.MoveTo(class4.Coordinate);
						class3.SetRot(Rotation.Calculate(class3.X, class3.Y, class4.X, class4.Y));
					}
					PhoenixEnvironment.GetGame().GetAchievementManager().UnlockAchievement(Session, 6u, 1);
					Session.GetHabbo().CalledGuideBot = true;
				}
			}
		}
	}
}
