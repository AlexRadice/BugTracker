using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BugTracker.Common;
using Microsoft.Extensions.Configuration;

namespace BugTracker.Data
{
	internal class UserRepository : IUserRepository
	{
		private readonly IConfiguration _configuration;

		public UserRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			var results = new List<User>();
			using (var connection = new SqlConnection(_configuration.GetConnectionString("BugTracker")))
			using (var command = new SqlCommand("select Id, Name from dbo.Users", connection))
			{
				await connection.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					var idOrdinal = reader.GetOrdinal("Id");
					var nameOrdinal = reader.GetOrdinal("Name");
					while (reader.Read())
					{
						results.Add(new User
						{
							Id = reader.GetInt32(idOrdinal),
							Name = reader.GetString(nameOrdinal)
						});
					}
				}
			}

			return results;
		}

		public async Task<User> GetUserAsync(int userId)
		{
			using (var connection = new SqlConnection(_configuration.GetConnectionString("BugTracker")))
			using (var command = new SqlCommand("select Id, Name from dbo.Users where Id = @id", connection))
			{
				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = userId});
				await connection.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					if (reader.Read())
					{
						var idOrdinal = reader.GetOrdinal("Id");
						var nameOrdinal = reader.GetOrdinal("Name");
						return new User
						{
							Id = reader.GetInt32(idOrdinal),
							Name = reader.GetString(nameOrdinal)
						};
					}

					return null;
				}
			}
		}

		public async Task<User> CreateUserAsync(NewUser user)
		{
			using (var connection = new SqlConnection(_configuration.GetConnectionString("BugTracker")))
			using (var command = new SqlCommand("insert into dbo.Users (Name) values (@name);select @@IDENTITY", connection))
			{
				command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50){Value = user.Name});
				await connection.OpenAsync();
				var id = await command.ExecuteScalarAsync();
				return new User{Id = Convert.ToInt32(id), Name = user.Name};
			}
		}

		public async Task UpdateUserAsync(User user)
		{
			using (var connection = new SqlConnection(_configuration.GetConnectionString("BugTracker")))
			using (var command = new SqlCommand("update dbo.Users set Name = @name where Id = @id", connection))
			{
				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = user.Id });
				command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50) { Value = user.Name });
				await connection.OpenAsync();
				await command.ExecuteNonQueryAsync();
			}
		}
	}
}
