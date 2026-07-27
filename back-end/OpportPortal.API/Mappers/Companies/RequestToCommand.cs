using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.Companies.CreateCompany;
using OpportPortal.Application.Commands.Companies.UpdateCompany;

namespace OpportPortal.API.Mappers.Companies;

internal static class RequestToCommand
{
    public static CreateCompanyCommand CreateCompanyRequestToCommand(this CreateCompanyRequest request, long userId)
    {
        return new CreateCompanyCommand(
            userId,
            request.CompanyName,
            request.Cnpj,
            request.Sector,
            request.Email,
            request.PhoneNumber,
            request.ZipCode,
            request.Street,
            request.Number,
            request.Neighborhood,
            request.City,
            request.State,
            request.Country,
            request.Complement
        );
    }

    public static UpdateCompanyCommand UpdateCompanyRequestToCommand(this UpdateCompanyRequest request, long id)
    {
        return new UpdateCompanyCommand(
            id,
            request.CompanyName,
            request.Sector,
            request.PhoneNumber,
            request.ZipCode,
            request.Street,
            request.Number,
            request.Neighborhood,
            request.City,
            request.State,
            request.Country,
            request.Complement
        );
    }

}
