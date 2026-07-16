using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Candidates.DeleteCandidate;

public class DeleteCandidateHandler : IHandler<DeleteCandidateCommand, ResultViewModel<long>>
{
    private readonly ICandidateRepository _candidateRepository;

    public DeleteCandidateHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(DeleteCandidateCommand request)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.Id);

        if (candidate is null)
            return ResultViewModel<long>.Error("Candidate not found.");

        await _candidateRepository.Delete(candidate);

        return ResultViewModel<long>.Success(candidate.Id, "Candidate deleted successfully.");
    }
}
