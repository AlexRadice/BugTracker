using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BugTracker.UI.Tests
{
	internal class TestContext
	{
		public TestContext()
		{
			var serviceCollection = new ServiceCollection();
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json").Build();
			serviceCollection.AddSingleton<IConfiguration>(
				configuration);
		}
	}
}
