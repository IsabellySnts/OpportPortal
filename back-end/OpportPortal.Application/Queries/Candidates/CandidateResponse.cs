namespace OpportPortal.Application.Queries.Candidates;

public record CandidateResponse(
    long Id,
    long UserId,
    string FullName,
    DateTime BirthDate,
    string PhoneNumber,
    string ZipCode,
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string Country,
    string? Complement,
    string? UrlLinkedin,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
