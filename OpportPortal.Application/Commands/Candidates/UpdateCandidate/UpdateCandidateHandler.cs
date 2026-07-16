using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Application.Commands.Candidates.UpdateCandidate;

public class UpdateCandidateHandler : IHandler<UpdateCandidateCommand, ResultViewModel<long>>
{
    private readonly ICandidateRepository _candidateRepository;

    public UpdateCandidateHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(UpdateCandidateCommand request)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.Id);

        if (candidate is null)
            return ResultViewModel<long>.Error("Candidate not found.");

        var address = new Address(
            request.ZipCode,
            request.Street,
            request.Number,
            request.Neighborhood,
            request.City,
            request.State,
            request.Country,
            request.Complement
        );

        var phone = new Phone(request.PhoneNumber);

        candidate.Update(request.FullName, request.BirthDate, address, phone);

        if (!string.IsNullOrWhiteSpace(request.UrlLinkedin))
            candidate.SetLinkedin(request.UrlLinkedin);

        await _candidateRepository.Update(candidate);

        return ResultViewModel<long>.Success(candidate.Id, "Candidate updated successfully.");
    }
}