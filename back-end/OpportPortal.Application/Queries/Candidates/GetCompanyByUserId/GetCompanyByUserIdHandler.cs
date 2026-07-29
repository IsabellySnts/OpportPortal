using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Companies;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Companies.GetCompanyByUserId;

public class GetCompanyByUserIdHandler : IHandler<GetCompanyByUserIdQuery, ResultViewModel<CompanyResponse>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyByUserIdHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ResultViewModel<CompanyResponse>> HandleAsync(GetCompanyByUserIdQuery request)
    {
        var company = await _companyRepository.GetByUserIdAsync(request.UserId);

        if (company is null)
            return ResultViewModel<CompanyResponse>.Error("Company not found.");

        return ResultViewModel<CompanyResponse>.Success(new CompanyResponse(
            company.Id, company.UserId, company.CompanyName, company.Cnpj.Value,
            company.Sector, company.Email.Value, company.PhoneNumber.Value,
            company.Address.ZipCode, company.Address.Street, company.Address.Number,
            company.Address.Neighborhood, company.Address.City, company.Address.State,
            company.Address.Country, company.Address.Complement,
            company.CreatedAt, company.UpdatedAt
        ));
    }
}