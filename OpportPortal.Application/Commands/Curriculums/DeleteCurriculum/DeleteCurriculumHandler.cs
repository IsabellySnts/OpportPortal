using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Curriculums.DeleteCurriculum;

public class DeleteCurriculumHandler : IHandler<DeleteCurriculumCommand, ResultViewModel<long>>
{
    private readonly ICurriculumRepository _curriculumRepository;

    public DeleteCurriculumHandler(ICurriculumRepository curriculumRepository)
    {
        _curriculumRepository = curriculumRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(DeleteCurriculumCommand request)
    {
        var curriculum = await _curriculumRepository.GetByIdAsync(request.Id);

        if (curriculum is null)
            return ResultViewModel<long>.Error("Curriculum not found.");

        await _curriculumRepository.Delete(curriculum);

        return ResultViewModel<long>.Success(curriculum.Id, "Curriculum deleted successfully.");
    }
}
