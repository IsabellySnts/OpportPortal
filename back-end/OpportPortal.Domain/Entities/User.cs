using OpportPortal.Domain.Enums;
using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Domain.Entities;

public class User : BaseEntity
{
    public Email UserEmail { get; private set; }
    public string Password { get; private set; }
    public UserType Type { get; set; }
    public bool Active { get; set; }

    private User()
    {
        
    }
    public User(Email emailUser, string password, UserType userType) : base()
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.");

        UserEmail = emailUser;
        Password = password;
        Type = userType;
        Active = true;
    }

    public void Deactivate()
    {
        Active = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
