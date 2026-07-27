using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Queries.Curriculums;

public record CurriculumResponse(
    long Id,
    long CandidateId,
    EducationLevel EducationLevel,
    List<string> HardSkills,
    List<string> SoftSkills,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
