using OpportPortal.Domain.Entities;

namespace OpportPortal.Domain.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<long> Create(Company company);
    Task<Company?> GetByIdAsync(long id);
    Task<bool> ExistsByUserIdAsync(long userId);
    Task<bool> ExistsByCnpjAsync(string cnpj);
    Task<IEnumerable<Company>> GetAllAsync();
    Task Update(Company company);
    Task Delete(Company company);
}