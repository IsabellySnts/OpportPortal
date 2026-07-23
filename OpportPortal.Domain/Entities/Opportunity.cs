using OpportPortal.Domain.Enums;

namespace OpportPortal.Domain.Entities;

public class Opportunity : BaseEntity
{
    public long CompanyId { get; private set; }
    public Company Company { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public OpportunityType Type { get; private set; }
    public JobFormat Format { get; private set; }
    public string Requirements { get; private set; }
    public string FieldOfWork { get; private set; }
    public string State { get; private set; }
    public decimal Salary { get; private set; }
    public OpportunityStatus Status { get; private set; }
    public ICollection<JobApplication> JobApplications { get; private set; } = [];



    public Opportunity(long companyId, string title, string description, OpportunityType type, JobFormat format, string requirements, string fieldOfWork, string state, decimal salary) 
        : base()
    {
        if (companyId <= 0)
            throw new ArgumentException("Invalid company");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("description is required");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("title is required");

        if (string.IsNullOrWhiteSpace(requirements))
            throw new ArgumentException("requirements is required");

        if (string.IsNullOrWhiteSpace(fieldOfWork))
            throw new ArgumentException("fieldOfWork is required");

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative.");

        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State is required.");

        CompanyId = companyId;
        Title = title;
        Description = description;
        Type = type;
        Format = format;
        Requirements = requirements;
        FieldOfWork = fieldOfWork;
        Salary = salary;
        Status = OpportunityStatus.Active;
        State = state;

    }

    private Opportunity()
    {
        
    }

    public void Close()
    {
        if (Status == OpportunityStatus.Closed)
            throw new InvalidOperationException("Opportunity is already closed.");

        Status = OpportunityStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, OpportunityType type, JobFormat format, string requirements, string fieldOfWork, string state, decimal salary)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.");

        if (string.IsNullOrWhiteSpace(requirements))
            throw new ArgumentException("Requirements is required.");

        if (string.IsNullOrWhiteSpace(fieldOfWork))
            throw new ArgumentException("Field of work is required.");

        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State is required.");

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative.");

        Title = title;
        Description = description;
        Type = type;
        Format = format;
        Requirements = requirements;
        FieldOfWork = fieldOfWork;
        State = state;
        Salary = salary;
        UpdatedAt = DateTime.UtcNow;
    }
}
