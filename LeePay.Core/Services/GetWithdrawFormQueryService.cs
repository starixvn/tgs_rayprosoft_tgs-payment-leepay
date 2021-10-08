using System;
using System.Threading;
using System.Threading.Tasks;
using Tgs.Payment.Clients;
using TGS.ServiceUtility;

namespace Tgs.Payment.LeePay.Services
{
	internal class GetWithdrawFormQueryService : IGetService<GetWithdrawFormQuery, WithdrawFormInfo?>
	{
		private readonly LeePaySerializer _leePaySerializer;
		private readonly IPaymentServiceClient _paymentServiceClient;

		public GetWithdrawFormQueryService(IPaymentServiceClient paymentServiceClient, LeePaySerializer leePaySerializer)
		{
			_paymentServiceClient = paymentServiceClient ?? throw new ArgumentNullException(nameof(paymentServiceClient));
			_leePaySerializer = leePaySerializer ?? throw new ArgumentNullException(nameof(leePaySerializer));
		}

		public async ValueTask<WithdrawFormInfo?> GetAsync(GetWithdrawFormQuery query, CancellationToken cancellationToken = default)
		{
			var withdrawFormId = new Guid(Convert.FromBase64String(query.CustomOrderNo));

			var withdrawFormData = await _paymentServiceClient
				.GetWithdrawFormAsync(withdrawFormId)
				.ConfigureAwait(false);

			if (withdrawFormData == null)
				return null;

			var applyInfo = string.IsNullOrEmpty(withdrawFormData.ApplyInfo)
				? null
				: _leePaySerializer.Deserialize<LeePayWithdrawApplyInfo>(withdrawFormData.ApplyInfo);

			return new WithdrawFormInfo(
				withdrawFormData.Id,
				withdrawFormData.UserId,
				withdrawFormData.UserType,
				withdrawFormData.BrandId,
				withdrawFormData.BrandPaymentId,
				withdrawFormData.Amount,
				withdrawFormData.Status,
				withdrawFormData.BankInfoId,
				withdrawFormData.BankInfo,
				applyInfo,
				withdrawFormData.PaymentReturn);
		}
	}
}
