using User.API.Models;

namespace User.API.Services;

public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<ApplicationUser?> GetByIdAsync(int id);
    Task<ApplicationUser> CreateAsync(ApplicationUser user);
    Task<ApplicationUser?> UpdateAsync(int id, ApplicationUser user);
    Task<bool> DeleteAsync(int id);
}

