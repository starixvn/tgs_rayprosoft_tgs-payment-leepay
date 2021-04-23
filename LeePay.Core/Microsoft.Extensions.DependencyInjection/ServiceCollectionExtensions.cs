using System;
using Microsoft.Extensions.Options;
using Tgs.Payment.LeePay;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddLeePay(this IServiceCollection services, Action<LeePayOptions, IServiceProvider> configure)
		{
			return services
				.AddOptions<LeePayOptions>()
				.Configure(configure)
				.Services
				.AddHttpClient<LeePayClient>()
				.ConfigureHttpClient((sp, httpClient) =>
				{
					var options = sp.GetRequiredService<IOptions<LeePayOptions>>().Value;

					httpClient.BaseAddress = options.PaymentApiUrl;
				})
				.Services
				.AddNatsMessageQueue(config =>
				{
					config
						.ConfigQueueOptions((options, sp) =>
						{
							var leePayOptions = sp.GetRequiredService<IOptions<LeePayOptions>>().Value;

							options.Url = leePayOptions.NatsUrl;
						})
						.AddProcessor<DepositProcessor>("tgs.payment.leepay", "tgs.payment.leepay");
				});
		}
	}
}
