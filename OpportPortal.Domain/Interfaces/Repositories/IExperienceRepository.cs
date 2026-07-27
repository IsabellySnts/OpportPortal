using OpportPortal.Domain.Entities;

namespace OpportPortal.Domain.Interfaces.Repositories;

public interface IExperienceRepository
{
    Task<long> Create(Experience experience);
    Task<Experience?> GetByIdAsync(long id);
    Task<IEnumerable<Experience>> GetByCurriculumIdAsync(long curriculumId);
    Task Update(Experience experience);
    Task Delete(Experience experience);
}