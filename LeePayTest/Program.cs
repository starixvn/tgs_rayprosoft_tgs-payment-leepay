using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tgs.Payment.LeePay;

namespace LeePayTest
{
	internal class Program
	{
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices(services =>
				{
					services.AddNatsMessageQueue(config =>
					{
						config.ConfigQueueOptions((options, ap) =>
						{
							options.Url = "nats://192.168.50.106:30022";
						});
					});
					services.AddPaymentClient();
					services
						.AddSystemJsonSerializer()
						.AddSingleton<LeePaySerializer>();
					services.AddHostedService<Demo>();
				});

		private static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}
	}
}
