using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.Application.Commands.Users.CreateUser;
using OpportPortal.Application.Commands.Users.GoogleAuth;
using OpportPortal.Application.Commands.Users.Login;
using OpportPortal.Application.Common;
using Swashbuckle.AspNetCore.Annotations;


namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Autenticação e cadastro de usuários — registro e login com emissão de token JWT")]

public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerOperation(
            Summary = "Criar usuário",
            Description = "Cadastra um novo usuário no sistema com e-mail, senha e perfil (1 = Adm, 2 = Candidate, 3 = Company).")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand request)
    {
        var result = await _mediator.DispatchAsync<CreateUserCommand, ResultViewModel<long>>(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation(
            Summary = "Autenticar usuário (login)",
            Description = "Realiza a autenticação com e-mail e senha. Retorna o token JWT para uso nos endpoints protegidos. Não requer autenticação prévia.")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var result = await _mediator.DispatchAsync<LoginCommand, ResultViewModel<LoginResponse>>(request);

        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }

    [HttpPost("google-auth")]
    [AllowAnonymous]
    [SwaggerOperation(
    Summary = "Login/cadastro com Google",
    Description = "Valida o ID token do Google. Se a conta existe, loga; se não existe e o userType for enviado (cadastro), cria a conta. Retorna o JWT.")]
    [SwaggerResponse(200, "Autenticado com sucesso", typeof(ResultViewModel<LoginResponse>))]
    [SwaggerResponse(401, "Token inválido ou conta não encontrada")]
    public async Task<IActionResult> GoogleAuth([FromBody] GoogleAuthCommand request)
    {
        var result = await _mediator.DispatchAsync<GoogleAuthCommand, ResultViewModel<GoogleAuthResponse>>(request);

        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }
}