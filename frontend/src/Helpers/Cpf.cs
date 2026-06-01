using System.Text.RegularExpressions;

namespace ClientAcquisition.Frontend.Helpers;

public static class Cpf
{
    public static string Digits(string cpf) => Regex.Replace(cpf ?? string.Empty, "[^0-9]", string.Empty);

    public static bool IsValid(string cpf)
    {
        var digits = Digits(cpf);
        if (digits.Length != 11 || digits.Distinct().Count() == 1)
        {
            return false;
        }

        var numbers = digits.Select(c => c - '0').ToArray();
        return numbers[9] == Digit(numbers, 9, 10) && numbers[10] == Digit(numbers, 10, 11);
    }

    public static string Mask(string cpf)
    {
        var digits = Digits(cpf);
        if (digits.Length != 11)
        {
            return cpf ?? string.Empty;
        }

        return $"{digits[..3]}.{digits[3..6]}.{digits[6..9]}-{digits[9..]}";
    }

    private static int Digit(int[] numbers, int count, int weight)
    {
        var sum = 0;
        for (var i = 0; i < count; i++)
        {
            sum += numbers[i] * (weight - i);
        }

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
