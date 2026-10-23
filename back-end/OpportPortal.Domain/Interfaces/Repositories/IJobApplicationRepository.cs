using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Enums;

namespace OpportPortal.Domain.Interfaces.Repositories;

public interface IJobApplicationRepository
{
    Task<long> Create(JobApplication jobApplication);
    Task<JobApplication?> GetByIdAsync(long id);
    Task<IEnumerable<JobApplication>> GetByCandidateIdAsync(long candidateId);
    Task<IEnumerable<JobApplication>> GetByOpportunityIdAsync(long opportunityId);
    Task<bool> ExistsAsync(long candidateId, long opportunityId);
    Task Update(JobApplication jobApplication);
    Task Delete(JobApplication jobApplication);
    Task<JobApplication?> GetByCandidateAndOpportunityAsync(long candidateId, long opportunityId);
}