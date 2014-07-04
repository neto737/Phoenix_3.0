using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.HabboHotel.Rooms;
using Phoenix.Messages;
namespace Phoenix.HabboHotel.Rooms
{
	class RoomData
	{
		public uint Id;
		public uint Achievement;
		public string Name;
		public string Description;
		public string Type;
		public string Owner;
		public string Password;
		public int State;
		public int Category;
		public int UsersNow;
		public int UsersMax;
		public string ModelName;
		public string CCTs;
		public int Score;
		public List<string> Tags;
		public bool AllowPet;
		public bool AllowPetsEating;
		public bool AllowWalkthrough;
		internal bool bool_3;
		public bool Hidewall;
		public int Wallthick;
		public int Floorthick;
		private RoomIcon myIcon;
		public RoomEvent Event;
		public string Wallpaper;
		public string Floor;
		public string Landscape;
		private RoomModel class28_0;

		public bool IsPublicRoom
		{
			get
			{
				return (Type.ToLower() == "public");
			}
		}
		public RoomIcon Icon
		{
			get
			{
				return myIcon;
			}
		}
		public int TagCount
		{
			get
			{
				return Tags.Count;
			}
		}
		public RoomModel Model
		{
			get
			{
				return class28_0;
			}
		}
		public void FillNull(uint mId)
		{
			this.Id = mId;
			this.Name = "Unknown Room";
			this.Description = "-";
			this.Type = "private";
			this.Owner = "-";
			this.Category = 0;
			this.UsersNow = 0;
			this.UsersMax = 0;
			this.ModelName = "NO_MODEL";
			this.CCTs = "";
			this.Score = 0;
			this.Tags = new List<string>();
			this.AllowPet = true;
			this.AllowPetsEating = false;
			this.AllowWalkthrough = true;
			this.Hidewall = false;
			this.Wallthick = 0;
			this.Floorthick = 0;
			this.Password = "";
			this.Wallpaper = "0.0";
			this.Floor = "0.0";
			this.Landscape = "0.0";
			this.Event = null;
			this.Achievement = 0;
			this.bool_3 = false;
			this.myIcon = new RoomIcon(1, 1, new Dictionary<int, int>());
			this.class28_0 = PhoenixEnvironment.GetGame().GetRoomManager().GetModel(ModelName, mId);
		}
		public void method_1(DataRow Row)
		{
			this.Id = (uint)Row["Id"];
			this.Name = (string)Row["caption"];
			this.Description = (string)Row["description"];
			this.Type = (string)Row["roomtype"];
			this.Owner = (string)Row["owner"];
			this.Achievement = Convert.ToUInt32(Row["achievement"]);
			string text = Row["state"].ToString().ToLower();
			if (text != null)
			{
				if (text == "open")
				{
					this.State = 0;
					goto IL_DF;
				}
				if (text == "password")
				{
					this.State = 2;
					goto IL_DF;
				}
				if (!(text == "locked"))
				{
				}
			}
			this.State = 1;
			IL_DF:
			this.Category = (int)Row["category"];
			this.UsersNow = (int)Row["users_now"];
			this.UsersMax = (int)Row["users_max"];
			this.ModelName = (string)Row["model_name"];
			this.CCTs = (string)Row["public_ccts"];
			this.Score = (int)Row["score"];
			this.Tags = new List<string>();
			this.AllowPet = PhoenixEnvironment.EnumToBool(Row["allow_pets"].ToString());
			this.AllowPetsEating = PhoenixEnvironment.EnumToBool(Row["allow_pets_eat"].ToString());
			this.AllowWalkthrough = PhoenixEnvironment.EnumToBool(Row["allow_walkthrough"].ToString());
			this.bool_3 = false;
			this.Hidewall = PhoenixEnvironment.EnumToBool(Row["allow_hidewall"].ToString());
			this.Wallthick = (int)Row["wallthick"];
			this.Floorthick = (int)Row["floorthick"];
			this.Password = (string)Row["password"];
			this.Wallpaper = (string)Row["wallpaper"];
			this.Floor = (string)Row["floor"];
			this.Landscape = (string)Row["landscape"];
			this.Event = null;
			Dictionary<int, int> IconItems = new Dictionary<int, int>();
			string[] array;
			if (Row["icon_items"].ToString() != "")
			{
				array = Row["icon_items"].ToString().Split(new char[]
				{
					'|'
				});
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = array[i];
					if (!string.IsNullOrEmpty(text2))
					{
						string[] array2 = text2.Replace('.', ',').Split(new char[]
						{
							','
						});
						int key = 0;
						int value = 0;
						int.TryParse(array2[0], out key);
						if (array2.Length > 1)
						{
							int.TryParse(array2[1], out value);
						}
						try
						{
							if (!IconItems.ContainsKey(key))
							{
								IconItems.Add(key, value);
							}
						}
						catch (Exception ex)
						{
                            Logging.LogException(string.Concat(new string[]
							{
								"Exception: ",
								ex.ToString(),
								"[",
								text2,
								"]"
							}));
						}
					}
				}
			}
			this.myIcon = new RoomIcon((int)Row["icon_bg"], (int)Row["icon_fg"], IconItems);
			array = Row["tags"].ToString().Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string Tag = array[i];
				this.Tags.Add(Tag);
			}
			this.class28_0 = PhoenixEnvironment.GetGame().GetRoomManager().GetModel(this.ModelName, this.Id);
		}
		public void Fill(Room Room)
		{
			this.Id = Room.RoomId;
			this.Name = Room.Name;
			this.Description = Room.Description;
			this.Type = Room.Type;
			this.Owner = Room.Owner;
			this.Category = Room.Category;
			this.State = Room.State;
			this.UsersNow = Room.UsersNow;
			this.UsersMax = Room.UsersMax;
			this.ModelName = Room.ModelName;
			this.CCTs = Room.CCTs;
			this.Score = Room.Score;
			this.Tags = Room.Tags;
			this.AllowPet = Room.AllowPet;
			this.AllowPetsEating = Room.AllowPetsEating;
			this.AllowWalkthrough = Room.AllowWalkthrough;
			this.Hidewall = Room.Hidewall;
			this.Wallthick = Room.Wallthick;
			this.Floorthick = Room.Floorthick;
			this.myIcon = Room.myIcon;
			this.Password = Room.Password;
			this.Event = Room.Event;
			this.Wallpaper = Room.Wallpaper;
			this.Floor = Room.Floor;
			this.Landscape = Room.Landscape;
			this.Achievement = Room.Achievement;
			this.class28_0 = PhoenixEnvironment.GetGame().GetRoomManager().GetModel(ModelName, Id);
		}
		public void method_3(ServerMessage Message, bool ShowEvents, bool bool_6)
		{
			Message.AppendUInt(this.Id);
			if (Event == null || !ShowEvents)
			{
				Message.AppendBoolean(false);
				Message.AppendStringWithBreak(Name);
				Message.AppendStringWithBreak(Owner);
				Message.AppendInt32(State);
				Message.AppendInt32(UsersNow);
				Message.AppendInt32(UsersMax);
				Message.AppendStringWithBreak(Description);
				Message.AppendBoolean(true);
				if (this.Category > 0)
				{
					Message.AppendBoolean(PhoenixEnvironment.GetGame().GetNavigator().method_2(Category).CanTrade);
				}
				else
				{
					Message.AppendBoolean(false);
				}
				Message.AppendInt32(Score);
				Message.AppendInt32(Category);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(TagCount);
				using (List<string>.Enumerator enumerator = this.Tags.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						Message.AppendStringWithBreak(current);
					}
					goto IL_1FA;
				}
			}
			Message.AppendBoolean(true);
			Message.AppendStringWithBreak(this.Event.Name);
			Message.AppendStringWithBreak(this.Owner);
			Message.AppendInt32(this.State);
			Message.AppendInt32(this.UsersNow);
			Message.AppendInt32(this.UsersMax);
			Message.AppendStringWithBreak(this.Event.Description);
			Message.AppendBoolean(true);
			Message.AppendBoolean(true);
			Message.AppendInt32(this.Score);
			Message.AppendInt32(this.Event.Category);
			Message.AppendStringWithBreak(this.Event.StartTime);
			Message.AppendInt32(this.Event.Tags.Count);
			foreach (string current in this.Event.Tags)
			{
				Message.AppendStringWithBreak(current);
			}
			IL_1FA:
			this.Icon.Serialize(Message);
			Message.AppendBoolean(this.AllowPet);
			Message.AppendBoolean(this.AllowPetsEating);
			if (bool_6)
			{
				Message.AppendBoolean(bool_6);
			}
		}
	}
}
