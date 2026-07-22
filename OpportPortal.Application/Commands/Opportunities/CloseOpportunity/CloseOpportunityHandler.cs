using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Opportunities.CloseOpportunity;

public class CloseOpportunityHandler : IHandler<CloseOpportunityCommand, ResultViewModel<long>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public CloseOpportunityHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(CloseOpportunityCommand request)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(request.Id);

        if (opportunity is null)
            return ResultViewModel<long>.Error("Opportunity not found.");

        opportunity.Close();

        await _opportunityRepository.Update(opportunity);

        return ResultViewModel<long>.Success(opportunity.Id, "Opportunity closed successfully.");
    }
}