using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Tgs.MessageQueue;
using Tgs.Payment.Protos;

namespace Tgs.Payment.LeePay
{
	internal class DepositProcessor : IMessageProcessor
	{
		private readonly LeePayClient _payClient;
		private readonly LeePaySerializer _leePaySerializer;
		private readonly ILogger<DepositProcessor> _logger;

		public DepositProcessor(LeePayClient payClient, LeePaySerializer leePaySerializer, ILogger<DepositProcessor> logger)
		{
			_payClient = payClient ?? throw new ArgumentNullException(nameof(payClient));
			_leePaySerializer = leePaySerializer ?? throw new ArgumentNullException(nameof(leePaySerializer));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async ValueTask<byte[]> HandleAsync(byte[] data, CancellationToken cancellationToken = default)
		{
			var request = DepositRequest.Parser.ParseFrom(data);
			var settings = _leePaySerializer.Deserialize<LeePayDepositSettings>(request.ApplyInfo);

			var order = new DepositOrder
			{
				CustomOrderNo = Convert.ToBase64String(settings.CustomOrderNo.ToByteArray()),
				Amount = decimal.Parse(request.Amount),
				NotifyUrl = "https://payment.yggdrasill.dev/api/deposit/callback",
				ReturnUrl = settings.ReturnUrl,
				DepositLimitTime = 1,
				ChannelCurrency = settings.ChannelCurrency,
				Channel = string.IsNullOrEmpty(settings.Channel) ? null : settings.Channel
			};

			var result = await _payClient
				.DepositAsync(settings.SystemId, settings.SystemKey, order)
				.ConfigureAwait(false);

			var orderResult = result.Data!;

			var response = new LeePayDepositResponse(
				result.Code,
				result.Message,
				orderResult.OrderId,
				orderResult.OrderNo,
				new Guid(Convert.FromBase64String(orderResult.CustomOrderNo)),
				orderResult.WalletAddress,
				orderResult.PaymentUrl,
				orderResult.UsdtAmount,
				orderResult.ChannelCurrency,
				orderResult.ChannelRate,
				orderResult.Channel);

			var applyInfo = new LeePayApplyInfo(settings, response);

			var msg = new DepositResponse
			{
				Result = _leePaySerializer.Serialize(applyInfo)
			};

			return msg.ToByteArray();
		}
	}
}
