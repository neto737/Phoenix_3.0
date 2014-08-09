using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using System.Data;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.FriendStream
{
    internal sealed class GetEventStreamComposer : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage message)
        {
            ServerMessage response = new ServerMessage(950);

            Session.GetHabbo().GetHabboData.UpdateFriendStream();

            int streamCount = Session.GetHabbo().GetHabboData.GetFriendStream.Rows.Count;

            response.AppendInt32(streamCount);

            DataTable dataTable_ = Session.GetHabbo().GetHabboData.GetFriendStream;

            foreach (DataRow row in dataTable_.Rows)
            {
                int type = (int)row["type"];

                if (type >= 0 && type <= 4)
                {
                    uint id = (uint)row["id"];

                    int likes = 0;
                    bool canlike = false;

                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        likes = adapter.ReadInt32("SELECT COUNT(friend_stream_id) FROM friend_stream_likes WHERE friend_stream_id = '" + id + "' LIMIT 1");

                        DataRow datarow = adapter.ReadDataRow("SELECT id FROM friend_stream_likes WHERE friend_stream_id = '" + id + "' AND userid = '" + Session.GetHabbo().Id + "' LIMIT 1");

                        if (datarow == null)
                        {
                            canlike = true;
                        }
                        else
                        {
                            canlike = false;
                        }
                    }

                    uint userid = (uint)row["userid"];
                    string username = PhoenixEnvironment.GetGame().GetClientManager().GetNameById(userid);

                    string gender = (string)row["gender"].ToString().ToLower();
                    string look = (string)row["look"];

                    int time = (int)((PhoenixEnvironment.GetUnixTimestamp() - (double)row["time"]) / 60);

                    string data = (string)row["data"];

                    response.AppendUInt(id);
                    response.AppendInt32(type);
                    response.AppendStringWithBreak(userid.ToString());
                    response.AppendStringWithBreak(username);
                    response.AppendStringWithBreak(gender);
                    //response.AppendStringWithBreak("http://localhost/swfphx/head.php?figure=" + look);
                    response.AppendStringWithBreak("http://localhost/swfphx/c_images/album1584/ADM.gif");
                    response.AppendInt32(time);

                    if (type == 0)
                    {
                        string data_extra = (string)row["data_extra"];

                        //uint friend_id = uint.Parse(data, CustomCultureInfo.GetCustomCultureInfo());

                        //if (Session.GetHabbo().Id == friend_id || Session.GetHabbo().GetMessenger().UserInFriends(friend_id))
                        {
                            response.AppendInt32(0);
                        }
                        //else
                        {
                            response.AppendInt32(5);
                        }
                        response.AppendInt32(likes);
                        response.AppendBoolean(canlike);
                        response.AppendStringWithBreak(data);
                        response.AppendStringWithBreak(data_extra);
                    }
                    else if (type == 1)
                    {
                        response.AppendInt32(2);
                        response.AppendInt32(likes);
                        response.AppendBoolean(canlike);

                        uint roomId;

                        RoomData RoomData;

                        if (uint.TryParse(data, out roomId))
                            RoomData = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId);
                        else
                            RoomData = PhoenixEnvironment.GetGame().GetRoomManager().GenerateRoomData(0);

                        if (RoomData != null)
                        {
                            response.AppendStringWithBreak(RoomData.Id.ToString()); //data
                            response.AppendStringWithBreak(RoomData.Name); //extra data
                        }
                        else
                        {
                            response.AppendStringWithBreak("");
                            response.AppendStringWithBreak("Room deleted");
                        }
                    }
                    else if (type == 2)
                    {
                        response.AppendInt32(3);
                        response.AppendInt32(likes);
                        response.AppendBoolean(canlike);
                        response.AppendStringWithBreak(data);
                    }
                    else if (type == 3)
                    {
                        response.AppendInt32(4);
                        response.AppendInt32(likes);
                        response.AppendBoolean(canlike);
                        response.AppendStringWithBreak(data);
                    }
                }
            }

            Session.SendMessage(response);
        }
    }
}