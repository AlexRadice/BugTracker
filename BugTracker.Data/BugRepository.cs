using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BugTracker.Common;
using Microsoft.Extensions.Configuration;

namespace BugTracker.Data
{
	internal class BugRepository : IBugRepository
	{
		private readonly IConfiguration _configuration;
		private readonly IUserRepository _userRepository;

		public BugRepository(IConfiguration configuration, IUserRepository userRepository)
		{
			_configuration = configuration;
			_userRepository = userRepository;
		}

		public async Task<IEnumerable<Bug>> GetBugsAsync()
		{
			var results = new List<Bug>();
			using (var connection = new SqlConnection(_configuration.GetConnectionString("BugTracker")))
			using (var command = new SqlCommand("select Id, Title, Description, DateOpened, DateClosed, AssignedToUser from dbo.Bugs", connection))
			{
				await connection.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					var idOrdinal = reader.GetOrdinal("Id");
					var titleOrdinal = reader.GetOrdinal("Title");
					var descriptionOrdinal = reader.GetOrdinal("Description");
					var dateOpenedOrdinal = reader.GetOrdinal("DateOpened");
					var dateClosedOrdinal = reader.GetOrdinal("DateClosed");
					var assignedToUserOrdinal = reader.GetOrdinal("AssignedToUser");

					while (reader.Read())
					{
						var bug = new Bug
						{
							Id = reader.GetInt32(idOrdinal),
							Title = reader.GetString(titleOrdinal),
							Description = reader.GetString(descriptionOrdinal),
							DateOpened = reader.GetDateTime(dateOpenedOrdinal),
							DateClosed = reader.IsDBNull(dateClosedOrdinal) ? (DateTime?)null : reader.GetDateTime(dateClosedOrdinal)
						};
						if (!reader.IsDBNull(assignedToUserOrdinal))
						{
							bug.AssignedToUser =
								await _userRepository.GetUserAsync(reader.GetInt32(assignedToUserOrdinal));
						}
						results.Add(bug);
					}
				}
			}

			return results;
		}

		public async Task<Bug> CreateBugAsync(NewBug newBug)
		{
			var now = DateTime.Now;
			using (var connection = new SqlConnection(_configuration.GetConnectionString("BugTracker")))
			using (var command = new SqlCommand("insert into dbo.Bugs (Title, Description, DateOpened) values (@title, @description, @dateOpened);select @@IDENTITY", connection))
			{
				command.Parameters.Add(new SqlParameter("@title", SqlDbType.NVarChar, 100) { Value = newBug.Title});
				command.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar, 4000) { Value = newBug.Description });
				command.Parameters.Add(new SqlParameter("@dateOpened", SqlDbType.DateTime) { Value = now });

				await connection.OpenAsync();
				var id = await command.ExecuteScalarAsync();

				return new Bug
				{
					Id = Convert.ToInt32(id),
					Title = newBug.Title,
					Description = newBug.Description,
					DateOpened = now
				};
			}
		}

		public async Task UpdateBugAsync(Bug bug)
		{
			using (var connection = new SqlConnection(_configuration.GetConnectionString("BugTracker")))
			using (var command = new SqlCommand("update dbo.Bugs set Title = @title, Description = @description, DateClosed = @dateClosed, AssignedToUser = @assignedToUser where Id = @id", connection))
			{
				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = bug.Id });
				command.Parameters.Add(new SqlParameter("@title", SqlDbType.NVarChar, 100) { Value = bug.Title });
				command.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar, 4000) { Value = bug.Description });
				if (bug.DateClosed.HasValue)
				{
					command.Parameters.Add(new SqlParameter("@dateClosed", SqlDbType.DateTime)
						{Value = bug.DateClosed});
				}
				else
				{
					command.Parameters.Add(new SqlParameter("@dateClosed", SqlDbType.DateTime)
						{ Value = DBNull.Value });
				}

				if (bug.AssignedToUser == null)
				{
					command.Parameters.Add(new SqlParameter("@assignedToUser", SqlDbType.Int) { Value = DBNull.Value });
				}
				else
				{
					command.Parameters.Add(new SqlParameter("@assignedToUser", SqlDbType.Int) { Value = bug.AssignedToUser.Id });
				}

				await connection.OpenAsync();
				await command.ExecuteNonQueryAsync();
			}
		}
	}
}