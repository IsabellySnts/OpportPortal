using Microsoft.EntityFrameworkCore;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Infrastructure.Persistence.Repositories;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly OpportPortalDbContext _context;

    public JobApplicationRepository(OpportPortalDbContext context)
    {
        _context = context;
    }

    public async Task<long> Create(JobApplication jobApplication)
    {
        await _context.JobApplications.AddAsync(jobApplication);
        await _context.SaveChangesAsync();
        return jobApplication.Id;
    }

    public async Task<JobApplication?> GetByIdAsync(long id)
        => await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Opportunity)
            .FirstOrDefaultAsync(ja => ja.Id == id);

    public async Task<IEnumerable<JobApplication>> GetByCandidateIdAsync(long candidateId)
        => await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Opportunity)
            .Where(ja => ja.CandidateId == candidateId)
            .ToListAsync();

    public async Task<IEnumerable<JobApplication>> GetByOpportunityIdAsync(long opportunityId)
        => await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Opportunity)
            .Where(ja => ja.OpportunityId == opportunityId)
            .ToListAsync();

    public async Task<bool> ExistsAsync(long candidateId, long opportunityId)
        => await _context.JobApplications
            .AnyAsync(ja => ja.CandidateId == candidateId && ja.OpportunityId == opportunityId);

    public async Task Update(JobApplication jobApplication)
    {
        _context.JobApplications.Update(jobApplication);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(JobApplication jobApplication)
    {
        jobApplication.Delete();
        _context.JobApplications.Update(jobApplication);
        await _context.SaveChangesAsync();
    }
}