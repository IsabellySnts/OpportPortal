using Microsoft.EntityFrameworkCore;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Infrastructure.Persistence.Repositories;

public class CurriculumRepository : ICurriculumRepository
{
    private readonly OpportPortalDbContext _context;

    public CurriculumRepository(OpportPortalDbContext context)
    {
        _context = context;
    }

    public async Task<long> Create(Curriculum curriculum)
    {
        await _context.Curriculums.AddAsync(curriculum);
        await _context.SaveChangesAsync();
        return curriculum.Id;
    }

    public async Task<Curriculum?> GetByIdAsync(long id)
        => await _context.Curriculums
            .Include(cv => cv.Candidate)
            .Include(cv => cv.Experiences)
            .FirstOrDefaultAsync(cv => cv.Id == id);

    public async Task<Curriculum?> GetByCandidateIdAsync(long candidateId)
        => await _context.Curriculums
            .Include(cv => cv.Experiences)
            .FirstOrDefaultAsync(cv => cv.CandidateId == candidateId);

    public async Task<bool> ExistsByCandidateIdAsync(long candidateId)
        => await _context.Curriculums.AnyAsync(cv => cv.CandidateId == candidateId);

    public async Task Update(Curriculum curriculum)
    {
        _context.Curriculums.Update(curriculum);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Curriculum curriculum)
    {
        curriculum.Delete();
        _context.Curriculums.Update(curriculum);
        await _context.SaveChangesAsync();
    }
}