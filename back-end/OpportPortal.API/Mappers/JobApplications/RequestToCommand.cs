using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.JobApplications.CreateJobApplication;

namespace OpportPortal.API.Mappers.JobApplications;

internal static class RequestToCommand
{
    public static CreateJobApplicationCommand CreateJobApplicationRequestToCommand(this CreateJobApplicationRequest request, long userId)
    {
        return new CreateJobApplicationCommand(
            userId,
            request.OpportunityId
        );
    }
}