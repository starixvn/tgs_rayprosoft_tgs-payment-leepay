using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tgs.Payment.LeePay;

namespace LeePayTest;

internal class Program
{
	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureServices(services =>
			{
				_ = services
					.AddMessageQueue()
					.AddNatsMessageQueue(config =>
				  {
					  _ = config.ConfigQueueOptions((options, ap) =>
					  {
						  options.Url = "nats://192.168.50.106:30022";
					  });
				  });
				_ = services.AddPaymentClient();
				_ = services
					.AddSingleton<LeePaySerializer>();
				_ = services.AddHostedService<Demo>();
			});

	private static void Main(string[] args)
	{
		CreateHostBuilder(args).Build().Run();
	}
}
