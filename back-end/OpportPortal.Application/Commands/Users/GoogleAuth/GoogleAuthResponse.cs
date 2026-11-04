namespace OpportPortal.Application.Commands.Users.GoogleAuth;

public record GoogleAuthResponse(string? Token, bool RequiresRegistration);
