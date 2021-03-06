﻿using System;
using UserManagement.Domain.RepositoryInterfaces;
using UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace UserManagement.Infrastructure.Database
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DataContext _dataContext;
        public RefreshTokenRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateRefreshToken(RefreshToken refreshToken)
        {
            await _dataContext.refreshTokens.AddAsync(refreshToken);
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<RefreshToken> ReadRefreshToken(string refreshToken)
        {
            var token = await _dataContext.refreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
            return token;
        }

        public async Task<bool> UpdateRefreshToken(RefreshToken refreshToken)
        {
            _dataContext.refreshTokens.Update(refreshToken);
            var changes = await _dataContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}
