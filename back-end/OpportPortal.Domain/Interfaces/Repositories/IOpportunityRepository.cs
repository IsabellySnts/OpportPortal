using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Enums;

namespace OpportPortal.Domain.Interfaces.Repositories;

public interface IOpportunityRepository
{
    Task<long> Create(Opportunity opportunity);
    Task<Opportunity?> GetByIdAsync(long id);
    Task<IEnumerable<Opportunity>> GetAllAsync();
    Task<IEnumerable<Opportunity>> GetByCompanyIdAsync(long companyId);
    Task<IEnumerable<Opportunity>> GetByFilterAsync(string? fieldOfWork, string? state, OpportunityType? type, JobFormat? format);
    Task Update(Opportunity opportunity);
    Task Delete(Opportunity opportunity);
}