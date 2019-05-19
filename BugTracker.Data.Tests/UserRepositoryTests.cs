using System.Linq;
using System.Threading.Tasks;
using BugTracker.Common;
using NUnit.Framework;

namespace BugTracker.Data.Tests
{
	[TestFixture]
	public class UserRepositoryTests
	{
		private TestContext _context;

		[SetUp]
		public void Setup()
		{
			_context = new TestContext();
			_context.ClearDatabase();
		}

		[Test]
		public async Task ReadCreateReadUser()
		{
			var repository = _context.GetUserRepository();
			Assert.That(await repository.GetUsersAsync(), Is.Empty);

			var createResult = await repository.CreateUserAsync(new NewUser {Name = "Fred Bloggs"});
			Assert.That(createResult.Name, Is.EqualTo("Fred Bloggs"));
			Assert.That(createResult.Id, Is.GreaterThan(0));

			var users = (await repository.GetUsersAsync()).ToList();
			Assert.That(users, Has.Count.EqualTo(1));
			Assert.That(users[0].Id, Is.EqualTo(createResult.Id));
			Assert.That(users[0].Name, Is.EqualTo("Fred Bloggs"));
		}

		[Test]
		public async Task UpdateUser()
		{
			var repository = _context.GetUserRepository();
			var createResult = await repository.CreateUserAsync(new NewUser { Name = "Fred Bloggs" });

			await repository.UpdateUserAsync(new User{Id = createResult.Id, Name = "George Formby"});

			var users = (await repository.GetUsersAsync()).ToList();
			Assert.That(users, Has.Count.EqualTo(1));
			Assert.That(users[0].Id, Is.EqualTo(createResult.Id));
			Assert.That(users[0].Name, Is.EqualTo("George Formby"));
		}
	}
}
