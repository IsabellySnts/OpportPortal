using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Application.Commands.Companies.UpdateCompany;

public class UpdateCompanyHandler : IHandler<UpdateCompanyCommand, ResultViewModel<long>>
{
    private readonly ICompanyRepository _companyRepository;

    public UpdateCompanyHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(UpdateCompanyCommand request)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id);

        if (company is null)
            return ResultViewModel<long>.Error("Company not found.");

        var phone = new Phone(request.PhoneNumber);
        var address = new Address(
            request.ZipCode, request.Street, request.Number,
            request.Neighborhood, request.City, request.State,
            request.Country, request.Complement
        );

        company.Update(request.CompanyName, request.Sector, phone, address);

        await _companyRepository.Update(company);

        return ResultViewModel<long>.Success(company.Id, "Company updated successfully.");
    }
}
