using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Guilds
{
	internal sealed class Guild
	{
		public int Id;
		public string Name;
		public string Desc;
		public int OwnerId;
		public List<int> List;
		public string Badge;
		public uint RoomId;
		public string Locked;
		public Guild(int Id, DataRow Row, DatabaseClient adapter)
		{
			this.Id = Id;
			this.Name = (string)Row["name"];
			this.Desc = (string)Row["desc"];
			this.OwnerId = (int)Row["OwnerId"];
			this.Badge = (string)Row["badge"];
			this.RoomId = (uint)Row["roomid"];
			this.Locked = (string)Row["locked"];
			this.List = new List<int>();
			DataTable dataTable = adapter.ReadDataTable("SELECT userid FROM group_memberships WHERE groupid = " + Id + ";");
			foreach (DataRow dataRow in dataTable.Rows)
			{
				this.AddMember((int)dataRow["userid"]);
			}
		}
		public void AddMember(int MemberId)
		{
			if (!this.List.Contains(MemberId))
			{
				this.List.Add(MemberId);
			}
		}
		public void RemoveMember(int MemberId)
		{
			if (this.List.Contains(MemberId))
			{
				this.List.Remove(MemberId);
			}
		}
	}
}
