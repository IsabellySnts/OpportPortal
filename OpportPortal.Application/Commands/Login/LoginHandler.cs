using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.Interfaces.Services;

namespace OpportPortal.Application.Commands.Login;

public class LoginHandler : IHandler<LoginCommand, ResultViewModel<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public LoginHandler(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<ResultViewModel<LoginResponse>> HandleAsync(LoginCommand request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            return ResultViewModel<LoginResponse>.Error("Invalid email or password.");

        var hash = _authService.ComputeHash(request.Password);

        if (user.Password != hash)
            return ResultViewModel<LoginResponse>.Error("Invalid email or password.");

        if (!user.Active)
            return ResultViewModel<LoginResponse>.Error("User is inactive.");

        var token = _authService.GenerateToken(user.UserEmail.Value, user.Type.ToString());

        return ResultViewModel<LoginResponse>.Success(new LoginResponse(token), "Authenticated user");
    }
}