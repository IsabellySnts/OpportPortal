namespace OpportPortal.Application.Commands.Companies.CreateCompany;

public record CreateCompanyCommand(
    long UserId,
    string CompanyName,
    string Cnpj,
    string Sector,
    string Email,
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
