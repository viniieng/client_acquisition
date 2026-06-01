using ClientAcquisition.Infrastructure.Configuration;
using Xunit;

namespace ClientAcquisition.Tests;

public class PostgresConnectionStringTests
{
    [Fact]
    public void KeywordFormat_IsReturnedUnchanged()
    {
        const string keyword = "Host=db.example.com;Port=5432;Database=postgres;Username=postgres;Password=secret;SSL Mode=Require;Trust Server Certificate=True";

        Assert.Equal(keyword, PostgresConnectionString.Normalize(keyword));
    }

    [Fact]
    public void Uri_IsConvertedToKeywordFormat_WithSslDefaults()
    {
        const string uri = "postgresql://postgres.abc:p%40ss%2Fword@aws-1-us-east-1.pooler.supabase.com:5432/postgres";

        var result = PostgresConnectionString.Normalize(uri);

        Assert.Contains("Host=aws-1-us-east-1.pooler.supabase.com", result);
        Assert.Contains("Port=5432", result);
        Assert.Contains("Database=postgres", result);
        Assert.Contains("Username=postgres.abc", result);
        Assert.Contains("Password=p@ss/word", result); // percent-decoded
        Assert.Contains("SSL Mode=Require", result);
        Assert.DoesNotContain("postgresql://", result);
    }

    [Fact]
    public void Uri_RespectsExplicitSslMode()
    {
        const string uri = "postgres://user:pass@localhost:5432/db?sslmode=disable";

        var result = PostgresConnectionString.Normalize(uri);

        Assert.Contains("SSL Mode=Disable", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NullOrWhitespace_IsReturnedAsIs(string? input)
    {
        Assert.Equal(input ?? string.Empty, PostgresConnectionString.Normalize(input));
    }
}
