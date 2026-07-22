using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Experiences.GetExperienceById;

public class GetExperienceByIdHandler : IHandler<GetExperienceByIdQuery, ResultViewModel<ExperienceResponse>>
{
    private readonly IExperienceRepository _experienceRepository;

    public GetExperienceByIdHandler(IExperienceRepository experienceRepository)
    {
        _experienceRepository = experienceRepository;
    }

    public async Task<ResultViewModel<ExperienceResponse>> HandleAsync(GetExperienceByIdQuery request)
    {
        var experience = await _experienceRepository.GetByIdAsync(request.Id);

        if (experience is null)
            return ResultViewModel<ExperienceResponse>.Error("Experience not found.");

        return ResultViewModel<ExperienceResponse>.Success(ToResponse(experience));
    }

    private static ExperienceResponse ToResponse(Domain.Entities.Experience e) => new(
        e.Id, e.CurriculumId, e.JobTitle, e.CompanyName,
        e.StartDate, e.EndDate, e.Description,
        e.CreatedAt, e.UpdatedAt
    );
}