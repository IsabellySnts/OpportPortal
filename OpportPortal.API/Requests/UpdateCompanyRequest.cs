namespace OpportPortal.API.Requests;

public record UpdateCompanyRequest(
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
