using ClientAcquisition.Domain.Services;

namespace ClientAcquisition.Tests;

public sealed class CpfValidatorTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    public void IsValid_ReturnsTrue_ForValidCpf(string cpf)
    {
        Assert.True(CpfValidator.IsValid(cpf));
    }

    [Theory]
    [InlineData("111.111.111-11")]
    [InlineData("123")]
    [InlineData("")]
    public void IsValid_ReturnsFalse_ForInvalidCpf(string cpf)
    {
        Assert.False(CpfValidator.IsValid(cpf));
    }
}