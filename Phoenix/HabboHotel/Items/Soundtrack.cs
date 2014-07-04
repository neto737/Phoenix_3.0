using System;
namespace Phoenix.HabboHotel.Items
{
	internal sealed class Soundtrack
	{
		public int Id;
		public string Name;
		public string Author;
		public string Track;
		public int Length;
		public Soundtrack(int Id, string name, string author, string track, int length)
		{
			this.Id = Id;
			this.Name = name;
			this.Author = author;
			this.Track = track;
			this.Length = length * 1000;
		}
	}
}
