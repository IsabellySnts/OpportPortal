using OpportPortal.Application.Common;
using OpportPortal.Application.Services;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Enums;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.Interfaces.Services;
using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Application.Commands.Users.CreateUser;

public class CreateUserHandler : IHandler<CreateUserCommand, ResultViewModel<long>>
{
    private readonly IUserRepository _repository;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;

    public CreateUserHandler(IUserRepository repository, IAuthService authService, IEmailService emailService, INotificationService notificationService)
    {
        _repository = repository;
        _authService = authService;
        _emailService = emailService;
        _notificationService = notificationService;
    }

    public async Task<ResultViewModel<long>> HandleAsync(CreateUserCommand request)
    {
        if (await _repository.ExistsByEmailAsync(request.UserEmail))
            return ResultViewModel<long>.Error("Email already in use.");

        var email = new Email(request.UserEmail);
        var hash = _authService.ComputeHash(request.Password);
        var user = new User(email, hash, request.Type);

        var id = await _repository.Create(user);

        var name = request.UserEmail.Split('@')[0];
        var (subject, html) = EmailTemplates.Welcome(name);
        await _emailService.SendAsync(request.UserEmail, name, subject, html);

        await _notificationService.NotifyAsync(id, "Bem-vindo(a)! 🎉", "Sua conta foi criada. Complete seu perfil para começar.", NotificationType.Welcome);

        return ResultViewModel<long>.Success(id, "User created successfully.");
    }
}