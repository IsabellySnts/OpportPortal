namespace OpportPortal.Application.Commands.JobApplications.CreateJobApplication;

public record CreateJobApplicationCommand(
    long UserId,
    long OpportunityId
);