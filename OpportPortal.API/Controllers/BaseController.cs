// API/Controllers/BaseController.cs
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OpportPortal.API.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected long GetUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (userIdClaim is null)
            throw new UnauthorizedAccessException("User not authenticated.");

        return long.Parse(userIdClaim);
    }

    protected string GetUserEmail()
        => User.Claims.FirstOrDefault(c => c.Type == "username")?.Value
            ?? throw new UnauthorizedAccessException("User not authenticated.");

    protected string GetUserRole()
        => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
            ?? throw new UnauthorizedAccessException("User not authenticated.");
}