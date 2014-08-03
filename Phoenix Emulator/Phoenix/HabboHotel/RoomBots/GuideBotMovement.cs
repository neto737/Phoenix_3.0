using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.RoomBots
{
	internal sealed class GuideBotMovement : BotAI
	{
		private int int_2;
		private int int_3;
		public GuideBotMovement(int int_4)
		{
			this.int_2 = new Random((int_4 ^ 2) + DateTime.Now.Millisecond).Next(10, 250);
			this.int_3 = new Random((int_4 ^ 2) + DateTime.Now.Millisecond).Next(10, 30);
		}
		public override void OnSelfEnterRoom()
		{
		}
		public override void OnSelfLeaveRoom(bool bool_0)
		{
		}
		public override void OnUserEnterRoom(RoomUser RoomUser_0)
		{
		}
		public override void OnUserLeaveRoom(GameClient Session)
		{
		}
		public override void OnUserSay(RoomUser RoomUser_0, string string_0)
		{
		}
		public override void OnUserShout(RoomUser RoomUser_0, string string_0)
		{
		}
		public override void OnTimerTick()
		{
			if (this.int_2 <= 0)
			{
				if (base.GetBotData().RandomSpeech.Count > 0)
				{
					RandomSpeech @class = base.GetBotData().GetRandomSpeech();
					base.GetRoomUser().Chat(null, @class.Message, @class.Shout);
				}
				this.int_2 = PhoenixEnvironment.GetRandomNumber(10, 300);
			}
			else
			{
				this.int_2--;
			}
			if (this.int_3 <= 0)
			{
				string text = base.GetBotData().WalkingMode.ToLower();
				if (text != null && !(text == "stand"))
				{
					if (!(text == "freeroam"))
					{
						if (text == "specified_range")
						{
							int int_ = PhoenixEnvironment.GetRandomNumber(base.GetBotData().minX, base.GetBotData().maxX);
							int int_2 = PhoenixEnvironment.GetRandomNumber(base.GetBotData().minY, base.GetBotData().maxY);
							base.GetRoomUser().MoveTo(int_, int_2);
						}
					}
					else
					{
						int int_ = PhoenixEnvironment.GetRandomNumber(0, base.GetRoom().Model.MapSizeX);
						int int_2 = PhoenixEnvironment.GetRandomNumber(0, base.GetRoom().Model.MapSizeY);
						base.GetRoomUser().MoveTo(int_, int_2);
					}
				}
				this.int_3 = PhoenixEnvironment.GetRandomNumber(1, 30);
			}
			else
			{
				this.int_3--;
			}
		}
	}
}
