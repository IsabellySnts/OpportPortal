using OpportPortal.Domain.Enums;
using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Domain.Entities;

public class User : BaseEntity
{
    public Email UserEmail { get; private set; }
    public string? Password { get; private set; }
    public UserType Type { get; set; }
    public bool Active { get; set; }
    public AuthProvider Provider { get; private set; }


    public User(Email emailUser, string password, UserType userType) : base()
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.");

        UserEmail = emailUser;
        Password = password;
        Type = userType;
        Active = true;
        Provider = AuthProvider.Local;
    }

    private User()
    {
        
    }
    private User(Email emailUser, UserType userType, AuthProvider provider) : base()
    {
        UserEmail = emailUser;
        Password = null;
        Type = userType;
        Active = true;
        Provider = provider;
    }

    public static User CreateGoogleUser(Email emailUser, UserType userType)
        => new User(emailUser, userType, AuthProvider.Google);

    public void Deactivate()
    {
        Active = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
