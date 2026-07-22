using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Opportunities.GetOpportunityById;

public class GetOpportunityByIdHandler : IHandler<GetOpportunityByIdQuery, ResultViewModel<OpportunityResponse>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public GetOpportunityByIdHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<ResultViewModel<OpportunityResponse>> HandleAsync(GetOpportunityByIdQuery request)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(request.Id);

        if (opportunity is null)
            return ResultViewModel<OpportunityResponse>.Error("Opportunity not found.");

        return ResultViewModel<OpportunityResponse>.Success(ToResponse(opportunity));
    }

    private static OpportunityResponse ToResponse(Domain.Entities.Opportunity o) => new(
        o.Id, o.CompanyId, o.Company.CompanyName,
        o.Title, o.Description, o.Type, o.Format,
        o.Requirements, o.FieldOfWork, o.State,
        o.Salary, o.Status, o.CreatedAt, o.UpdatedAt
    );
}