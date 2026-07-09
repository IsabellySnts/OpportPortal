using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.Application.Commands.Login;
using OpportPortal.Application.Commands.Users.CreateUser;
using OpportPortal.Application.Common;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand request)
    {
        var result = await _mediator.DispatchAsync<CreateUserCommand, ResultViewModel<long>>(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var result = await _mediator.DispatchAsync<LoginCommand, ResultViewModel<LoginResponse>>(request);

        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }
}