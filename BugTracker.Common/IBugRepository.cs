using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Common
{
	public interface IBugRepository
	{
		Task<IEnumerable<Bug>> GetBugsAsync();

		Task<Bug> CreateBugAsync(NewBug newBug);

		Task UpdateBugAsync(Bug bug);
	}
}
