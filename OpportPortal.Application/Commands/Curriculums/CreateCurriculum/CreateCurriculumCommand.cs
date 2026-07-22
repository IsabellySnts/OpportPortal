using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Commands.Curriculums.CreateCurriculum;

public record CreateCurriculumCommand(
    long CandidateId,
    EducationLevel EducationLevel,
    List<string> HardSkills,
    List<string> SoftSkills
);
