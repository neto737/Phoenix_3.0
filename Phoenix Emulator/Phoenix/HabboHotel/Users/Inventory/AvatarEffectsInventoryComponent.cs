using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Users.Inventory
{
	class AvatarEffectsInventoryComponent
	{
		private List<AvatarEffect> Effects;
		private uint UserId;
		public int CurrentEffect;
		private GameClient Session;

		public int Count
		{
			get
			{
				return this.Effects.Count;
			}
		}

		public AvatarEffectsInventoryComponent(uint UserId, GameClient pClient, HabboData UserData)
		{
			this.Session = pClient;
			this.Effects = new List<AvatarEffect>();
			this.UserId = UserId;
			this.CurrentEffect = -1;
			this.Effects.Clear();

			DataTable getUserEffects = UserData.GetUserEffects;
			StringBuilder QueryBuilder = new StringBuilder();
			foreach (DataRow dataRow in getUserEffects.Rows)
			{
				AvatarEffect item = new AvatarEffect((int)dataRow["effect_id"], (int)dataRow["total_duration"], PhoenixEnvironment.EnumToBool(dataRow["is_activated"].ToString()), (double)dataRow["activated_stamp"]);
				if (item.HasExpired)
				{
					QueryBuilder.Append(string.Concat(new object[]
					{
						"DELETE FROM user_effects WHERE user_id = '",
						UserId,
						"' AND effect_id = '",
						item.EffectId,
						"' LIMIT 1; "
					}));
				}
				else
				{
					this.Effects.Add(item);
				}
			}
			if (QueryBuilder.Length > 0)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery(QueryBuilder.ToString());
				}
			}
		}

		public void AddEffect(int EffectId, int Duration)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.ExecuteQuery("INSERT INTO user_effects (user_id,effect_id,total_duration,is_activated,activated_stamp) VALUES ('" + UserId + "','" + EffectId + "','" + Duration + "','0','0')");
			}
			this.Effects.Add(new AvatarEffect(EffectId, Duration, false, 0.0));
			ServerMessage Message = new ServerMessage(461);
			Message.AppendInt32(EffectId);
			Message.AppendInt32(Duration);
			this.GetClient().SendMessage(Message);
		}

		public void StopEffect(int EffectId)
		{
			AvatarEffect Effect = this.GetEffect(EffectId, true);
			if (Effect != null && Effect.HasExpired)
			{
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.ExecuteQuery("DELETE FROM user_effects WHERE user_id = '" + UserId + "' AND effect_id = '" + EffectId + "' AND is_activated = '1' LIMIT 1");
				}
				this.Effects.Remove(Effect);
				ServerMessage Message = new ServerMessage(463);
				Message.AppendInt32(EffectId);
				this.GetClient().SendMessage(Message);
				if (this.CurrentEffect >= 0)
				{
					this.ApplyEffect(-1, false);
				}
			}
		}

		public void ApplyEffect(int EffectId, bool bool_0)
		{
			if (this.HasEffect(EffectId, true) || bool_0)
			{
				Room Room = GetUserRoom();
				if (Room != null && (this.GetClient() != null && this.GetClient().GetHabbo() != null))
				{
					RoomUser User = Room.GetRoomUserByHabbo(this.GetClient().GetHabbo().Id);
					if (User != null && (User.byte_1 <= 0 || EffectId == -1 || bool_0) && (User.class34_1 == null || EffectId == 77 || EffectId == -1))
					{
						this.CurrentEffect = EffectId;
						if (User.GetClient() != null && User.GetClient().GetHabbo().CurrentQuestId == 19 && (this.CurrentEffect == 28 || this.CurrentEffect == 29 || this.CurrentEffect == 30 || this.CurrentEffect == 37))
						{
							PhoenixEnvironment.GetGame().GetQuestManager().ProgressUserQuest(19, User.GetClient());
						}
						ServerMessage Message = new ServerMessage(485);
						Message.AppendInt32(User.VirtualId);
						Message.AppendInt32(EffectId);
						Room.SendMessage(Message, null);
					}
				}
			}
		}

		public void EnableEffect(int EffectId)
		{
			AvatarEffect Effect = this.GetEffect(EffectId, false);
			if (Effect != null && !Effect.HasExpired && !Effect.Activated && (this.GetClient() != null && this.GetClient().GetHabbo() != null))
			{
				Room class2 = this.GetUserRoom();
				if (class2 != null)
				{
					RoomUser User = class2.GetRoomUserByHabbo(this.GetClient().GetHabbo().Id);
					if (User.byte_1 <= 0 && User.class34_1 == null)
					{
						using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
						{
							adapter.ExecuteQuery("UPDATE user_effects SET is_activated = '1', activated_stamp = '" + PhoenixEnvironment.GetUnixTimestamp() + "' WHERE user_id = '" + UserId + "' AND effect_id = '" + EffectId + "' LIMIT 1");
						}
						Effect.Activate();
						ServerMessage Message = new ServerMessage(462);
						Message.AppendInt32(Effect.EffectId);
						Message.AppendInt32(Effect.TotalDuration);
						this.GetClient().SendMessage(Message);
					}
				}
			}
		}

		public bool HasEffect(int EffectId, bool IfEnabledOnly)
		{
			if (EffectId == -1 || EffectId == 28 || EffectId == 29)
			{
				return true;
			}
			else
			{
				using (TimedLock.Lock(Effects))
				{
					foreach (AvatarEffect Effect in Effects)
					{
						if ((!IfEnabledOnly || Effect.Activated) && !Effect.HasExpired && Effect.EffectId == EffectId)
						{
                            return true;
						}
					}
				}
				return false;
			}
		}

        public AvatarEffect GetEffect(int EffectId, bool IfEnabledOnly)
        {
            foreach (AvatarEffect Effect in Effects)
            {
                if ((!IfEnabledOnly || Effect.Activated) && Effect.EffectId == EffectId)
                {
                    return Effect;
                }
            }
            return null;
        }

        public ServerMessage Serialize()
        {
            ServerMessage Message = new ServerMessage(460);
            Message.AppendInt32(Count);
            foreach (AvatarEffect Effect in Effects)
            {
                Message.AppendInt32(Effect.EffectId);
                Message.AppendInt32(Effect.TotalDuration);
                Message.AppendBoolean(!Effect.Activated);
                Message.AppendInt32(Effect.TimeLeft);
            }
            return Message;
        }

        public void CheckExpired()
        {
            List<int> list = new List<int>();
            foreach (AvatarEffect Effect in Effects)
            {
                if (Effect.HasExpired)
                {
                    list.Add(Effect.EffectId);
                }
            }
            foreach (int current2 in list)
            {
                this.StopEffect(current2);
            }
        }

		private GameClient GetClient()
		{
			return this.Session;
		}

		private Room GetUserRoom()
		{
			return this.Session.GetHabbo().CurrentRoom;
		}
	}
}
