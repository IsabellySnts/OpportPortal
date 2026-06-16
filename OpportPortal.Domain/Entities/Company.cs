using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Domain.Entities;

public class Company : BaseEntity
{
    public long UserId { get; private set; }
    public User User { get; private set; }
    public string CompanyName { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public string Sector { get; private set; }
    public Email Email { get; private set; }
    public Phone PhoneNumber { get; private set; }
    public Address Address { get; private set; }
    public ICollection<Opportunity> Opportunities { get; private set; } = [];

    private Company()
    {
        
    }
    public Company(long userId, string companyName, Cnpj cnpj, string sector, Email email, Phone phoneNumber, Address address) :base()
    {
        UserId = userId;
        CompanyName = companyName;
        Cnpj = cnpj;
        Sector = sector;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
    }

}
