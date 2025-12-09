namespace Domain.Enums;

public enum OrderStatus
{
    Pending,
    Processing, 
    Confirmed,
    Completed,
    Rejected, // system
    Cancelled // user
}
