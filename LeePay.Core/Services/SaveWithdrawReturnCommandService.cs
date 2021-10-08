using System;
using System.Threading;
using System.Threading.Tasks;
using Tgs.Payment.Clients;
using TGS.ServiceUtility;

namespace Tgs.Payment.LeePay.Services
{
	internal class SaveWithdrawReturnCommandService : ICommandService<SaveWithdrawReturnCommand>
	{
		private readonly LeePaySerializer _leePaySerializer;
		private readonly IPaymentServiceClient _paymentServiceClient;

		public SaveWithdrawReturnCommandService(IPaymentServiceClient paymentServiceClient, LeePaySerializer leePaySerializer)
		{
			_paymentServiceClient = paymentServiceClient ?? throw new ArgumentNullException(nameof(paymentServiceClient));
			_leePaySerializer = leePaySerializer ?? throw new ArgumentNullException(nameof(leePaySerializer));
		}

		public ValueTask ExecuteAsync(SaveWithdrawReturnCommand command, CancellationToken cancellationToken = default)
			=> _paymentServiceClient.SubmitWithdrawFormStatusAsync(
				command.FormId,
				command.State,
				_leePaySerializer.Serialize(command.ReturnData),
				null);
	}
}
