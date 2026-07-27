using OpportPortal.Domain.Enums;

namespace OpportPortal.API.Requests;

public record UpdateCurriculumRequest(
    EducationLevel EducationLevel,
    List<string> HardSkills,
    List<string> SoftSkills
);

