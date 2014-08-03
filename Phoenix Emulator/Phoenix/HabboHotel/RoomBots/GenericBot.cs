using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.RoomBots
{
	internal sealed class GenericBot : BotAI
	{
		private int SpeechTimer;
		private int ActionTimer;

		public GenericBot(int VirtualId)
		{
			this.SpeechTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 250);
			this.ActionTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 30);
		}
        public override void OnSelfEnterRoom() { }
        public override void OnSelfLeaveRoom(bool Kicked) { }
        public override void OnUserEnterRoom(RoomUser User) { }
        public override void OnUserLeaveRoom(GameClient Session) { }

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

        public override void OnUserShout(RoomUser User, string Message) { }

		public override void OnTimerTick()
		{
			if (this.SpeechTimer <= 0)
			{
				if (base.GetBotData().RandomSpeech.Count > 0)
				{
					RandomSpeech Speech = base.GetBotData().GetRandomSpeech();
					base.GetRoomUser().Chat(null, Speech.Message, Speech.Shout);
				}
				this.SpeechTimer = PhoenixEnvironment.GetRandomNumber(10, 300);
			}
			else
			{
				this.SpeechTimer--;
			}

            if (this.ActionTimer <= 0)
            {
                int randomX = 0;
                int randomY = 0;

                switch (GetBotData().WalkingMode.ToLower())
                {
                    default:
                    case "stand":
                        // (8) Why is my life so boring?
                        break;

                    case "freeroam":
                        randomX = PhoenixEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeX);
                        randomY = PhoenixEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeY);
                        GetRoomUser().MoveTo(randomX, randomY);
                        break;

                    case "specified_range":
                        randomX = PhoenixEnvironment.GetRandomNumber(GetBotData().minX, GetBotData().maxX);
                        randomY = PhoenixEnvironment.GetRandomNumber(GetBotData().minY, GetBotData().maxY);
                        GetRoomUser().MoveTo(randomX, randomY);
                        break;
                }
                ActionTimer = PhoenixEnvironment.GetRandomNumber(1, 30);
            }
            else
            {
                this.ActionTimer--;
            }
		}
	}
}
