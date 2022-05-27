using System.Reflection;
using Microsoft.Extensions.Options;
using Tgs.Payment.Clients;
using Tgs.Payment.LeePay;
using Tgs.Payment.LeePay.MessageQueue;

namespace Microsoft.Extensions.DependencyInjection;

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
			.AddPaymentServiceClient((options, sp) =>
			{
				var leePayOptions = sp.GetRequiredService<IOptions<LeePayOptions>>().Value;

				options.PaymentServiceBaseUrl = leePayOptions.PaymentServiceBaseUrl;
			})
			.AddTransient<ILeePayClient>(sp =>
			{
				var options = sp.GetRequiredService<IOptions<LeePayOptions>>().Value;

				return options.IsSimulation
					? ActivatorUtilities.CreateInstance<LeePayEmulatorClient>(
						sp, sp.GetRequiredService<LeePayClient>())
					: sp.GetRequiredService<LeePayClient>();
			})
			.RegisterAllServiceUtilityTypes(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient)
			.AddSingleton<LeePayHasher>()
			.AddMessageQueue()
			.AddNatsMessageQueue(config => config
				.ConfigQueueOptions((options, sp) =>
				{
					var leePayOptions = sp.GetRequiredService<IOptions<LeePayOptions>>().Value;

					options.Url = leePayOptions.NatsUrl;
				})
				.AddProcessor<DepositProcessor>("tgs.payment.leepay.deposit", "tgs.payment.leepay.deposit")
				.AddProcessor<WithdrawProcessor>("tgs.payment.leepay.withdraw", "tgs.payment.leepay.withdraw")
				.AddProcessor<QueryExchangeRateProcessor>("tgs.payment.leepay.queryexchangerate"))
			.AddNatsGlobPatternExchange("*")
			.Services
			.AddPaymentClient();
	}
}
