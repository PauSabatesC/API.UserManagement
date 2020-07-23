using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.UserManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.UserManagement.Data
{
    public class UsersAdminManagementRepository : IUsersAdminManagementRepository
    {
        private readonly DataContext _dataContext;

        public UsersAdminManagementRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> CreateUser(User user)
        {
            await _dataContext.Users.AddAsync(user);
            var changes = await _dataContext.SaveChangesAsync();
            if (changes > 0) return await ReadUser(user.Id);
            else throw new ApplicationException($"Cannot add user: {user.Id} to database");
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = ReadUser(id);
            
            if (user == null) return false;

            _dataContext.Users.Remove(user.Result);
            var changes = await _dataContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<User> ReadUser(string id)
        {
            var queryable = _dataContext.users.AsQueryable();
            return await queryable.SingleOrDefaultAsync<User>(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> ReadUsers()
        {
            var queryable = _dataContext.users.AsQueryable();
            return await queryable.ToListAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            _dataContext.Users.Update(user);
            var changes = await _dataContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}