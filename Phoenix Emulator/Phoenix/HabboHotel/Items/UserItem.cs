using System;
using Phoenix.Core;
using Phoenix.Messages;
namespace Phoenix.HabboHotel.Items
{
	internal class UserItem
	{
		internal uint Id;
		internal uint BaseItem;
		internal string ExtraData;
		private Item Item;
		internal UserItem(uint Id, uint BaseItem, string ExtraData)
		{
			this.Id = Id;
			this.BaseItem = BaseItem;
			this.ExtraData = ExtraData;
			this.Item = this.GetBaseItem();
		}
		internal void Serialize(ServerMessage Message, bool Inventory)
		{
			if (this.Item == null)
			{
                Logging.LogException("Unknown base: " + this.BaseItem);
			}
			Message.AppendUInt(this.Id);
			Message.AppendStringWithBreak(this.Item.Type.ToString().ToUpper());
			Message.AppendUInt(this.Id);
			Message.AppendInt32(this.Item.SpriteId);
			if (this.Item.Name.Contains("a2 "))
			{
				Message.AppendInt32(3);
			}
			else
			{
				if (this.Item.Name.Contains("wallpaper"))
				{
					Message.AppendInt32(2);
				}
				else
				{
					if (this.Item.Name.Contains("landscape"))
					{
						Message.AppendInt32(4);
					}
					else
					{
						if (this.GetBaseItem().Name == "poster")
						{
							Message.AppendInt32(6);
						}
						else
						{
							if (this.GetBaseItem().Name == "song_disk")
							{
								Message.AppendInt32(8);
							}
							else
							{
								Message.AppendInt32(1);
							}
						}
					}
				}
			}
			if (this.GetBaseItem().Name == "song_disk")
			{
				Message.AppendInt32(0);
				Message.AppendStringWithBreak("");
			}
			else
			{
				if (this.GetBaseItem().Name.StartsWith("poster_"))
				{
					Message.AppendStringWithBreak(this.GetBaseItem().Name.Split(new char[]
					{
						'_'
					})[1]);
				}
				else
				{
					Message.AppendInt32(0);
					Message.AppendStringWithBreak(this.ExtraData);
				}
			}
			Message.AppendBoolean(this.Item.AllowRecycle);
			Message.AppendBoolean(this.Item.AllowTrade);
			Message.AppendBoolean(this.Item.AllowInventoryStack);
			Message.AppendBoolean(PhoenixEnvironment.GetGame().GetCatalog().GetMarketplace().method_0(this));
			Message.AppendInt32(-1);
			if (this.Item.Type == 's')
			{
				Message.AppendStringWithBreak("");
				if (this.GetBaseItem().Name == "song_disk" && this.ExtraData.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(this.ExtraData));
				}
				else
				{
					Message.AppendInt32(0);
				}
			}
		}
		internal Item GetBaseItem()
		{
			return PhoenixEnvironment.GetGame().GetItemManager().GetItem(BaseItem);
		}
	}
}
