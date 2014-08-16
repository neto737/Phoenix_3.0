using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
namespace Phoenix.HabboHotel.Quests
{
	class Quest
	{
		private readonly uint Id;
		public string Type;
		public string Action;
		public int NeedForLevel;
		public int Level;
		public int PixelReward;

		public Quest(uint mId, string mType, string mAction, int mNeedForLevel, int mLevel, int mPixelReward)
		{
			this.Id = mId;
			this.Type = mType;
			this.Action = mAction;
			this.NeedForLevel = mNeedForLevel;
			this.Level = mLevel;
			this.PixelReward = mPixelReward;
		}

		public uint QuestId()
		{
			return this.Id;
		}

        public void Serialize(ServerMessage Message, GameClient Session, bool Single)
        {
            Message.AppendStringWithBreak(Type);
            if (Session.GetHabbo().CompletedQuests.Contains(Id))
            {
                Message.AppendInt32(Level);
            }
            else
            {
                Message.AppendInt32(Level - 1);
            }
            Message.AppendInt32(PhoenixEnvironment.GetGame().GetQuestManager().GetHighestLevelForType(Type));
            if (PhoenixEnvironment.GetGame().GetQuestManager().GetHighestLevelForType(Type) == Level && Session.GetHabbo().CompletedQuests.Contains(Id) && !Single)
            {
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendStringWithBreak("");
                Message.AppendStringWithBreak("");
                Message.AppendInt32(0);
                Message.AppendStringWithBreak("");
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
            }
            else
            {
                Message.AppendBoolean(false);
                Message.AppendUInt(Id);
                Message.AppendBoolean(Session.GetHabbo().CurrentQuestId == Id);
                Message.AppendStringWithBreak(Action.StartsWith("FIND") ? "FIND_STUFF" : Action);
                Message.AppendStringWithBreak("_2");
                Message.AppendInt32(PixelReward);
                Message.AppendStringWithBreak(Action.Replace("_", ""));
                Message.AppendInt32(Session.GetHabbo().CurrentQuestProgress);
                Message.AppendInt32(NeedForLevel);
                Message.AppendInt32(0);
            }
        }
	}
}
