using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Experiences.DeleteExperience;

public class DeleteExperienceHandler : IHandler<DeleteExperienceCommand, ResultViewModel<long>>
{
    private readonly IExperienceRepository _experienceRepository;

    public DeleteExperienceHandler(IExperienceRepository experienceRepository)
    {
        _experienceRepository = experienceRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(DeleteExperienceCommand request)
    {
        var experience = await _experienceRepository.GetByIdAsync(request.Id);

        if (experience is null)
            return ResultViewModel<long>.Error("Experience not found.");

        await _experienceRepository.Delete(experience);

        return ResultViewModel<long>.Success(experience.Id, "Experience deleted successfully.");
    }
}