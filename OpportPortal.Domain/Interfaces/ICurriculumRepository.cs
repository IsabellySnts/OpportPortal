using OpportPortal.Domain.Entities;

namespace OpportPortal.Domain.Interfaces.Repositories;

public interface ICurriculumRepository
{
    Task<long> Create(Curriculum curriculum);
    Task<Curriculum?> GetByIdAsync(long id);
    Task<Curriculum?> GetByCandidateIdAsync(long candidateId);
    Task<bool> ExistsByCandidateIdAsync(long candidateId);
    Task Update(Curriculum curriculum);
    Task Delete(Curriculum curriculum);
}
