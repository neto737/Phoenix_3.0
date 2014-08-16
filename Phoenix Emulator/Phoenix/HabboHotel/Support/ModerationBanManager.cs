using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Util;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Support
{
	class ModerationBanManager
	{
        public List<ModerationBan> Bans = new List<ModerationBan>();

		public void LoadBans(DatabaseClient dbClient)
		{
            Logging.Write("Loading bans..");
			this.Bans.Clear();
			DataTable dataTable = dbClient.ReadDataTable("SELECT bantype,value,reason,expire FROM bans WHERE expire > '" + PhoenixEnvironment.GetUnixTimestamp() + "'");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					ModerationBanType iP = ModerationBanType.IP;
					if ((string)dataRow["bantype"] == "user")
					{
						iP = ModerationBanType.USERNAME;
					}
					this.Bans.Add(new ModerationBan(iP, (string)dataRow["value"], (string)dataRow["reason"], (double)dataRow["expire"]));
				}
				Logging.WriteLine("completed!");
			}
		}
		public void CheckForBanConflicts(GameClient Client)
		{
			foreach (ModerationBan ban in this.Bans)
			{
				if (!ban.Expired)
				{
					if (ban.Type == ModerationBanType.IP && Client.GetConnection().ipAddress == ban.Variable)
					{
						throw new ModerationBanException(ban.ReasonMessage);
					}
					if (Client.GetHabbo() != null && (ban.Type == ModerationBanType.USERNAME && Client.GetHabbo().Username.ToLower() == ban.Variable.ToLower()))
					{
						throw new ModerationBanException(ban.ReasonMessage);
					}
				}
			}
		}
		public void BanUser(GameClient Client, string Moderator, double LengthSeconds, string Reason, bool IpBan)
		{
			if (!Client.GetHabbo().isAaron)
			{
				ModerationBanType uSERNAME = ModerationBanType.USERNAME;
				string username = Client.GetHabbo().Username;
				string val = "user";
				double Expire = PhoenixEnvironment.GetUnixTimestamp() + LengthSeconds;
				if (IpBan)
				{
					uSERNAME = ModerationBanType.IP;
					if (!GlobalClass.UseIP_Last)
					{
                        username = Client.GetConnection().ipAddress;
					}
					else
					{
						using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
						{
							username = adapter.ReadString("SELECT ip_last FROM users WHERE Id = " + Client.GetHabbo().Id + " LIMIT 1;");
						}
					}
					val = "ip";
				}
				this.Bans.Add(new ModerationBan(uSERNAME, username, Reason, Expire));
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.AddParamWithValue("rawvar", val);
					adapter.AddParamWithValue("var", username);
					adapter.AddParamWithValue("reason", Reason);
					adapter.AddParamWithValue("mod", Moderator);
					adapter.ExecuteQuery("INSERT INTO bans (bantype,value,reason,expire,added_by,added_date,appeal_state) VALUES (@rawvar,@var,@reason,'" + Expire + "',@mod,'" + DateTime.Now.ToLongDateString() + "', '1')");
				}
                if (IpBan)
                {
                    DataTable table = null;
                    using (DatabaseClient client3 = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        client3.AddParamWithValue("var", username);
                        table = client3.ReadDataTable("SELECT id FROM users WHERE ip_last = @var");
                    }
                    if (table != null)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                            {
                                adapter.ExecuteQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = '" + ((uint)row["id"]) + "' LIMIT 1");
                            }
                        }
                    }
                }
                else
                {
                    using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
                    {
                        adapter.ExecuteQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = '" + Client.GetHabbo().Id + "' LIMIT 1");
                    }
                }
                Client.SendBanMessage("You have been banned: " + Reason);
                Client.Disconnect();
            }
        }
	}
}
