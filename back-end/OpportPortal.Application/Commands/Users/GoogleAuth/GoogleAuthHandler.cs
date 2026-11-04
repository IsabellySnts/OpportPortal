using OpportPortal.Application.Commands.Users.GoogleAuth;
using OpportPortal.Application.Common;
using OpportPortal.Application.Services;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Enums;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.Interfaces.Services;
using OpportPortal.Domain.ValueObjects;

public class GoogleAuthHandler : IHandler<GoogleAuthCommand, ResultViewModel<GoogleAuthResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IGoogleAuthService _googleAuthService;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;

    public GoogleAuthHandler(
        IUserRepository userRepository,
        IAuthService authService,
        IGoogleAuthService googleAuthService,
        IEmailService emailService,
        INotificationService notificationService)
    {
        _userRepository = userRepository;
        _authService = authService;
        _googleAuthService = googleAuthService;
        _emailService = emailService;
        _notificationService = notificationService;
    }

    public async Task<ResultViewModel<GoogleAuthResponse>> HandleAsync(GoogleAuthCommand request)
    {
        var googleUser = await _googleAuthService.ValidateAsync(request.IdToken);
        if (googleUser is null)
            return ResultViewModel<GoogleAuthResponse>.Error("Token do Google inválido.");

        var user = await _userRepository.GetByEmailAsync(googleUser.Email);

        if (user is null)
        {
            if (request.UserType is null)
                return ResultViewModel<GoogleAuthResponse>.Success(new GoogleAuthResponse(null, true), "Registration required");

            var email = new Email(googleUser.Email);
            user = User.CreateGoogleUser(email, request.UserType.Value);
            await _userRepository.Create(user);

            var name = string.IsNullOrWhiteSpace(googleUser.Name) ? googleUser.Email.Split('@')[0] : googleUser.Name;

            var (subject, html) = EmailTemplates.Welcome(name);
            await _emailService.SendAsync(googleUser.Email, name, subject, html);

            await _notificationService.NotifyAsync(user.Id, "Bem-vindo(a)! 🎉", "Sua conta foi criada com o Google.", NotificationType.Welcome);
        }
        else if (!user.Active)
        {
            return ResultViewModel<GoogleAuthResponse>.Error("User is inactive.");
        }

        var token = _authService.GenerateToken(user);
        return ResultViewModel<GoogleAuthResponse>.Success(
            new GoogleAuthResponse(token, false), "Authenticated user");
    }
}