using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Groups
{
	internal class GroupManager
	{
        public static Dictionary<int, Group> GroupList = new Dictionary<int, Group>();

        public static void LoadGroups(DatabaseClient dbClient)
        {
            Logging.Write("Loading groups...");
            GroupManager.ClearGroups();
            DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM groups;");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                GroupManager.GroupList.Add((int)dataRow["Id"], new Group((int)dataRow["Id"], dataRow, dbClient));
            }
            Logging.WriteLine("completed!");
        }

		public static void ClearGroups()
		{
			GroupManager.GroupList.Clear();
		}

        public static Group GetGroup(int id)
        {
            if (GroupList.ContainsKey(id))
            {
                return GroupList[id];
            }
            return null;
        }

		public static void UpdateGroup(DatabaseClient dbClient, int id)
		{
            Group group = GroupManager.GetGroup(id);
			if (group != null)
			{
				DataRow Row = dbClient.ReadDataRow("SELECT * FROM groups WHERE Id = " + id + " LIMIT 1");
				group.Name = (string)Row["name"];
				group.Badge = (string)Row["badge"];
				group.RoomId = (uint)Row["roomid"];
				group.Desc = (string)Row["desc"];
				group.Locked = (string)Row["locked"];
				group.List.Clear();
				DataTable dataTable = dbClient.ReadDataTable("SELECT userid FROM group_memberships WHERE groupid = " + id + ";");
				foreach (DataRow row2 in dataTable.Rows)
				{
					group.AddMember((int)row2["userid"]);
				}
			}
		}
	}
}
