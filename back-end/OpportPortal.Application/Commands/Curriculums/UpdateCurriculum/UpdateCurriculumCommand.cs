using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Commands.Curriculums.UpdateCurriculum;

public record UpdateCurriculumCommand(
    long Id,
    EducationLevel EducationLevel,
    List<string> HardSkills,
    List<string> SoftSkills
);
