using Microsoft.EntityFrameworkCore;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Infrastructure.Persistence.Repositories;

public class ExperienceRepository : IExperienceRepository
{
    private readonly OpportPortalDbContext _context;

    public ExperienceRepository(OpportPortalDbContext context)
    {
        _context = context;
    }

    public async Task<long> Create(Experience experience)
    {
        await _context.Experiences.AddAsync(experience);
        await _context.SaveChangesAsync();
        return experience.Id;
    }

    public async Task<Experience?> GetByIdAsync(long id)
        => await _context.Experiences
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Experience>> GetByCurriculumIdAsync(long curriculumId)
        => await _context.Experiences
            .Where(e => e.CurriculumId == curriculumId)
            .ToListAsync();

    public async Task Update(Experience experience)
    {
        _context.Experiences.Update(experience);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Experience experience)
    {
        experience.Delete();
        _context.Experiences.Update(experience);
        await _context.SaveChangesAsync();
    }
}