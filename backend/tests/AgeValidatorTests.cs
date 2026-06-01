using ClientAcquisition.Domain.Services;

namespace ClientAcquisition.Tests;

public sealed class AgeValidatorTests
{
    [Fact]
    public void IsAtLeast18YearsOld_ReturnsTrue_WhenCustomerIsAdult()
    {
        var result = AgeValidator.IsAtLeast18YearsOld(new DateOnly(2000, 1, 1), new DateOnly(2026, 6, 1));

        Assert.True(result);
    }

    [Fact]
    public void IsAtLeast18YearsOld_ReturnsFalse_WhenCustomerIsMinor()
    {
        var result = AgeValidator.IsAtLeast18YearsOld(new DateOnly(2010, 1, 1), new DateOnly(2026, 6, 1));

        Assert.False(result);
    }
}