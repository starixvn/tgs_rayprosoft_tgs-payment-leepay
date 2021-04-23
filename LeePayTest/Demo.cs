using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tgs.Payment.LeePay.Client;

namespace LeePayTest
{
	internal class Demo : BackgroundService
	{
		private readonly LeePayClient _payClient;
		private readonly ILogger<Demo> _logger;

		public Demo(LeePayClient payClient, ILogger<Demo> logger)
		{
			_payClient = payClient ?? throw new ArgumentNullException(nameof(payClient));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var applyInfo = new LeePayApplyInfo
			{
				CustomOrderNo = Guid.NewGuid(),
				ChannelCurrency = "CNY",
				ReturnUrl = "http://localhost",
				SystemId = "WG",
				SystemKey = "7ujm5tgB"
			};

			var paymentReturn = await _payClient.DepositAsync(100M, applyInfo).ConfigureAwait(false);
		}
	}
}
