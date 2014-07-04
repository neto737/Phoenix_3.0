using System;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Advertisements
{
	internal sealed class RoomAdvertisement
	{
		public uint Id;
		public string AdImage;
		public string AdLink;
		public int Views;
		public int ViewsLimit;

		public bool ExceededLimit
		{
			get
			{
				return this.ViewsLimit > 0 && this.Views >= this.ViewsLimit;
			}
		}

		public RoomAdvertisement(uint Id, string AdImage, string AdLink, int Views, int ViewsLimit)
		{
			this.Id = Id;
			this.AdImage = AdImage;
			this.AdLink = AdLink;
			this.Views = Views;
			this.ViewsLimit = ViewsLimit;
		}

		public void OnView()
		{
			this.Views++;
			using (DatabaseClient client = PhoenixEnvironment.GetDatabase().GetClient())
			{
				client.ExecuteQuery("UPDATE room_ads SET views = views + 1 WHERE Id = '" + this.Id + "' LIMIT 1");
			}
		}
	}
}
