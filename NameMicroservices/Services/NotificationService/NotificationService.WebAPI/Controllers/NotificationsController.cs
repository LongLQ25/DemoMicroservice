using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Models;
using NotificationService.Application.UseCase.Interfaces;
using Shared.Common;
using System.Net;

namespace NotificationService.WebAPI.Controllers
{
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationUseCase _notificationUseCase;

        public NotificationsController(INotificationUseCase notificationUseCase)
        {
            _notificationUseCase = notificationUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<GetNotificationResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetNotificationsByUserId([FromQuery] Guid userId)
        {
            var response = await _notificationUseCase.GetNotificationsByUserIdAsync(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("unread")]
        [ProducesResponseType(typeof(ApiResponse<List<GetNotificationResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUnreadNotificationsByUserId([FromQuery] Guid userId)
        {
            var response = await _notificationUseCase.GetUnreadNotificationsByUserIdAsync(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{notificationId:guid}/read")]
        [ProducesResponseType(typeof(ApiResponse<GetNotificationResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
        {
            var response = await _notificationUseCase.MarkNotificationAsReadAsync(notificationId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("{notificationId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<GetNotificationResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteNotification(Guid notificationId)
        {
            var response = await _notificationUseCase.DeleteNotificationAsync(notificationId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
