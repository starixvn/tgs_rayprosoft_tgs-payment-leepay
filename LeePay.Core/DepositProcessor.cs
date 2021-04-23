using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Tgs.MessageQueue;

namespace Tgs.Payment.LeePay
{
	internal class DepositProcessor : IMessageProcessor
	{
		private readonly LeePayClient _payClient;
		private readonly ILogger<DepositProcessor> _logger;

		public DepositProcessor(LeePayClient payClient, ILogger<DepositProcessor> logger)
		{
			_payClient = payClient ?? throw new ArgumentNullException(nameof(payClient));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async ValueTask<byte[]> HandleAsync(byte[] data, CancellationToken cancellationToken = default)
		{
			var request = DepositRequest.Parser.ParseFrom(data);

			var order = new DepositOrder
			{
				CustomOrderNo = request.CustomOrderNo,
				Amount = decimal.Parse(request.Amount),
				NotifyUrl = "https://payment.yggdrasill.dev/api/deposit/callback",
				ReturnUrl = request.ReturnUrl,
				DepositLimitTime = 1,
				ChannelCurrency = request.ChannelCurrency,
				Channel = string.IsNullOrEmpty(request.Channel) ? null : request.Channel
			};

			var result = await _payClient
				.DepositAsync(request.SystemId, request.SystemKey, order)
				.ConfigureAwait(false);

			var orderResult = result.Data!;

			var response = new DepositResponse
			{
				Code = result.Code,
				CustomOrderNo = orderResult.CustomOrderNo,
				OrderId = orderResult.OrderId,
				OrderNo = orderResult.OrderNo,
				Message = result.Message ?? string.Empty,
				WalletAddress = orderResult.WalletAddress,
				PaymentUrl = orderResult.PaymentUrl,
				UsdtAmount = orderResult.UsdtAmount.ToString(),
				ChannelCurrency = orderResult.ChannelCurrency,
				ChannelRate = orderResult.ChannelRate.ToString(),
				Channel = orderResult.Channel
			};

			return response.ToByteArray();
		}
	}
}
