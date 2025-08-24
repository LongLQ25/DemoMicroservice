using AuthService.Application.Models.Responses.User;
using AuthService.Application.Repositories;
using AuthService.Application.UseCases.Interfaces.User;
using AuthService.Shared.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthService.Application.UseCases.Implementations.User
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        public UserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<PagingResponse<GetUserResponse>>> GetAllUsers(PagingRequest request)
        {
            var users = await _userRepository.GetAllUsersWithRoleUserAsync();
            if (users == null || !users.Any())
            {
                return ApiResponse<PagingResponse<GetUserResponse>>.Fail(MessageId.E0005);
            }

            var response = new PagingResponse<GetUserResponse>()
            {
                Items = users.Select(user => new GetUserResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleName = user.Role?.RoleName

                }).ToList(),
                TotalItems = users.Count(),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return ApiResponse<PagingResponse<GetUserResponse>>.Ok(response);
        }

        public async Task<ApiResponse<GetUserResponse>> GetUserById(Guid userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);

            if (user == null)
            {
                return ApiResponse<GetUserResponse>.Fail(MessageId.E0005);
            }

            var response = new GetUserResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleName = user.Role?.RoleName
            };

            return ApiResponse<GetUserResponse>.Ok(response);
        }

        public async Task<ApiResponse<GetUserResponse>> GetUserByUserName(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);

            if (user == null)
            {
                return ApiResponse<GetUserResponse>.Fail(MessageId.E0005);
            }

            var response = new GetUserResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleName = user.Role?.RoleName
            };

            return ApiResponse<GetUserResponse>.Ok(response);
        }
    }
}
