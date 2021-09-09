using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.MessageQueue;
using Tgs.Payment.Protos;

namespace Tgs.Payment.LeePay.MessageQueue
{
	internal class DepositProcessor : IMessageProcessor
	{
		private readonly LeePaySerializer _leePaySerializer;
		private readonly ILogger<DepositProcessor> _logger;
		private readonly LeePayOptions _options;
		private readonly ILeePayClient _payClient;

		public DepositProcessor(
			IOptions<LeePayOptions> optionsAccessor,
			ILeePayClient payClient,
			LeePaySerializer leePaySerializer,
			ILogger<DepositProcessor> logger)
		{
			_options = optionsAccessor.Value;
			_payClient = payClient ?? throw new ArgumentNullException(nameof(payClient));
			_leePaySerializer = leePaySerializer ?? throw new ArgumentNullException(nameof(leePaySerializer));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async ValueTask<byte[]> HandleAsync(byte[] data, CancellationToken cancellationToken = default)
		{
			var request = DepositRequest.Parser.ParseFrom(data);
			var settings = _leePaySerializer.Deserialize<LeePayDepositSettings>(request.ApplyInfo)!;

			var order = new DepositOrder
			{
				CustomOrderNo = Convert.ToBase64String(settings.CustomOrderNo.ToByteArray()),
				Amount = decimal.Parse(request.Amount),
				NotifyUrl = $"{_options.CallbackBaseUrl}/deposit/callback",
				ReturnUrl = settings.ReturnUrl,
				DepositLimitTime = 60,
				ChannelCurrency = settings.ChannelCurrency,
				Channel = string.IsNullOrEmpty(settings.Channel) ? null : settings.Channel
			};

			var result = await _payClient
				.DepositAsync(settings.SystemId, settings.SystemKey, order)
				.ConfigureAwait(false);

			var orderResult = result.Data!;

			if (result.Code == "00")
			{
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

				var applyInfo = new LeePayDepositApplyInfo(settings, response);

				var msg = new DepositResponse
				{
					Result = _leePaySerializer.Serialize(applyInfo)
				};

				return msg.ToByteArray();
			}
			else
			{
				var response = new LeePayDepositResponse(
					result.Code,
					result.Message,
					ResponseContent: result.OriginalResponse);

				var applyInfo = new LeePayDepositApplyInfo(settings, response);

				var msg = new WithdrawResponse
				{
					Result = _leePaySerializer.Serialize(applyInfo)
				};

				return msg.ToByteArray();
			}
		}
	}
}
