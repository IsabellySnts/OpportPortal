using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.Application.Common;
using OpportPortal.Application.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json")]
[SwaggerTag("Notificações in-app do usuário autenticado")]
public class NotificationController : BaseController
{
    private readonly INotificationService _notificationService;
    public NotificationController(INotificationService notificationService) => _notificationService = notificationService;

    [HttpGet]
    [SwaggerOperation(Summary = "Lista as notificações do usuário logado")]
    public async Task<IActionResult> GetMy()
        => Ok(ResultViewModel<List<NotificationResponse>>.Success(await _notificationService.GetMyAsync(GetUserId()), "ok"));

    [HttpGet("unread-count")]
    [SwaggerOperation(Summary = "Quantidade de não lidas")]
    public async Task<IActionResult> GetUnreadCount()
        => Ok(ResultViewModel<int>.Success(await _notificationService.GetUnreadCountAsync(GetUserId()), "ok"));

    [HttpPatch("{id:long}/read")]
    [SwaggerOperation(Summary = "Marca uma notificação como lida")]
    public async Task<IActionResult> MarkAsRead(long id)
    {
        await _notificationService.MarkAsReadAsync(id, GetUserId());
        return Ok(ResultViewModel<bool>.Success(true, "ok"));
    }

    [HttpPatch("read-all")]
    [SwaggerOperation(Summary = "Marca todas como lidas")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        await _notificationService.MarkAllAsReadAsync(GetUserId());
        return Ok(ResultViewModel<bool>.Success(true, "ok"));
    }
}