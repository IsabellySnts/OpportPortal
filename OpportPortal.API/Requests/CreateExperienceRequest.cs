namespace OpportPortal.API.Requests;

public record CreateExperienceRequest(
    long CurriculumId,
    string JobTitle,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description
);

