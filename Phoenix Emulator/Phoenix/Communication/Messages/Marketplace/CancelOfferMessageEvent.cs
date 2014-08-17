using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Items;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Marketplace
{
	internal class CancelOfferMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			if (Session != null)
			{
				uint ItemId = Event.PopWiredUInt();
				DataRow Row = null;
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					Row = adapter.ReadDataRow("SELECT furni_id, item_id, user_id, extra_data, offer_id, state, timestamp FROM catalog_marketplace_offers WHERE offer_id = '" + ItemId + "' LIMIT 1");
				}
				if (Row != null)
				{
					int num2 = (int)Math.Floor(((double)Row["timestamp"] + 172800.0 - PhoenixEnvironment.GetUnixTimestamp()) / 60.0);
					int num3 = int.Parse(Row["state"].ToString());
					if (num2 <= 0)
					{
						num3 = 3;
					}
					if ((uint)Row["user_id"] == Session.GetHabbo().Id && num3 != 2)
					{
						Item class2 = PhoenixEnvironment.GetGame().GetItemManager().GetItem((uint)Row["item_id"]);
						if (class2 != null)
						{
							PhoenixEnvironment.GetGame().GetCatalog().DeliverItems(Session, class2, 1, (string)Row["extra_data"], false, (uint)Row["furni_id"]);
							using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
							{
								adapter.ExecuteQuery("DELETE FROM catalog_marketplace_offers WHERE offer_id = '" + ItemId + "' LIMIT 1");
							}
							ServerMessage Message = new ServerMessage(614);
							Message.AppendUInt((uint)Row["offer_id"]);
							Message.AppendBoolean(true);
							Session.SendMessage(Message);
						}
					}
				}
			}
		}
	}
}
