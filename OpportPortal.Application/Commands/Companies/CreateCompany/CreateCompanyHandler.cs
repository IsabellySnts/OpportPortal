using OpportPortal.Application.Common;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Application.Commands.Companies.CreateCompany;

public class CreateCompanyHandler : IHandler<CreateCompanyCommand, ResultViewModel<long>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository;

    public CreateCompanyHandler(ICompanyRepository companyRepository, IUserRepository userRepository)
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(CreateCompanyCommand request)
    {
        if (await _userRepository.GetByIdAsync(request.UserId) is null)
            return ResultViewModel<long>.Error("User not found.");

        if (await _companyRepository.ExistsByUserIdAsync(request.UserId))
            return ResultViewModel<long>.Error("Company already exists for this user.");

        if (await _companyRepository.ExistsByCnpjAsync(request.Cnpj))
            return ResultViewModel<long>.Error("CNPJ already registered.");

        var cnpj = new Cnpj(request.Cnpj);
        var email = new Email(request.Email);
        var phone = new Phone(request.PhoneNumber);
        var address = new Address(
            request.ZipCode, request.Street, request.Number,
            request.Neighborhood, request.City, request.State,
            request.Country, request.Complement
        );

        var company = new Company(
            request.UserId, request.CompanyName, cnpj,
            request.Sector, email, phone, address
        );

        var id = await _companyRepository.Create(company);

        return ResultViewModel<long>.Success(id, "Company created successfully.");
    }
}
