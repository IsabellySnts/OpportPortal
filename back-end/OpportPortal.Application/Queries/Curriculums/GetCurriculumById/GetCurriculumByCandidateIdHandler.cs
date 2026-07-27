using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Curriculums.GetCurriculumByCandidateId;

public class GetCurriculumByCandidateIdHandler : IHandler<GetCurriculumByCandidateIdQuery, ResultViewModel<CurriculumResponse>>
{
    private readonly ICurriculumRepository _curriculumRepository;

    public GetCurriculumByCandidateIdHandler(ICurriculumRepository curriculumRepository)
    {
        _curriculumRepository = curriculumRepository;
    }

    public async Task<ResultViewModel<CurriculumResponse>> HandleAsync(GetCurriculumByCandidateIdQuery request)
    {
        var curriculum = await _curriculumRepository.GetByCandidateIdAsync(request.CandidateId);

        if (curriculum is null)
            return ResultViewModel<CurriculumResponse>.Error("Curriculum not found.");

        return ResultViewModel<CurriculumResponse>.Success(new CurriculumResponse(
            curriculum.Id, curriculum.CandidateId, curriculum.EducationLevel,
            curriculum.HardSkills, curriculum.SoftSkills,
            curriculum.CreatedAt, curriculum.UpdatedAt
        ));
    }
}