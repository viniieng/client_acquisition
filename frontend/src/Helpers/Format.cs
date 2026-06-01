using System.Globalization;

namespace ClientAcquisition.Frontend.Helpers;

public static class Format
{
    private static readonly NumberFormatInfo BrlFormat = new()
    {
        CurrencyDecimalSeparator = ",",
        CurrencyGroupSeparator = ".",
        NumberDecimalSeparator = ",",
        NumberGroupSeparator = "."
    };

    public static string Currency(decimal value) => "R$ " + value.ToString("#,0.00", BrlFormat);

    public static string Date(DateTime value) => value.ToLocalTime().ToString("dd MMM yyyy", CultureInfo.InvariantCulture);

    public static string DateTimeShort(DateTime value) => value.ToLocalTime().ToString("dd MMM yyyy, HH:mm", CultureInfo.InvariantCulture);

    public static string Date(DateOnly value) => value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);

    public static string Initials(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "?";
        }

        var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
        {
            return parts[0][..Math.Min(2, parts[0].Length)].ToUpperInvariant();
        }

        return ($"{parts[0][0]}{parts[^1][0]}").ToUpperInvariant();
    }

    public static string ShortId(Guid id) => "#" + id.ToString("N")[..6].ToUpperInvariant();
}
