using Microsoft.EntityFrameworkCore;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Enums;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Infrastructure.Persistence.Repositories;

public class OpportunityRepository : IOpportunityRepository
{
    private readonly OpportPortalDbContext _context;

    public OpportunityRepository(OpportPortalDbContext context)
    {
        _context = context;
    }

    public async Task<long> Create(Opportunity opportunity)
    {
        await _context.Opportunities.AddAsync(opportunity);
        await _context.SaveChangesAsync();
        return opportunity.Id;
    }

    public async Task<Opportunity?> GetByIdAsync(long id)
        => await _context.Opportunities
            .Include(o => o.Company)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Opportunity>> GetAllAsync()
        => await _context.Opportunities
            .Include(o => o.Company)
            .Where(o => o.Status == OpportunityStatus.Active)
            .ToListAsync();

    public async Task<IEnumerable<Opportunity>> GetByCompanyIdAsync(long companyId)
        => await _context.Opportunities
            .Where(o => o.CompanyId == companyId)
            .ToListAsync();

    public async Task<IEnumerable<Opportunity>> GetByFilterAsync(string? fieldOfWork, string? state, OpportunityType? type, JobFormat? format)
        => await _context.Opportunities
            .Include(o => o.Company)
            .Where(o => o.Status == OpportunityStatus.Active)
            .Where(o => string.IsNullOrWhiteSpace(fieldOfWork) || o.FieldOfWork == fieldOfWork)
            .Where(o => string.IsNullOrWhiteSpace(state) || o.State == state)
            .Where(o => type == null || o.Type == type)
            .Where(o => format == null || o.Format == format)
            .ToListAsync();

    public async Task Update(Opportunity opportunity)
    {
        _context.Opportunities.Update(opportunity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Opportunity opportunity)
    {
        opportunity.Delete();
        _context.Opportunities.Update(opportunity);
        await _context.SaveChangesAsync();
    }
}