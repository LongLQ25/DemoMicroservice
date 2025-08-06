using System.Security.Claims;

namespace AuthService.Application.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(Guid userId, string role);
        Task<string> GenerateRefreshTokenAsync(Guid userId);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
        Task<Guid> GetUserIdAsync(string token);
    }
}
