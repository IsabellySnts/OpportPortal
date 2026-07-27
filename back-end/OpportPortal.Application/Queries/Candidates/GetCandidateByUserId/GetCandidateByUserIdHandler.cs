using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Candidates;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Candidates.GetCandidateByUserId;

public class GetCandidateByUserIdHandler : IHandler<GetCandidateByUserIdQuery, ResultViewModel<CandidateResponse>>
{
    private readonly ICandidateRepository _candidateRepository;

    public GetCandidateByUserIdHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<ResultViewModel<CandidateResponse>> HandleAsync(GetCandidateByUserIdQuery request)
    {
        var candidate = await _candidateRepository.GetByUserIdAsync(request.UserId);

        if (candidate is null)
            return ResultViewModel<CandidateResponse>.Error("Candidate not found.");

        var response = new CandidateResponse(
            candidate.Id, candidate.UserId, candidate.FullName,
            candidate.BirthDate, candidate.PhoneNumber.Value,
            candidate.Address.ZipCode, candidate.Address.Street,
            candidate.Address.Number, candidate.Address.Neighborhood,
            candidate.Address.City, candidate.Address.State,
            candidate.Address.Country, candidate.Address.Complement,
            candidate.UrlLinkedin, candidate.CreatedAt, candidate.UpdatedAt
        );

        return ResultViewModel<CandidateResponse>.Success(response);
    }
}