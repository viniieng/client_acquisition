namespace ClientAcquisition.Frontend.Models;

public sealed record CustomerSpendingDto(
    Guid CustomerId,
    string CustomerFullName,
    int OrderCount,
    decimal TotalSpent);
