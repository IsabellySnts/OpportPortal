using Microsoft.EntityFrameworkCore;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Infrastructure.Persistence.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly OpportPortalDbContext _context;

    public CompanyRepository(OpportPortalDbContext context)
    {
        _context = context;
    }

    public async Task<long> Create(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        return company.Id;
    }

    public async Task<Company?> GetByIdAsync(long id)
        => await _context.Companies
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> ExistsByUserIdAsync(long userId)
        => await _context.Companies.AnyAsync(c => c.UserId == userId);

    public async Task<bool> ExistsByCnpjAsync(string cnpj)
        => await _context.Companies.AnyAsync(c => c.Cnpj.Value == cnpj);

    public async Task<IEnumerable<Company>> GetAllAsync()
        => await _context.Companies
            .Include(c => c.User)
            .ToListAsync();

    public async Task Update(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Company company)
    {
        company.Delete();
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }
}
