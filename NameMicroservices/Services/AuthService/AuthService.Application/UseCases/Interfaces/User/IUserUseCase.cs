using AuthService.Application.Models.Requests.User;
using AuthService.Application.Models.Responses.User;
using AuthService.Shared.Common;

namespace AuthService.Application.UseCases.Interfaces.User
{
    public interface IUserUseCase
    {
        Task<ApiResponse<PagingResponse<GetUserResponse>>> GetAllUsers(PagingRequest request);
        Task<ApiResponse<GetUserResponse>> GetUserById(Guid userId);
        Task<ApiResponse<GetUserResponse>> GetUserByUserName(string userName);
    }
}
