using User.API.Models;
using User.API.Repositories;

namespace User.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IEnumerable<ApplicationUser>> GetAllAsync()
        => _userRepository.GetAllAsync();

    public Task<ApplicationUser?> GetByIdAsync(int id)
        => _userRepository.GetByIdAsync(id);

    public async Task<ApplicationUser> CreateAsync(ApplicationUser user)
    {
        user.CreatedAt = DateTime.UtcNow;
        return await _userRepository.AddAsync(user);
    }

    public async Task<ApplicationUser?> UpdateAsync(int id, ApplicationUser user)
    {
        user.Id = id;
        user.UpdatedAt = DateTime.UtcNow;
        return await _userRepository.UpdateAsync(user);
    }

    public Task<bool> DeleteAsync(int id)
        => _userRepository.DeleteAsync(id);
}

