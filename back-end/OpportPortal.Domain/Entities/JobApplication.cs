using OpportPortal.Domain.Enums;

namespace OpportPortal.Domain.Entities;

public class JobApplication : BaseEntity
{
    public long CandidateId { get; private set; }
    public Candidate Candidate { get; private set; }
    public long OpportunityId { get; private set; }
    public Opportunity Opportunity { get; private set; }
    public JobApplicationStatus Status { get; private set; }

    private static readonly Dictionary<JobApplicationStatus, IEnumerable<JobApplicationStatus>> _validTransitions = new()
    {
        [JobApplicationStatus.Registered] = [JobApplicationStatus.UnderReview, JobApplicationStatus.NotAdvanced, JobApplicationStatus.Withdrawn, JobApplicationStatus.PositionClosed],
        [JobApplicationStatus.UnderReview] = [JobApplicationStatus.InterviewScheduled, JobApplicationStatus.NotAdvanced, JobApplicationStatus.PositionClosed],
        [JobApplicationStatus.InterviewScheduled] = [JobApplicationStatus.FinalProcess, JobApplicationStatus.NotAdvanced, JobApplicationStatus.PositionClosed],
        [JobApplicationStatus.FinalProcess] = [JobApplicationStatus.Approved, JobApplicationStatus.NotAdvanced, JobApplicationStatus.PositionClosed],
        [JobApplicationStatus.Approved] = [],
        [JobApplicationStatus.NotAdvanced] = [],
        [JobApplicationStatus.Withdrawn] = [],
        [JobApplicationStatus.PositionClosed] = [],
    };

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
        Status = JobApplicationStatus.NotAdvanced;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        Status = JobApplicationStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void StartReview() => Transition(JobApplicationStatus.UnderReview);
    public void MoveToFinalProcess() => Transition(JobApplicationStatus.FinalProcess);
    public void Withdraw() => Transition(JobApplicationStatus.Withdrawn);
    public void ClosePosition() => Transition(JobApplicationStatus.PositionClosed);

    private void Transition(JobApplicationStatus newStatus)
    {
        if (!_validTransitions[Status].Contains(newStatus))
            throw new InvalidOperationException($"Invalid transition: {Status} → {newStatus}.");

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsActive() =>
        Status is not (JobApplicationStatus.Approved
            or JobApplicationStatus.NotAdvanced
            or JobApplicationStatus.Withdrawn
            or JobApplicationStatus.PositionClosed);
}
