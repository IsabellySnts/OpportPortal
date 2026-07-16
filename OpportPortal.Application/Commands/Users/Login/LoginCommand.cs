namespace OpportPortal.Application.Commands.Users.Login;

public record LoginCommand(string Email, string Password);

public record LoginResponse(string Token);

