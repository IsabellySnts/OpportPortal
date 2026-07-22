using Microsoft.Extensions.DependencyInjection;
using OpportPortal.Application.Commands.Candidates.CreateCandidate;
using OpportPortal.Application.Commands.Candidates.DeleteCandidate;
using OpportPortal.Application.Commands.Candidates.UpdateCandidate;
using OpportPortal.Application.Commands.Companies.CreateCompany;
using OpportPortal.Application.Commands.Companies.DeleteCompany;
using OpportPortal.Application.Commands.Companies.UpdateCompany;
using OpportPortal.Application.Commands.Curriculums.CreateCurriculum;
using OpportPortal.Application.Commands.Curriculums.DeleteCurriculum;
using OpportPortal.Application.Commands.Curriculums.UpdateCurriculum;
using OpportPortal.Application.Commands.Users.CreateUser;
using OpportPortal.Application.Commands.Users.Login;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Candidates;
using OpportPortal.Application.Queries.Candidates.GetAllCandidates;
using OpportPortal.Application.Queries.Candidates.GetCandidateById;
using OpportPortal.Application.Queries.Companies;
using OpportPortal.Application.Queries.Companies.GetAllCompanies;
using OpportPortal.Application.Queries.Companies.GetCompanyById;
using OpportPortal.Application.Queries.Curriculums;
using OpportPortal.Application.Queries.Curriculums.GetCurriculumByCandidateId;
using OpportPortal.Application.Queries.Curriculums.GetCurriculumById;
using OpportPortal.Domain.Interfaces.Repositories;

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
        services.AddScoped<IHandler<CreateCompanyCommand, ResultViewModel<long>>, CreateCompanyHandler>();
        services.AddScoped<IHandler<UpdateCompanyCommand, ResultViewModel<long>>, UpdateCompanyHandler>();
        services.AddScoped<IHandler<DeleteCompanyCommand, ResultViewModel<long>>, DeleteCompanyHandler>();
        services.AddScoped<IHandler<GetCompanyByIdQuery, ResultViewModel<CompanyResponse>>, GetCompanyByIdHandler>();
        services.AddScoped<IHandler<GetAllCompaniesQuery, ResultViewModel<IEnumerable<CompanyResponse>>>, GetAllCompaniesHandler>();
        services.AddScoped<IHandler<CreateCurriculumCommand, ResultViewModel<long>>, CreateCurriculumHandler>();
        services.AddScoped<IHandler<UpdateCurriculumCommand, ResultViewModel<long>>, UpdateCurriculumHandler>();
        services.AddScoped<IHandler<DeleteCurriculumCommand, ResultViewModel<long>>, DeleteCurriculumHandler>();
        services.AddScoped<IHandler<GetCurriculumByIdQuery, ResultViewModel<CurriculumResponse>>, GetCurriculumByIdHandler>();
        services.AddScoped<IHandler<GetCurriculumByCandidateIdQuery, ResultViewModel<CurriculumResponse>>, GetCurriculumByCandidateIdHandler>();
        return services;
    }
}
