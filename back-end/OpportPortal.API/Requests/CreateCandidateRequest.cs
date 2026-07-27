namespace OpportPortal.API.Requests;

public record CreateCandidateRequest(
    string FullName,
    DateTime BirthDate,
    string ZipCode,
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string Country,
    string? Complement,
    string PhoneNumber,
    string? UrlLinkedin
);

