using BugTracker.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BugTracker.Data
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IUserRepository, UserRepository>();
			serviceCollection.AddTransient<IBugRepository, BugRepository>();
			return serviceCollection;
		}
	}
}
