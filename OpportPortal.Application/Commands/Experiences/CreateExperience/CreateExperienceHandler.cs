using OpportPortal.Application.Common;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Experiences.CreateExperience;

public class CreateExperienceHandler : IHandler<CreateExperienceCommand, ResultViewModel<long>>
{
    private readonly IExperienceRepository _experienceRepository;
    private readonly ICurriculumRepository _curriculumRepository;

    public CreateExperienceHandler(
        IExperienceRepository experienceRepository,
        ICurriculumRepository curriculumRepository)
    {
        _experienceRepository = experienceRepository;
        _curriculumRepository = curriculumRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(CreateExperienceCommand request)
    {
        if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) is null)
            return ResultViewModel<long>.Error("Curriculum not found.");

        var experience = new Experience(
            request.CurriculumId,
            request.JobTitle,
            request.CompanyName,
            request.StartDate,
            request.EndDate,
            request.Description
        );

        var id = await _experienceRepository.Create(experience);

        return ResultViewModel<long>.Success(id, "Experience created successfully.");
    }
}