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
	internal class ExchangeRateProcessor : IMessageProcessor
	{
		private readonly LeePaySerializer _leePaySerializer;
		private readonly ILogger<ExchangeRateProcessor> _logger;
		private readonly LeePayOptions _options;
		private readonly ILeePayClient _payClient;

		public ExchangeRateProcessor(
			IOptions<LeePayOptions> optionsAccessor,
			ILeePayClient payClient,
			LeePaySerializer leePaySerializer,
			ILogger<ExchangeRateProcessor> logger)
		{
			_options = optionsAccessor.Value;
			_payClient = payClient ?? throw new ArgumentNullException(nameof(payClient));
			_leePaySerializer = leePaySerializer ?? throw new ArgumentNullException(nameof(leePaySerializer));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async ValueTask<byte[]> HandleAsync(byte[] data, CancellationToken cancellationToken = default)
		{
			var request = GeneralRequest.Parser.ParseFrom(data);
			var settings = _leePaySerializer.Deserialize<LeePayExchangeRateSettings>(request.Content);

			var order = new ExchangeRateOrder
			{
				From = settings.From,
				To = settings.To
			};

			var result = await _payClient
				.ExchangeRateAsync(settings.SystemId, settings.SystemKey, order)
				.ConfigureAwait(false);

			var orderResult = result.Data!;

			if (result.Code == "00")
			{
				var response = new LeePayExchangeRateResponse(
					result.Code,
					orderResult.BuyPrice,
					orderResult.SellPrice,
					orderResult.BizTime);

				var applyInfo = new LeePayExchangeRateApplyInfo(settings, response);

				var msg = new WithdrawResponse
				{
					Result = _leePaySerializer.Serialize(applyInfo)
				};

				return msg.ToByteArray();
			}
			else
			{
				var response = new LeePayExchangeRateResponse(
					result.Code,
					null,
					null,
					null,
					result.ClientCode,
					result.Message,
					result.TraceId,
					result.DisplayMessage,
					result.Timestamp,
					result.Path);

				var applyInfo = new LeePayExchangeRateApplyInfo(settings, response);

				var msg = new GeneralResponse
				{
					Result = _leePaySerializer.Serialize(applyInfo)
				};

				return msg.ToByteArray();
			}
		}
	}
}