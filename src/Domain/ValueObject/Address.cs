namespace Domain.ValueObject;

public record Address
{
    public string Street { get; } 
    public string City { get; }
    public string ZipCode { get; }

    private Address(string street, string city, string zipCode) 
    {
        Street = street;
        City = city;
        ZipCode = zipCode;
    }

    public static Address Of(string street, string city, string zipCode) 
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Street is required.", nameof(street));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(zipCode)) throw new ArgumentException("ZipCode is required.", nameof(zipCode));

        return new Address( street, city, zipCode);
    }
}