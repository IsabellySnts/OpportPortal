using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Domain.Entities;

public class Candidate : BaseEntity
{
    public long UserId { get; private set; }
    public User User { get; private set; }
    public string FullName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public Address Address { get; private set; }
    public Phone PhoneNumber { get; private set; }
    public Curriculum Curriculum { get; private set; }
    public string? UrlLinkedin { get; private set; }
    public ICollection<JobApplication> JobApplications { get; private set; } = [];


    private Candidate() { }

    public Candidate(long idUser, string name, DateTime birthDate, Address address, Phone phoneNumber) : base()
    {
        if (idUser <= 0)
            throw new ArgumentException("Invalid user.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        if (birthDate >= DateTime.UtcNow)
            throw new ArgumentException("The date of birth must be earlier than the current date.");

        UserId = idUser;
        FullName = name;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    public void SetLinkedin(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("LinkedIn URL is required.");

        UrlLinkedin = url;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, DateTime birthDate, Address address, Phone phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        if (birthDate >= DateTime.UtcNow)
            throw new ArgumentException("The date of birth must be earlier than the current date.");

        FullName = name;
        BirthDate = birthDate;
        Address = address;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }

}
