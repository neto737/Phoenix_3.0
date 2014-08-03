using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Util;
namespace Phoenix.HabboHotel.RoomBots
{
	internal sealed class GuideBot : BotAI
	{
		private int SpeechTimer;
		private int ActionTimer;

		public GuideBot()
		{
			this.SpeechTimer = 0;
			this.ActionTimer = 0;
		}

		public override void OnSelfEnterRoom()
		{
			base.GetRoomUser().Chat(null, TextManager.GetText("guidebot_welcome1"), true);
			base.GetRoomUser().Chat(null, TextManager.GetText("guidebot_welcome2"), false);
		}

        public override void OnSelfLeaveRoom(bool Kicked) { }
        public override void OnUserEnterRoom(RoomUser User) { }

		public override void OnUserLeaveRoom(GameClient Session)
		{
			if (base.GetRoom().Owner.ToLower() == Session.GetHabbo().Username.ToLower())
			{
				base.GetRoom().RemoveBot(base.GetRoomUser().VirtualId, false);
			}
		}

        public override void OnUserSay(RoomUser User, string msg)
        {
            if (base.GetRoom().TileDistance(base.GetRoomUser().X, base.GetRoomUser().Y, User.X, User.Y) > 8)
            {
                return;
            }

            BotResponse Response = base.GetBotData().GetResponse(msg);

            if (Response == null)
            {
                return;
            }

            switch (Response.ResponseType.ToLower())
            {
                case "say":
                    GetRoomUser().Chat(null, Response.ResponseText, false);
                    break;

                case "shout":
                    GetRoomUser().Chat(null, Response.ResponseText, true);
                    break;

                case "whisper":
                    ServerMessage TellMsg = new ServerMessage(25);
                    TellMsg.AppendInt32(GetRoomUser().VirtualId);
                    TellMsg.AppendStringWithBreak(Response.ResponseText);
                    TellMsg.AppendBoolean(false);

                    User.GetClient().SendMessage(TellMsg);
                    break;
            }

            if (Response.ServeId >= 1)
            {
                User.CarryItem(Response.ServeId);
            }
        }

        public override void OnUserShout(RoomUser RoomUser_0, string string_0) { }

		public override void OnTimerTick()
		{
			if (this.SpeechTimer <= 0)
			{
				if (base.GetBotData() != null && base.GetBotData().RandomSpeech.Count > 0)
				{
					RandomSpeech Speech = base.GetBotData().GetRandomSpeech();
					GetRoomUser().Chat(null, Speech.Message, Speech.Shout);
				}
				this.SpeechTimer = PhoenixEnvironment.GetRandomNumber(0, 150);
			}
			else
			{
				this.SpeechTimer--;
			}

			if (this.ActionTimer <= 0)
			{
				int randomX = PhoenixEnvironment.GetRandomNumber(0, base.GetRoom().Model.MapSizeX);
				int randomY = PhoenixEnvironment.GetRandomNumber(0, base.GetRoom().Model.MapSizeY);
				base.GetRoomUser().MoveTo(randomX, randomY);
				this.ActionTimer = PhoenixEnvironment.GetRandomNumber(0, 30);
			}
			else
			{
				this.ActionTimer--;
			}
		}
	}
}
