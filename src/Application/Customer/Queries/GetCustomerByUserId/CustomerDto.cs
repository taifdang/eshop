namespace Application.Customer.Queries.GetCustomerByUserId;

public class CustomerDto 
{
    public Guid Id { get; init; }
    public string? FullName { get; init; }
    public string Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
}

