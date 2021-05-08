using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Tgs.MessageQueue;
using Tgs.Payment.Protos;

namespace Tgs.Payment.LeePay
{
	internal class WithdrawProcessor : IMessageProcessor
	{
		private readonly LeePaySerializer _leePaySerializer;
		private readonly ILogger<WithdrawProcessor> _logger;
		private readonly ILeePayClient _payClient;

		public WithdrawProcessor(ILeePayClient payClient, LeePaySerializer leePaySerializer, ILogger<WithdrawProcessor> logger)
		{
			_payClient = payClient ?? throw new ArgumentNullException(nameof(payClient));
			_leePaySerializer = leePaySerializer ?? throw new ArgumentNullException(nameof(leePaySerializer));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async ValueTask<byte[]> HandleAsync(byte[] data, CancellationToken cancellationToken = default)
		{
			var request = WithdrawRequest.Parser.ParseFrom(data);
			var settings = _leePaySerializer.Deserialize<LeePayWithdrawSettings>(request.ApplyInfo);

			var order = new WithdrawOrder
			{
				CustomOrderNo = Convert.ToBase64String(settings.CustomOrderNo.ToByteArray()),
				Amount = decimal.Parse(request.Amount),
				DestinationAddress = settings.DestinationAddress,
				NotifyUrl = "https://payment.yggdrasill.dev/api/withdraw/callback",
				ChannelCurrency = settings.ChannelCurrency,
				Channel = string.IsNullOrEmpty(settings.Channel) ? null : settings.Channel
			};

			var result = await _payClient
				.WidthdrawAsync(settings.SystemId, settings.SystemKey, order)
				.ConfigureAwait(false);

			var orderResult = result.Data!;

			if (result.Code == "00")
			{
				var response = new LeePayWithdrawResponse(
					result.Code,
					result.Message,
					orderResult.OrderNo,
					new Guid(Convert.FromBase64String(orderResult.CustomOrderNo)),
					orderResult.UsdtAmount,
					orderResult.ChannelCurrency,
					orderResult.ChannelRate,
					orderResult.Channel);

				var applyInfo = new LeePayWithdrawApplyInfo(settings, response);

				var msg = new WithdrawResponse
				{
					Result = _leePaySerializer.Serialize(applyInfo)
				};

				return msg.ToByteArray();
			}
			else
			{
				var response = new LeePayWithdrawResponse(
					result.Code,
					result.Message,
					ResponseContent: result.OriginalResponse);

				var applyInfo = new LeePayWithdrawApplyInfo(settings, response);

				var msg = new WithdrawResponse
				{
					Result = _leePaySerializer.Serialize(applyInfo)
				};

				return msg.ToByteArray();
			}
		}
	}
}
