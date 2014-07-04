using System;
using System.Data;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.Catalogs
{
	internal sealed class VoucherHandler
	{
		public bool IsValidCode(string Code)
		{
			using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("code", Code);
				if (adapter.ReadDataRow("SELECT null FROM vouchers WHERE code = @code LIMIT 1") != null)
				{
					return true;
				}
			}
			return false;
		}

		public void DeleteVoucher(string Code)
		{
			using (DatabaseClient dbClient = PhoenixEnvironment.GetDatabase().GetClient())
			{
				dbClient.AddParamWithValue("code", Code);
				dbClient.ExecuteQuery("DELETE FROM vouchers WHERE code = @code LIMIT 1");
			}
		}

		public void TryRedeemVoucher(GameClient Session, string Code)
		{
			if (!IsValidCode(Code))
			{
				ServerMessage Message = new ServerMessage(213);
				Message.AppendRawInt32(0);
				Session.SendMessage(Message);
			}
			else
			{
				DataRow row = null;
				using (DatabaseClient adapter = PhoenixEnvironment.GetDatabase().GetClient())
				{
					adapter.AddParamWithValue("code", Code);
					row = adapter.ReadDataRow("SELECT * FROM vouchers WHERE code = @code LIMIT 1");
				}
				int num = (int)row["credits"];
				int num2 = (int)row["pixels"];
				int num3 = (int)row["vip_points"];
				this.DeleteVoucher(Code);
				if (num > 0)
				{
					Session.GetHabbo().Credits += num;
					Session.GetHabbo().UpdateCreditsBalance(true);
				}
				if (num2 > 0)
				{
					Session.GetHabbo().ActivityPoints += num2;
					Session.GetHabbo().UpdateActivityPointsBalance(true);
				}
				if (num3 > 0)
				{
					Session.GetHabbo().shells += num3;
					Session.GetHabbo().UpdateShellsBalance(false, true);
				}
				Session.SendMessage(new ServerMessage(212));
			}
		}
	}
}
