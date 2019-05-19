using System;
using System.Collections.Generic;
using System.Text;

namespace BugTracker.Common
{
	public class Bug
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime DateOpened { get; set; }

		public DateTime? DateClosed { get; set; }

		public User AssignedToUser { get; set; }
	}
}
