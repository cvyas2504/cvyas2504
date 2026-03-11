using System.Security.Cryptography;
using System.Text;
using FrontOfficeApp.Data;
using FrontOfficeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeApp.Services;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        using var sha = SHA256.Create();
        return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    public bool Verify(string password, string hash) => Hash(password) == hash;
}

public interface ISessionService
{
    User? CurrentUser { get; }
    void Start(User user);
    void End();
}

public class SessionService : ISessionService
{
    public User? CurrentUser { get; private set; }
    public void Start(User user) => CurrentUser = user;
    public void End() => CurrentUser = null;
}

public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task<List<User>> GetUsersAsync();
    Task SaveUserAsync(User user, string? plainPassword = null);
    Task DeleteUserAsync(int userId);
}

public class AuthService(AppDbContext db, IPasswordHasher hasher, ISessionService session) : IAuthService
{
    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null || !hasher.Verify(password, user.PasswordHash)) return null;
        session.Start(user);
        return user;
    }

    public Task LogoutAsync() { session.End(); return Task.CompletedTask; }
    public Task<List<User>> GetUsersAsync() => db.Users.OrderBy(x => x.Username).ToListAsync();

    public async Task SaveUserAsync(User user, string? plainPassword = null)
    {
        if (!string.IsNullOrWhiteSpace(plainPassword)) user.PasswordHash = hasher.Hash(plainPassword);
        if (user.UserID == 0) db.Users.Add(user); else db.Users.Update(user);
        await db.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await db.Users.FindAsync(userId);
        if (user is null) return;
        db.Users.Remove(user);
        await db.SaveChangesAsync();
    }
}
