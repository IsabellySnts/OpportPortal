using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Companies.GetAllCompanies;

public class GetAllCompaniesHandler : IHandler<GetAllCompaniesQuery, ResultViewModel<IEnumerable<CompanyResponse>>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetAllCompaniesHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ResultViewModel<IEnumerable<CompanyResponse>>> HandleAsync(GetAllCompaniesQuery request)
    {
        var companies = await _companyRepository.GetAllAsync();

        var response = companies.Select(c => new CompanyResponse(
            c.Id, c.UserId, c.CompanyName, c.Cnpj.Value, c.Sector,
            c.Email.Value, c.PhoneNumber.Value,
            c.Address.ZipCode, c.Address.Street, c.Address.Number,
            c.Address.Neighborhood, c.Address.City, c.Address.State,
            c.Address.Country, c.Address.Complement,
            c.CreatedAt, c.UpdatedAt
        ));

        return ResultViewModel<IEnumerable<CompanyResponse>>.Success(response);
    }
}

