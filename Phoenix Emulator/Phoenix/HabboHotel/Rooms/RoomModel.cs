using System;
using System.Globalization;
using System.Text;
using Phoenix.Messages;
using Phoenix.Util;
namespace Phoenix.HabboHotel.Rooms
{
	internal sealed class RoomModel
	{
		public string Name;
		public int DoorX;
		public int DoorY;
		public double DoorZ;
		public int DoorOrientation;
		public string Heightmap;
		public SquareState[,] SqState;
		public double[,] SqFloorHeight;
		public int[,] SqSeatRot;
		public int MapSizeX;
		public int MapSizeY;
		public string StaticFurniMap;
		public bool ClubOnly;

		public RoomModel(string mName, int mDoorX, int mDoorY, double mDoorZ, int mDoorOrientation, string mHeightmap, string mStaticFurniMap, bool mClubOnly)
		{
			this.Name = mName;

			this.DoorX = mDoorX;
			this.DoorY = mDoorY;
			this.DoorZ = mDoorZ;
			this.DoorOrientation = mDoorOrientation;

			this.Heightmap = mHeightmap.ToLower();
			this.StaticFurniMap = mStaticFurniMap;

            string[] tmpHeightmap = mHeightmap.Split(Convert.ToChar(13));

			this.MapSizeX = tmpHeightmap[0].Length;
			this.MapSizeY = tmpHeightmap.Length;

			this.ClubOnly = mClubOnly;

			SqState = new SquareState[MapSizeX, MapSizeY];
			SqFloorHeight = new double[MapSizeX, MapSizeY];
			SqSeatRot = new int[MapSizeX, MapSizeY];

			for (int y = 0; y < this.MapSizeY; y++)
			{
				if (y > 0)
				{
					tmpHeightmap[y] = tmpHeightmap[y].Substring(1);
				}
				for (int x = 0; x < this.MapSizeX; x++)
				{
					string Square = tmpHeightmap[y].Substring(x, 1).Trim().ToLower();
					if (Square == "x")
					{
						this.SqState[x, y] = SquareState.BLOCKED;
					}
					else
					{
						if (this.method_0(Square, NumberStyles.Integer))
						{
							this.SqState[x, y] = SquareState.OPEN;
							this.SqFloorHeight[x, y] = double.Parse(Square);
						}
					}
				}
			}
			this.SqFloorHeight[mDoorX, mDoorY] = mDoorZ;

			int pointer = 0;
			int num2 = 0;
			if (mStaticFurniMap != "")
			{
				num2 = OldEncoding.decodeVL64(mStaticFurniMap);
			}
			pointer += OldEncoding.encodeVL64(num2).Length;
			for (int k = 0; k < num2; k++)
			{
				mStaticFurniMap.Substring(pointer);
				int num3 = OldEncoding.decodeVL64(mStaticFurniMap.Substring(pointer));
				pointer += OldEncoding.encodeVL64(num3).Length;
				mStaticFurniMap.Substring(pointer, 1);
				pointer++;
				int.Parse(mStaticFurniMap.Substring(pointer).Split(new char[]
				{
					Convert.ToChar(2)
				})[0]);
				pointer += mStaticFurniMap.Substring(pointer).Split(new char[]
				{
					Convert.ToChar(2)
				})[0].Length;
				pointer++;
				string text2 = mStaticFurniMap.Substring(pointer).Split(new char[]
				{
					Convert.ToChar(2)
				})[0];
				pointer += mStaticFurniMap.Substring(pointer).Split(new char[]
				{
					Convert.ToChar(2)
				})[0].Length;
				pointer++;
				int j = OldEncoding.decodeVL64(mStaticFurniMap.Substring(pointer));
				pointer += OldEncoding.encodeVL64(j).Length;
				int i = OldEncoding.decodeVL64(mStaticFurniMap.Substring(pointer));
				pointer += OldEncoding.encodeVL64(i).Length;
				int num4 = OldEncoding.decodeVL64(mStaticFurniMap.Substring(pointer));
				pointer += OldEncoding.encodeVL64(num4).Length;
				int num5 = OldEncoding.decodeVL64(mStaticFurniMap.Substring(pointer));
				pointer += OldEncoding.encodeVL64(num5).Length;
				this.SqState[j, i] = SquareState.BLOCKED;
				if (text2.Contains("bench") || text2.Contains("chair") || text2.Contains("stool") || text2.Contains("seat") || text2.Contains("sofa"))
				{
					this.SqState[j, i] = SquareState.SEAT;
					this.SqSeatRot[j, i] = num5;
				}
			}
		}
		public bool method_0(string string_3, NumberStyles numberStyles_0)
		{
			double num;
			return double.TryParse(string_3, numberStyles_0, CultureInfo.CurrentCulture, out num);
		}
		public ServerMessage method_1()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = this.Heightmap.Split("\r\n".ToCharArray());
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (!(text == ""))
				{
					stringBuilder.Append(text);
					stringBuilder.Append(Convert.ToChar(13));
				}
			}
			ServerMessage Message = new ServerMessage(31u);
			Message.AppendStringWithBreak(stringBuilder.ToString());
			return Message;
		}
		public ServerMessage method_2()
		{
			ServerMessage Message = new ServerMessage(470u);
			string[] array = this.Heightmap.Split(new char[]
			{
				Convert.ToChar(13)
			});
			for (int i = 0; i < this.MapSizeY; i++)
			{
				if (i > 0)
				{
					array[i] = array[i].Substring(1);
				}
				for (int j = 0; j < this.MapSizeX; j++)
				{
					string text = array[i].Substring(j, 1).Trim().ToLower();
					if (this.DoorX == j && this.DoorY == i)
					{
						text = string.Concat((int)this.DoorZ);
					}
					Message.AppendString(text);
				}
				Message.AppendString(string.Concat(Convert.ToChar(13)));
			}
			return Message;
		}
	}
}
