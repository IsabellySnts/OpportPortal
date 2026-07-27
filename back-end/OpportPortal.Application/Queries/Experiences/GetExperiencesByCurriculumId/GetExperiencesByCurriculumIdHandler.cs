using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Experiences.GetExperiencesByCurriculumId;

public class GetExperiencesByCurriculumIdHandler : IHandler<GetExperiencesByCurriculumIdQuery, ResultViewModel<IEnumerable<ExperienceResponse>>>
{
    private readonly IExperienceRepository _experienceRepository;

    public GetExperiencesByCurriculumIdHandler(IExperienceRepository experienceRepository)
    {
        _experienceRepository = experienceRepository;
    }

    public async Task<ResultViewModel<IEnumerable<ExperienceResponse>>> HandleAsync(GetExperiencesByCurriculumIdQuery request)
    {
        var experiences = await _experienceRepository.GetByCurriculumIdAsync(request.CurriculumId);

        var response = experiences.Select(e => new ExperienceResponse(
            e.Id, e.CurriculumId, e.JobTitle, e.CompanyName,
            e.StartDate, e.EndDate, e.Description,
            e.CreatedAt, e.UpdatedAt
        ));

        return ResultViewModel<IEnumerable<ExperienceResponse>>.Success(response);
    }
}