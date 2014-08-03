using System;
using System.Data;
using Phoenix.HabboHotel.Users.UserDataManagement;
using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Users;
namespace Phoenix.HabboHotel.Users.Authenticator
{
	internal class Authenticator
	{
		internal static Habbo TryLoginHabbo(string AuthTicket, GameClient Session, HabboData pData, HabboData UserData)
		{
			return GenerateHabbo(pData.GetHabboDataRow, AuthTicket, Session, UserData);
		}

        private static Habbo GenerateHabbo(DataRow Data, string AuthTicket, GameClient Client, HabboData UserData)
        {
            uint id = (uint)Data["id"];
            string username = (string)Data["username"];
            string realName = (string)Data["real_name"];
            uint rank = (uint)Data["rank"];
            string motto = (string)Data["motto"];
            string ip_last = (string)Data["ip_last"];
            string look = (string)Data["look"];
            string gender = (string)Data["gender"];
            int credits = (int)Data["credits"];
            int activityPoints = (int)Data["activity_points"];
            return new Habbo(id, username, realName, AuthTicket, rank, motto, look, gender, credits, activityPoints, (double)Data["activity_points_lastupdate"], PhoenixEnvironment.EnumToBool(Data["is_muted"].ToString()), (uint)Data["home_room"], (int)Data["newbie_status"], PhoenixEnvironment.EnumToBool(Data["block_newfriends"].ToString()), PhoenixEnvironment.EnumToBool(Data["hide_inroom"].ToString()), PhoenixEnvironment.EnumToBool(Data["hide_online"].ToString()), PhoenixEnvironment.EnumToBool(Data["vip"].ToString()), (int)Data["volume"], (int)Data["vip_points"], PhoenixEnvironment.EnumToBool(Data["accept_trading"].ToString()), ip_last, Client, UserData, PhoenixEnvironment.EnumToBool(Data["friend_stream_enabled"].ToString()));
        }

		internal static Habbo GetHabboViaUsername(string Data)
		{
			HabboData userData = new HabboData(Data, false);
			return GenerateHabbo(userData.GetHabboDataRow, "", null, userData);
		}
	}
}
