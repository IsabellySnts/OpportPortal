using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.Companies.GetCompanyById;

public class GetCompanyByIdHandler : IHandler<GetCompanyByIdQuery, ResultViewModel<CompanyResponse>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyByIdHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ResultViewModel<CompanyResponse>> HandleAsync(GetCompanyByIdQuery request)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id);

        if (company is null)
            return ResultViewModel<CompanyResponse>.Error("Company not found.");

        return ResultViewModel<CompanyResponse>.Success(ToResponse(company));
    }

    private static CompanyResponse ToResponse(Domain.Entities.Company c) => new(
        c.Id, c.UserId, c.CompanyName, c.Cnpj.Value, c.Sector,
        c.Email.Value, c.PhoneNumber.Value,
        c.Address.ZipCode, c.Address.Street, c.Address.Number,
        c.Address.Neighborhood, c.Address.City, c.Address.State,
        c.Address.Country, c.Address.Complement,
        c.CreatedAt, c.UpdatedAt
    );
}
