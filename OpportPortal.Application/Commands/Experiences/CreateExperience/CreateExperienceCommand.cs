namespace OpportPortal.Application.Commands.Experiences.CreateExperience;

public record CreateExperienceCommand(
    long CurriculumId,
    string JobTitle,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description
);
