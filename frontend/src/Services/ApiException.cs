namespace ClientAcquisition.Frontend.Services;

public sealed class ApiException : Exception
{
    public ApiException(string message) : base(message)
    {
    }
}
