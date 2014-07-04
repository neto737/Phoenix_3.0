using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Phoenix.HabboHotel.Items;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Items
{
	internal sealed class MoodlightData
	{
		public bool Enabled;
		public int CurrentPreset;
		public List<MoodlightPreset> Presets;
		public uint ItemId;
		public MoodlightData(uint mItemId)
		{
			this.ItemId = mItemId;
			DataRow Row = null;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				Row = @class.ReadDataRow("SELECT enabled,current_preset,preset_one,preset_two,preset_three FROM room_items_moodlight WHERE item_id = '" + mItemId + "' LIMIT 1");
			}
			if (Row == null)
			{
				throw new ArgumentException();
			}
			this.Enabled = PhoenixEnvironment.EnumToBool(Row["enabled"].ToString());
			this.CurrentPreset = (int)Row["current_preset"];
			this.Presets = new List<MoodlightPreset>();
			this.Presets.Add(this.method_3((string)Row["preset_one"]));
			this.Presets.Add(this.method_3((string)Row["preset_two"]));
			this.Presets.Add(this.method_3((string)Row["preset_three"]));
		}
		public void method_0()
		{
			this.Enabled = true;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				@class.ExecuteQuery("UPDATE room_items_moodlight SET enabled = '1' WHERE item_id = '" + this.ItemId + "' LIMIT 1");
			}
		}
		public void method_1()
		{
			this.Enabled = false;
			using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
			{
				@class.ExecuteQuery("UPDATE room_items_moodlight SET enabled = '0' WHERE item_id = '" + this.ItemId + "' LIMIT 1");
			}
		}
		public void method_2(int int_1, string string_0, int int_2, bool bool_1)
		{
            string text = null;
			if (this.method_5(string_0) && this.method_6(int_2))
			{
				switch (int_1)
				{
				case 1:
				{
					text = "one";
					goto IL_44;
				}
				case 2:
				{
					text = "two";
					goto IL_44;
				}
				case 3:
				{
					text = "three";
					goto IL_44;
				}
				}
				/*goto IL_2E;*/
				IL_44:
				using (DatabaseClient @class = PhoenixEnvironment.GetDatabase().GetClient())
				{
					@class.AddParamWithValue("color", string_0);
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE room_items_moodlight SET preset_",
						text,
						" = '@color,",
						int_2,
						",",
						PhoenixEnvironment.BoolToEnum(bool_1),
						"' WHERE item_id = '",
						this.ItemId,
						"' LIMIT 1"
					}));
				}
				this.method_4(int_1).ColorCode = string_0;
				this.method_4(int_1).ColorIntensity = int_2;
				this.method_4(int_1).BackgroundOnly = bool_1;
			}
		}
		public MoodlightPreset method_3(string string_0)
		{
			string[] array = string_0.Split(new char[]
			{
				','
			});
			if (!this.method_5(array[0]))
			{
				array[0] = "#000000";
			}
			return new MoodlightPreset(array[0], int.Parse(array[1]), PhoenixEnvironment.EnumToBool(array[2]));
		}
		public MoodlightPreset method_4(int int_1)
		{
			int_1--;
			MoodlightPreset result;
			if (this.Presets[int_1] != null)
			{
				result = this.Presets[int_1];
			}
			else
			{
				result = new MoodlightPreset("#000000", 255, false);
			}
			return result;
		}
		public bool method_5(string string_0)
		{
			bool result;
			switch (string_0)
			{
			case "#000000":
			case "#0053F7":
			case "#EA4532":
			case "#82F349":
			case "#74F5F5":
			case "#E759DE":
			case "#F2F851":
				result = true;
				return result;
			}
			result = false;
			return result;
		}
		public bool method_6(int int_1)
		{
			return int_1 >= 0 && int_1 <= 255;
		}
		public string method_7()
		{
			MoodlightPreset @class = this.method_4(this.CurrentPreset);
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Enabled)
			{
				stringBuilder.Append(2);
			}
			else
			{
				stringBuilder.Append(1);
			}
			stringBuilder.Append(",");
			stringBuilder.Append(this.CurrentPreset);
			stringBuilder.Append(",");
			if (@class.BackgroundOnly)
			{
				stringBuilder.Append(2);
			}
			else
			{
				stringBuilder.Append(1);
			}
			stringBuilder.Append(",");
			stringBuilder.Append(@class.ColorCode);
			stringBuilder.Append(",");
			stringBuilder.Append(@class.ColorIntensity);
			return stringBuilder.ToString();
		}
	}
}
