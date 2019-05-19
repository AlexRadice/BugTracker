using System;
using System.Data.SqlClient;
using BugTracker.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BugTracker.Data.Tests
{
	internal class TestContext
	{
		private IServiceProvider _provider;

		public TestContext()
		{
			var serviceCollection = new ServiceCollection();
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json").Build();
			serviceCollection.AddSingleton<IConfiguration>(
				configuration);
			serviceCollection.AddRepositories();
			_provider = serviceCollection.BuildServiceProvider();
			Configuration = _provider.GetService<IConfiguration>();
		}

		public IUserRepository GetUserRepository() => _provider.GetService<IUserRepository>();

		public IBugRepository GetBugRepository() => _provider.GetService<IBugRepository>();

		private IConfiguration Configuration { get; set; }

		public void ClearDatabase()
		{
			using (var connection = new SqlConnection(Configuration.GetConnectionString("BugTracker")))
			using (var command = new SqlCommand("delete from dbo.Users;delete from dbo.Bugs", connection))
			{
				connection.Open();
				command.ExecuteNonQuery();
			}
		}
	}
}
