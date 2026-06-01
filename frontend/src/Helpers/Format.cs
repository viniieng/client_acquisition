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

    private static readonly string[] MonthsPt =
    {
        "jan", "fev", "mar", "abr", "mai", "jun", "jul", "ago", "set", "out", "nov", "dez"
    };

    private static readonly string[] WeekdaysPt =
    {
        "dom", "seg", "ter", "qua", "qui", "sex", "sáb"
    };

    public static string Currency(decimal value) => "R$ " + value.ToString("#,0.00", BrlFormat);

    public static string Date(DateTime value)
    {
        var local = value.ToLocalTime();
        return $"{local.Day:00} {MonthsPt[local.Month - 1]} {local.Year}";
    }

    public static string DateTimeShort(DateTime value)
    {
        var local = value.ToLocalTime();
        return $"{local.Day:00} {MonthsPt[local.Month - 1]} {local.Year}, {local:HH:mm}";
    }

    public static string Date(DateOnly value) => $"{value.Day:00} {MonthsPt[value.Month - 1]} {value.Year}";

    public static string WeekdayDate(DateTime value)
    {
        var local = value.ToLocalTime();
        return $"{WeekdaysPt[(int)local.DayOfWeek]}, {local.Day:00} {MonthsPt[local.Month - 1]} {local.Year}";
    }

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
