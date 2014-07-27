using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Avatar
{
    internal class GetWardrobeMessageEvent : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Request)
        {
            ServerMessage message = new ServerMessage(267);
            message.AppendBoolean(Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"));
            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.AddParamWithValue("userid", Session.GetHabbo().Id);
                    DataTable Row = adapter.ReadDataTable("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = @userid;");
                    if (Row == null)
                    {
                        message.AppendInt32(0);
                    }
                    else
                    {
                        message.AppendInt32(Row.Rows.Count);
                        foreach (DataRow row in Row.Rows)
                        {
                            message.AppendUInt((uint)row["slot_id"]);
                            message.AppendStringWithBreak((string)row["look"]);
                            message.AppendStringWithBreak((string)row["gender"]);
                        }
                    }
                }
                Session.SendMessage(message);
            }
            else
            {
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.AddParamWithValue("userid", Session.GetHabbo().Id);
                    DataTable Row = adapter.ReadDataTable("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = @userid;");
                    if (Row == null)
                    {
                        message.AppendInt32(0);
                    }
                    else
                    {
                        message.AppendInt32(Row.Rows.Count);
                        foreach (DataRow row in Row.Rows)
                        {
                            message.AppendUInt((uint)row["slot_id"]);
                            message.AppendStringWithBreak((string)row["look"]);
                            message.AppendStringWithBreak((string)row["gender"]);
                        }
                    }
                }
                Session.SendMessage(message);
            }
        }
    }
}