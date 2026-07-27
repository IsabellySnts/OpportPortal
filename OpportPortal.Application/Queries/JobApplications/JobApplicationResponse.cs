using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Queries.JobApplications;

public record JobApplicationResponse(
    long Id,
    long CandidateId,
    string CandidateName,
    long OpportunityId,
    string OpportunityTitle,
    JobApplicationStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);