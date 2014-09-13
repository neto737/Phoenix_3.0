using System;
using System.Collections;
using System.Collections.Generic;
using Phoenix.Catalogs;
using Phoenix.Messages;
namespace Phoenix.HabboHotel.Catalogs
{
	internal class CatalogPage
	{
		private int Id;
		public int ParentId;
		public string Caption;
		public bool Visible;
		public bool Enabled;
		public uint MinRank;
		public bool ClubOnly;
		public int IconColor;
		public int IconImage;
		public string Layout;
		public string LayoutHeadline;
		public string LayoutTeaser;
		public string LayoutSpecial;
		public string Text1;
		public string Text2;
		public string TextDetails;
		public string TextTeaser;
		public string TextLinkDesc;
		public string TextLinkPage;
		public List<CatalogItem> Items;
		private ServerMessage mMessage;

		public int PageId
		{
			get
			{
				return this.Id;
			}
		}

		internal ServerMessage GetMessage
		{
			get
			{
				return this.mMessage;
			}
		}

		public CatalogPage(int Id, int ParentId, string Caption, bool Visible, bool Enabled, uint MinRank, bool ClubOnly, int IconColor, int IconImage, string Layout, string LayoutHeadline, string LayoutTeaser, string LayoutSpecial, string Text1, string Text2, string TextDetails, string TextTeaser, string TextLinkDesc, string TextLinkPage, ref Hashtable CataItems)
		{
			this.Items = new List<CatalogItem>();
			this.Id = Id;
			this.ParentId = ParentId;
			this.Caption = Caption;
			this.Visible = Visible;
			this.Enabled = Enabled;
			this.MinRank = MinRank;
			this.ClubOnly = ClubOnly;
			this.IconColor = IconColor;
			this.IconImage = IconImage;
			this.Layout = Layout;
			this.LayoutHeadline = LayoutHeadline;
			this.LayoutTeaser = LayoutTeaser;
			this.LayoutSpecial = LayoutSpecial;
			this.Text1 = Text1;
			this.Text2 = Text2;
			this.TextDetails = TextDetails;
			this.TextTeaser = TextTeaser;
			this.TextLinkDesc = TextLinkDesc;
			this.TextLinkPage = TextLinkPage;
			foreach (CatalogItem item in CataItems.Values)
			{
				if (item.PageID == Id)
				{
					this.Items.Add(item);
				}
			}
		}

		internal void InitMsg()
		{
			this.mMessage = PhoenixEnvironment.GetGame().GetCatalog().SerializePage(this);
		}

		public CatalogItem GetItem(uint Id)
		{
            for (int i = 0; i < Items.Count; i++)
            {
                CatalogItem Item = Items[i]; if (Item == null) { continue; }

                if (Item.Id == Id)
                {
                    return Item;
                }
            }
			return null;
		}

		public void Serialize(int Rank, ServerMessage Message)
		{
			Message.AppendInt32(this.IconColor);
			Message.AppendInt32(this.IconImage);
			Message.AppendInt32(this.Id);
			Message.AppendStringWithBreak(this.Caption);
			Message.AppendInt32(PhoenixEnvironment.GetGame().GetCatalog().GetTreeSize(Rank, Id));
			Message.AppendBoolean(this.Visible);
		}
	}
}
