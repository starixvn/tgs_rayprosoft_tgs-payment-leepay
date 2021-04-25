using System;
using System.Threading;
using System.Threading.Tasks;
using TGS.ServiceUtility;

namespace Tgs.Payment.LeePay.Services
{
	internal class GetDepositFormQueryService : IGetService<GetDepositFormQuery, DepositFormInfo>
	{
		private readonly ITgsRepository _tgsRepository;

		public GetDepositFormQueryService(ITgsRepository tgsRepository)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		}

		public ValueTask<DepositFormInfo> GetAsync(GetDepositFormQuery query, CancellationToken cancellationToken = default)
		{
			var depositFormId = new Guid(Convert.FromBase64String(query.CustomOrderNo));

			return _tgsRepository
				.GetDepositFormByIdAsync(depositFormId);
		}
	}
}
