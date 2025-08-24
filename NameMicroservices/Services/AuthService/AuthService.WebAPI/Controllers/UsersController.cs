using AuthService.Application.Models.Responses.User;
using AuthService.Application.UseCases.Interfaces.User;
using AuthService.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AuthService.WebAPI.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserUseCase _userUseCase;

        public UsersController(IUserUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetUserResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUsers([FromQuery] PagingRequest request)
        {
            var response = await _userUseCase.GetAllUsers(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<GetUserResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var response = await _userUseCase.GetUserById(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("username/{userName}")]
        [ProducesResponseType(typeof(ApiResponse<GetUserResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var response = await _userUseCase.GetUserByUserName(userName);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
