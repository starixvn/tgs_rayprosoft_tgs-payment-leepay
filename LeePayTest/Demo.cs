using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tgs.Payment;
using Tgs.Payment.LeePay;

namespace LeePayTest
{
	internal class Demo : BackgroundService
	{
		private readonly IPaymentClient _paymentClient;
		private readonly LeePaySerializer _leePaySerializer;
		private readonly ILogger<Demo> _logger;

		public Demo(IPaymentClient paymentClient, LeePaySerializer leePaySerializer, ILogger<Demo> logger)
		{
			_paymentClient = paymentClient ?? throw new ArgumentNullException(nameof(paymentClient));
			_leePaySerializer = leePaySerializer ?? throw new ArgumentNullException(nameof(leePaySerializer));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var settings = new LeePayDepositSettings(
				"WG",
				"7ujm5tgB",
				Guid.NewGuid(),
				"CNY",
				"http://localhost",
				null);

			var paymentReturn = await _paymentClient
				.DepositAsync("tgs.payment.leepay", 100M, _leePaySerializer.Serialize(settings))
				.ConfigureAwait(false);
		}
	}
}
