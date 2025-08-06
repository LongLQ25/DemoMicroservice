using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Models.Requests.Auth;
using AuthService.Application.Models.Responses.Auth;
using AuthService.Application.UseCases.Interfaces.Auth;
using AuthService.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationUseCase _authUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authUseCase"></param>
        public AuthenticationController(IAuthenticationUseCase authUseCase)
        {
            _authUseCase = authUseCase;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authUseCase.RegisterAsync(request);

            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authUseCase.LoginAsync(request);

            if (response.Success) return Ok(response);
            return Unauthorized(response);
        }

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request) { }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshToken(TokenRequest request)
        {
            var response = await _authUseCase.GetNewAccessToken(request);

            if (response.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}
