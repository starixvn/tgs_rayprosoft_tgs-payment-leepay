using System;
using System.Threading;
using System.Threading.Tasks;
using Tgs.ServiceUtility;

namespace Tgs.Payment.LeePay.Services;

internal class GetLeePayDepositOrderQueryService : IGetService<GetLeePayDepositOrderQuery, DepositOrderDetailResult?>
{
	private readonly ILeePayClient _leePayClient;

	public GetLeePayDepositOrderQueryService(ILeePayClient leePayClient)
	{
		_leePayClient = leePayClient ?? throw new ArgumentNullException(nameof(leePayClient));
	}

	public async ValueTask<DepositOrderDetailResult?> GetAsync(
		GetLeePayDepositOrderQuery query,
		CancellationToken cancellationToken = default)
	{
		var response = await _leePayClient
			.GetDepositOrderAsync(
				query.SystemId,
				query.SystemKey,
				query.OrderNo,
				query.CustomOrderNo)
			.ConfigureAwait(false);

		return response.Code == "00" ? response.Data : throw new Exception(response.OriginalResponse);
	}
}
