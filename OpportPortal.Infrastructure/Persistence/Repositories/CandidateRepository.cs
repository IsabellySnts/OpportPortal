using Microsoft.EntityFrameworkCore;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Infrastructure.Persistence.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly OpportPortalDbContext _context;

    public CandidateRepository(OpportPortalDbContext context)
    {
        _context = context;
    }
    public async Task<long> Create(Candidate candidate)
    {
        await _context.Candidates.AddAsync(candidate);
        await _context.SaveChangesAsync();
        return candidate.Id;
    }

    public async Task<bool> ExistsByUserIdAsync(long userId)
    => await _context.Candidates.AnyAsync(c => c.UserId == userId);

    public async Task<Candidate?> GetByIdAsync(long id)
        => await _context.Candidates
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task Update(Candidate candidate)
    {
        _context.Candidates.Update(candidate);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Candidate candidate)
    {
        candidate.Delete(); 
        _context.Candidates.Update(candidate);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Candidate>> GetAllAsync()
    => await _context.Candidates
        .Include(c => c.User)
        .ToListAsync();
}
