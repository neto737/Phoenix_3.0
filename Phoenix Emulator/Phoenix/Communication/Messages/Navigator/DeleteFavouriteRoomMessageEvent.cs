using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Navigator
{
	internal class DeleteFavouriteRoomMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			uint Id = Event.PopWiredUInt();

			Session.GetHabbo().FavoriteRooms.Remove(Id);
			ServerMessage Message = new ServerMessage(459);
			Message.AppendUInt(Id);
			Message.AppendBoolean(false);
			Session.SendMessage(Message);
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.ExecuteQuery("DELETE FROM user_favorites WHERE user_id = '" + Session.GetHabbo().Id + "' AND room_id = '" + Id + "' LIMIT 1");
			}
		}
	}
}
