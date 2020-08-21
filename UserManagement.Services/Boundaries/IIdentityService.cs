using System.Threading.Tasks;
using UserManagement.Services.DTOs.Requests;
using UserManagement.Services.DTOs.Responses;

namespace UserManagement.Services.Boundaries
{
    public interface IIdentityService
    {
        Task<AuthenticationResponse> RegisterAsync(UserAuthenticationRequest user);
        Task<AuthenticationResponse> LoginAsync(UserAuthenticationRequest user);
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest refreshToken);
    }
}
