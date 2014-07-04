using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Communication.Messages.FriendStream
{
    class EventStreamingLikeButton : MessageEvent
    {
        public void parse(GameClient session, ClientMessage message)
        {
            int id = message.PopWiredInt32();
            int userid = message.PopWiredInt32();

            if (id <= 0 || userid <= 0)
                return;

            using (DatabaseClient dbclient = PhoenixEnvironment.GetDatabase().GetClient())
            {
                DataRow datarow = dbclient.ReadDataRow("SELECT id FROM friend_stream_likes WHERE friend_stream_id = '" + id + "' AND userid = '" + userid + "' LIMIT 1");

                if (datarow == null)
                {
                    dbclient.AddParamWithValue("fsi", id);
                    dbclient.AddParamWithValue("userid", userid);
                    dbclient.ExecuteQuery("INSERT INTO friend_stream_likes (friend_stream_id, userid) VALUES (@fsi, @userid)");
                }
                else
                {
                    session.SendNotif("You cant like twice!");
                }
            }
        }
    }
}