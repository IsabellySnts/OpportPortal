namespace OpportPortal.Domain.Entities;

public class Experience : BaseEntity
{
    public long CurriculumId { get; private set; }
    public Curriculum Curriculum { get; private set; }
    public string JobTitle { get; private set; }
    public string CompanyName { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Description { get; private set; }

    public Experience(long curriculumId, string jobTitle, string companyName, DateTime startDate, DateTime? endDate, string? description) : base()
    {
        if (curriculumId <= 0)
            throw new ArgumentException("Invalid curriculum");

        if (string.IsNullOrWhiteSpace(jobTitle))
            throw new ArgumentException("jobTitle is required");

        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name is required.");

        if (endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date cannot be earlier than start date");

        CurriculumId = curriculumId;
        JobTitle = jobTitle;
        CompanyName = companyName;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
    }

    public void Update(string jobTitle, string companyName, DateTime startDate, DateTime? endDate, string? description)
    {
        if (string.IsNullOrWhiteSpace(jobTitle))
            throw new ArgumentException("Job title is required.");

        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name is required.");

        if (endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date cannot be earlier than start date.");

        JobTitle = jobTitle;
        CompanyName = companyName;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    private Experience()
    {
        
    }
    
}
