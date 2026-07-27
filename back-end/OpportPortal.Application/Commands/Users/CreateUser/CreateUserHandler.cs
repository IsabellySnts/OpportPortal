using OpportPortal.Application.Common;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.Interfaces.Services;
using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Application.Commands.Users.CreateUser;

public class CreateUserHandler : IHandler<CreateUserCommand, ResultViewModel<long>>
{
    private readonly IUserRepository _repository;
    private readonly IAuthService _authService;

    public CreateUserHandler(IUserRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }
    public async Task<ResultViewModel<long>> HandleAsync(CreateUserCommand request)
    {
        if (await _repository.ExistsByEmailAsync(request.UserEmail))
            return ResultViewModel<long>.Error("Email already in use.");

        var email = new Email(request.UserEmail);  
        var hash = _authService.ComputeHash(request.Password);
        var user = new User(email, hash, request.Type);

        var id = await _repository.Create(user);

        return ResultViewModel<long>.Success(id, "User created successfully.");
    }
}
