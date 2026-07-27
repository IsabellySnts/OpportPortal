using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.Curriculums.CreateCurriculum;
using OpportPortal.Application.Commands.Curriculums.UpdateCurriculum;

namespace OpportPortal.API.Mappers.Curriculums;

internal static class RequestToCommand
{
    public static CreateCurriculumCommand CreateCurriculumRequestToCommand(this CreateCurriculumRequest request, long userId)
    {
        return new CreateCurriculumCommand(
            userId,
            request.EducationLevel,
            request.HardSkills,
            request.SoftSkills
        );
    }

    public static UpdateCurriculumCommand UpdateCurriculumRequestToCommand(this UpdateCurriculumRequest request, long userId)
    {
        return new UpdateCurriculumCommand(
            userId,
            request.EducationLevel,
            request.HardSkills,
            request.SoftSkills
        );
    }
}
