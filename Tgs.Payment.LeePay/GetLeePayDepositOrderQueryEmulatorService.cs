using System.Threading;
using System.Threading.Tasks;
using Tgs.Payment.LeePay.Services;
using TGS.ServiceUtility;

namespace Tgs.Payment.LeePay
{
	public class GetLeePayDepositOrderQueryEmulatorService : IGetService<GetLeePayDepositOrderQuery, DepositOrderDetailResult>
	{
		private readonly DepositOrderDetailResult _dummyResult;

		public GetLeePayDepositOrderQueryEmulatorService(DepositOrderDetailResult dummyResult)
		{
			_dummyResult = dummyResult;
		}

		public ValueTask<DepositOrderDetailResult> GetAsync(
			GetLeePayDepositOrderQuery query,
			CancellationToken cancellationToken = default)
			=> ValueTask.FromResult(_dummyResult);
	}
}
