using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Curriculums.GetCurriculumById;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Curriculums.GetCurriculumByCandidateId;

public class GetCurriculumByIdHandler : IHandler<GetCurriculumByIdQuery, ResultViewModel<CurriculumResponse>>
{
    private readonly ICurriculumRepository _curriculumRepository;

    public GetCurriculumByIdHandler(ICurriculumRepository curriculumRepository)
    {
        _curriculumRepository = curriculumRepository;
    }

    public async Task<ResultViewModel<CurriculumResponse>> HandleAsync(GetCurriculumByIdQuery request)
    {
        var curriculum = await _curriculumRepository.GetByIdAsync(request.Id);

        if (curriculum is null)
            return ResultViewModel<CurriculumResponse>.Error("Curriculum not found.");

        return ResultViewModel<CurriculumResponse>.Success(ToResponse(curriculum));
    }

    private static CurriculumResponse ToResponse(Domain.Entities.Curriculum cv) => new(
        cv.Id, cv.CandidateId, cv.EducationLevel,
        cv.HardSkills, cv.SoftSkills,
        cv.CreatedAt, cv.UpdatedAt
    );
}