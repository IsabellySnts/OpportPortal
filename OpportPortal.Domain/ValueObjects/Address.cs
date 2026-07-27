namespace OpportPortal.Domain.ValueObjects;

public sealed class Address
{
    public string ZipCode { get; }
    public string Street { get; }
    public string Number { get; }
    public string Neighborhood { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string? Complement { get; }

    public Address(
        string zipCode,
        string street,
        string number,
        string neighborhood,
        string city,
        string state,
        string country,
        string? complement = null)
    {
        if (string.IsNullOrWhiteSpace(zipCode))
            throw new ArgumentException("ZipCode is required.");

        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street is required.");

        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Number is required.");

        if (string.IsNullOrWhiteSpace(neighborhood))
            throw new ArgumentException("Neighborhood is required.");

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.");

        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State is required.");

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required.");

        ZipCode = zipCode;
        Street = street;
        Number = number;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
        Complement = complement;
    }
}
