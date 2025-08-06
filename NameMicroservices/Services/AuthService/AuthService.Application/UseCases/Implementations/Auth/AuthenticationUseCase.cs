using FluentValidation;
using AuthService.Application.Models.Requests.Auth;
using AuthService.Application.Models.Responses.Auth;
using AuthService.Application.Repositories;
using AuthService.Application.Services;
using AuthService.Application.UseCases.Interfaces.Auth;
using AuthService.Domain.Entities;
using AuthService.Shared.Common;
using AuthService.Shared.Enums;
using ReadNest.Application.Validators.Auth;
using AuthService.Application.Validators.Auth;

namespace AuthService.Application.UseCases.Implementations.Auth
{
    public class AuthenticationUseCase : IAuthenticationUseCase
    {
        private readonly RegisterRequestValidator _registerRequestValidator;
        private readonly LoginRequestValidator _loginRequestValidator;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Role, Guid> _roleRepository;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="registerRequestValidator"></param>
        /// <param name="loginRequestValidator"></param>
        /// <param name="userRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="jwtService"></param>
        public AuthenticationUseCase(RegisterRequestValidator registerRequestValidator, LoginRequestValidator loginRequestValidator, IUserRepository userRepository, IGenericRepository<Role, Guid> roleRepository, IJwtService jwtService)
        {
            _registerRequestValidator = registerRequestValidator;
            _loginRequestValidator = loginRequestValidator;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<TokenResponse>> GetNewAccessToken(TokenRequest request)
        {
            var isValid = await _jwtService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (isValid)
            {
                var user = await _userRepository.GetByUserIdAsync(request.UserId);
                if (user == null)
                {
                    return new ApiResponse<TokenResponse>
                    {
                        Success = false,
                        Message = Message.GetMessageById(MessageId.E0000),
                        MessageId = MessageId.E0000,
                    };
                }

                var responseToken = new TokenResponse
                {
                    AccessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.RoleName),
                    RefreshToken = request.RefreshToken,
                };

                return new ApiResponse<TokenResponse>
                {
                    Data = responseToken,
                    Success = true,
                    Message = Message.GetMessageById(MessageId.I0000),
                    MessageId = MessageId.I0000,
                };
            }

            return new ApiResponse<TokenResponse>
            {
                Success = false,
                Message = Message.GetMessageById(MessageId.E0000),
                MessageId = MessageId.E0000,
            };
        }

        public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
        {
            await _loginRequestValidator.ValidateAndThrowAsync(request);
            var user = await _userRepository.LoginAsync(request.UserName, request.Password);
            if (user == null)
            {
                return new ApiResponse<TokenResponse>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = Message.GetMessageById(MessageId.E0000)
                };
            }

            var tokenResponse = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.RoleName),
                RefreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id),
            };

            return new ApiResponse<TokenResponse>
            {
                Data = tokenResponse,
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000)
            };
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequest request)
        {
            await _registerRequestValidator.ValidateAndThrowAsync(request);

            var role = (await _roleRepository.FindAsync(x => x.RoleName == RoleEnum.User.ToString(), asNoTracking: true)).FirstOrDefault();
            if (role == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = "Role doesn't exists."
                };
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                UserName = request.UserName,
                AvatarUrl = string.Empty,
                Email = request.Email,
                HashPassword = hashedPassword,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth,
                RoleId = role.Id,
            };

            _ = await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000)
            };
        }
    }
}
