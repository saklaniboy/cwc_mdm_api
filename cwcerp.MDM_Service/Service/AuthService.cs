using System.Collections.Generic;
using Microsoft.Extensions.Options;

public interface IAuthService
{
    bool ValidateCredentials(string username, string password);
}

public class AuthService : IAuthService
{
    private readonly Dictionary<string, string> _validUsers = new();

    public AuthService(IOptions<AuthSettings> authSettings)
    {
        // Load users from appsettings.json
        foreach (var user in authSettings.Value.Users)
        {
            _validUsers[user.Username] = user.Password;
        }
    }

    public bool ValidateCredentials(string username, string password)
    {
        return _validUsers.TryGetValue(username, out var validPassword) && validPassword == password;
    }
}