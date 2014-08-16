using System;
using System.Collections.Generic;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Catalogs;
namespace Phoenix.Communication.Messages.Recycler
{
	internal class GetRecyclerPrizesMessageEvent : MessageEvent
	{
		public void parse(GameClient Session, ClientMessage Event)
		{
			ServerMessage message = new ServerMessage(506);
			message.AppendInt32(5);
			for (uint i = 5; i >= 1; i -= 1)
			{
				message.AppendUInt(i);
				if (i <= 1)
				{
					message.AppendInt32(0);
				}
                else if (i == 2)
                {
                    message.AppendInt32(4);
                }
                else if (i == 3)
                {
                    message.AppendInt32(40);
                }
                else if (i == 4)
                {
                    message.AppendInt32(200);
                }
                else if (i == 5)
                {
                    message.AppendInt32(2000);
                }
				List<EcotronReward> list = PhoenixEnvironment.GetGame().GetCatalog().GetEcotronRewardsForLevel(i);
				message.AppendInt32(list.Count);
				foreach (EcotronReward reward in list)
				{
					message.AppendStringWithBreak(reward.GetBaseItem().Type.ToString().ToLower());
					message.AppendUInt(reward.DisplayId);
				}
			}
			Session.SendMessage(message);
		}
	}
}
