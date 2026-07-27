namespace OpportPortal.Application.Commands.Experiences.UpdateExperience;

public record UpdateExperienceCommand(
    long Id,
    string JobTitle,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description
);
