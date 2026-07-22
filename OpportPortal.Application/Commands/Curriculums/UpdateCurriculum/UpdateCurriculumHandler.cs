using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Curriculums.UpdateCurriculum;

public class UpdateCurriculumHandler : IHandler<UpdateCurriculumCommand, ResultViewModel<long>>
{
    private readonly ICurriculumRepository _curriculumRepository;

    public UpdateCurriculumHandler(ICurriculumRepository curriculumRepository)
    {
        _curriculumRepository = curriculumRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(UpdateCurriculumCommand request)
    {
        var curriculum = await _curriculumRepository.GetByIdAsync(request.Id);

        if (curriculum is null)
            return ResultViewModel<long>.Error("Curriculum not found.");

        curriculum.Update(request.EducationLevel, request.HardSkills, request.SoftSkills);

        await _curriculumRepository.Update(curriculum);

        return ResultViewModel<long>.Success(curriculum.Id, "Curriculum updated successfully.");
    }
}