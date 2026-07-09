namespace OpportPortal.Application.Commands.Login;

public record LoginCommand(string Email, string Password);

public record LoginResponse(string Token);

