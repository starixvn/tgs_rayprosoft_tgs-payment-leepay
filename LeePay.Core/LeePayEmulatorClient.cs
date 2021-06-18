using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Tgs.Payment.LeePay
{
	internal class LeePayEmulatorClient : ILeePayClient
	{
		private readonly LeePayOptions _options;
		private readonly ILeePayClient _realClient;

		public LeePayEmulatorClient(ILeePayClient realClient, IOptions<LeePayOptions> optionsAccessor)
		{
			_realClient = realClient ?? throw new ArgumentNullException(nameof(realClient));
			_options = optionsAccessor.Value;
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
					Channel = order.Channel ?? "ETHEREUM",
					PaymentUrl = $"{_options.CallbackBaseUrl}/deposit/payment/{new Guid(Convert.FromBase64String(order.CustomOrderNo))}"
				}
			});

		public ValueTask<LeePayResponse<DepositOrderDetailResult>> GetDepositOrderAsync(
			string systemId,
			string systemKey,
			string orderNo,
			string customOrderNo) => throw new NotImplementedException();

		public ValueTask<LeePayResponse<QueryExchangeRateResult>> QueryExchangeRateAsync(
			string systemId,
			string systemKey,
			string fromCurrency,
			string toCurrency)
			=> _realClient.QueryExchangeRateAsync(
				systemId,
				systemKey,
				fromCurrency,
				toCurrency);

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
