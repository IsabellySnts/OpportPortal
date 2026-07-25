using OpportPortal.Domain.Enums;

namespace OpportPortal.API.Requests;

public record CreateCurriculumRequest(
    EducationLevel EducationLevel,
    List<string> HardSkills,
    List<string> SoftSkills
);
