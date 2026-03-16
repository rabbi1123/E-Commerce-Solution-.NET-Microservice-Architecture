using User.API.Models;

namespace User.API.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<ApplicationUser?> GetByIdAsync(int id);
    Task<ApplicationUser> AddAsync(ApplicationUser user);
    Task<ApplicationUser?> UpdateAsync(ApplicationUser user);
    Task<bool> DeleteAsync(int id);
}

