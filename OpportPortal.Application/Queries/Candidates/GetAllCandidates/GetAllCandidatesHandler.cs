using global::OpportPortal.Application.Common;
using global::OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Candidates.GetAllCandidates;

public class GetAllCandidatesHandler : IHandler<GetAllCandidatesQuery, ResultViewModel<IEnumerable<CandidateResponse>>>
{
    private readonly ICandidateRepository _candidateRepository;

    public GetAllCandidatesHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<ResultViewModel<IEnumerable<CandidateResponse>>> HandleAsync(GetAllCandidatesQuery request)
    {
        var candidates = await _candidateRepository.GetAllAsync();

        var response = candidates.Select(candidate => new CandidateResponse(
            candidate.Id,
            candidate.UserId,
            candidate.FullName,
            candidate.BirthDate,
            candidate.PhoneNumber.Value,
            candidate.Address.ZipCode,
            candidate.Address.Street,
            candidate.Address.Number,
            candidate.Address.Neighborhood,
            candidate.Address.City,
            candidate.Address.State,
            candidate.Address.Country,
            candidate.Address.Complement,
            candidate.UrlLinkedin,
            candidate.CreatedAt,
            candidate.UpdatedAt
        ));

        return ResultViewModel<IEnumerable<CandidateResponse>>.Success(response);
    }
}