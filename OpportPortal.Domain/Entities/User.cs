using OpportPortal.Domain.Enums;
using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Domain.Entities;

public class User : BaseEntity
{
    public Email UserEmail { get; set; }
    public string Password { get; set; }
    public UserType Type { get; set; }
    public bool Active { get; set; }

    private User()
    {
        
    }
    public User(Email emailUser, string password, UserType type, bool active) : base()
    {
        UserEmail = emailUser;
        Password = password;
        Type = type;
        Active = active;
    }
}
