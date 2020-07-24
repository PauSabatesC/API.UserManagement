using UserManagement.Domain.Entities;
using System.Threading.Tasks;

namespace UserManagement.Domain.RepositoryInterfaces
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
