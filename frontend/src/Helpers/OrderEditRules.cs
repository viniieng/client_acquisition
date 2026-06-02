namespace ClientAcquisition.Frontend.Helpers;

public static class OrderEditRules
{
    public const string LockMessage = "Pedidos só podem ser editados 24 horas após a criação.";

    public static bool IsEditable(DateTime createdAtUtc) =>
        DateTime.UtcNow - createdAtUtc >= TimeSpan.FromHours(24);
}
