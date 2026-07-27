using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Companies.DeleteCompany;

public class DeleteCompanyHandler : IHandler<DeleteCompanyCommand, ResultViewModel<long>>
{
    private readonly ICompanyRepository _companyRepository;

    public DeleteCompanyHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(DeleteCompanyCommand request)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id);

        if (company is null)
            return ResultViewModel<long>.Error("Company not found.");

        await _companyRepository.Delete(company);

        return ResultViewModel<long>.Success(company.Id, "Company deleted successfully.");
    }
}
