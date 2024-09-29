using Dapper;
using Master.Core.DTO;
using Master.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Master.Data.Repositories
{
    public class BaseIdentityRepository : IBaseIdentityRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public BaseIdentityRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = dbContext.Database.GetDbConnection().ConnectionString;

        }

        private async Task<T> ExecuteInConnectionAsync<T>(Func<MySqlConnection, Task<T>> action)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await action(connection);
            }
        }

        public async Task<bool> CheckIfUserExistsByEmail(string email)
        {
            string sql = "SELECT * FROM mdl_user WHERE email = @Email;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                var users = await connection.QueryAsync(sql, new { Email = email });
                return users.Any();
            });
        }

        public async Task<bool> CheckIfUserIsConfirmed(string email)
        {
            string sql = "SELECT * FROM mdl_user WHERE email = @Email AND confirmed = true;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                var users = await connection.QueryAsync(sql, new { Email = email });
                return users.Any();
            });
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            string sql = "SELECT u.firstname AS FirstName, u.lastname AS LastName " +
                         "FROM mdl_user u WHERE email = @Email AND confirmed = true;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                var users = await connection.QueryAsync<UserDto>(sql, new { Email = email });
                return users.FirstOrDefault();
            });
        }

        public async Task<List<UserDto>> GetUserEmailsByIds(List<int> userIds)
        {
            var userEmails = new List<UserDto>();
            string sql = "SELECT u.email AS Email, u.firstname AS FirstName, u.lastname AS LastName FROM mdl_user u WHERE u.id = @UserId;";

            foreach (var userId in userIds)
            {
                var user = await ExecuteInConnectionAsync(async connection =>
                {
                    return await connection.QueryAsync<UserDto>(sql, new { UserId = userId });
                });
                userEmails.Add(user.FirstOrDefault());
            }

            return userEmails;
        }

        public async Task<AuthUserDto> GetUserRole(string email)
        {
            string sql = "SELECT r.shortname AS Role, u.id AS Id FROM mdl_role_assignments ra " +
                         "LEFT JOIN mdl_role r ON ra.roleid = r.id " +
                         "LEFT JOIN mdl_user u ON ra.userid = u.id " +
                         "WHERE u.email = @Email " +
                         "ORDER BY r.id " +
                         "LIMIT 1;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                return await connection.QueryFirstOrDefaultAsync<AuthUserDto>(sql, new { Email = email });
            });
        }

        public async Task<List<AuthUserDto>> GetUserRoles(string email)
        {
            string sql = "SELECT r.shortname AS Role, u.id AS Id FROM mdl_role_assignments ra " +
                         "LEFT JOIN mdl_role r ON ra.roleid = r.id " +
                         "LEFT JOIN mdl_user u ON ra.userid = u.id " +
                         "WHERE u.email = @Email;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                var result = await connection.QueryAsync<AuthUserDto>(sql, new { Email = email });
                return result.ToList();
            });
        }
    }
}