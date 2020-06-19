using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.UserManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.UserManagement.Data
{
    public class UserEFRepository : IUsersRepository
    {
        private readonly DataContext _dataContext;

        public UserEFRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<bool> CreateUser()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser()
        {
            throw new NotImplementedException();
        }

        public Task<User> ReadUser(Guid id)
        {
            
        }

        public async Task<IEnumerable<User>> ReadUsers()
        {
            return await _dataContext.Users.ToArrayAsync();
        }

        public Task<bool> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}