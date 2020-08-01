using UserManagement.Domain.RepositoryInterfaces;
using UserManagement.Domain.Entities;
using System.Threading.Tasks;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Services.Boundaries
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
