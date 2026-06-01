using System.Text.RegularExpressions;

namespace ClientAcquisition.Domain.Services;

public static class CpfValidator
{
    public static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return false;
        }

        var digitsOnly = Regex.Replace(cpf, "[^0-9]", string.Empty);
        if (digitsOnly.Length != 11)
        {
            return false;
        }

        if (digitsOnly.Distinct().Count() == 1)
        {
            return false;
        }

        var numbers = digitsOnly.Select(character => character - '0').ToArray();
        var firstDigit = CalculateDigit(numbers, 9, 10);
        var secondDigit = CalculateDigit(numbers, 10, 11);

        return numbers[9] == firstDigit && numbers[10] == secondDigit;
    }

    private static int CalculateDigit(int[] numbers, int count, int weight)
    {
        var sum = 0;
        for (var index = 0; index < count; index++)
        {
            sum += numbers[index] * (weight - index);
        }

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}