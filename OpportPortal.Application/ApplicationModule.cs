using Microsoft.Extensions.DependencyInjection;
using OpportPortal.Application.Commands.Login;
using OpportPortal.Application.Commands.Users.CreateUser;
using OpportPortal.Application.Common;

namespace OpportPortal.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        services.AddScoped<IHandler<CreateUserCommand, ResultViewModel<long>>, CreateUserHandler>();
        services.AddScoped<IHandler<LoginCommand, ResultViewModel<LoginResponse>>, LoginHandler>();

        return services;
    }
}
