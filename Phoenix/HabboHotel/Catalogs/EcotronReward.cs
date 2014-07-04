using System;
using Phoenix.HabboHotel.Items;
namespace Phoenix.HabboHotel.Catalogs
{
	internal class EcotronReward
	{
		public uint Id;
		public uint DisplayId;
		public uint BaseId;
		public uint RewardLevel;
		public EcotronReward(uint Id, uint DisplayId, uint BaseId, uint RewardLevel)
		{
			this.Id = Id;
			this.DisplayId = DisplayId;
			this.BaseId = BaseId;
			this.RewardLevel = RewardLevel;
		}
		public Item GetBaseItem()
		{
			return PhoenixEnvironment.GetGame().GetItemManager().GetItem(this.BaseId);
		}
	}
}
