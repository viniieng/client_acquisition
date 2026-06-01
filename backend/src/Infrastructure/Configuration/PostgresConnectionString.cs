using Npgsql;

namespace ClientAcquisition.Infrastructure.Configuration;

/// <summary>
/// Normalizes a PostgreSQL connection string. Accepts either the Npgsql keyword
/// format (<c>Host=...;Username=...</c>) or a <c>postgres://</c> / <c>postgresql://</c>
/// URI as handed out by managed providers like Supabase, Render or Heroku, and always
/// returns the Npgsql keyword format that Npgsql/EF Core expects.
/// </summary>
public static class PostgresConnectionString
{
    public static string Normalize(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            return connectionString ?? string.Empty;

        var trimmed = connectionString.Trim();

        var isUri = trimmed.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
                    || trimmed.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase);
        if (!isUri)
            return trimmed; // already in Npgsql keyword format

        var uri = new Uri(trimmed);
        var userInfo = uri.UserInfo.Split(':', 2);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')),
            Username = Uri.UnescapeDataString(userInfo[0]),
            // Managed Postgres (Supabase/Render/Heroku) requires SSL. In Npgsql 8,
            // SslMode.Require encrypts without enforcing certificate-chain validation.
            SslMode = SslMode.Require,
        };

        if (userInfo.Length > 1)
            builder.Password = Uri.UnescapeDataString(userInfo[1]);

        foreach (var pair in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var kv = pair.Split('=', 2);
            var key = Uri.UnescapeDataString(kv[0]);
            var value = kv.Length > 1 ? Uri.UnescapeDataString(kv[1]) : string.Empty;

            if (key.Equals("sslmode", StringComparison.OrdinalIgnoreCase))
                builder.SslMode = ParseSslMode(value, builder.SslMode);
            else
                builder[key] = value;
        }

        return builder.ConnectionString;
    }

    private static SslMode ParseSslMode(string value, SslMode fallback) => value.ToLowerInvariant() switch
    {
        "disable" => SslMode.Disable,
        "allow" => SslMode.Allow,
        "prefer" => SslMode.Prefer,
        "require" => SslMode.Require,
        "verify-ca" => SslMode.VerifyCA,
        "verify-full" => SslMode.VerifyFull,
        _ => fallback,
    };
}
