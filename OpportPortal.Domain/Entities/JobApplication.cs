using OpportPortal.Domain.Enums;

namespace OpportPortal.Domain.Entities;

public class JobApplication : BaseEntity
{
    public long CandidateId { get; private set; }
    public Candidate Candidate { get; private set; }
    public long OpportunityId { get; private set; }
    public Opportunity Opportunity { get; private set; }
    public JobApplicationStatus Status { get; private set; }

    public JobApplication(long candidateId, long opportunityId) : base()
    {
        CandidateId = candidateId;
        OpportunityId = opportunityId;
        Status = JobApplicationStatus.Registered;
    }

    private JobApplication() { }

    public void ScheduleInterview()
    {
        Status = JobApplicationStatus.InterviewScheduled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = JobApplicationStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        Status = JobApplicationStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }
}
