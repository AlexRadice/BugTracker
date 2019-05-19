using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BugTracker.Data.Tests
{
	[TestFixture()]
	public class BugRepositoryTests
	{
		private TestContext _context;

		[SetUp]
		public void Setup()
		{
			_context = new TestContext();
			_context.ClearDatabase();
		}

		[Test]
		public async Task ReadCreateReadUnassignedBug()
		{
			var now = DateTime.Now;
			var repository = _context.GetBugRepository();
			Assert.That(await repository.GetBugsAsync(), Is.Empty);

			var createResult = await repository.CreateBugAsync(new Common.NewBug
			{
				Title = "Bug title",
				Description = "Bug description"
			});
			Assert.That(createResult.Title, Is.EqualTo("Bug title"));
			Assert.That(createResult.Description, Is.EqualTo("Bug description"));
			Assert.That(createResult.DateOpened, Is.GreaterThan(now));
			Assert.That(createResult.DateClosed, Is.Null);
			Assert.That(createResult.Id, Is.GreaterThan(0));

			var bugs = (await repository.GetBugsAsync()).ToList();
			Assert.That(bugs, Has.Count.EqualTo(1));
			Assert.That(bugs[0].Id, Is.EqualTo(createResult.Id));
			Assert.That(bugs[0].Title, Is.EqualTo("Bug title"));
			Assert.That(bugs[0].Description, Is.EqualTo("Bug description"));
			Assert.That(bugs[0].DateOpened, Is.EqualTo(createResult.DateOpened).Within(TimeSpan.FromSeconds(1)));
			Assert.That(bugs[0].DateClosed, Is.Null);
		}

		[Test]
		public async Task UpdateUnassignedBug()
		{
			var repository = _context.GetBugRepository();
			var createResult = await repository.CreateBugAsync(new Common.NewBug
			{
				Title = "Bug title",
				Description = "Bug description"
			});

			await repository.UpdateBugAsync(new Common.Bug
			{
				Id = createResult.Id,
				Title = "Updated title",
				Description = "Updated description",
				DateClosed = DateTime.Today
			});

			var bugs = (await repository.GetBugsAsync()).ToList();
			Assert.That(bugs, Has.Count.EqualTo(1));
			Assert.That(bugs[0].Id, Is.EqualTo(createResult.Id));
			Assert.That(bugs[0].Title, Is.EqualTo("Updated title"));
			Assert.That(bugs[0].Description, Is.EqualTo("Updated description"));
			Assert.That(bugs[0].DateClosed, Is.EqualTo(DateTime.Today));
		}

	}
}
