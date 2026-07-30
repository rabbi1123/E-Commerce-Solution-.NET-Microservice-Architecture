using Microsoft.EntityFrameworkCore;
using User.API.Data;
using UserEntity = User.API.Models.User;

namespace User.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<UserEntity?> GetByIdAsync(Guid userId)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<UserEntity> AddAsync(UserEntity user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<UserEntity?> UpdateAsync(UserEntity user)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
        if (existing is null)
        {
            return null;
        }

        existing.Email = user.Email;
        existing.FirstName = user.FirstName;
        existing.LastName = user.LastName;
        existing.Phone = user.Phone;
        existing.Address = user.Address;
        existing.Photo = user.Photo;
        existing.Gender = user.Gender;
        existing.BirthDate = user.BirthDate;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid userId)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (existing is null)
        {
            return false;
        }

        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
