using CarDealershipManagement.Core.Domain.Models;
using CarDealershipManagement.Core.Interfaces.Repositories;
using CarDealershipManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlHelper _sqlHelper;

        public UserRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@UserId", id)
            };

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetUserByUserId", parameters);

            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                    RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                };
            }

            return null;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = new List<User>();

            using var reader = await _sqlHelper.ExecuteReaderAsync("SELECT u.UserId, u.Username, u.PasswordHash, u.Email, u.FullName, u.RoleId, r.Name as RoleName, u.CreatedDate, u.ModifiedDate FROM Users u JOIN Roles r ON u.RoleId = r.RoleId");

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                    RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                });
            }

            return users;
        }

        public async Task<int> AddAsync(User entity)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@Username", entity.Username),
                    new SqlParameter("@PasswordHash", entity.PasswordHash),
                    new SqlParameter("@Email", entity.Email),
                    new SqlParameter("@FullName", entity.FullName),
                    new SqlParameter("@RoleId", entity.RoleId)
            };

            return await _sqlHelper.ExecuteScalarAsync<int>("sp_CreateUser", parameters);
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@UserId", entity.UserId),
                    new SqlParameter("@Username", entity.Username),
                    new SqlParameter("@Email", entity.Email),
                    new SqlParameter("@FullName", entity.FullName),
                    new SqlParameter("@RoleId", entity.RoleId),
                    new SqlParameter("@ModifiedDate", DateTime.Now)
            };

            await _sqlHelper.ExecuteNonQueryAsync("UPDATE Users SET Username = @Username, Email = @Email, FullName = @FullName, RoleId = @RoleId, ModifiedDate = @ModifiedDate WHERE UserId = @UserId", parameters);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@UserId", id)
            };

            await _sqlHelper.ExecuteNonQueryAsync("DELETE FROM Users WHERE UserId = @UserId", parameters);
            return true;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@Username", username)
            };

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetUserByUsername", parameters);

            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                    RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                };
            }

            return null;
        }

        public async Task<IEnumerable<MenuItem>> GetMenuByRoleIdAsync(int roleId)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@RoleId", roleId)
            };

            var menuItems = new List<MenuItem>();

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetMenuByRoleId", parameters);

            while (await reader.ReadAsync())
            {
                menuItems.Add(new MenuItem
                {
                    MenuId = reader.GetInt32(reader.GetOrdinal("MenuId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Path = reader.GetString(reader.GetOrdinal("Path")),
                    Icon = !reader.IsDBNull(reader.GetOrdinal("Icon")) ? reader.GetString(reader.GetOrdinal("Icon")) : null,
                    ParentId = !reader.IsDBNull(reader.GetOrdinal("ParentId")) ? reader.GetInt32(reader.GetOrdinal("ParentId")) : (int?)null,
                    SortOrder = reader.GetInt32(reader.GetOrdinal("SortOrder"))
                });
            }

            return menuItems;
        }
    }
}
