namespace OpportPortal.Application.Queries.Experiences;

public record ExperienceResponse(
    long Id,
    long CurriculumId,
    string JobTitle,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);