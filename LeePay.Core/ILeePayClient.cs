using System.Threading.Tasks;

namespace Tgs.Payment.LeePay
{
	internal interface ILeePayClient
	{
		ValueTask<LeePayResponse<DepositResult>> DepositAsync(string systemId, string systemKey, DepositOrder order);

		ValueTask<LeePayResponse<WithdrawResult>> WidthdrawAsync(string systemId, string systemKey, WithdrawOrder order);

		ValueTask<LeePayResponse<QueryExchangeRateResult>> QueryExchangeRateAsync(
			string systemId,
			string systemKey,
			string fromCurrency,
			string toCurrency);

		ValueTask<LeePayResponse<DepositOrderDetailResult>> GetDepositOrderAsync(
			string systemId,
			string systemKey,
			string orderNo,
			string customOrderNo);
	}
}