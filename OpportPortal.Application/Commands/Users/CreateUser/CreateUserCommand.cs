using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Commands.Users.CreateUser;

public record CreateUserCommand
(
    string UserEmail,
    string Password,
    UserType Type
);
