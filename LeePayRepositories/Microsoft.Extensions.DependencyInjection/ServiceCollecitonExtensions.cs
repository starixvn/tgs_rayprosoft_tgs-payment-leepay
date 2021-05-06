using System;
using LeePayRepositories.Entities;
using Microsoft.EntityFrameworkCore;
using Tgs.Payment.LeePay;
using Tgs.Payment.LeePay.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollecitonExtensions
	{
		public static IServiceCollection AddLeePayRepositories(
			this IServiceCollection services,
			Action<IServiceProvider, DbContextOptionsBuilder> configure,
			int poolSize = 128)
			=> services
				.AddDbContextPool<TgsContext>(configure, poolSize)
				.AddTransient<ITgsRepository, TgsRepository>();
	}
}
