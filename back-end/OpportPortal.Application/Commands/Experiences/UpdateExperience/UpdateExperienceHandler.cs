using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Experiences.UpdateExperience;

public class UpdateExperienceHandler : IHandler<UpdateExperienceCommand, ResultViewModel<long>>
{
    private readonly IExperienceRepository _experienceRepository;

    public UpdateExperienceHandler(IExperienceRepository experienceRepository)
    {
        _experienceRepository = experienceRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(UpdateExperienceCommand request)
    {
        var experience = await _experienceRepository.GetByIdAsync(request.Id);

        if (experience is null)
            return ResultViewModel<long>.Error("Experience not found.");

        experience.Update(
            request.JobTitle,
            request.CompanyName,
            request.StartDate,
            request.EndDate,
            request.Description
        );

        await _experienceRepository.Update(experience);

        return ResultViewModel<long>.Success(experience.Id, "Experience updated successfully.");
    }
}