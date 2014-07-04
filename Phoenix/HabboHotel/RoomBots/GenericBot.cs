using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.HabboHotel.RoomBots
{
	internal sealed class GenericBot : BotAI
	{
		private int int_2;
		private int int_3;
		public GenericBot(int int_4)
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
			if (base.method_1().method_100(base.GetRoomUser().X, base.GetRoomUser().Y, RoomUser_0.X, RoomUser_0.Y) <= 8)
			{
				BotResponse @class = base.method_3().method_2(string_0);
				if (@class != null)
				{
					string text = base.method_1().VariablePhxMagic(RoomUser_0, @class.Response);
					string text2 = @class.ResponseType.ToLower();
					if (text2 != null)
					{
						if (!(text2 == "say"))
						{
							if (!(text2 == "shout"))
							{
								if (text2 == "whisper")
								{
									ServerMessage Message = new ServerMessage(25u);
									Message.AppendInt32(base.GetRoomUser().VirtualId);
									Message.AppendStringWithBreak(text);
									Message.AppendBoolean(false);
									RoomUser_0.GetClient().SendMessage(Message);
								}
							}
							else
							{
								base.GetRoomUser().Chat(null, text, true);
							}
						}
						else
						{
							base.GetRoomUser().Chat(null, text, false);
						}
					}
					if (@class.ServeId >= 1)
					{
						RoomUser_0.CarryItem(@class.ServeId);
					}
				}
			}
		}
		public override void OnUserShout(RoomUser RoomUser_0, string string_0)
		{
		}
		public override void OnTimerTick()
		{
			if (this.int_2 <= 0)
			{
				if (base.method_3().list_0.Count > 0)
				{
					RandomSpeech @class = base.method_3().method_3();
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
				string text = base.method_3().WalkMode.ToLower();
				if (text != null && !(text == "stand"))
				{
					if (!(text == "freeroam"))
					{
						if (text == "specified_range")
						{
							int int_ = PhoenixEnvironment.GetRandomNumber(base.method_3().min_x, base.method_3().max_x);
							int int_2 = PhoenixEnvironment.GetRandomNumber(base.method_3().min_y, base.method_3().max_y);
							base.GetRoomUser().MoveTo(int_, int_2);
						}
					}
					else
					{
						int int_ = PhoenixEnvironment.GetRandomNumber(0, base.method_1().Model.MapSizeX);
						int int_2 = PhoenixEnvironment.GetRandomNumber(0, base.method_1().Model.MapSizeY);
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
