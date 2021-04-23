using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Tgs.MessageQueue;

namespace Tgs.Payment.LeePay.Client
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

			return new LeePayPaymentReturn
			{
				Code = response.Code,
				Message = string.IsNullOrEmpty(response.Message) ? null : response.Message,
				OrderId = response.OrderId,
				OrderNo = response.OrderNo,
				CustomOrderNo = Guid.Parse(response.CustomOrderNo),
				WalletAddress = response.WalletAddress,
				PaymentUrl = response.PaymentUrl,
				UsdtAmount = decimal.Parse(response.UsdtAmount),
				ChannelCurrency = response.ChannelCurrency,
				ChannelRate = decimal.Parse(response.ChannelRate),
				Channel = string.IsNullOrEmpty(response.Channel) ? null : response.Channel
			};
		}
	}
}
