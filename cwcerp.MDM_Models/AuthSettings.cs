using System.Collections.Generic;

public class AuthUser
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class AuthSettings
{
    public List<AuthUser> Users { get; set; }
}