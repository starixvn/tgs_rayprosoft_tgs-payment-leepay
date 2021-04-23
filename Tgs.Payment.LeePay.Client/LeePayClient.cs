using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Tgs.MessageQueue;
using Tgs.Payment.LeePay;

namespace Tgs.Payment.LeePay
{
	public class LeePayClient
	{
		private readonly IMessageSender _messageSender;
		private readonly ILogger<LeePayClient> _logger;

		public LeePayClient(IMessageSender messageSender, ILogger<LeePayClient> logger)
		{
			_messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async ValueTask<LeePayPaymentReturn> DepositAsync(decimal amount, LeePayApplyInfo applyInfo)
		{
			var request = new DepositRequest
			{
				SystemId = applyInfo.SystemId,
				SystemKey = applyInfo.SystemKey,
				CustomOrderNo = Convert.ToBase64String(applyInfo.CustomOrderNo.ToByteArray()),
				Amount = amount.ToString(),
				ReturnUrl = applyInfo.ReturnUrl,
				ChannelCurrency = applyInfo.ChannelCurrency,
				Channel = applyInfo.Channel ?? string.Empty
			};

			var result = await _messageSender
				.RequestAsync("tgs.payment.leepay", request.ToByteArray())
				.ConfigureAwait(false);

			var response = DepositResponse.Parser.ParseFrom(result);

			_logger.LogInformation(JsonFormatter.ToDiagnosticString(response));

			return new LeePayPaymentReturn(
				response.Code,
				string.IsNullOrEmpty(response.Message) ? null : response.Message,
				response.OrderId,
				response.OrderNo,
				Guid.Parse(response.CustomOrderNo),
				response.WalletAddress,
				response.PaymentUrl,
				decimal.Parse(response.UsdtAmount),
				response.ChannelCurrency,
				decimal.Parse(response.ChannelRate),
				string.IsNullOrEmpty(response.Channel) ? null : response.Channel);
		}
	}
}
