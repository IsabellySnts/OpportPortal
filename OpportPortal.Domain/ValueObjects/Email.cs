namespace OpportPortal.Domain.ValueObjects;

public sealed class Email
{
    public string Value { get;}

    public Email(string value)
    {
        if (!value.Contains("@"))
            throw new Exception("Invalid email.");

        Value = value;
    }
}
