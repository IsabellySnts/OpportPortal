using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Candidates.GetCandidateById;

public class GetCandidateByIdHandler : IHandler<GetCandidateByIdQuery, ResultViewModel<CandidateResponse>>
{
    private readonly ICandidateRepository _candidateRepository;

    public GetCandidateByIdHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<ResultViewModel<CandidateResponse>> HandleAsync(GetCandidateByIdQuery request)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.Id);

        if (candidate is null)
            return ResultViewModel<CandidateResponse>.Error("Candidate not found.");

        var response = new CandidateResponse(
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
        );

        return ResultViewModel<CandidateResponse>.Success(response);
    }
}
