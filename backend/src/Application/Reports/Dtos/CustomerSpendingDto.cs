namespace ClientAcquisition.Application.Reports.Dtos;

public sealed record CustomerSpendingDto(
    Guid CustomerId,
    string CustomerFullName,
    int OrderCount,
    decimal TotalSpent);