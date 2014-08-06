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
				int Credits = (int)row["credits"];
				int Pixels = (int)row["pixels"];
				int Points = (int)row["vip_points"];
				this.DeleteVoucher(Code);
				if (Credits > 0)
				{
					Session.GetHabbo().Credits += Credits;
					Session.GetHabbo().UpdateCreditsBalance(true);
				}
				if (Pixels > 0)
				{
					Session.GetHabbo().ActivityPoints += Pixels;
					Session.GetHabbo().UpdateActivityPointsBalance(true);
				}
				if (Points > 0)
				{
					Session.GetHabbo().shells += Points;
					Session.GetHabbo().UpdateShellsBalance(false, true);
				}
				Session.SendMessage(new ServerMessage(212));
			}
		}
	}
}
