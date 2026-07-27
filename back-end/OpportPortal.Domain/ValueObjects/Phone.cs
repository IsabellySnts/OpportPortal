namespace OpportPortal.Domain.ValueObjects;

public sealed class Phone
{
    public string Value { get; set; }

    public Phone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception("Phone is required");

        var phoneNumber = RemoveFormatting(value);

        if (!IsValid(phoneNumber))
            throw new Exception("Invalid phone number");

        Value = value;
    }

    private static string RemoveFormatting(string phoneNumber)
        => new string(phoneNumber.Where(char.IsDigit).ToArray());

    private static bool IsValid(string phoneNumber)
         => phoneNumber.Length == 11;
}
