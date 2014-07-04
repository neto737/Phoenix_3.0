using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.Storage;
using Phoenix.HabboHotel.SoundMachine;
namespace Phoenix.HabboHotel.Items
{
	internal sealed class ItemManager
	{
		private Dictionary<uint, Item> Item = new Dictionary<uint, Item>();
		//private Dictionary<int, Soundtrack> Sound;
		public ItemManager()
		{
			//this.Sound = new Dictionary<int, Soundtrack>();
		}
		public void LoadItems(DatabaseClient adapter)
		{
            Logging.Write("Loading Items..");
			this.Item = new Dictionary<uint, Item>();
			DataTable ItemData = adapter.ReadDataTable("SELECT * FROM furniture;");
			if (ItemData != null)
			{
				foreach (DataRow dataRow in ItemData.Rows)
				{
					try
					{
						this.Item.Add((uint)dataRow["Id"], new Item((uint)dataRow["Id"], (int)dataRow["sprite_id"], (string)dataRow["public_name"], (string)dataRow["item_name"], (string)dataRow["type"], (int)dataRow["width"], (int)dataRow["length"], (double)dataRow["stack_height"], PhoenixEnvironment.EnumToBool(dataRow["can_stack"].ToString()), PhoenixEnvironment.EnumToBool(dataRow["is_walkable"].ToString()), PhoenixEnvironment.EnumToBool(dataRow["can_sit"].ToString()), PhoenixEnvironment.EnumToBool(dataRow["allow_recycle"].ToString()), PhoenixEnvironment.EnumToBool(dataRow["allow_trade"].ToString()), PhoenixEnvironment.EnumToBool(dataRow["allow_marketplace_sell"].ToString()), PhoenixEnvironment.EnumToBool(dataRow["allow_gift"].ToString()), PhoenixEnvironment.EnumToBool(dataRow["allow_inventory_stack"].ToString()), (string)dataRow["interaction_type"], (int)dataRow["interaction_modes_count"], (string)dataRow["vending_ids"], dataRow["height_adjustable"].ToString(), Convert.ToByte((int)dataRow["EffectF"]), Convert.ToByte((int)dataRow["EffectM"])));
					}
					catch (Exception)
					{
						Logging.WriteLine("Could not load item #" + (uint)dataRow["Id"] + ", please verify the data is okay.");
					}
				}
			}
			Logging.WriteLine("completed!");
            //Logging.Write("Loading Soundtracks..");
            //this.Sound = new Dictionary<int, Soundtrack>();
            //DataTable SoundData = adapter.ReadDataTable("SELECT * FROM soundtracks;");
            //if (SoundData != null)
            //{
            //    foreach (DataRow dataRow in SoundData.Rows)
            //    {
            //        try
            //        {
            //            this.Sound.Add((int)dataRow["Id"], new Soundtrack((int)dataRow["Id"], (string)dataRow["name"], (string)dataRow["author"], (string)dataRow["track"], (int)dataRow["length"]));
            //        }
            //        catch (Exception)
            //        {
            //            Logging.WriteLine("Could not load item #" + (uint)dataRow["Id"] + ", please verify the data is okay.");
            //        }
            //    }
            //}
            //Logging.WriteLine("completed!");
            Logging.Write("Loading Soundtracks..");
            SongManager.Initialize();
            Logging.WriteLine("completed!");
		}
		public bool method_1(uint uint_0)
		{
			return this.Item.ContainsKey(uint_0);
		}
		public Item GetItem(uint uint_0)
		{
			Item result;
			if (this.method_1(uint_0))
			{
				result = this.Item[uint_0];
			}
			else
			{
				result = null;
			}
			return result;
		}
        //public bool method_3(int int_0)
        //{
        //    return this.Sound.ContainsKey(int_0);
        //}
        //public Soundtrack method_4(int int_0)
        //{
        //    Soundtrack result;
        //    if (this.method_3(int_0))
        //    {
        //        result = this.Sound[int_0];
        //    }
        //    else
        //    {
        //        result = null;
        //    }
        //    return result;
        //}
	}
}
