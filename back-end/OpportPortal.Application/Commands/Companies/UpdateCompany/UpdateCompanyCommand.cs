namespace OpportPortal.Application.Commands.Companies.UpdateCompany;

public record UpdateCompanyCommand(
    long Id,
    string CompanyName,
    string Sector,
    string PhoneNumber,
    string ZipCode,
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string Country,
    string? Complement
);
