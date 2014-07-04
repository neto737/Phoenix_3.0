using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.Messages;
using Phoenix.Storage;
using Phoenix.HabboHotel.Rooms;
namespace Phoenix.Communication.Messages.Rooms.Furniture
{
	internal sealed class CreditFurniRedeemMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null && @class.CheckRights(Session, true))
				{
					RoomItem class2 = @class.GetItem(Event.PopWiredUInt());
					UserItem class3 = Session.GetHabbo().GetInventoryComponent().GetItem(class2.Id);
					if (class2 != null)
					{
						if (class2.GetBaseItem().Name.StartsWith("CF_") || class2.GetBaseItem().Name.StartsWith("CFC_") || class2.GetBaseItem().Name.StartsWith("PixEx_") || class2.GetBaseItem().Name.StartsWith("PntEx_"))
						{
							if (class3 != null)
							{
								@class.RemoveFurniture(null, class3.Id, true, true);
							}
							else
							{
								DataRow dataRow = null;
								using (DatabaseClient class4 = PhoenixEnvironment.GetDatabase().GetClient())
								{
									dataRow = class4.ReadDataRow("SELECT ID FROM items WHERE ID = '" + class2.Id + "' LIMIT 1");
								}
								if (dataRow != null)
								{
									string[] array = class2.GetBaseItem().Name.Split(new char[]
									{
										'_'
									});
									int num = int.Parse(array[1]);
									if (num > 0)
									{
										if (class2.GetBaseItem().Name.StartsWith("CF_") || class2.GetBaseItem().Name.StartsWith("CFC_"))
										{
											Session.GetHabbo().Credits += num;
											Session.GetHabbo().UpdateCreditsBalance(true);
										}
										else
										{
											if (class2.GetBaseItem().Name.StartsWith("PixEx_"))
											{
												Session.GetHabbo().ActivityPoints += num;
												Session.GetHabbo().UpdateActivityPointsBalance(true);
											}
											else
											{
												if (class2.GetBaseItem().Name.StartsWith("PntEx_"))
												{
													Session.GetHabbo().shells += num;
													Session.GetHabbo().UpdateShellsBalance(false, true);
												}
											}
										}
									}
								}
								@class.RemoveFurniture(null, class2.Id, true, true);
								ServerMessage Message5_ = new ServerMessage(219u);
								Session.SendMessage(Message5_);
							}
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
