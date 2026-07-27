using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Opportunities.DeleteOpportunity;

public class DeleteOpportunityHandler : IHandler<DeleteOpportunityCommand, ResultViewModel<long>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public DeleteOpportunityHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(DeleteOpportunityCommand request)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(request.Id);

        if (opportunity is null)
            return ResultViewModel<long>.Error("Opportunity not found.");

        await _opportunityRepository.Delete(opportunity);

        return ResultViewModel<long>.Success(opportunity.Id, "Opportunity deleted successfully.");
    }
}