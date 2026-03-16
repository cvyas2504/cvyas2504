using FrontOfficeERP.API.Data;
using FrontOfficeERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeERP.API.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public Task<User?> GetByUsernameAsync(string username)
        => context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Username == username);

    public Task<List<User>> GetAllAsync()
        => context.Users.Include(x => x.Role).AsNoTracking().ToListAsync();

    public async Task<User> AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }
}
