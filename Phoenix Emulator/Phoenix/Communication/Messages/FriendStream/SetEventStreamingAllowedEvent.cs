using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.FriendStream
{
	internal sealed class SetEventStreamingAllowedEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                bool Enabled = Event.PopBase64Boolean();
                Session.GetHabbo().FriendStreamEnabled = Enabled;
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.AddParamWithValue("user_id", Session.GetHabbo().Id);
                    adapter.ExecuteQuery("UPDATE users SET friend_stream_enabled = '" + (Enabled ? 1 : 0) + "' WHERE Id = @user_id LIMIT 1;");
                }
            }
        }
	}
}
