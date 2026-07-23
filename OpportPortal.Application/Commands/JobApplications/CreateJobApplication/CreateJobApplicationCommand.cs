namespace OpportPortal.Application.Commands.JobApplications.CreateJobApplication;

public record CreateJobApplicationCommand(
    long CandidateId,
    long OpportunityId
);