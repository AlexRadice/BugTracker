using System.Collections.Generic;
using System.Threading.Tasks;
using BugTracker.Common;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public UsersController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[HttpGet]
		public Task<IEnumerable<User>> FetchAll()
		{
			return _userRepository.GetUsersAsync();
		}

		[HttpPost]
		public Task<User> Create([FromBody] NewUser newUser)
		{
			return _userRepository.CreateUserAsync(newUser);
		}

		[HttpPut]
		public Task Update([FromBody] User bug)
		{
			return _userRepository.UpdateUserAsync(bug);
		}
	}
}
