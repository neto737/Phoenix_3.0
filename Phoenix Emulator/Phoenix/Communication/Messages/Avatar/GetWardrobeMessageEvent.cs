using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Communication.Messages.Avatar
{
    internal sealed class GetWardrobeMessageEvent : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Event)
        {
            ServerMessage message = new ServerMessage(267);
            message.AppendBoolean(Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"));
            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                {
                    adapter.AddParamWithValue("userid", Session.GetHabbo().Id);
                    DataTable dataTable = adapter.ReadDataTable("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = @userid;");
                    if (dataTable == null)
                    {
                        message.AppendInt32(0);
                    }
                    else
                    {
                        message.AppendInt32(dataTable.Rows.Count);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            message.AppendUInt((uint)dataRow["slot_id"]);
                            message.AppendStringWithBreak((string)dataRow["look"]);
                            message.AppendStringWithBreak((string)dataRow["gender"]);
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
                    DataTable dataTable = adapter.ReadDataTable("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = @userid;");
                    if (dataTable == null)
                    {
                        message.AppendInt32(0);
                    }
                    else
                    {
                        message.AppendInt32(dataTable.Rows.Count);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            message.AppendUInt((uint)dataRow["slot_id"]);
                            message.AppendStringWithBreak((string)dataRow["look"]);
                            message.AppendStringWithBreak((string)dataRow["gender"]);
                        }
                    }
                }
                Session.SendMessage(message);
            }
        }
    }
}