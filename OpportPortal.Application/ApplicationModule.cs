using Microsoft.Extensions.DependencyInjection;
using OpportPortal.Application.Commands.CreateCandidate;
using OpportPortal.Application.Commands.DeleteCandidate;
using OpportPortal.Application.Commands.Login;
using OpportPortal.Application.Commands.UpdateCandidate;
using OpportPortal.Application.Commands.Users.CreateUser;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Candidates;
using OpportPortal.Application.Queries.Candidates.GetAllCandidates;
using OpportPortal.Application.Queries.Candidates.GetCandidateById;

namespace OpportPortal.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        services.AddScoped<IHandler<CreateUserCommand, ResultViewModel<long>>, CreateUserHandler>();
        services.AddScoped<IHandler<LoginCommand, ResultViewModel<LoginResponse>>, LoginHandler>();
        services.AddScoped<IHandler<CreateCandidateCommand, ResultViewModel<long>>, CreateCandidateHandler>();
        services.AddScoped<IHandler<UpdateCandidateCommand, ResultViewModel<long>>, UpdateCandidateHandler>();
        services.AddScoped<IHandler<DeleteCandidateCommand, ResultViewModel<long>>, DeleteCandidateHandler>();
        services.AddScoped<IHandler<GetCandidateByIdQuery, ResultViewModel<CandidateResponse>>, GetCandidateByIdHandler>();
        services.AddScoped<IHandler<GetAllCandidatesQuery, ResultViewModel<IEnumerable<CandidateResponse>>>, GetAllCandidatesHandler>();
        return services;
    }
}
