using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.HabboHotel.Catalogs;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Recycler
{
	internal class RecycleItemsMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().InRoom)
			{
				int num = Event.PopWiredInt32();
				if (num == 5)
				{
					for (int i = 0; i < num; i++)
					{
						UserItem item = Session.GetHabbo().GetInventoryComponent().GetItem(Event.PopWiredUInt());
						if (item == null || !item.GetBaseItem().AllowRecycle)
						{
							return;
						}
						Session.GetHabbo().GetInventoryComponent().RemoveItem(item.Id, 0, false);
					}
					uint Id = PhoenixEnvironment.GetGame().GetCatalog().GenerateItemId();
					EcotronReward randomEcotronReward = PhoenixEnvironment.GetGame().GetCatalog().GetRandomEcotronReward();
					using (DatabaseClient client = PhoenixEnvironment.GetDatabase().GetClient())
					{
						client.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items (Id,user_id,base_item,extra_data,wall_pos) VALUES ('",
							Id,
							"','",
							Session.GetHabbo().Id,
							"','1478','",
							DateTime.Now.ToLongDateString(),
							"', '')"
						}));
						client.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
							Id,
							"','",
							randomEcotronReward.BaseId,
							"','1','')"
						}));
					}
					Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
					ServerMessage message = new ServerMessage(508);
					message.AppendBoolean(true);
					message.AppendUInt(Id);
					Session.SendMessage(message);
				}
			}
		}
	}
}
