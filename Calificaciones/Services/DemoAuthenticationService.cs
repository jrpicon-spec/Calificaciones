namespace Calificaciones.Services;

/// <summary>
/// In-memory credentials for the classroom demonstration only. This is not production authentication.
/// </summary>
public sealed class DemoAuthenticationService
{
    // Change these values to customize the local demo credentials.
    private const string DemoUsername = "demo";
    private const string DemoPassword = "Sudamericano123";

    public bool IsAuthenticated { get; private set; }

    public bool TrySignIn(string username, string password)
    {
        if (!string.Equals(username, DemoUsername, StringComparison.Ordinal)
            || !string.Equals(password, DemoPassword, StringComparison.Ordinal))
        {
            return false;
        }

        IsAuthenticated = true;
        return true;
    }

    public void SignOut() => IsAuthenticated = false;
}
