using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Common;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.UI.Controllers
{
	[Route("api/[controller]")]
	public class BugsController : Controller
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
	}
}
