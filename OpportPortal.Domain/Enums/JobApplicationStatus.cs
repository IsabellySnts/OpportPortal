namespace OpportPortal.Domain.Enums;

public enum JobApplicationStatus
{
    Registered = 1, 
    UnderReview = 2, 
    InterviewScheduled = 3, 
    FinalProcess = 4,
    Approved = 5,
    NotAdvanced = 6,
    Withdrawn = 7,
    PositionClosed = 8 
}