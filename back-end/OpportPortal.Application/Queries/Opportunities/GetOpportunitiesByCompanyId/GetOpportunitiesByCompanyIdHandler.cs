using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Opportunities.GetOpportunitiesByCompanyId;

public class GetOpportunitiesByCompanyIdHandler : IHandler<GetOpportunitiesByCompanyIdQuery, ResultViewModel<IEnumerable<OpportunityResponse>>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public GetOpportunitiesByCompanyIdHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<ResultViewModel<IEnumerable<OpportunityResponse>>> HandleAsync(GetOpportunitiesByCompanyIdQuery request)
    {
        var opportunities = await _opportunityRepository.GetByCompanyIdAsync(request.CompanyId);

        var response = opportunities.Select(o => new OpportunityResponse(
            o.Id, o.CompanyId, o.Company?.CompanyName ?? "",
            o.Title, o.Description, o.Type, o.Format,
            o.Requirements, o.FieldOfWork, o.State,
            o.Salary, o.Status, o.CreatedAt, o.UpdatedAt
        ));

        return ResultViewModel<IEnumerable<OpportunityResponse>>.Success(response);
    }
}