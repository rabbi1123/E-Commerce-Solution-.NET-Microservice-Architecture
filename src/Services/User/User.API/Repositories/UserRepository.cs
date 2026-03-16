using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.Models;

namespace User.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<ApplicationUser?> GetByIdAsync(int id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<ApplicationUser> AddAsync(ApplicationUser user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<ApplicationUser?> UpdateAsync(ApplicationUser user)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UserName = user.UserName;
        existing.Email = user.Email;
        existing.Name = user.Name;
        existing.LastName = user.LastName;
        existing.Address = user.Address;
        existing.IsActive = user.IsActive;
        existing.UserType = user.UserType;
        existing.UpdatedAt = user.UpdatedAt;
        existing.UpdatedBy = user.UpdatedBy;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existing is null)
        {
            return false;
        }

        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}

