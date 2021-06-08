using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tgs.Payment.Clients;
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
			//var settings = new LeePayDepositSettings(
			//	"WG",
			//	"7ujm5tgB",
			//	Guid.NewGuid(),
			//	"USDT",
			//	"http://localhost",
			//	"TRON");

			//var paymentReturn = await _paymentClient
			//	.DepositAsync("tgs.payment.leepay.deposit", 2M, _leePaySerializer.Serialize(settings))
			//	.ConfigureAwait(false);

			//var withdrawSettings = new LeePayWithdrawSettings(
			//	"WG",
			//	"7ujm5tgB",
			//	Guid.NewGuid(),
			//	"TPRRBg6zL54vm8AHpbLJAeYqem4ntJsUKa",
			//	"USDT",
			//	"TRON");

			//var withdrawReturn = await _paymentClient
			//	.WithdrawAsync("tgs.payment.leepay.withdraw", 0M, _leePaySerializer.Serialize(withdrawSettings))
			//	.ConfigureAwait(false);

			var exchangeRateSettings = new LeePayQueryExchangeRateSettings(
				"WG",
				"7ujm5tgB",
				"CNY",
				"USDT"
				);

			var exchangeReturn = await _paymentClient
				.AskAsync("tgs.payment.leepay.queryexchangerate", _leePaySerializer.Serialize(exchangeRateSettings))
				.ConfigureAwait(false);

			Console.WriteLine(exchangeReturn);
		}
	}
}