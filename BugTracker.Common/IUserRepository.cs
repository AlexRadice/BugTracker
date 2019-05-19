using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Common
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetUsersAsync();

		Task<User> GetUserAsync(int userId);

		Task<User> CreateUserAsync(NewUser user);

		Task UpdateUserAsync(User user);
	}
}
