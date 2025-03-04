using System.Collections.Generic;
using System.Linq;
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
        if (!_cache.TryGetValue(UsersKey, out List<User> users))
        {
            _cache.Set(UsersKey, new List<User>());
        }
    }

    public bool Register(string username, string password)
    {
        var users = _cache.Get<List<User>>(UsersKey) ?? new List<User>();

        if (users.Any(u => u.Username == username))
            return false; // Käyttäjä on jo olemassa

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        users.Add(new User { Username = username, PasswordHash = passwordHash });

        _cache.Set(UsersKey, users);
        return true;
    }

    public bool ValidateUser(string username, string password)
    {
        var users = _cache.Get<List<User>>(UsersKey);
        var user = users?.FirstOrDefault(u => u.Username == username);
        return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}
