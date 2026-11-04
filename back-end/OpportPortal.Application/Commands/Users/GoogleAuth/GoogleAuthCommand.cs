using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Commands.Users.GoogleAuth;

public record GoogleAuthCommand(string IdToken, UserType? UserType);
