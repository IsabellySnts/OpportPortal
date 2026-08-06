using OpportPortal.Application.Common;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Opportunities.CreateOpportunity;

public class CreateOpportunityHandler : IHandler<CreateOpportunityCommand, ResultViewModel<long>>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly ICompanyRepository _companyRepository;

    public CreateOpportunityHandler(
        IOpportunityRepository opportunityRepository,
        ICompanyRepository companyRepository)
    {
        _opportunityRepository = opportunityRepository;
        _companyRepository = companyRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(CreateOpportunityCommand request)
    {
        var company = await _companyRepository.GetByUserIdAsync(request.UserId);

        if (company is null)
            return ResultViewModel<long>.Error("Company not found.");

        var opportunity = new Opportunity(
            company.Id, 
            request.Title,
            request.Description,
            request.Type,
            request.Format,
            request.Requirements,
            request.FieldOfWork,
            request.State,
            request.Salary
        );

        var id = await _opportunityRepository.Create(opportunity);

        return ResultViewModel<long>.Success(id, "Opportunity created successfully.");
    }
}