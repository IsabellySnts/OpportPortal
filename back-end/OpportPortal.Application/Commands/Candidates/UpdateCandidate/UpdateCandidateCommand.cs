namespace OpportPortal.Application.Commands.Candidates.UpdateCandidate;

public record UpdateCandidateCommand(
    long Id,
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
