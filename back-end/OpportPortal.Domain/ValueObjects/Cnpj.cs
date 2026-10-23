namespace OpportPortal.Domain.ValueObjects;

public sealed class Cnpj
{
    public string Value { get; }

    public Cnpj(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CNPJ is required.");

        var cnpj = RemoveFormatting(value);

        if (!IsValid(cnpj))
            throw new ArgumentException("CNPJ invalid.");

        Value = cnpj;
    }

    private static string RemoveFormatting(string cnpj)
    {
        return new string(cnpj.Where(char.IsDigit).ToArray());
    }

    private static bool IsValid(string cnpj)
    {
        if (cnpj.Length != 14)
            return false;

        if (cnpj.All(c => c == cnpj[0]))
            return false;

        return true;
    }
}