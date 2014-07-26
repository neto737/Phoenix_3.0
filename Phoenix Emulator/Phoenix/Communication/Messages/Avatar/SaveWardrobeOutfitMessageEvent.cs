using System;
using Phoenix.HabboHotel.Misc;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Avatar
{
	internal sealed class SaveWardrobeOutfitMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			string Look = Event.PopFixedString();
			string Gender = Event.PopFixedString();
			using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
			{
				dbClient.AddParamWithValue("userid", Session.GetHabbo().Id);
				dbClient.AddParamWithValue("slotid", num);
				dbClient.AddParamWithValue("look", Look);
				dbClient.AddParamWithValue("gender", Gender.ToUpper());
				if (dbClient.ReadDataRow("SELECT null FROM user_wardrobe WHERE user_id = @userid AND slot_id = @slotid LIMIT 1") != null)
				{
					dbClient.ExecuteQuery("UPDATE user_wardrobe SET look = @look, gender = @gender WHERE user_id = @userid AND slot_id = @slotid LIMIT 1;");
				}
				else
				{
					dbClient.ExecuteQuery("INSERT INTO user_wardrobe (user_id,slot_id,look,gender) VALUES (@userid,@slotid,@look,@gender)");
				}
			}
		}
	}
}