using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Domain.RepositoryInterfaces;
using UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace UserManagement.Infrastructure.Database
{
    public class UsersAdminManagementRepository : IUsersAdminManagementRepository
    {
        private readonly DataContext _dataContext;

        public UsersAdminManagementRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _dataContext.Users.AddAsync(user);
            var changes = await _dataContext.SaveChangesAsync();
            if (changes > 0) return await ReadUserAsync(user.Id);
            else throw new ApplicationException($"Cannot add user: {user.Id} to database");
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await ReadUserAsync(id);
            
            if (user == null) return false;

            _dataContext.Users.Remove(user);
            var changes = await _dataContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<User> ReadUserAsync(string id)
        {
            var queryable = _dataContext.users.AsQueryable();
            return await queryable.SingleOrDefaultAsync<User>(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> ReadUsersAsync()
        {
            var queryable = _dataContext.users.AsQueryable();
            return await queryable.ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _dataContext.Users.Update(user);
            var changes = await _dataContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}