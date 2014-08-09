using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Guilds
{
	internal sealed class GuildManager
	{
        public static Dictionary<int, Guild> GuildList = new Dictionary<int, Guild>();

		public static void LoadGroups(DatabaseClient dbClient)
		{
            Logging.Write("Loading groups...");
			GuildManager.ClearGroups();
			DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM groups;");
			foreach (DataRow dataRow in dataTable.Rows)
			{
                GuildManager.GuildList.Add((int)dataRow["Id"], new Guild((int)dataRow["Id"], dataRow, dbClient));
			}
			Logging.WriteLine("completed!");
		

		public static void ClearGroups()
		{
			GuildManager.GuildList.Clear();
		}

        public static Guild GetGuild(int id)
        {
            if (GuildList.ContainsKey(id))
            {
                return GuildList[id];
            }
            return null;
        }

		public static void UpdateGroup(DatabaseClient dbClient, int id)
		{
            Guild guild = GuildManager.GetGuild(id);
			if (guild != null)
			{
				DataRow Row = dbClient.ReadDataRow("SELECT * FROM groups WHERE Id = " + id + " LIMIT 1");
				guild.Name = (string)Row["name"];
				guild.Badge = (string)Row["badge"];
				guild.RoomId = (uint)Row["roomid"];
				guild.Desc = (string)Row["desc"];
				guild.Locked = (string)Row["locked"];
				guild.List.Clear();
				DataTable dataTable = dbClient.ReadDataTable("SELECT userid FROM group_memberships WHERE groupid = " + id + ";");
				foreach (DataRow row2 in dataTable.Rows)
				{
					guild.AddMember((int)row2["userid"]);
				}
			}
		}
	}
}
