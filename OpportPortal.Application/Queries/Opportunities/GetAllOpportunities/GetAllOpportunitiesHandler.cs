using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Opportunities.GetAllOpportunities;

public class GetAllOpportunitiesHandler : IHandler<GetAllOpportunitiesQuery, ResultViewModel<IEnumerable<OpportunityResponse>>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public GetAllOpportunitiesHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<ResultViewModel<IEnumerable<OpportunityResponse>>> HandleAsync(GetAllOpportunitiesQuery request)
    {
        var opportunities = await _opportunityRepository.GetAllAsync();

        var response = opportunities.Select(o => new OpportunityResponse(
            o.Id, o.CompanyId, o.Company.CompanyName,
            o.Title, o.Description, o.Type, o.Format,
            o.Requirements, o.FieldOfWork, o.State,
            o.Salary, o.Status, o.CreatedAt, o.UpdatedAt
        ));

        return ResultViewModel<IEnumerable<OpportunityResponse>>.Success(response);
    }
}