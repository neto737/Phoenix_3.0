using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Advertisements
{
	internal sealed class AdvertisementManager
	{
        public List<RoomAdvertisement> RoomAdvertisements = new List<RoomAdvertisement>();

		public void LoadRoomAdvertisements(DatabaseClient dbClient)
		{
            Logging.Write("Loading Room Adverts..");
			this.RoomAdvertisements.Clear();
			DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM room_ads WHERE enabled = '1'");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.RoomAdvertisements.Add(new RoomAdvertisement((uint)dataRow["Id"], (string)dataRow["ad_image"], (string)dataRow["ad_link"], (int)dataRow["views"], (int)dataRow["views_limit"]));
				}
				Logging.WriteLine("completed!");
			}
		}
		public RoomAdvertisement GetRandomRoomAdvertisement()
		{
			if (this.RoomAdvertisements.Count <= 0)
			{
				return null;
			}
			else
			{
				int i;
				do
				{
					i = PhoenixEnvironment.GetRandomNumber(0, this.RoomAdvertisements.Count - 1);
				}
				while (this.RoomAdvertisements[i] == null || this.RoomAdvertisements[i].ExceededLimit);
				return RoomAdvertisements[i];
			}
		}
	}
}
