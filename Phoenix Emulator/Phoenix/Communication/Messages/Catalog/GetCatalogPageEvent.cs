using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Catalogs;
namespace Phoenix.Communication.Messages.Catalog
{
	internal class GetCatalogPageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			CatalogPage Page = PhoenixEnvironment.GetGame().GetCatalog().GetPage(Event.PopWiredInt32());
			if (Page != null && Page.Enabled && Page.Visible && Page.MinRank <= Session.GetHabbo().Rank)
			{
				if (Page.ClubOnly && !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
				{
					Session.SendNotif("This page is for Phoenix Club members only!");
				}
				else
				{
					Session.SendMessage(Page.GetMessage);
					if (Page.Layout == "recycler")
					{
						ServerMessage Message = new ServerMessage(507);
						Message.AppendBoolean(true);
						Message.AppendBoolean(false);
						Session.SendMessage(Message);
					}
					else
					{
						if (Page.Layout == "club_buy")
						{
							ServerMessage Message2 = new ServerMessage(625);
							if (Session.GetHabbo().Vip)
							{
								Message2.AppendInt32(2);
								Message2.AppendInt32(4535);
								Message2.AppendStringWithBreak("HABBO_CLUB_VIP_1_MONTH");
								Message2.AppendInt32(25);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(1);
								Message2.AppendInt32(101);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Day);
								Message2.AppendInt32(4536);
								Message2.AppendStringWithBreak("HABBO_CLUB_VIP_3_MONTHS");
								Message2.AppendInt32(60);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(3);
								Message2.AppendInt32(163);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Day);
							}
							else
							{
								Message2.AppendInt32(4);
								Message2.AppendInt32(4533);
								Message2.AppendStringWithBreak("HABBO_CLUB_BASIC_1_MONTH");
								Message2.AppendInt32(15);
								Message2.AppendInt32(0);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(31);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Day);
								Message2.AppendInt32(4534);
								Message2.AppendStringWithBreak("HABBO_CLUB_BASIC_3_MONTHS");
								Message2.AppendInt32(45);
								Message2.AppendInt32(0);
								Message2.AppendInt32(0);
								Message2.AppendInt32(3);
								Message2.AppendInt32(93);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Day);
								Message2.AppendInt32(4535);
								Message2.AppendStringWithBreak("HABBO_CLUB_VIP_1_MONTH");
								Message2.AppendInt32(25);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(1);
								Message2.AppendInt32(101);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Day);
								Message2.AppendInt32(4536);
								Message2.AppendStringWithBreak("HABBO_CLUB_VIP_3_MONTHS");
								Message2.AppendInt32(60);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(3);
								Message2.AppendInt32(163);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Day);
							}
							Session.SendMessage(Message2);
						}
					}
				}
			}
		}
	}
}
