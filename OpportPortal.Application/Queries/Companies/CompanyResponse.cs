namespace OpportPortal.Application.Queries.Companies;

public record CompanyResponse(
    long Id,
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
    string? Complement,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

