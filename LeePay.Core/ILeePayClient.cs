using System.Threading.Tasks;

namespace Tgs.Payment.LeePay
{
	internal interface ILeePayClient
	{
		ValueTask<LeePayResponse<DepositResult>> DepositAsync(string systemId, string systemKey, DepositOrder order);

		ValueTask<LeePayResponse<WithdrawResult>> WidthdrawAsync(string systemId, string systemKey, WithdrawOrder order);

		ValueTask<LeePayResponse<ExchangeRateResult>> ExchangeRateAsync(string systemId, string systemKey, ExchangeRateOrder order);
	}
}