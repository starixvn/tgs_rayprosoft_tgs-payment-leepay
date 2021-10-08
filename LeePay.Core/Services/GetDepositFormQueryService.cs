using System;
using System.Threading;
using System.Threading.Tasks;
using Tgs.Payment.Clients;
using TGS.ServiceUtility;

namespace Tgs.Payment.LeePay.Services
{
	internal class GetDepositFormQueryService : IGetService<GetDepositFormQuery, DepositFormInfo?>
	{
		private readonly LeePaySerializer _leePaySerializer;
		private readonly IPaymentServiceClient _paymentServiceClient;

		public GetDepositFormQueryService(IPaymentServiceClient paymentServiceClient, LeePaySerializer leePaySerializer)
		{
			_paymentServiceClient = paymentServiceClient ?? throw new ArgumentNullException(nameof(paymentServiceClient));
			_leePaySerializer = leePaySerializer ?? throw new ArgumentNullException(nameof(leePaySerializer));
		}

		public async ValueTask<DepositFormInfo?> GetAsync(GetDepositFormQuery query, CancellationToken cancellationToken = default)
		{
			var depositFormId = new Guid(Convert.FromBase64String(query.CustomOrderNo));

			var formData = await _paymentServiceClient
				.GetDepositFormAsync(depositFormId)
				.ConfigureAwait(false);

			if (formData == null)
				return null;

			var applyInfo = string.IsNullOrEmpty(formData.ApplyInfo)
				? null
				: _leePaySerializer.Deserialize<LeePayDepositApplyInfo>(formData.ApplyInfo);

			return formData == null
				? null
				: new DepositFormInfo(
					formData.Id,
					formData.PlayerCode,
					formData.BrandId,
					formData.BrandPaymentId,
					formData.Amount,
					formData.Status,
					applyInfo,
					formData.PaymentReturn);
		}
	}
}
