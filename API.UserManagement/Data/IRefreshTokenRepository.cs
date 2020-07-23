using API.UserManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Data
{
    public interface IRefreshTokenRepository
    {
        Task<bool> CreateRefreshToken(RefreshToken refreshToken);
        //Task<IEnumerable<RefreshToken>> ReadTokens();
        Task<RefreshToken> ReadRefreshToken(string refreshToken);
        Task<bool> UpdateRefreshToken(RefreshToken refreshToken);
        //Task<bool> DeleteRefreshToken();
    }
}
