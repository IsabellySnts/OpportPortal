using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Commands.JobApplications.UpdateJobApplicationStatus;

public record UpdateJobApplicationStatusCommand(
    long Id,
    JobApplicationStatus NewStatus
);