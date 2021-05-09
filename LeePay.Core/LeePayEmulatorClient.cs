using System;
using System.Threading.Tasks;

namespace Tgs.Payment.LeePay
{
	internal class LeePayEmulatorClient : ILeePayClient
	{
		private readonly ILeePayClient _realClient;

		public LeePayEmulatorClient(ILeePayClient realClient)
		{
			_realClient = realClient ?? throw new ArgumentNullException(nameof(realClient));
		}

		public ValueTask<LeePayResponse<DepositResult>> DepositAsync(string systemId, string systemKey, DepositOrder order)
			=> ValueTask.FromResult(new LeePayResponse<DepositResult>
			{
				Code = "00",
				Data = new DepositResult
				{
					OrderId = Guid.NewGuid().ToString("N"),
					CustomOrderNo = order.CustomOrderNo,
					OrderNo = "WG-050714-cca518-A",
					UsdtAmount = order.Amount,
					ChannelCurrency = order.ChannelCurrency,
					ChannelRate = 1,
					Channel = order.Channel ?? "ETHEREUM"
				}
			});

		public ValueTask<LeePayResponse<WithdrawResult>> WidthdrawAsync(string systemId, string systemKey, WithdrawOrder order)
			=> ValueTask.FromResult(new LeePayResponse<WithdrawResult>
			{
				Code = "00",
				Data = new WithdrawResult
				{
					OrderId = Guid.NewGuid(),
					OrderNo = "WG-050717-3EaSnG-A",
					CustomOrderNo = order.CustomOrderNo,
					UsdtAmount = order.Amount,
					ChannelCurrency = order.ChannelCurrency,
					ChannelRate = 1,
					Channel = order.Channel ?? "ETHEREUM"
				}
			});
	}
}
