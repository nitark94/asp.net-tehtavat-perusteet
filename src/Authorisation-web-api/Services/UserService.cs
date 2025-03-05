using Microsoft.Extensions.Caching.Memory;
using Authorisation_web_api.Models;

namespace Authorisation_web_api.Services;

public class UserService
{
    private readonly IMemoryCache _cache;
    private const string UsersKey = "Users";

    public UserService(IMemoryCache cache)
    {
        _cache = cache;

        // Tarkista, onko käyttäjälista jo olemassa. Jos ei, luo uusi.
        if (!_cache.TryGetValue(UsersKey, out List<User>? users))
        {
            users = new List<User>();
            _cache.Set(UsersKey, users);
        }
    }

    /// <summary>
    /// Rekisteröi uusi käyttäjä
    /// </summary>
    /// <param name="username">Käyttäjänimi</param>
    /// <param name="password">Salasana</param>
    /// <returns>True, jos rekisteröinti onnistui. False, jos käyttäjä on jo olemassa.</returns>
    public bool Register(string username, string password)
    {
        var users = _cache.Get<List<User>>(UsersKey) ?? new List<User>();

        if (users.Any(u => u.Username == username))
            return false; // Käyttäjä on jo olemassa

        // Hashataan salasana ennen tallennusta
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        users.Add(new User { Username = username, PasswordHash = passwordHash });

        _cache.Set(UsersKey, users);
        return true;
    }

    /// <summary>
    /// Tarkistaa käyttäjän kirjautumistiedot
    /// </summary>
    /// <param name="username">Käyttäjänimi</param>
    /// <param name="password">Salasana</param>
    /// <returns>True, jos kirjautuminen onnistuu. False, jos tiedot eivät täsmää.</returns>
    public bool ValidateUser(string username, string password)
    {
        var users = _cache.Get<List<User>>(UsersKey) ?? new List<User>();

        var user = users.FirstOrDefault(u => u.Username == username);
        return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}
