using System;
using System.Data;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Items
{
	internal sealed class TeleHandler
	{
		public static uint smethod_0(uint uint_0)
		{
			uint result;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataRow dataRow = @class.ReadDataRow("SELECT tele_two_id FROM tele_links WHERE tele_one_id = '" + uint_0 + "' LIMIT 1;");
				if (dataRow == null)
				{
					result = 0u;
				}
				else
				{
					result = (uint)dataRow[0];
				}
			}
			return result;
		}
		public static uint smethod_1(uint uint_0)
		{
			uint result;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				DataRow dataRow = @class.ReadDataRow("SELECT room_id FROM items WHERE Id = '" + uint_0 + "' LIMIT 1;");
				if (dataRow == null)
				{
					result = 0u;
				}
				else
				{
					result = (uint)dataRow[0];
				}
			}
			return result;
		}
		public static bool smethod_2(uint uint_0)
		{
			uint num = TeleHandler.smethod_0(uint_0);
			bool result;
			if (num == 0u)
			{
				result = false;
			}
			else
			{
				uint num2 = TeleHandler.smethod_1(num);
				result = (num2 != 0u);
			}
			return result;
		}
	}
}
