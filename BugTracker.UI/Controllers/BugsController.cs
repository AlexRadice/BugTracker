using System.Collections.Generic;
using System.Threading.Tasks;
using BugTracker.Common;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BugsController : ControllerBase
	{
		private readonly IBugRepository _bugRepository;

		public BugsController(IBugRepository bugRepository)
		{
			_bugRepository = bugRepository;
		}

		[HttpGet]
		public Task<IEnumerable<Bug>> FetchAll()
		{
			return _bugRepository.GetBugsAsync();
		}

		[HttpPost]
		public Task<Bug> Create([FromBody] NewBug newBug)
		{
			return _bugRepository.CreateBugAsync(newBug);
		}

		[HttpPut]
		public Task Update([FromBody] Bug bug)
		{
			return _bugRepository.UpdateBugAsync(bug);
		}
	}
}
