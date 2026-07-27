using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Opportunities.UpdateOpportunity;

public class UpdateOpportunityHandler : IHandler<UpdateOpportunityCommand, ResultViewModel<long>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public UpdateOpportunityHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(UpdateOpportunityCommand request)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(request.Id);

        if (opportunity is null)
            return ResultViewModel<long>.Error("Opportunity not found.");

        if (opportunity.Status == Domain.Enums.OpportunityStatus.Closed)
            return ResultViewModel<long>.Error("Cannot update a closed opportunity.");

        opportunity.Update(
            request.Title, request.Description, request.Type,
            request.Format, request.Requirements, request.FieldOfWork,
            request.State, request.Salary
        );

        await _opportunityRepository.Update(opportunity);

        return ResultViewModel<long>.Success(opportunity.Id, "Opportunity updated successfully.");
    }
}