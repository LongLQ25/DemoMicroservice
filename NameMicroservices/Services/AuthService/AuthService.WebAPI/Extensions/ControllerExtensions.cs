using Microsoft.AspNetCore.Mvc;
using AuthService.Shared.Common;

namespace ReadNest.WebAPI.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ApiFail(this ControllerBase ctrl, string messageId)
            => ctrl.BadRequest(ApiResponse<string>.Fail(messageId));

        public static IActionResult ApiOk<T>(this ControllerBase ctrl, T data, string messageId = MessageId.I0000) where T : class => ctrl.Ok(ApiResponse<T>.Ok(data: data, messageId: messageId));
    }
}
