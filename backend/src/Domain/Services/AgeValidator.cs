namespace ClientAcquisition.Domain.Services;

public static class AgeValidator
{
    public static bool IsAtLeast18YearsOld(DateOnly birthDate, DateOnly? referenceDate = null)
    {
        var today = referenceDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        if (birthDate > today)
        {
            return false;
        }

        var age = today.Year - birthDate.Year;
        if (birthDate.Month > today.Month || (birthDate.Month == today.Month && birthDate.Day > today.Day))
        {
            age--;
        }

        return age >= 18;
    }
}