using OpportPortal.Domain.Entities;

namespace OpportPortal.Domain.Interfaces.Repositories;

public interface ICandidateRepository
{
    Task<long> Create(Candidate candidate);
    Task<bool> ExistsByUserIdAsync (long userId);
    Task<Candidate?> GetByIdAsync(long id);
    Task Update(Candidate candidate);
    Task Delete(Candidate candidate);
    Task<IEnumerable<Candidate>> GetAllAsync();

}
