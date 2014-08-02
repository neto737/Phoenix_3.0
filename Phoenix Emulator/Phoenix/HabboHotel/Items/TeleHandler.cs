using System;
using System.Data;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Items
{
	internal sealed class TeleHandler
	{
		public static uint GetLinkedTele(uint TeleId)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataRow dataRow = adapter.ReadDataRow("SELECT tele_two_id FROM tele_links WHERE tele_one_id = '" + TeleId + "' LIMIT 1;");
				if (dataRow == null)
				{
					return 0;
				}
				else
				{
					return (uint)dataRow[0];
				}
			}
		}
		public static uint GetTeleRoomId(uint TeleId)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataRow dataRow = adapter.ReadDataRow("SELECT room_id FROM items WHERE Id = '" + TeleId + "' LIMIT 1;");
				if (dataRow == null)
				{
					return 0;
				}
				else
				{
					return (uint)dataRow[0];
				}
			}
		}
        public static bool IsTeleLinked(uint TeleId)
        {
            uint LinkId = GetLinkedTele(TeleId);
            if (LinkId == 0)
            {
                return false;
            }

            uint RoomId = GetTeleRoomId(LinkId);
            if (RoomId == 0)
            {
                return false;
            }

            return true;
        }
	}
}
