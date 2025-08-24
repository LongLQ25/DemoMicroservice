using AuthService.Application.Models.Requests.Auth;
using AuthService.Application.Models.Responses.Auth;
using AuthService.Shared.Common;

namespace AuthService.Application.UseCases.Interfaces.Auth
{
    public interface IAuthenticationUseCase
    {
        Task<ApiResponse<string>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<TokenResponse>> GetNewAccessToken(TokenRequest request);
        Task<ApiResponse<TokenResponse>> GoogleLoginAsync(IdTokenRequest request);
    }
}
