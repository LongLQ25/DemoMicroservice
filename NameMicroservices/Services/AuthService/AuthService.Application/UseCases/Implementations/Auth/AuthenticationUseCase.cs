using FluentValidation;
using AuthService.Application.Models.Requests.Auth;
using AuthService.Application.Models.Responses.Auth;
using AuthService.Application.Repositories;
using AuthService.Application.Services;
using AuthService.Application.UseCases.Interfaces.Auth;
using AuthService.Domain.Entities;
using AuthService.Shared.Common;
using AuthService.Shared.Enums;
using AuthService.Application.Validators.Auth;
using EventBus.Interfaces;
using System.Text.Json;
using EventBus.Models;

namespace AuthService.Application.UseCases.Implementations.Auth
{
    public class AuthenticationUseCase : IAuthenticationUseCase
    {
        private readonly RegisterRequestValidator _registerRequestValidator;
        private readonly LoginRequestValidator _loginRequestValidator;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Role, Guid> _roleRepository;
        private readonly IJwtService _jwtService;
        private readonly IRabbitMQService _rabbitMQ;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="registerRequestValidator"></param>
        /// <param name="loginRequestValidator"></param>
        /// <param name="userRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="jwtService"></param>
        public AuthenticationUseCase(RegisterRequestValidator registerRequestValidator, LoginRequestValidator loginRequestValidator, IUserRepository userRepository, IGenericRepository<Role, Guid> roleRepository, IJwtService jwtService, IRabbitMQService rabbitMQ)
        {
            _registerRequestValidator = registerRequestValidator;
            _loginRequestValidator = loginRequestValidator;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
            _rabbitMQ = rabbitMQ;
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

            var userRegisteredEvent = new UserRegisteredEvent
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                RegisteredAt = DateTime.UtcNow
            };

            await _rabbitMQ.PublishAsync("user.registered", userRegisteredEvent);

            return new ApiResponse<string>
            {
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000)
            };
        }

        public async Task<ApiResponse<TokenResponse>> GoogleLoginAsync(IdTokenRequest request)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={request.IdToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<TokenResponse>
                    {
                        Success = false,
                        Message = "Invalid Google ID Token"
                    };
                }

                var content = await response.Content.ReadAsStringAsync();
                var tokenInfo = JsonSerializer.Deserialize<GoogleTokenInfo>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Kiểm tra token và domain email
                if (tokenInfo == null || tokenInfo.EmailVerified != "true")
                {
                    return new ApiResponse<TokenResponse>
                    {
                        Success = false,
                        Message = "Email must be verified"
                    };
                }

                var email = tokenInfo.Email;
                if (!email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase) &&
                    !email.EndsWith(".edu.vn", StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResponse<TokenResponse>
                    {
                        Success = false,
                        Message = "Email must end with @gmail.com or .edu.vn"
                    };
                }

                var name = tokenInfo.Name;
                var avatar = tokenInfo.Picture;

                // Nếu user đã tồn tại → login
                var existingUser = await _userRepository.GetByEmailAsync(email);
                if (existingUser != null)
                {
                    var accessToken = _jwtService.GenerateAccessToken(existingUser.Id, existingUser.Role.RoleName);
                    var refreshToken = await _jwtService.GenerateRefreshTokenAsync(existingUser.Id);

                    return new ApiResponse<TokenResponse>
                    {
                        Data = new TokenResponse
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        },
                        Success = true,
                        Message = "Login successfully"
                    };
                }

                // Nếu chưa tồn tại → tạo mới
                var role = (await _roleRepository.FindAsync(r => r.RoleName == RoleEnum.User.ToString(), asNoTracking: true)).FirstOrDefault();
                if (role == null)
                {
                    return new ApiResponse<TokenResponse>
                    {
                        Success = false,
                        Message = "Role User doesn't exist."
                    };
                }

                var user = new Domain.Entities.User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    FullName = name,
                    AvatarUrl = avatar ?? string.Empty,
                    RoleId = role.Id,
                    UserName = email,
                    HashPassword = string.Empty, // login qua Google nên không cần
                    Address = string.Empty,
                    DateOfBirth = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                // Publish RabbitMQ
                var userRegisteredEvent = new UserRegisteredEvent
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    RegisteredAt = DateTime.UtcNow
                };
                await _rabbitMQ.PublishAsync("user.registered", userRegisteredEvent);

                var newAccessToken = _jwtService.GenerateAccessToken(user.Id, role.RoleName);
                var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id);

                return new ApiResponse<TokenResponse>
                {
                    Data = new TokenResponse
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken
                    },
                    Success = true,
                    Message = "Login successfully"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GoogleLoginAsync] Exception: {ex.Message}");
                return new ApiResponse<TokenResponse>
                {
                    Success = false,
                    Message = "Login failed"
                };
            }
        }


    }
}
