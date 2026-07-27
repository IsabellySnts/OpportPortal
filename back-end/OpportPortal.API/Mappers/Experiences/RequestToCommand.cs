using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.Experiences.CreateExperience;

namespace OpportPortal.API.Mappers.Experiences;

internal static class RequestToCommand
{
    public static CreateExperienceCommand CreateExperienceRequestToCommand (this CreateExperienceRequest request)
    {
        return new CreateExperienceCommand(
            request.CurriculumId,
            request.JobTitle,
            request.CompanyName,
            request.StartDate,
            request.EndDate,
            request.Description
        );
    }
}
